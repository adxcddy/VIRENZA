"use client";

import { useEffect, useMemo, useState } from "react";
import Link from "next/link";
import { useRouter } from "next/navigation";
import { apiFetch } from "@/lib/api";
import {
  clearAuth,
  getStoredUser,
  type AuthUser,
} from "@/lib/auth";

type MyCourse = {
  enrollmentId: string;
  courseId: string;
  enrolledAt: string;
  progressPercent: number;
  isCompleted: boolean;
  completedAt?: string | null;
  course: {
    title: string;
    slug: string;
    difficulty: string;
    estimatedHours: number;
  } | null;
};

type Certificate = {
  id: string;
  courseId: string;
  certificateNumber: string;
  verificationCode: string;
  issuedAt: string;
  isValid: boolean;
};

export default function DashboardPage() {
  const router = useRouter();

  const [user, setUser] = useState<AuthUser | null>(null);
  const [courses, setCourses] = useState<MyCourse[]>([]);
  const [certificates, setCertificates] = useState<Certificate[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    const stored = getStoredUser();

    if (!stored) {
      router.replace("/login");
      return;
    }

    setUser(stored);

    async function loadDashboard() {
      try {
        const [myCourses, myCertificates] = await Promise.all([
          apiFetch<MyCourse[]>("/learning/my-courses"),
          apiFetch<Certificate[]>("/learning/my-certificates"),
        ]);

        setCourses(myCourses || []);
        setCertificates(myCertificates || []);
      } catch (err) {
        if (err instanceof Error) {
          setError(err.message);
        } else {
          setError("Unable to load your learning dashboard.");
        }
      } finally {
        setLoading(false);
      }
    }

    loadDashboard();
  }, [router]);

  const completed = useMemo(
    () => courses.filter((course) => course.isCompleted).length,
    [courses],
  );

  const averageProgress = useMemo(() => {
    if (!courses.length) return 0;

    return Math.round(
      courses.reduce(
        (sum, course) => sum + Number(course.progressPercent || 0),
        0,
      ) / courses.length,
    );
  }, [courses]);

  const activeCourse =
    courses.find((course) => !course.isCompleted && course.progressPercent > 0) ||
    courses.find((course) => !course.isCompleted) ||
    null;

  function logout() {
    clearAuth();
    router.replace("/login");
  }

  if (loading || !user) {
    return (
      <main className="flex min-h-screen items-center justify-center bg-slate-950 text-white">
        <div className="text-sm text-slate-400">
          Loading your learning workspace...
        </div>
      </main>
    );
  }

  return (
    <main className="min-h-screen bg-slate-950 text-white">
      <header className="sticky top-0 z-30 border-b border-white/10 bg-slate-950/90 backdrop-blur">
        <div className="mx-auto flex max-w-7xl items-center justify-between px-5 py-4 sm:px-6">
          <Link href="/" className="flex items-center gap-3">
            <div className="flex h-10 w-10 items-center justify-center rounded-xl bg-white font-black text-slate-950">
              V
            </div>

            <div>
              <div className="font-black tracking-tight">VIRENZA</div>
              <div className="text-[9px] uppercase tracking-[0.22em] text-slate-500">
                Learning Platform
              </div>
            </div>
          </Link>

          <div className="flex items-center gap-3">
            <Link
              href="/courses"
              className="hidden rounded-lg px-3 py-2 text-sm font-semibold text-slate-300 hover:bg-white/5 hover:text-white sm:block"
            >
              Courses
            </Link>

            <div className="hidden text-right md:block">
              <div className="text-sm font-bold">
                {user.firstName} {user.lastName}
              </div>
              <div className="text-xs text-slate-500">{user.role}</div>
            </div>

            <button
              onClick={logout}
              className="rounded-lg border border-white/10 px-3 py-2 text-sm font-semibold text-slate-300 hover:bg-white/5 hover:text-white"
            >
              Logout
            </button>
          </div>
        </div>
      </header>

      <div className="mx-auto max-w-7xl px-5 py-8 sm:px-6 lg:py-10">
        <section>
          <p className="text-xs font-bold uppercase tracking-[0.25em] text-slate-500">
            Student workspace
          </p>

          <h1 className="mt-3 text-3xl font-black tracking-tight sm:text-4xl">
            Welcome back, {user.firstName}.
          </h1>

          <p className="mt-3 max-w-2xl text-sm leading-7 text-slate-400 sm:text-base">
            Keep building your skills. Continue where you stopped or discover
            something new.
          </p>
        </section>

        {error && (
          <div className="mt-6 rounded-2xl border border-red-500/20 bg-red-500/10 px-5 py-4 text-sm text-red-300">
            {error}
          </div>
        )}

        <section className="mt-8 grid gap-4 sm:grid-cols-2 lg:grid-cols-4">
          <Stat
            label="Enrolled courses"
            value={courses.length}
            icon="📚"
          />

          <Stat
            label="Completed"
            value={completed}
            icon="✓"
          />

          <Stat
            label="Average progress"
            value={`${averageProgress}%`}
            icon="↗"
          />

          <Stat
            label="Certificates"
            value={certificates.length}
            icon="🏆"
          />
        </section>

        <section className="mt-8">
          <div className="mb-4 flex items-end justify-between gap-4">
            <div>
              <p className="text-xs font-bold uppercase tracking-[0.2em] text-slate-500">
                Continue learning
              </p>
              <h2 className="mt-1 text-2xl font-black">Pick up where you left off</h2>
            </div>

            <Link
              href="/courses"
              className="text-sm font-bold text-slate-300 hover:text-white"
            >
              Browse all →
            </Link>
          </div>

          {activeCourse ? (
            <article className="overflow-hidden rounded-3xl border border-white/10 bg-white/[0.04]">
              <div className="grid lg:grid-cols-[1fr_320px]">
                <div className="p-7 sm:p-8">
                  <span className="inline-flex rounded-full border border-white/10 bg-white/5 px-3 py-1 text-xs font-bold text-slate-300">
                    {activeCourse.course?.difficulty || "Course"}
                  </span>

                  <h3 className="mt-5 text-2xl font-black">
                    {activeCourse.course?.title || "Untitled course"}
                  </h3>

                  <p className="mt-3 text-sm leading-6 text-slate-500">
                    {activeCourse.course?.estimatedHours || 0} estimated hours
                    of learning.
                  </p>

                  <div className="mt-7">
                    <div className="mb-2 flex justify-between text-xs">
                      <span className="text-slate-500">Course progress</span>
                      <span className="font-bold text-white">
                        {Number(activeCourse.progressPercent || 0).toFixed(0)}%
                      </span>
                    </div>

                    <div className="h-2 overflow-hidden rounded-full bg-white/10">
                      <div
                        className="h-full rounded-full bg-white transition-all"
                        style={{
                          width: `${Math.min(
                            100,
                            Math.max(0, Number(activeCourse.progressPercent || 0)),
                          )}%`,
                        }}
                      />
                    </div>
                  </div>

                  <Link
                    href={`/courses/${activeCourse.courseId}`}
                    className="mt-7 inline-flex rounded-xl bg-white px-5 py-3 text-sm font-bold text-slate-950 hover:bg-slate-200"
                  >
                    Continue course →
                  </Link>
                </div>

                <div className="border-t border-white/10 bg-white/[0.025] p-7 lg:border-l lg:border-t-0">
                  <p className="text-xs font-bold uppercase tracking-[0.18em] text-slate-600">
                    Enrolled
                  </p>

                  <p className="mt-2 text-sm text-slate-400">
                    {new Date(activeCourse.enrolledAt).toLocaleDateString()}
                  </p>

                  <div className="mt-8">
                    <p className="text-xs font-bold uppercase tracking-[0.18em] text-slate-600">
                      Status
                    </p>
                    <p className="mt-2 font-bold text-white">
                      {Number(activeCourse.progressPercent) > 0
                        ? "In progress"
                        : "Not started"}
                    </p>
                  </div>
                </div>
              </div>
            </article>
          ) : (
            <div className="rounded-3xl border border-dashed border-white/10 bg-white/[0.025] p-10 text-center">
              <div className="text-4xl">📚</div>
              <h3 className="mt-4 text-xl font-black">
                Your learning journey starts here
              </h3>
              <p className="mx-auto mt-2 max-w-md text-sm leading-6 text-slate-500">
                Browse the VIRENZA catalogue and enroll in your first course.
              </p>

              <Link
                href="/courses"
                className="mt-6 inline-flex rounded-xl bg-white px-5 py-3 text-sm font-bold text-slate-950 hover:bg-slate-200"
              >
                Explore courses →
              </Link>
            </div>
          )}
        </section>

        <section className="mt-10">
          <div className="mb-4">
            <p className="text-xs font-bold uppercase tracking-[0.2em] text-slate-500">
              Your learning
            </p>
            <h2 className="mt-1 text-2xl font-black">My courses</h2>
          </div>

          {courses.length === 0 ? (
            <div className="rounded-2xl border border-white/10 bg-white/[0.03] p-6 text-sm text-slate-500">
              You haven't enrolled in a course yet.
            </div>
          ) : (
            <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-3">
              {courses.map((item) => (
                <Link
                  key={item.enrollmentId}
                  href={`/courses/${item.courseId}`}
                  className="group rounded-2xl border border-white/10 bg-white/[0.035] p-5 transition hover:-translate-y-1 hover:border-white/20"
                >
                  <div className="flex items-start justify-between gap-3">
                    <span className="rounded-full bg-white/5 px-3 py-1 text-xs font-bold text-slate-400">
                      {item.course?.difficulty || "Course"}
                    </span>

                    {item.isCompleted && (
                      <span className="text-xs font-bold text-emerald-400">
                        Completed
                      </span>
                    )}
                  </div>

                  <h3 className="mt-5 font-black group-hover:text-slate-200">
                    {item.course?.title || "Course"}
                  </h3>

                  <div className="mt-5 h-1.5 overflow-hidden rounded-full bg-white/10">
                    <div
                      className="h-full rounded-full bg-white"
                      style={{
                        width: `${Math.min(
                          100,
                          Math.max(0, Number(item.progressPercent || 0)),
                        )}%`,
                      }}
                    />
                  </div>

                  <div className="mt-2 flex justify-between text-xs text-slate-500">
                    <span>{Number(item.progressPercent || 0).toFixed(0)}%</span>
                    <span>{item.course?.estimatedHours || 0}h</span>
                  </div>
                </Link>
              ))}
            </div>
          )}
        </section>

        <section className="mt-10">
          <div className="mb-4 flex items-end justify-between">
            <div>
              <p className="text-xs font-bold uppercase tracking-[0.2em] text-slate-500">
                Achievements
              </p>
              <h2 className="mt-1 text-2xl font-black">Certificates</h2>
            </div>
          </div>

          {certificates.length === 0 ? (
            <div className="rounded-2xl border border-white/10 bg-white/[0.03] p-6 text-sm text-slate-500">
              Complete your courses and pass their assessments to earn
              certificates.
            </div>
          ) : (
            <div className="grid gap-4 md:grid-cols-2">
              {certificates.map((certificate) => (
                <div
                  key={certificate.id}
                  className="rounded-2xl border border-white/10 bg-white/[0.04] p-6"
                >
                  <div className="text-3xl">🏆</div>

                  <h3 className="mt-4 font-black">
                    {certificate.certificateNumber}
                  </h3>

                  <p className="mt-2 text-sm text-slate-500">
                    Issued{" "}
                    {new Date(certificate.issuedAt).toLocaleDateString()}
                  </p>

                  <span
                    className={`mt-4 inline-flex rounded-full px-3 py-1 text-xs font-bold ${
                      certificate.isValid
                        ? "bg-emerald-500/10 text-emerald-400"
                        : "bg-red-500/10 text-red-400"
                    }`}
                  >
                    {certificate.isValid ? "Valid" : "Invalid"}
                  </span>
                </div>
              ))}
            </div>
          )}
        </section>
      </div>
    </main>
  );
}

function Stat({
  label,
  value,
  icon,
}: {
  label: string;
  value: string | number;
  icon: string;
}) {
  return (
    <div className="rounded-2xl border border-white/10 bg-white/[0.035] p-5">
      <div className="flex items-center justify-between">
        <span className="text-2xl">{icon}</span>
        <span className="text-2xl font-black">{value}</span>
      </div>

      <p className="mt-4 text-xs font-bold uppercase tracking-[0.15em] text-slate-500">
        {label}
      </p>
    </div>
  );
}
