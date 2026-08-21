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
