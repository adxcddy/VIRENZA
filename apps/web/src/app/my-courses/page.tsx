"use client";

import { useEffect, useState } from "react";
import Link from "next/link";
import { apiFetch } from "@/lib/api";
import { getStoredUser, type AuthUser } from "@/lib/auth";

type Course = {
  title: string;
  slug: string;
  difficulty: string;
  estimatedHours: number;
};

type Enrollment = {
  enrollmentId: string;
  courseId: string;
  enrolledAt: string;
  progressPercent: number;
  isCompleted: boolean;
  completedAt: string | null;
  course: Course | null;
};

export default function MyCoursesPage() {
  const [user, setUser] = useState<AuthUser | null>(null);
  const [courses, setCourses] = useState<Enrollment[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    setUser(getStoredUser());

    apiFetch<Enrollment[]>("/learning/my-courses")
      .then(setCourses)
      .catch((err) => {
        setError(
          err instanceof Error
            ? err.message
            : "Unable to load your courses.",
        );
      })
      .finally(() => {
        setLoading(false);
      });
  }, []);

  if (loading) {
    return (
      <main className="min-h-screen bg-slate-950 text-white">
        <div className="mx-auto max-w-7xl px-6 py-20">
          <div className="animate-pulse text-slate-500">
            Loading your courses...
          </div>
        </div>
      </main>
    );
  }

  return (
    <main className="min-h-screen bg-slate-950 text-white">
      <header className="border-b border-white/10">
        <div className="mx-auto flex max-w-7xl items-center justify-between px-6 py-5">
          <Link href="/" className="flex items-center gap-3">
            <div className="flex h-10 w-10 items-center justify-center rounded-xl bg-white font-black text-slate-950">
              V
            </div>

            <div>
              <div className="font-black">VIRENZA</div>
              <div className="text-[9px] uppercase tracking-[0.2em] text-slate-500">
                Learn Without Limits
              </div>
            </div>
          </Link>

          <div className="flex items-center gap-4">
            {user && (
              <span className="hidden text-sm text-slate-400 sm:block">
                {user.firstName} {user.lastName}
              </span>
            )}

            <Link
              href="/dashboard"
              className="rounded-lg border border-white/10 px-4 py-2 text-sm font-semibold text-slate-300 hover:bg-white/5 hover:text-white"
            >
              Dashboard
            </Link>
          </div>
        </div>
      </header>

      <div className="mx-auto max-w-7xl px-6 py-12">
        <div className="flex flex-col justify-between gap-5 md:flex-row md:items-end">
          <div>
            <p className="text-sm font-semibold uppercase tracking-[0.2em] text-slate-500">
              Learning workspace
            </p>

            <h1 className="mt-2 text-4xl font-black">
              My Courses
            </h1>

            <p className="mt-3 max-w-2xl text-slate-400">
              Continue learning, track your progress and complete your
              courses.
            </p>
          </div>

          <Link
            href="/courses"
            className="rounded-xl bg-white px-5 py-3 text-center text-sm font-bold text-slate-950 hover:bg-slate-200"
          >
            Browse Courses →
          </Link>
        </div>

        {error && (
          <div className="mt-8 rounded-2xl border border-red-500/20 bg-red-500/10 p-5 text-sm text-red-300">
            {error}
          </div>
        )}

        {!error && courses.length === 0 && (
          <section className="mt-10 rounded-3xl border border-white/10 bg-white/[0.04] px-6 py-16 text-center">
            <div className="text-5xl">📚</div>

            <h2 className="mt-5 text-2xl font-black">
              You haven't enrolled in a course yet.
            </h2>

            <p className="mx-auto mt-3 max-w-lg text-slate-500">
              Explore the VIRENZA catalogue and choose a course to begin
              your learning journey.
            </p>

            <Link
              href="/courses"
              className="mt-7 inline-flex rounded-xl bg-white px-6 py-3 font-bold text-slate-950 hover:bg-slate-200"
            >
              Explore Courses
            </Link>
          </section>
        )}

        {courses.length > 0 && (
          <div className="mt-10 grid gap-6 md:grid-cols-2 lg:grid-cols-3">
            {courses.map((enrollment) => {
              const course = enrollment.course;

              if (!course) return null;

              const progress = Math.min(
                100,
                Math.max(0, Number(enrollment.progressPercent)),
              );

              return (
                <article
                  key={enrollment.enrollmentId}
                  className="overflow-hidden rounded-3xl border border-white/10 bg-white/[0.04] transition hover:-translate-y-1 hover:border-white/20"
                >
                  <div className="h-2 bg-white/10">
                    <div
                      className="h-full bg-white transition-all"
                      style={{ width: `${progress}%` }}
                    />
                  </div>

                  <div className="p-6">
                    <div className="flex items-center justify-between gap-3">
                      <span className="rounded-full border border-white/10 px-3 py-1 text-xs font-semibold text-slate-400">
                        {course.difficulty}
                      </span>

                      {enrollment.isCompleted ? (
                        <span className="text-xs font-bold text-emerald-400">
                          ✓ Completed
                        </span>
                      ) : (
                        <span className="text-xs font-semibold text-slate-500">
                          {progress}% complete
                        </span>
                      )}
                    </div>

                    <h2 className="mt-5 text-xl font-black">
                      {course.title}
                    </h2>

                    <p className="mt-2 text-sm text-slate-500">
                      {course.estimatedHours} hours estimated
                    </p>

                    <div className="mt-6">
                      <div className="mb-2 flex justify-between text-xs">
                        <span className="text-slate-500">
                          Progress
                        </span>

                        <span className="font-bold text-white">
                          {progress}%
                        </span>
                      </div>

                      <div className="h-2 overflow-hidden rounded-full bg-white/10">
                        <div
                          className="h-full rounded-full bg-white"
                          style={{ width: `${progress}%` }}
                        />
                      </div>
                    </div>

                    <Link
                      href={`/courses/${enrollment.courseId}`}
                      className="mt-7 block rounded-xl bg-white px-5 py-3 text-center text-sm font-bold text-slate-950 hover:bg-slate-200"
                    >
                      {enrollment.isCompleted
                        ? "Review Course"
                        : progress > 0
                          ? "Continue Learning"
                          : "Start Course"}
                    </Link>
                  </div>
                </article>
              );
            })}
          </div>
        )}
      </div>
    </main>
  );
}
