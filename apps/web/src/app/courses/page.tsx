"use client";

import { useEffect, useState } from "react";
import Link from "next/link";
import {
  getCourses,
  type LearningCourse,
} from "@/lib/api";

export default function CoursesPage() {
  const [courses, setCourses] = useState<LearningCourse[]>([]);
  const [search, setSearch] = useState("");
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  async function loadCourses(value = "") {
    try {
      setLoading(true);
      setError("");

      const data = await getCourses({
        search: value,
      });

      setCourses(data);
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

  return (
    <main className="min-h-screen bg-slate-950 text-white">
      <header className="border-b border-white/10">
        <div className="mx-auto flex max-w-7xl items-center justify-between px-6 py-5">
          <Link href="/dashboard" className="font-black">
            ← Dashboard
          </Link>

          <div className="font-black tracking-wide">
            VIRENZA
          </div>
        </div>
      </header>

      <div className="mx-auto max-w-7xl px-6 py-10">
        <div>
          <p className="text-sm font-semibold uppercase tracking-[0.2em] text-slate-500">
            Learning Library
          </p>

          <h1 className="mt-2 text-4xl font-black">
            Explore courses
          </h1>

          <p className="mt-3 text-slate-400">
            Discover published VIRENZA learning content.
          </p>
        </div>

        <form
          className="mt-8 flex gap-3"
          onSubmit={(event) => {
            event.preventDefault();
            loadCourses(search);
          }}
        >
          <input
            value={search}
            onChange={(event) => setSearch(event.target.value)}
            placeholder="Search courses..."
            className="flex-1 rounded-xl border border-white/10 bg-white/[0.04] px-4 py-3 text-white outline-none placeholder:text-slate-600 focus:border-white/30"
          />

          <button
            type="submit"
            className="rounded-xl bg-white px-6 py-3 font-bold text-slate-950 hover:bg-slate-200"
          >
            Search
          </button>
        </form>

        {error && (
          <div className="mt-6 rounded-xl border border-red-500/20 bg-red-500/10 px-4 py-3 text-sm text-red-300">
            {error}
          </div>
        )}

        {loading ? (
          <div className="mt-10 grid gap-5 md:grid-cols-2 lg:grid-cols-3">
            {[1, 2, 3].map((item) => (
              <div
                key={item}
                className="h-64 animate-pulse rounded-2xl bg-white/5"
              />
            ))}
          </div>
        ) : courses.length === 0 ? (
          <div className="mt-10 rounded-2xl border border-white/10 p-10 text-center text-slate-500">
            No courses found.
          </div>
        ) : (
          <div className="mt-10 grid gap-5 md:grid-cols-2 lg:grid-cols-3">
            {courses.map((course) => (
              <article
                key={course.id}
                className="flex flex-col rounded-2xl border border-white/10 bg-white/[0.04] p-6"
              >
                <div className="flex items-center justify-between">
                  <span className="rounded-full border border-white/10 px-3 py-1 text-xs font-bold text-slate-400">
                    {course.difficulty}
                  </span>

                  <span className="text-xs font-semibold text-slate-500">
                    {course.estimatedHours}h
                  </span>
                </div>

                <h2 className="mt-5 text-xl font-black">
                  {course.title}
                </h2>

                <p className="mt-3 flex-1 text-sm leading-6 text-slate-500">
                  {course.description ||
                    "Start learning with this VIRENZA course."}
                </p>

                <Link
                  href={`/courses/${course.id}`}
                  className="mt-6 rounded-xl bg-white px-5 py-3 text-center text-sm font-bold text-slate-950 hover:bg-slate-200"
                >
                  View course
                </Link>
              </article>
            ))}
          </div>
        )}
      </div>
    </main>
  );
}
