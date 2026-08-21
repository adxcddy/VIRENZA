"use client";

import { useEffect, useState } from "react";
import Link from "next/link";
import { apiFetch } from "@/lib/api";

type Course = {
  id: string;
  title: string;
  slug: string;
  description?: string | null;
  difficulty: string;
  estimatedHours: number;
  isFree: boolean;
  subjectId?: string | null;
  learningLevelId?: string | null;
};

export default function CoursesPage() {
  const [courses, setCourses] = useState<Course[]>([]);
  const [search, setSearch] = useState("");
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  async function loadCourses(value = "") {
    setLoading(true);
    setError("");

    try {
      const query = value.trim()
        ? `?search=${encodeURIComponent(value.trim())}`
        : "";

      const data = await apiFetch<Course[]>(
        `/learning/courses${query}`,
      );

      setCourses(data || []);
    } catch (err) {
      setError(
        err instanceof Error
          ? err.message
          : "Unable to load courses.",
      );
    } finally {
      setLoading(false);
    }
  }

  useEffect(() => {
    loadCourses();
  }, []);

  function submitSearch(event: React.FormEvent) {
    event.preventDefault();
    loadCourses(search);
  }

  return (
    <main className="min-h-screen bg-slate-950 text-white">
      <header className="border-b border-white/10">
        <div className="mx-auto flex max-w-7xl items-center justify-between px-5 py-5 sm:px-6">
          <Link href="/" className="flex items-center gap-3">
            <div className="flex h-10 w-10 items-center justify-center rounded-xl bg-white font-black text-slate-950">
              V
            </div>
            <div className="font-black">VIRENZA</div>
          </Link>

          <div className="flex gap-3">
            <Link
              href="/dashboard"
              className="rounded-lg px-4 py-2 text-sm font-semibold text-slate-400 hover:text-white"
            >
              Dashboard
            </Link>
            <Link
              href="/login"
              className="rounded-lg border border-white/10 px-4 py-2 text-sm font-semibold hover:bg-white/5"
            >
              Sign in
            </Link>
          </div>
        </div>
      </header>

      <div className="mx-auto max-w-7xl px-5 py-10 sm:px-6">
        <div className="max-w-3xl">
          <p className="text-xs font-bold uppercase tracking-[0.25em] text-slate-500">
            VIRENZA catalogue
          </p>

          <h1 className="mt-3 text-4xl font-black tracking-tight sm:text-5xl">
            Learn skills that move you forward.
          </h1>

          <p className="mt-4 text-slate-400">
            Explore published courses and find your next learning challenge.
          </p>
        </div>

        <form onSubmit={submitSearch} className="mt-8 flex max-w-2xl gap-3">
          <input
            value={search}
            onChange={(event) => setSearch(event.target.value)}
            placeholder="Search courses..."
            className="min-w-0 flex-1 rounded-xl border border-white/10 bg-white/[0.04] px-4 py-3 text-sm outline-none placeholder:text-slate-600 focus:border-white/30"
          />

          <button
            type="submit"
            className="rounded-xl bg-white px-5 py-3 text-sm font-bold text-slate-950 hover:bg-slate-200"
          >
            Search
          </button>
        </form>

        {error && (
          <div className="mt-7 rounded-2xl border border-red-500/20 bg-red-500/10 p-4 text-sm text-red-300">
            {error}
          </div>
        )}

        {loading ? (
          <div className="py-20 text-center text-sm text-slate-500">
            Loading courses...
          </div>
        ) : courses.length === 0 ? (
          <div className="mt-10 rounded-3xl border border-dashed border-white/10 p-12 text-center">
            <div className="text-4xl">🔎</div>
            <h2 className="mt-4 text-xl font-black">No courses found</h2>
            <p className="mt-2 text-sm text-slate-500">
              Try another search term.
            </p>
          </div>
        ) : (
          <div className="mt-10 grid gap-5 sm:grid-cols-2 lg:grid-cols-3">
            {courses.map((course) => (
              <Link
                key={course.id}
                href={`/courses/${course.id}`}
                className="group overflow-hidden rounded-3xl border border-white/10 bg-white/[0.035] transition hover:-translate-y-1 hover:border-white/20"
              >
                <div className="h-36 bg-gradient-to-br from-white/10 via-white/[0.03] to-transparent p-5">
                  <div className="flex justify-between">
                    <span className="rounded-full border border-white/10 bg-black/20 px-3 py-1 text-xs font-bold text-slate-300">
                      {course.difficulty}
                    </span>

                    {course.isFree && (
                      <span className="rounded-full bg-emerald-500/10 px-3 py-1 text-xs font-bold text-emerald-400">
                        Free
                      </span>
                    )}
                  </div>
                </div>

                <div className="p-6">
                  <h2 className="text-xl font-black group-hover:text-slate-300">
                    {course.title}
                  </h2>

                  <p className="mt-3 line-clamp-3 text-sm leading-6 text-slate-500">
                    {course.description || "Start learning with VIRENZA."}
                  </p>

                  <div className="mt-6 flex items-center justify-between border-t border-white/10 pt-4 text-xs text-slate-500">
                    <span>{course.estimatedHours} hours</span>
                    <span className="font-bold text-white">
                      View course →
                    </span>
                  </div>
                </div>
              </Link>
            ))}
          </div>
        )}
      </div>
    </main>
  );
}
