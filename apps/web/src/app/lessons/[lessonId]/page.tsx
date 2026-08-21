"use client";

import { useEffect, useState } from "react";
import Link from "next/link";
import { useParams } from "next/navigation";
import {
  getLesson,
  type LessonDetails,
} from "@/lib/api";

export default function LessonPage() {
  const params = useParams();
  const lessonId = String(params.lessonId);

  const [lesson, setLesson] = useState<LessonDetails | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    async function load() {
      try {
        const data = await getLesson(lessonId);
        setLesson(data);
      } catch (err) {
        setError(
          err instanceof Error
            ? err.message
            : "Unable to load lesson.",
        );
      } finally {
        setLoading(false);
      }
    }

    load();
  }, [lessonId]);

  if (loading) {
    return (
      <main className="min-h-screen bg-slate-950 p-10 text-white">
        <div className="mx-auto max-w-4xl animate-pulse">
          <div className="h-8 w-1/2 rounded bg-white/10" />
          <div className="mt-5 h-40 rounded bg-white/5" />
        </div>
      </main>
    );
  }

  if (error || !lesson) {
    return (
      <main className="min-h-screen bg-slate-950 p-10 text-white">
        <div className="mx-auto max-w-4xl">
          <div className="rounded-2xl border border-red-500/20 bg-red-500/10 p-6 text-red-300">
            {error || "Lesson not found."}
          </div>

          <Link
            href="/dashboard"
            className="mt-6 inline-block font-bold underline"
          >
            Back to dashboard
          </Link>
        </div>
      </main>
    );
  }

  return (
    <main className="min-h-screen bg-slate-950 text-white">
      <header className="border-b border-white/10">
        <div className="mx-auto max-w-4xl px-6 py-5">
          <Link
            href={`/courses/${lesson.module.courseId}`}
            className="text-sm font-semibold text-slate-400 hover:text-white"
          >
            ← Back to course
          </Link>
        </div>
      </header>

      <article className="mx-auto max-w-4xl px-6 py-12">
        <div className="text-xs font-bold uppercase tracking-[0.2em] text-slate-600">
          {lesson.module.title}
        </div>

        <h1 className="mt-3 text-4xl font-black">
          {lesson.title}
        </h1>

        {lesson.summary && (
          <p className="mt-4 text-lg leading-8 text-slate-400">
            {lesson.summary}
          </p>
        )}

        <div className="mt-8 flex flex-wrap gap-3 text-xs font-semibold text-slate-500">
          <span className="rounded-full border border-white/10 px-3 py-1">
            {lesson.contentType}
          </span>

          <span className="rounded-full border border-white/10 px-3 py-1">
            {lesson.estimatedMinutes} minutes
          </span>

          {lesson.progress?.isCompleted && (
            <span className="rounded-full border border-emerald-500/20 bg-emerald-500/10 px-3 py-1 text-emerald-400">
              Completed
            </span>
          )}
        </div>

        <div className="mt-10 rounded-3xl border border-white/10 bg-white/[0.04] p-8">
          <div className="whitespace-pre-wrap text-base leading-8 text-slate-300">
            {lesson.content}
          </div>
        </div>

        {lesson.progress && (
          <div className="mt-8 rounded-2xl border border-white/10 bg-white/[0.03] p-5">
            <div className="flex justify-between text-sm">
              <span className="text-slate-500">
                Lesson progress
              </span>

              <span className="font-bold">
                {lesson.progress.progressPercent}%
              </span>
            </div>

            <div className="mt-3 h-2 overflow-hidden rounded-full bg-white/10">
              <div
                className="h-full rounded-full bg-white"
                style={{
                  width: `${Math.min(
                    100,
                    Math.max(
                      0,
                      Number(lesson.progress.progressPercent),
                    ),
                  )}%`,
                }}
              />
            </div>
          </div>
        )}

        <div className="mt-10 flex items-center justify-between gap-4">
          {lesson.previousLesson ? (
            <Link
              href={`/lessons/${lesson.previousLesson.id}`}
              className="rounded-xl border border-white/10 px-5 py-3 text-sm font-bold hover:bg-white/5"
            >
              ← Previous
            </Link>
          ) : (
            <div />
          )}

          {lesson.nextLesson ? (
            <Link
              href={`/lessons/${lesson.nextLesson.id}`}
              className="rounded-xl bg-white px-5 py-3 text-sm font-bold text-slate-950 hover:bg-slate-200"
            >
              Next lesson →
            </Link>
          ) : (
            <Link
              href={`/courses/${lesson.module.courseId}`}
              className="rounded-xl bg-white px-5 py-3 text-sm font-bold text-slate-950 hover:bg-slate-200"
            >
              Finish course →
            </Link>
          )}
        </div>
      </article>
    </main>
  );
}
