"use client";

import { useEffect, useMemo, useState } from "react";
import Link from "next/link";
import { useParams, useRouter } from "next/navigation";
import { apiFetch } from "@/lib/api";
import { getToken } from "@/lib/auth";

type LessonNavigation = {
  id: string;
  moduleId: string;
  title: string;
  order: number;
};

type LessonData = {
  id: string;
  moduleId: string;
  title: string;
  summary?: string | null;
  content: string;
  contentType: string;
  estimatedMinutes: number;
  order: number;

  module: {
    id: string;
    courseId: string;
    title: string;
    order: number;
  };

  progress: {
    progressPercent: number;
    timeSpentSeconds: number;
    isCompleted: boolean;
    completedAt?: string | null;
  };

  previousLesson?: LessonNavigation | null;
  nextLesson?: LessonNavigation | null;
};

function formatTime(seconds: number) {
  const minutes = Math.floor(seconds / 60);
  const remaining = seconds % 60;

  return `${minutes}m ${remaining.toString().padStart(2, "0")}s`;
}

export default function LessonPlayerPage() {
  const params = useParams();
  const router = useRouter();

  const courseId = params.courseId as string;
  const lessonId = params.lessonId as string;

  const [lesson, setLesson] = useState<LessonData | null>(null);
  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState("");
  const [secondsSpent, setSecondsSpent] = useState(0);
  const [completed, setCompleted] = useState(false);

  const progressPercent = useMemo(() => {
    if (completed) return 100;

    if (!lesson) return 0;

    return Math.max(
      lesson.progress.progressPercent,
      Math.min(95, Math.round((secondsSpent / 60) * 10)),
    );
  }, [completed, lesson, secondsSpent]);

  useEffect(() => {
    if (!getToken()) {
      router.replace(`/login?returnTo=/courses/${courseId}/lessons/${lessonId}`);
      return;
    }

    async function loadLesson() {
      try {
        setLoading(true);
        setError("");

        const data = await apiFetch<LessonData>(
          `/learning/lessons/${lessonId}`,
        );

        setLesson(data);
        setSecondsSpent(data.progress.timeSpentSeconds || 0);
        setCompleted(data.progress.isCompleted);
      } catch (err) {
        setError(
          err instanceof Error
            ? err.message
            : "Unable to load this lesson.",
        );
      } finally {
        setLoading(false);
      }
    }

    if (lessonId) {
      loadLesson();
    }
  }, [courseId, lessonId, router]);

  useEffect(() => {
    if (!lesson || completed) return;

    const timer = window.setInterval(() => {
      setSecondsSpent((current) => current + 1);
    }, 1000);

    return () => window.clearInterval(timer);
  }, [lesson, completed]);

  useEffect(() => {
    if (!lesson || completed || secondsSpent === 0) return;

    if (secondsSpent % 15 !== 0) return;

    saveProgress(false);
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [secondsSpent]);

  async function saveProgress(markComplete: boolean) {
    if (!lesson || saving) return;

    setSaving(true);
    setError("");

    try {
      await apiFetch(
        `/learning/lessons/${lesson.id}/progress`,
        {
          method: "POST",
          body: JSON.stringify({
            progressPercent: markComplete ? 100 : progressPercent,
            timeSpentSeconds: secondsSpent,
          }),
        },
      );

      if (markComplete) {
        setCompleted(true);
        setLesson((current) =>
          current
            ? {
                ...current,
                progress: {
                  ...current.progress,
                  progressPercent: 100,
                  isCompleted: true,
                },
              }
            : current,
        );
      }
    } catch (err) {
      setError(
        err instanceof Error
          ? err.message
          : "Unable to save your progress.",
      );
    } finally {
      setSaving(false);
    }
  }

  async function completeLesson() {
    await saveProgress(true);
  }

  if (loading) {
    return (
      <main className="flex min-h-screen items-center justify-center bg-slate-950 text-white">
        <div className="text-sm text-slate-400">
          Loading lesson...
        </div>
      </main>
    );
  }

  if (!lesson) {
    return (
      <main className="flex min-h-screen items-center justify-center bg-slate-950 px-6 text-white">
        <div className="text-center">
          <h1 className="text-2xl font-black">
            Lesson unavailable
          </h1>

          <p className="mt-3 text-sm text-slate-500">
            {error || "This lesson could not be loaded."}
          </p>

          <Link
            href={`/courses/${courseId}`}
            className="mt-6 inline-block rounded-xl bg-white px-5 py-3 text-sm font-bold text-slate-950"
          >
            Back to course
          </Link>
        </div>
      </main>
    );
  }

  return (
    <main className="min-h-screen bg-slate-950 text-white">
      <header className="sticky top-0 z-20 border-b border-white/10 bg-slate-950/95 backdrop-blur">
        <div className="mx-auto flex max-w-7xl items-center justify-between gap-4 px-5 py-4 sm:px-6">
          <Link
            href={`/courses/${courseId}`}
            className="flex min-w-0 items-center gap-3"
          >
            <div className="flex h-10 w-10 shrink-0 items-center justify-center rounded-xl bg-white font-black text-slate-950">
              V
            </div>

            <div className="min-w-0">
              <div className="font-black">VIRENZA</div>

              <div className="truncate text-[10px] uppercase tracking-[0.18em] text-slate-600">
                {lesson.module.title}
              </div>
            </div>
          </Link>

          <Link
            href="/my-courses"
            className="shrink-0 text-sm font-semibold text-slate-400 hover:text-white"
          >
            My Courses
          </Link>
        </div>

        <div className="h-1 bg-white/5">
          <div
            className="h-full bg-white transition-all duration-500"
            style={{
              width: `${progressPercent}%`,
            }}
          />
        </div>
      </header>

      <div className="mx-auto grid max-w-7xl gap-8 px-5 py-8 sm:px-6 lg:grid-cols-[260px_minmax(0,1fr)]">
        <aside className="hidden lg:block">
          <div className="sticky top-24 rounded-2xl border border-white/10 bg-white/[0.03] p-5">
            <p className="text-[10px] font-bold uppercase tracking-[0.2em] text-slate-600">
              Current module
            </p>

            <h2 className="mt-2 font-black">
              {lesson.module.title}
            </h2>

            <div className="mt-6 rounded-xl bg-white/[0.04] p-4">
              <div className="flex items-center justify-between">
                <span className="text-xs text-slate-500">
                  Lesson progress
                </span>

                <span className="text-xs font-bold text-white">
                  {Math.round(progressPercent)}%
                </span>
              </div>

              <div className="mt-3 h-1.5 overflow-hidden rounded-full bg-white/10">
                <div
                  className="h-full rounded-full bg-white transition-all"
                  style={{
                    width: `${progressPercent}%`,
                  }}
                />
              </div>
            </div>

            <Link
              href={`/courses/${courseId}`}
              className="mt-5 block text-sm font-semibold text-slate-500 hover:text-white"
            >
              ← Course overview
            </Link>
          </div>
        </aside>

        <section className="min-w-0">
          <div className="flex flex-wrap items-center gap-3">
            <span className="rounded-full bg-white/5 px-3 py-1 text-xs font-bold text-slate-400">
              Module {lesson.module.order + 1}
            </span>

            <span className="text-xs text-slate-600">
              {lesson.estimatedMinutes} min
            </span>

            <span className="text-xs text-slate-600">
              {formatTime(secondsSpent)} learning time
            </span>

            {completed && (
              <span className="rounded-full bg-emerald-500/10 px-3 py-1 text-xs font-bold text-emerald-400">
                ✓ Completed
              </span>
            )}
          </div>

          <h1 className="mt-5 text-4xl font-black tracking-tight sm:text-5xl">
            {lesson.title}
          </h1>

          {lesson.summary && (
            <p className="mt-5 max-w-3xl text-lg leading-8 text-slate-400">
              {lesson.summary}
            </p>
          )}

          {error && (
            <div className="mt-7 rounded-xl border border-red-500/20 bg-red-500/10 px-4 py-3 text-sm text-red-300">
              {error}
            </div>
          )}

          <article className="mt-10 rounded-3xl border border-white/10 bg-white/[0.04] p-6 sm:p-9">
            {lesson.contentType.toLowerCase() === "html" ? (
              <div
                className="prose prose-invert max-w-none prose-headings:font-black prose-p:leading-8"
                dangerouslySetInnerHTML={{
                  __html: lesson.content,
                }}
              />
            ) : (
              <div className="whitespace-pre-wrap text-[16px] leading-8 text-slate-300">
                {lesson.content}
              </div>
            )}
          </article>

          <div className="mt-8 flex flex-col gap-3 sm:flex-row sm:items-center sm:justify-between">
            <div>
              {lesson.previousLesson ? (
                <Link
                  href={`/courses/${courseId}/lessons/${lesson.previousLesson.id}`}
                  className="inline-flex rounded-xl border border-white/10 px-5 py-3 text-sm font-bold text-slate-300 hover:bg-white/5 hover:text-white"
                >
                  ← Previous
                </Link>
              ) : (
                <Link
                  href={`/courses/${courseId}`}
                  className="inline-flex rounded-xl border border-white/10 px-5 py-3 text-sm font-bold text-slate-300 hover:bg-white/5 hover:text-white"
                >
                  ← Course
                </Link>
              )}
            </div>

            <div className="flex flex-col gap-3 sm:flex-row">
              {!completed && (
                <button
                  onClick={completeLesson}
                  disabled={saving}
                  className="rounded-xl border border-white/10 px-5 py-3 text-sm font-bold text-slate-300 hover:bg-white/5 hover:text-white disabled:opacity-50"
                >
                  {saving ? "Saving..." : "Mark complete"}
                </button>
              )}

              {lesson.nextLesson ? (
                <Link
                  href={`/courses/${courseId}/lessons/${lesson.nextLesson.id}`}
                  onClick={() => {
                    if (!completed) {
                      saveProgress(false);
                    }
                  }}
                  className="rounded-xl bg-white px-5 py-3 text-center text-sm font-black text-slate-950 hover:bg-slate-200"
                >
                  Next lesson →
                </Link>
              ) : (
                <Link
                  href="/my-courses"
                  className="rounded-xl bg-white px-5 py-3 text-center text-sm font-black text-slate-950 hover:bg-slate-200"
                >
                  Finish course →
                </Link>
              )}
            </div>
          </div>

          <div className="mt-10 rounded-2xl border border-white/10 bg-white/[0.02] p-5">
            <div className="flex items-center justify-between gap-4">
              <div>
                <p className="text-xs font-bold uppercase tracking-[0.16em] text-slate-600">
                  Your progress
                </p>

                <p className="mt-1 text-sm text-slate-400">
                  {completed
                    ? "This lesson is complete."
                    : "Your progress is saved automatically while you learn."}
                </p>
              </div>

              <span className="text-lg font-black">
                {Math.round(progressPercent)}%
              </span>
            </div>

            <div className="mt-4 h-2 overflow-hidden rounded-full bg-white/10">
              <div
                className="h-full rounded-full bg-white transition-all duration-500"
                style={{
                  width: `${progressPercent}%`,
                }}
              />
            </div>
          </div>
        </section>
      </div>
    </main>
  );
}
