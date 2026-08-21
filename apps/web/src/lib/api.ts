import {
  authHeaders,
  clearAuth,
  saveAuth,
  type AuthResponse,
} from "@/lib/auth";

async function readResponse(response: Response): Promise<unknown> {
  const contentType = response.headers.get("content-type") || "";

  if (contentType.includes("application/json")) {
    return response.json();
  }

  const text = await response.text();
  return text || null;
}

function getErrorMessage(body: unknown, fallback: string): string {
  if (typeof body === "object" && body !== null) {
    const data = body as {
      message?: string;
      title?: string;
      detail?: string;
    };

    return data.message || data.detail || data.title || fallback;
  }

  if (typeof body === "string" && body) {
    return body;
  }

  return fallback;
}

export async function apiFetch<T>(
  path: string,
  options: RequestInit = {},
): Promise<T> {
  const response = await fetch(`/api/backend${path}`, {
    ...options,
    cache: "no-store",
    headers: {
      ...authHeaders(),
      ...(options.headers || {}),
    },
  });

  const body = await readResponse(response);

  if (response.status === 401) {
    clearAuth();

    if (typeof window !== "undefined") {
      window.location.href = "/login";
    }

    throw new Error("Your session has expired. Please sign in again.");
  }

  if (!response.ok) {
    throw new Error(
      getErrorMessage(body, `Request failed: ${response.status}`),
    );
  }

  return body as T;
}

/* =========================================================
   AUTH
   ========================================================= */

export async function login(
  email: string,
  password: string,
): Promise<AuthResponse> {
  const response = await fetch("/api/backend/auth/login", {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify({
      email: email.trim(),
      password,
    }),
  });

  const body = await readResponse(response);

  if (!response.ok) {
    throw new Error(
      getErrorMessage(body, "Invalid email or password."),
    );
  }

  if (
    typeof body !== "object" ||
    body === null ||
    typeof (body as AuthResponse).token !== "string"
  ) {
    throw new Error(
      "The server returned an invalid authentication response.",
    );
  }

  const data = body as AuthResponse;

  saveAuth(data);

  return data;
}

export async function register(
  firstName: string,
  lastName: string,
  email: string,
  password: string,
): Promise<AuthResponse> {
  const response = await fetch("/api/backend/auth/register", {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify({
      firstName: firstName.trim(),
      lastName: lastName.trim(),
      email: email.trim(),
      password,
    }),
  });

  const body = await readResponse(response);

  if (!response.ok) {
    throw new Error(
      getErrorMessage(body, "Registration failed."),
    );
  }

  if (
    typeof body !== "object" ||
    body === null ||
    typeof (body as AuthResponse).token !== "string"
  ) {
    throw new Error(
      "The server returned an invalid authentication response.",
    );
  }

  const data = body as AuthResponse;

  saveAuth(data);

  return data;
}

export async function getCurrentUser() {
  return apiFetch<{
    userId: string;
    email: string;
    role: string;
    firstName: string;
    lastName: string;
  }>("/auth/me");
}

/* =========================================================
   LEARNING TYPES
   ========================================================= */

export type LearningCourse = {
  id: string;
  title: string;
  slug: string;
  description?: string | null;
  difficulty: string;
  estimatedHours: number;
  isFree: boolean;
  subjectId: string;
  learningLevelId: string;
};

export type LearningLesson = {
  id: string;
  moduleId: string;
  title: string;
  summary?: string | null;
  content?: string | null;
  contentType: string;
  estimatedMinutes: number;
  order: number;
};

export type LearningModule = {
  id: string;
  courseId: string;
  title: string;
  description?: string | null;
  order: number;
  lessons: LearningLesson[];
};

export type CourseDetails = LearningCourse & {
  modules: LearningModule[];
};

export type Enrollment = {
  id: string;
  studentId: string;
  courseId: string;
  enrolledAt: string;
  completedAt?: string | null;
  progressPercent: number;
  isActive: boolean;
  isCompleted: boolean;
};

export type MyCourse = {
  enrollmentId: string;
  courseId: string;
  enrolledAt: string;
  progressPercent: number;
  isCompleted: boolean;
  completedAt?: string | null;
  course: {
    title: string;
    slug: string;
    difficulty: string;
    estimatedHours: number;
  } | null;
};

export type LessonDetails = LearningLesson & {
  module: {
    id: string;
    courseId: string;
    title: string;
    order: number;
  };

  progress?: {
    progressPercent: number;
    timeSpentSeconds: number;
    isCompleted: boolean;
    completedAt?: string | null;
  } | null;

  previousLesson?: {
    id: string;
    moduleId: string;
    title: string;
    order: number;
  } | null;

  nextLesson?: {
    id: string;
    moduleId: string;
    title: string;
    order: number;
  } | null;
};

/* =========================================================
   LEARNING API
   ========================================================= */

/**
 * Get published courses.
 *
 * Optional filters:
 *   search
 *   subjectId
 *   learningLevelId
 */
export async function getCourses(params?: {
  search?: string;
  subjectId?: string;
  learningLevelId?: string;
}): Promise<LearningCourse[]> {
  const query = new URLSearchParams();

  if (params?.search?.trim()) {
    query.set("search", params.search.trim());
  }

  if (params?.subjectId) {
    query.set("subjectId", params.subjectId);
  }

  if (params?.learningLevelId) {
    query.set("learningLevelId", params.learningLevelId);
  }

  const queryString = query.toString();

  return apiFetch<LearningCourse[]>(
    `/learning/courses${queryString ? `?${queryString}` : ""}`,
  );
}

/**
 * Get a published course with its modules and lessons.
 */
export async function getCourse(
  courseId: string,
): Promise<CourseDetails> {
  return apiFetch<CourseDetails>(
    `/learning/courses/${courseId}`,
  );
}

/**
 * Enroll the authenticated student in a course.
 */
export async function enrollCourse(
  courseId: string,
): Promise<Enrollment> {
  return apiFetch<Enrollment>(
    `/learning/courses/${courseId}/enroll`,
    {
      method: "POST",
    },
  );
}

/**
 * Get courses belonging to the authenticated student.
 */
export async function getMyCourses(): Promise<MyCourse[]> {
  return apiFetch<MyCourse[]>("/learning/my-courses");
}

/**
 * Get an individual lesson.
 *
 * The backend verifies that the authenticated student
 * is enrolled in the course before returning content.
 */
export async function getLesson(
  lessonId: string,
): Promise<LessonDetails> {
  return apiFetch<LessonDetails>(
    `/learning/lessons/${lessonId}`,
  );
}

/* =========================================================
   CONVENIENCE ALIASES
   ========================================================= */

export const enrollInCourse = enrollCourse;

export const fetchCourses = getCourses;

export const fetchCourse = getCourse;

export const fetchMyCourses = getMyCourses;

export const fetchLesson = getLesson;
