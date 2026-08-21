"use client";

import { FormEvent, useState } from "react";
import { useRouter } from "next/navigation";
import { saveAuth } from "@/lib/auth";

export default function RegisterPage() {
  const router = useRouter();

  const [form, setForm] = useState({
    firstName: "",
    lastName: "",
    email: "",
    password: "",
    country: "",
    learningGoal: "",
  });

  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);

  function updateField(field: keyof typeof form, value: string) {
    setForm((current) => ({
      ...current,
      [field]: value,
    }));
  }

  async function handleSubmit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();

    setError("");
    setLoading(true);

    try {
      const response = await fetch("/api/backend/auth/register", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(form),
      });

      if (!response.ok) {
        const text = await response.text();
        throw new Error(text || "Registration failed.");
      }

      const data = await response.json();

      saveAuth(data);

      router.replace("/dashboard");
    } catch (err) {
      setError(
        err instanceof Error
          ? err.message
          : "Unable to create your account.",
      );
    } finally {
      setLoading(false);
    }
  }

  return (
    <main className="min-h-screen bg-slate-950 px-6 py-12 text-white">
      <div className="mx-auto max-w-2xl">
        <a href="/" className="mb-8 inline-flex items-center gap-3">
          <div className="flex h-11 w-11 items-center justify-center rounded-xl bg-white font-black text-slate-950">
            V
          </div>

          <div>
            <div className="text-xl font-black">VIRENZA</div>
            <div className="text-[10px] font-semibold uppercase tracking-[0.2em] text-slate-500">
              Learn Without Limits
            </div>
          </div>
        </a>

        <div className="rounded-3xl border border-white/10 bg-white/[0.04] p-7 shadow-2xl sm:p-9">
          <h1 className="text-3xl font-black">Create your account</h1>

          <p className="mt-2 text-slate-400">
            Start your learning journey with VIRENZA.
          </p>

          {error && (
            <div className="mt-6 rounded-xl border border-red-500/20 bg-red-500/10 px-4 py-3 text-sm text-red-300">
              {error}
            </div>
          )}

          <form onSubmit={handleSubmit} className="mt-7 space-y-5">
            <div className="grid gap-5 sm:grid-cols-2">
              <div>
                <label className="mb-2 block text-sm font-semibold text-slate-300">
                  First name
                </label>

                <input
                  value={form.firstName}
                  onChange={(e) => updateField("firstName", e.target.value)}
                  required
                  autoComplete="given-name"
                  className="w-full rounded-xl border border-white/10 bg-black/20 px-4 py-3 outline-none focus:border-white/30"
                />
              </div>

              <div>
                <label className="mb-2 block text-sm font-semibold text-slate-300">
                  Last name
                </label>

                <input
                  value={form.lastName}
                  onChange={(e) => updateField("lastName", e.target.value)}
                  required
                  autoComplete="family-name"
                  className="w-full rounded-xl border border-white/10 bg-black/20 px-4 py-3 outline-none focus:border-white/30"
                />
              </div>
            </div>

            <div>
              <label className="mb-2 block text-sm font-semibold text-slate-300">
                Email
              </label>

              <input
                type="email"
                value={form.email}
                onChange={(e) => updateField("email", e.target.value)}
                required
                autoComplete="email"
                className="w-full rounded-xl border border-white/10 bg-black/20 px-4 py-3 outline-none focus:border-white/30"
              />
            </div>

            <div>
              <label className="mb-2 block text-sm font-semibold text-slate-300">
                Password
              </label>

              <input
                type="password"
                value={form.password}
                onChange={(e) => updateField("password", e.target.value)}
                required
                minLength={8}
                autoComplete="new-password"
                className="w-full rounded-xl border border-white/10 bg-black/20 px-4 py-3 outline-none focus:border-white/30"
              />

              <p className="mt-2 text-xs text-slate-500">
                Minimum 8 characters.
              </p>
            </div>

            <div className="grid gap-5 sm:grid-cols-2">
              <div>
                <label className="mb-2 block text-sm font-semibold text-slate-300">
                  Country
                </label>

                <input
                  value={form.country}
                  onChange={(e) => updateField("country", e.target.value)}
                  autoComplete="country-name"
                  placeholder="Uganda"
                  className="w-full rounded-xl border border-white/10 bg-black/20 px-4 py-3 outline-none focus:border-white/30"
                />
              </div>

              <div>
                <label className="mb-2 block text-sm font-semibold text-slate-300">
                  Learning goal
                </label>

                <input
                  value={form.learningGoal}
                  onChange={(e) =>
                    updateField("learningGoal", e.target.value)
                  }
                  placeholder="e.g. Cybersecurity"
                  className="w-full rounded-xl border border-white/10 bg-black/20 px-4 py-3 outline-none focus:border-white/30"
                />
              </div>
            </div>

            <button
              type="submit"
              disabled={loading}
              className="w-full rounded-xl bg-white px-5 py-3.5 font-bold text-slate-950 transition hover:bg-slate-200 disabled:cursor-not-allowed disabled:opacity-50"
            >
              {loading ? "Creating account..." : "Create account"}
            </button>
          </form>

          <div className="mt-7 text-center text-sm text-slate-400">
            Already have an account?{" "}
            <a
              href="/login"
              className="font-bold text-white hover:underline"
            >
              Sign in
            </a>
          </div>
        </div>
      </div>
    </main>
  );
}
