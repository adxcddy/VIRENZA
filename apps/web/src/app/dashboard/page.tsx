"use client";

import { useEffect, useState } from "react";
import Link from "next/link";
import { getCurrentUser, getMyCourses, type MyCourse } from "@/lib/api";
import { logout, type AuthUser } from "@/lib/auth";

export default function DashboardPage() {
  const [user, setUser] = useState<AuthUser | null>(null);
  const [courses, setCourses] = useState<MyCourse[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    async function loadDashboard() {
      try {
        const [currentUser, myCourses] = await Promise.all([
          getCurrentUser(),
          getMyCourses(),
        ]);

        setUser(currentUser);
        setCourses(myCourses);
      } catch (err) {
        setError(
          err instanceof Error
            ? err.message
            : "Unable to load your dashboard.",
        );
      } finally {
        setLoading(false);
      }
    }

    loadDashboard();
  }, []);

  if (loading) {
    return (
      <main className="min-h-screen bg-slate-950 text-white">
        <div className="mx-auto max-w-7xl px-6 py-12">
          <div className="animate-pulse">
            <div className="h-8 w-64 rounded bg-white/10" />
            <div className="mt-3 h-4 w-96 rounded bg-white/10" />
            <div className="mt-10 grid gap-5 md:grid-cols-3">
              {[1, 2, 3].map((item) => (
                <div
                  key={item}
                  className="h-48 rounded-2xl bg-white/5"
                />
              ))}
            </div>
          </div>
        </div>
      </main>
    );
  }

  return (
    <main className="min-h-screen bg-slate-950 text-white">
      <header className="border-b border-white/10 bg-slate-950/90">
        <div className="mx-auto flex max-w-7xl items-center justify-between px-6 py-5">
          <Link href="/" className="flex items-center gap-3">
            <div className="flex h-10 w-10 items-center justify-center rounded-xl bg-white font-black text-slate-950">
              V
            </div>

            <div>
              <div className="font-black tracking-wide">VIRENZA</div>
              <div className="text-[9px] font-semibold uppercase tracking-[0.2em] text-slate-500">
                Learn Without Limits
              </div>
            </div>
          </Link>

          <div className="flex items-center gap-4">
            {user && (
              <div className="hidden text-right sm:block">
                <div className="text-sm font-bold">
                  {user.firstName} {user.lastName}
                </div>
                <div className="text-xs text-slate-500">
                  {user.role}
                </div>
              </div>
            )}

            <button
              onClick={logout}
              className="rounded-xl border border-white/10 px-4 py-2 text-sm font-semibold text-slate-300 transition hover:bg-white/10 hover:text-white"
            >
              Sign out
            </button>
          </div>
        </div>
      </header>

      <div className="mx-auto max-w-7xl px-6 py-10">
        {error && (
          <div className="mb-8 rounded-2xl border border-red-500/20 bg-red-500/10 px-5 py-4 text-sm text-red-300">
            {error}
          </div>
        )}

        <section>
          <p className="text-sm font-semibold uppercase tracking-[0.2em] text-slate-500">
            Student Dashboard
          </p>

          <h1 className="mt-2 text-4xl font-black tracking-tight">
            Welcome back{user ? `, ${user.firstName}` : ""}.
          </h1>

          <p className="mt-3 max-w-2xl text-slate-400">
            Continue learning, track your progress, and explore new
            courses.
          </p>
        </section>

        <section className="mt-10 grid gap-5 sm:grid-cols-2 lg:grid-cols-3">
          <div className="rounded-2xl border border-white/10 bg-white/[0.04] p-6">
            <div className="text-sm text-slate-500">
              Enrolled courses
            </div>
            <div className="mt-2 text-3xl font-black">
              {courses.length}
            </div>
          </div>

          <div className="rounded-2xl border border-white/10 bg-white/[0.04] p-6">
            <div className="text-sm text-slate-500">
              Completed courses
            </div>
            <div className="mt-2 text-3xl font-black">
              {courses.filter((course) => course.isCompleted).length}
            </div>
          </div>

          <div className="rounded-2xl border border-white/10 bg-white/[0.04] p-6">
            <div className="text-sm text-slate-500">
              Average progress
            </div>
            <div className="mt-2 text-3xl font-black">
              {courses.length
                ? Math.round(
                    courses.reduce(
                      (sum, course) =>
                        sum + Number(course.progressPercent || 0),
                      0,
                    ) / courses.length,
                  )
                : 0}
              %
            </div>
          </div>
        </section>

        <section className="mt-12">
          <div className="flex items-end justify-between gap-4">
            <div>
              <h2 className="text-2xl font-black">
                Continue learning
              </h2>
              <p className="mt-1 text-sm text-slate-500">
                Pick up where you left off.
              </p>
            </div>

            <Link
              href="/courses"
              className="text-sm font-bold text-white hover:underline"
            >
              Browse courses →
            </Link>
          </div>

          {courses.length === 0 ? (
            <div className="mt-6 rounded-2xl border border-dashed border-white/10 bg-white/[0.02] p-10 text-center">
              <h3 className="text-xl font-bold">
                You haven't enrolled in a course yet.
              </h3>

              <p className="mt-2 text-sm text-slate-500">
                Explore VIRENZA courses and start learning.
              </p>

              <Link
                href="/courses"
                className="mt-6 inline-flex rounded-xl bg-white px-5 py-3 text-sm font-bold text-slate-950 transition hover:bg-slate-200"
              >
                Explore courses
              </Link>
            </div>
          ) : (
            <div className="mt-6 grid gap-5 lg:grid-cols-2">
              {courses.map((enrollment) => (
                <article
                  key={enrollment.enrollmentId}
                  className="rounded-2xl border border-white/10 bg-white/[0.04] p-6"
                >
                  <div className="flex items-start justify-between gap-4">
                    <div>
                      <h3 className="text-xl font-black">
                        {enrollment.course?.title ?? "Course"}
                      </h3>

                      <p className="mt-2 text-sm text-slate-500">
                        {enrollment.course?.difficulty ?? "Learning"}{" "}
                        ·{" "}
                        {enrollment.course?.estimatedHours ?? 0} hours
                      </p>
                    </div>

                    <span className="rounded-full border border-white/10 px-3 py-1 text-xs font-bold text-slate-400">
                      {enrollment.isCompleted
                        ? "Completed"
                        : `${Number(enrollment.progressPercent).toFixed(0)}%`}
                    </span>
                  </div>

                  <div className="mt-6">
                    <div className="mb-2 flex justify-between text-xs text-slate-500">
                      <span>Progress</span>
                      <span>
                        {Number(
                          enrollment.progressPercent,
                        ).toFixed(0)}
                        %
                      </span>
                    </div>

                    <div className="h-2 overflow-hidden rounded-full bg-white/10">
                      <div
                        className="h-full rounded-full bg-white transition-all"
                        style={{
                          width: `${Math.min(
                            100,
                            Math.max(
                              0,
                              Number(
                                enrollment.progressPercent,
                              ),
                            ),
                          )}%`,
                        }}
                      />
                    </div>
                  </div>

                  <Link
                    href={`/courses/${enrollment.courseId}`}
                    className="mt-6 inline-flex w-full items-center justify-center rounded-xl bg-white px-5 py-3 text-sm font-bold text-slate-950 transition hover:bg-slate-200"
                  >
                    {enrollment.isCompleted
                      ? "Review course"
                      : "Continue learning"}
                  </Link>
                </article>
              ))}
            </div>
          )}
        </section>
      </div>
    </main>
  );
}
