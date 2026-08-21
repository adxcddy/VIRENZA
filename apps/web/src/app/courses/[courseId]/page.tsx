"use client";

import { useEffect, useState } from "react";
import Link from "next/link";
import { useParams } from "next/navigation";
import {
  enrollCourse,
  getCourse,
  type CourseDetails,
} from "@/lib/api";

export default function CoursePage() {
  const params = useParams();
  const courseId = String(params.courseId);

  const [course, setCourse] = useState<CourseDetails | null>(null);
  const [loading, setLoading] = useState(true);
  const [enrolling, setEnrolling] = useState(false);
  const [error, setError] = useState("");
  const [message, setMessage] = useState("");

  useEffect(() => {
    async function load() {
      try {
        const data = await getCourse(courseId);
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

    load();
  }, [courseId]);

  async function handleEnroll() {
    try {
      setEnrolling(true);
      setError("");
      setMessage("");

      await enrollCourse(courseId);

      setMessage(
        "You are now enrolled in this course.",
      );
    } catch (err) {
      setError(
        err instanceof Error
          ? err.message
          : "Unable to enroll.",
      );
    } finally {
      setEnrolling(false);
    }
  }

  if (loading) {
    return (
      <main className="min-h-screen bg-slate-950 p-10 text-white">
        <div className="mx-auto max-w-5xl">
          <div className="h-10 w-2/3 animate-pulse rounded bg-white/10" />
          <div className="mt-4 h-5 w-1/2 animate-pulse rounded bg-white/10" />
        </div>
      </main>
    );
  }

  if (!course) {
    return (
      <main className="min-h-screen bg-slate-950 p-10 text-white">
        <div className="mx-auto max-w-5xl">
          <p className="text-red-300">
            {error || "Course not found."}
          </p>
          <Link
            href="/courses"
            className="mt-5 inline-block text-sm font-bold underline"
          >
            Back to courses
          </Link>
        </div>
      </main>
    );
  }

  return (
    <main className="min-h-screen bg-slate-950 text-white">
      <header className="border-b border-white/10">
        <div className="mx-auto max-w-5xl px-6 py-5">
          <Link
            href="/courses"
            className="text-sm font-semibold text-slate-400 hover:text-white"
          >
            ← All courses
          </Link>
        </div>
      </header>

      <div className="mx-auto max-w-5xl px-6 py-12">
        {error && (
          <div className="mb-6 rounded-xl border border-red-500/20 bg-red-500/10 px-4 py-3 text-sm text-red-300">
            {error}
          </div>
        )}

        {message && (
          <div className="mb-6 rounded-xl border border-emerald-500/20 bg-emerald-500/10 px-4 py-3 text-sm text-emerald-300">
            {message}
          </div>
        )}

        <div className="rounded-3xl border border-white/10 bg-white/[0.04] p-8">
          <div className="flex flex-wrap gap-3">
            <span className="rounded-full border border-white/10 px-3 py-1 text-xs font-bold text-slate-400">
              {course.difficulty}
            </span>

            <span className="rounded-full border border-white/10 px-3 py-1 text-xs font-bold text-slate-400">
              {course.estimatedHours} hours
            </span>

            <span className="rounded-full border border-white/10 px-3 py-1 text-xs font-bold text-slate-400">
              {course.isFree ? "Free" : "Premium"}
            </span>
          </div>

          <h1 className="mt-6 text-4xl font-black">
            {course.title}
          </h1>

          <p className="mt-4 max-w-3xl leading-7 text-slate-400">
            {course.description ||
              "Learn the key concepts and skills in this VIRENZA course."}
          </p>

          <button
            onClick={handleEnroll}
            disabled={enrolling}
            className="mt-8 rounded-xl bg-white px-6 py-3 font-bold text-slate-950 transition hover:bg-slate-200 disabled:cursor-not-allowed disabled:opacity-50"
          >
            {enrolling ? "Enrolling..." : "Enroll in course"}
          </button>
        </div>

        <section className="mt-10">
          <h2 className="text-2xl font-black">
            Course curriculum
          </h2>

          <div className="mt-5 space-y-4">
            {course.modules.map((module) => (
              <div
                key={module.id}
                className="rounded-2xl border border-white/10 bg-white/[0.03] p-6"
              >
                <div className="flex items-start justify-between gap-4">
                  <div>
                    <div className="text-xs font-bold uppercase tracking-widest text-slate-600">
                      Module {module.order}
                    </div>

                    <h3 className="mt-1 text-lg font-black">
                      {module.title}
                    </h3>

                    {module.description && (
                      <p className="mt-2 text-sm text-slate-500">
                        {module.description}
                      </p>
                    )}
                  </div>

                  <span className="text-xs text-slate-600">
                    {module.lessons.length} lessons
                  </span>
                </div>

                <div className="mt-5 space-y-2">
                  {module.lessons.map((lesson) => (
                    <Link
                      key={lesson.id}
                      href={`/lessons/${lesson.id}`}
                      className="flex items-center justify-between rounded-xl border border-white/5 bg-black/20 px-4 py-3 transition hover:border-white/20 hover:bg-white/[0.05]"
                    >
                      <div>
                        <div className="text-sm font-semibold">
                          {lesson.order}. {lesson.title}
                        </div>

                        <div className="mt-1 text-xs text-slate-600">
                          {lesson.estimatedMinutes} minutes ·{" "}
                          {lesson.contentType}
                        </div>
                      </div>

                      <span className="text-slate-600">
                        →
                      </span>
                    </Link>
                  ))}
                </div>
              </div>
            ))}
          </div>
        </section>
      </div>
    </main>
  );
}
