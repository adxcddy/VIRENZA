"use client";

import { FormEvent, useEffect, useState } from "react";
import Link from "next/link";
import { useRouter } from "next/navigation";
import { getStoredUser } from "@/lib/auth";
import { register } from "@/lib/api";

export default function RegisterPage() {
  const router = useRouter();

  const [form, setForm] = useState({
    firstName: "",
    lastName: "",
    email: "",
    password: "",
  });

  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    if (getStoredUser()) {
      router.replace("/dashboard");
    }
  }, [router]);

  function updateField(
    field: keyof typeof form,
    value: string,
  ) {
    setForm((current) => ({
      ...current,
      [field]: value,
    }));
  }

  async function handleSubmit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();

    setError("");

    if (form.password.length < 8) {
      setError("Password must contain at least 8 characters.");
      return;
    }

    setLoading(true);

    try {
      await register(
        form.firstName,
        form.lastName,
        form.email,
        form.password,
      );

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
        <Link href="/" className="mb-8 inline-flex items-center gap-3">
          <div className="flex h-11 w-11 items-center justify-center rounded-xl bg-white font-black text-slate-950">
            V
          </div>

          <div>
            <div className="text-xl font-black">VIRENZA</div>
            <div className="text-[10px] font-semibold uppercase tracking-[0.2em] text-slate-500">
              Learn Without Limits
            </div>
          </div>
        </Link>

        <div className="rounded-3xl border border-white/10 bg-white/[0.04] p-7 shadow-2xl sm:p-9">
          <h1 className="text-3xl font-black">Create your account</h1>

          <p className="mt-2 text-slate-400">
            Start your learning journey with VIRENZA.
          </p>

          {error && (
            <div
              role="alert"
              className="mt-6 rounded-xl border border-red-500/20 bg-red-500/10 px-4 py-3 text-sm text-red-300"
            >
              {error}
            </div>
          )}

          <form onSubmit={handleSubmit} className="mt-7 space-y-5">
            <div className="grid gap-5 sm:grid-cols-2">
              <div>
                <label
                  htmlFor="firstName"
                  className="mb-2 block text-sm font-semibold text-slate-300"
                >
                  First name
                </label>

                <input
                  id="firstName"
                  value={form.firstName}
                  onChange={(e) =>
                    updateField("firstName", e.target.value)
                  }
                  required
                  autoComplete="given-name"
                  className="w-full rounded-xl border border-white/10 bg-black/20 px-4 py-3 outline-none focus:border-white/30"
                />
              </div>

              <div>
                <label
                  htmlFor="lastName"
                  className="mb-2 block text-sm font-semibold text-slate-300"
                >
                  Last name
                </label>

                <input
                  id="lastName"
                  value={form.lastName}
                  onChange={(e) =>
                    updateField("lastName", e.target.value)
                  }
                  required
                  autoComplete="family-name"
                  className="w-full rounded-xl border border-white/10 bg-black/20 px-4 py-3 outline-none focus:border-white/30"
                />
              </div>
            </div>

            <div>
              <label
                htmlFor="register-email"
                className="mb-2 block text-sm font-semibold text-slate-300"
              >
                Email
              </label>

              <input
                id="register-email"
                type="email"
                value={form.email}
                onChange={(e) =>
                  updateField("email", e.target.value)
                }
                required
                autoComplete="email"
                className="w-full rounded-xl border border-white/10 bg-black/20 px-4 py-3 outline-none focus:border-white/30"
              />
            </div>

            <div>
              <label
                htmlFor="register-password"
                className="mb-2 block text-sm font-semibold text-slate-300"
              >
                Password
              </label>

              <input
                id="register-password"
                type="password"
                value={form.password}
                onChange={(e) =>
                  updateField("password", e.target.value)
                }
                required
                minLength={8}
                autoComplete="new-password"
                className="w-full rounded-xl border border-white/10 bg-black/20 px-4 py-3 outline-none focus:border-white/30"
              />

              <p className="mt-2 text-xs text-slate-500">
                Minimum 8 characters.
              </p>
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
            <Link
              href="/login"
              className="font-bold text-white hover:underline"
            >
              Sign in
            </Link>
          </div>
        </div>
      </div>
    </main>
  );
}
