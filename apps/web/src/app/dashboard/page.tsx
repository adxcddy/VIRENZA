"use client";

import { useEffect, useState } from "react";
import { useRouter } from "next/navigation";
import {
  authHeaders,
  clearAuth,
  getStoredUser,
  type AuthUser,
} from "@/lib/auth";

export default function DashboardPage() {
  const router = useRouter();

  const [user, setUser] = useState<AuthUser | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const stored = getStoredUser();

    if (!stored) {
      router.replace("/login");
      return;
    }

    setUser(stored);

    fetch("/api/backend/auth/me", {
      headers: authHeaders(),
    })
      .then((response) => {
        if (response.status === 401) {
          clearAuth();
          router.replace("/login");
        }
      })
      .catch(() => {
        // Keep the locally stored identity if the API is temporarily unavailable.
      })
      .finally(() => {
        setLoading(false);
      });
  }, [router]);

  function logout() {
    clearAuth();
    router.replace("/login");
  }

  if (loading || !user) {
    return (
      <main className="flex min-h-screen items-center justify-center bg-slate-950 text-white">
        <div className="text-sm text-slate-400">Loading your dashboard...</div>
      </main>
    );
  }

  return (
    <main className="min-h-screen bg-slate-950 text-white">
      <header className="border-b border-white/10">
        <div className="mx-auto flex max-w-7xl items-center justify-between px-6 py-5">
          <a href="/" className="flex items-center gap-3">
            <div className="flex h-10 w-10 items-center justify-center rounded-xl bg-white font-black text-slate-950">
              V
            </div>

            <div>
              <div className="font-black">VIRENZA</div>
              <div className="text-[9px] uppercase tracking-[0.2em] text-slate-500">
                Student
              </div>
            </div>
          </a>

          <div className="flex items-center gap-4">
            <div className="hidden text-right sm:block">
              <div className="text-sm font-bold">
                {user.firstName} {user.lastName}
              </div>
              <div className="text-xs text-slate-500">{user.role}</div>
            </div>

            <button
              onClick={logout}
              className="rounded-lg border border-white/10 px-4 py-2 text-sm font-semibold text-slate-300 transition hover:bg-white/5 hover:text-white"
            >
              Logout
            </button>
          </div>
        </div>
      </header>

      <div className="mx-auto max-w-7xl px-6 py-10">
        <div>
          <p className="text-sm font-semibold uppercase tracking-[0.2em] text-slate-500">
            Student dashboard
          </p>

          <h1 className="mt-2 text-4xl font-black">
            Welcome, {user.firstName}.
          </h1>

          <p className="mt-3 max-w-2xl text-slate-400">
            Your VIRENZA learning journey starts here. Courses, lessons,
            assessments and certificates will appear in this workspace.
          </p>
        </div>

        <div className="mt-10 grid gap-5 sm:grid-cols-2 lg:grid-cols-4">
          {[
            ["📚", "My Courses", "Explore your enrolled courses."],
            ["▶️", "Continue Learning", "Resume your latest lesson."],
            ["📝", "Assessments", "Take quizzes and view results."],
            ["🏆", "Certificates", "View and verify achievements."],
          ].map(([icon, title, description]) => (
            <article
              key={title}
              className="rounded-2xl border border-white/10 bg-white/[0.04] p-6 transition hover:-translate-y-1 hover:border-white/20"
            >
              <div className="text-3xl">{icon}</div>
              <h2 className="mt-5 font-bold">{title}</h2>
              <p className="mt-2 text-sm leading-6 text-slate-500">
                {description}
              </p>
            </article>
          ))}
        </div>

        <section className="mt-8 rounded-3xl border border-white/10 bg-white/[0.04] p-7">
          <div className="flex flex-col justify-between gap-4 sm:flex-row sm:items-center">
            <div>
              <p className="text-sm font-semibold text-slate-500">
                Learning progress
              </p>
              <h2 className="mt-1 text-2xl font-black">
                Your courses will appear here
              </h2>
            </div>

            <a
              href="/courses"
              className="rounded-xl bg-white px-5 py-3 text-center text-sm font-bold text-slate-950 hover:bg-slate-200"
            >
              Browse courses
            </a>
          </div>

          <div className="mt-7 h-2 overflow-hidden rounded-full bg-white/10">
            <div className="h-full w-0 rounded-full bg-white" />
          </div>

          <p className="mt-3 text-xs text-slate-600">
            Course catalogue integration is the next learning milestone.
          </p>
        </section>
      </div>
    </main>
  );
}
