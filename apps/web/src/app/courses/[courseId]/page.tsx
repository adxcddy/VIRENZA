"use client";

import { useEffect, useState } from "react";
import { useParams, useRouter } from "next/navigation";
import Link from "next/link";
import { apiFetch } from "@/lib/api";
import { getToken } from "@/lib/auth";

type Lesson = {
  id: string;
  title: string;
  summary?: string | null;
  contentType: string;
  estimatedMinutes: number;
  order: number;
};

type Module = {
  id: string;
  title: string;
  description?: string | null;
  order: number;
  lessons: Lesson[];
};

type Course = {
  id: string;
  title: string;
  slug: string;
  description?: string | null;
  difficulty: string;
  estimatedHours: number;
  isFree: boolean;
  modules: Module[];
};

export default function CourseDetailsPage() {
  const params = useParams();
  const router = useRouter();

  const courseId = params.courseId as string;

  const [course, setCourse] = useState<Course | null>(null);
  const [loading, setLoading] = useState(true);
  const [enrolling, setEnrolling] = useState(false);
  const [error, setError] = useState("");
  const [message, setMessage] = useState("");

  useEffect(() => {
    async function load() {
      try {
        const data = await apiFetch<Course>(
          `/learning/courses/${courseId}`,
        );

        setCourse(data);
      } catch (err) {
        setError(
          err instanceof Error
            ? err.message
            : "Unable to load course.",
        );
      } finally {
        setLoading(false);
      }
    }

    if (courseId) load();
  }, [courseId]);

  async function enroll() {
    if (!getToken()) {
      router.push(`/login?returnTo=/courses/${courseId}`);
      return;
    }

    setEnrolling(true);
    setError("");
    setMessage("");

    try {
      await apiFetch(`/learning/courses/${courseId}/enroll`, {
        method: "POST",
      });

      setMessage("You are now enrolled in this course.");
    } catch (err) {
      setError(
        err instanceof Error
          ? err.message
          : "Unable to enroll in this course.",
      );
    } finally {
      setEnrolling(false);
    }
  }

  if (loading) {
    return (
      <main className="flex min-h-screen items-center justify-center bg-slate-950 text-slate-400">
        Loading course...
      </main>
    );
  }

  if (!course) {
    return (
      <main className="flex min-h-screen items-center justify-center bg-slate-950 text-white">
        <div className="text-center">
          <h1 className="text-2xl font-black">Course not found</h1>
          <Link
            href="/courses"
            className="mt-5 inline-block text-sm font-bold text-slate-400 hover:text-white"
          >
            ← Back to courses
          </Link>
        </div>
      </main>
    );
  }

  const lessonCount = course.modules.reduce(
    (total, module) => total + module.lessons.length,
    0,
  );

  return (
    <main className="min-h-screen bg-slate-950 text-white">
      <header className="border-b border-white/10">
        <div className="mx-auto flex max-w-7xl items-center justify-between px-5 py-5 sm:px-6">
          <Link href="/courses" className="flex items-center gap-3">
            <div className="flex h-10 w-10 items-center justify-center rounded-xl bg-white font-black text-slate-950">
              V
            </div>
            <div className="font-black">VIRENZA</div>
          </Link>

          <Link
            href="/dashboard"
            className="text-sm font-semibold text-slate-400 hover:text-white"
          >
            Dashboard
          </Link>
        </div>
      </header>

      <div className="mx-auto max-w-7xl px-5 py-10 sm:px-6">
        <Link
          href="/courses"
          className="text-sm font-semibold text-slate-500 hover:text-white"
        >
          ← All courses
        </Link>

        <section className="mt-7 overflow-hidden rounded-3xl border border-white/10 bg-white/[0.04]">
          <div className="p-7 sm:p-10 lg:p-12">
            <div className="flex flex-wrap gap-2">
              <span className="rounded-full border border-white/10 px-3 py-1 text-xs font-bold text-slate-300">
                {course.difficulty}
              </span>

              {course.isFree && (
                <span className="rounded-full bg-emerald-500/10 px-3 py-1 text-xs font-bold text-emerald-400">
                  Free course
                </span>
              )}
            </div>

            <h1 className="mt-6 max-w-4xl text-4xl font-black tracking-tight sm:text-5xl">
              {course.title}
            </h1>

            <p className="mt-5 max-w-3xl text-base leading-8 text-slate-400">
              {course.description || "Begin your learning journey with VIRENZA."}
            </p>

            <div className="mt-7 flex flex-wrap gap-5 text-sm text-slate-500">
              <span>{course.estimatedHours} estimated hours</span>
              <span>{course.modules.length} modules</span>
              <span>{lessonCount} lessons</span>
            </div>

            {error && (
              <div className="mt-7 max-w-xl rounded-xl border border-red-500/20 bg-red-500/10 px-4 py-3 text-sm text-red-300">
                {error}
              </div>
            )}

            {message && (
              <div className="mt-7 max-w-xl rounded-xl border border-emerald-500/20 bg-emerald-500/10 px-4 py-3 text-sm text-emerald-300">
                {message}
              </div>
            )}

            <button
              onClick={enroll}
              disabled={enrolling}
              className="mt-8 rounded-xl bg-white px-6 py-3.5 text-sm font-black text-slate-950 hover:bg-slate-200 disabled:opacity-50"
            >
              {enrolling ? "Enrolling..." : "Enroll in course →"}
            </button>
          </div>
        </section>

        <section className="mt-10">
          <div className="mb-5">
            <p className="text-xs font-bold uppercase tracking-[0.2em] text-slate-500">
              Curriculum
            </p>
            <h2 className="mt-1 text-2xl font-black">
              Course content
            </h2>
          </div>

          <div className="space-y-4">
            {course.modules.map((module, index) => (
              <article
                key={module.id}
                className="rounded-2xl border border-white/10 bg-white/[0.03]"
              >
                <div className="flex items-start gap-4 p-6">
                  <div className="flex h-9 w-9 shrink-0 items-center justify-center rounded-lg bg-white/10 text-sm font-black">
                    {index + 1}
                  </div>

                  <div className="min-w-0 flex-1">
                    <h3 className="font-black">{module.title}</h3>

                    {module.description && (
                      <p className="mt-2 text-sm leading-6 text-slate-500">
                        {module.description}
                      </p>
                    )}

                    <div className="mt-5 space-y-2">
                      {module.lessons.map((lesson) => (
                        <div
                          key={lesson.id}
                          className="flex items-center justify-between gap-4 rounded-xl border border-white/5 bg-black/10 px-4 py-3"
                        >
                          <div className="min-w-0">
                            <p className="truncate text-sm font-semibold text-slate-300">
                              {lesson.title}
                            </p>
                            {lesson.summary && (
                              <p className="mt-1 truncate text-xs text-slate-600">
                                {lesson.summary}
                              </p>
                            )}
                          </div>

                          <span className="shrink-0 text-xs text-slate-600">
                            {lesson.estimatedMinutes} min
                          </span>
                        </div>
                      ))}
                    </div>
                  </div>
                </div>
              </article>
            ))}
          </div>
        </section>
      </div>
    </main>
  );
}
