const learningPaths = [
  {
    icon: "🌱",
    title: "Start From Zero",
    text: "Build strong foundations with guided learning for beginners.",
  },
  {
    icon: "🎓",
    title: "School & University",
    text: "Learn subjects, prepare for examinations, and master advanced concepts.",
  },
  {
    icon: "💻",
    title: "Professional Skills",
    text: "Develop practical technology, business, creative and career skills.",
  },
  {
    icon: "🔬",
    title: "Advanced & Research",
    text: "Progress toward advanced specialization, research and academic discovery.",
  },
];

const platformFeatures = [
  ["🎥", "Live Classrooms", "Learn directly with teachers through live video, audio, chat and screen sharing."],
  ["🤖", "AI Learning Assistant", "Get explanations, practice, study guidance and personalized learning support."],
  ["📚", "Knowledge Library", "Explore lessons, notes, resources, academic material and connected knowledge."],
  ["📝", "Assessments", "Test your understanding with quizzes, assignments, examinations and practical work."],
  ["🏆", "Certificates", "Earn verifiable certificates based on completed learning and demonstrated achievement."],
  ["🤝", "Scholarships", "Connect learners with sponsorship and scholarship opportunities that expand access to education."],
];

const subjects = [
  "Computer Science",
  "Artificial Intelligence",
  "Cybersecurity",
  "Mathematics",
  "Science",
  "Business",
  "Languages",
  "Engineering",
  "Arts & Design",
  "Research",
];

export default function Home() {
  return (
    <main className="min-h-screen bg-slate-950 text-white">
      {/* Navigation */}
      <header className="border-b border-white/10 bg-slate-950/90 backdrop-blur">
        <div className="mx-auto flex max-w-7xl items-center justify-between px-6 py-5">
          <a href="/" className="flex items-center gap-3">
            <div className="flex h-10 w-10 items-center justify-center rounded-xl bg-white text-lg font-black text-slate-950">
              V
            </div>
            <div>
              <div className="text-xl font-black tracking-tight">VIRENZA</div>
              <div className="text-[10px] font-semibold uppercase tracking-[0.25em] text-slate-400">
                Learn Without Limits
              </div>
            </div>
          </a>

          <nav className="hidden items-center gap-8 text-sm font-medium text-slate-300 md:flex">
            <a href="#learning" className="transition hover:text-white">Learning</a>
            <a href="#features" className="transition hover:text-white">Features</a>
            <a href="#subjects" className="transition hover:text-white">Subjects</a>
            <a href="#about" className="transition hover:text-white">About</a>
          </nav>

          <div className="flex items-center gap-3">
            <a
              href="/login"
              className="hidden rounded-lg px-4 py-2 text-sm font-semibold text-slate-300 transition hover:text-white sm:block"
            >
              Log in
            </a>
            <a
              href="/register"
              className="rounded-lg bg-white px-4 py-2 text-sm font-bold text-slate-950 transition hover:bg-slate-200"
            >
              Start Learning
            </a>
          </div>
        </div>
      </header>

      {/* Hero */}
      <section className="relative overflow-hidden">
        <div className="absolute inset-0 bg-[radial-gradient(circle_at_20%_20%,rgba(59,130,246,0.18),transparent_35%),radial-gradient(circle_at_80%_30%,rgba(168,85,247,0.14),transparent_35%)]" />

        <div className="relative mx-auto grid max-w-7xl gap-16 px-6 pb-24 pt-20 lg:grid-cols-[1.15fr_0.85fr] lg:items-center lg:pt-28">
          <div>
            <div className="mb-6 inline-flex items-center gap-2 rounded-full border border-white/10 bg-white/5 px-4 py-2 text-sm font-medium text-slate-300">
              <span className="h-2 w-2 rounded-full bg-emerald-400" />
              A global learning ecosystem
            </div>

            <h1 className="max-w-4xl text-5xl font-black leading-[1.02] tracking-tight sm:text-6xl lg:text-7xl">
              Start from zero.
              <span className="block text-slate-400">Go as far as you can imagine.</span>
            </h1>

            <p className="mt-7 max-w-2xl text-lg leading-8 text-slate-300 sm:text-xl">
              VIRENZA brings live teaching, structured courses, knowledge, practical
              learning, AI support, assessments, certificates and opportunity into
              one global education platform.
            </p>

            <div className="mt-9 flex flex-col gap-3 sm:flex-row">
              <a
                href="/register"
                className="rounded-xl bg-white px-7 py-4 text-center font-bold text-slate-950 transition hover:-translate-y-0.5 hover:bg-slate-200"
              >
                Start Learning Free
              </a>
              <a
                href="#learning"
                className="rounded-xl border border-white/15 bg-white/5 px-7 py-4 text-center font-bold text-white transition hover:bg-white/10"
              >
                Explore VIRENZA
              </a>
            </div>

            <div className="mt-10 flex flex-wrap gap-x-8 gap-y-3 text-sm text-slate-400">
              <span>✓ Beginner friendly</span>
              <span>✓ Live teachers</span>
              <span>✓ Practical learning</span>
              <span>✓ Global access</span>
            </div>
          </div>

          {/* Learning Journey Card */}
          <div className="rounded-3xl border border-white/10 bg-white/[0.04] p-6 shadow-2xl shadow-black/30 backdrop-blur">
            <div className="mb-6 flex items-center justify-between">
              <div>
                <p className="text-sm font-semibold text-slate-400">Your journey</p>
                <h2 className="mt-1 text-2xl font-bold">Learning Path</h2>
              </div>
              <div className="rounded-full bg-emerald-400/10 px-3 py-1 text-xs font-bold text-emerald-300">
                PERSONALIZED
              </div>
            </div>

            <div className="space-y-3">
              {[
                ["01", "Foundation", "Build the basics"],
                ["02", "Knowledge", "Understand deeply"],
                ["03", "Practice", "Apply what you learn"],
                ["04", "Mastery", "Prove your skills"],
                ["05", "Research", "Create new knowledge"],
              ].map(([number, title, description], index) => (
                <div
                  key={number}
                  className="flex items-center gap-4 rounded-2xl border border-white/8 bg-black/20 p-4"
                >
                  <div className="flex h-10 w-10 shrink-0 items-center justify-center rounded-xl bg-white/10 text-xs font-black text-slate-300">
                    {number}
                  </div>
                  <div className="flex-1">
                    <div className="font-bold">{title}</div>
                    <div className="text-sm text-slate-400">{description}</div>
                  </div>
                  {index === 0 && (
                    <span className="text-xs font-bold text-emerald-300">START</span>
                  )}
                </div>
              ))}
            </div>

            <div className="mt-5 rounded-2xl border border-white/10 bg-white/5 p-4">
              <div className="flex items-center justify-between text-sm">
                <span className="text-slate-400">Knowledge growth</span>
                <span className="font-bold">Ready to begin</span>
              </div>
              <div className="mt-3 h-2 overflow-hidden rounded-full bg-white/10">
                <div className="h-full w-[8%] rounded-full bg-white" />
              </div>
            </div>
          </div>
        </div>
      </section>

      {/* Vision */}
      <section id="learning" className="border-y border-white/10 bg-white/[0.025]">
        <div className="mx-auto max-w-7xl px-6 py-20">
          <div className="max-w-3xl">
            <p className="text-sm font-bold uppercase tracking-[0.2em] text-slate-500">
              One platform. Every stage.
            </p>
            <h2 className="mt-3 text-3xl font-black tracking-tight sm:text-5xl">
              Education should never have a dead end.
            </h2>
            <p className="mt-5 text-lg leading-8 text-slate-400">
              Whether you are taking your first lesson, preparing for an examination,
              developing professional skills or exploring advanced research, VIRENZA
              is designed to keep your learning journey moving forward.
            </p>
          </div>

          <div className="mt-12 grid gap-5 sm:grid-cols-2 lg:grid-cols-4">
            {learningPaths.map((item) => (
              <article
                key={item.title}
                className="rounded-2xl border border-white/10 bg-slate-900/60 p-6 transition hover:-translate-y-1 hover:border-white/20"
              >
                <div className="text-3xl">{item.icon}</div>
                <h3 className="mt-5 text-xl font-bold">{item.title}</h3>
                <p className="mt-3 text-sm leading-6 text-slate-400">{item.text}</p>
              </article>
            ))}
          </div>
        </div>
      </section>

      {/* Features */}
      <section id="features" className="mx-auto max-w-7xl px-6 py-20">
        <div className="text-center">
          <p className="text-sm font-bold uppercase tracking-[0.2em] text-slate-500">
            The learning ecosystem
          </p>
          <h2 className="mt-3 text-3xl font-black sm:text-5xl">
            Everything you need to learn better.
          </h2>
        </div>

        <div className="mt-12 grid gap-5 md:grid-cols-2 lg:grid-cols-3">
          {platformFeatures.map(([icon, title, text]) => (
            <article
              key={title}
              className="rounded-2xl border border-white/10 bg-white/[0.03] p-7"
            >
              <div className="text-3xl">{icon}</div>
              <h3 className="mt-5 text-xl font-bold">{title}</h3>
              <p className="mt-3 leading-7 text-slate-400">{text}</p>
            </article>
          ))}
        </div>
      </section>

      {/* Subjects */}
      <section id="subjects" className="border-y border-white/10 bg-white/[0.025]">
        <div className="mx-auto max-w-7xl px-6 py-20">
          <div className="grid gap-12 lg:grid-cols-[0.8fr_1.2fr] lg:items-center">
            <div>
              <p className="text-sm font-bold uppercase tracking-[0.2em] text-slate-500">
                Knowledge without borders
              </p>
              <h2 className="mt-3 text-3xl font-black sm:text-5xl">
                Explore what interests you.
              </h2>
              <p className="mt-5 leading-7 text-slate-400">
                VIRENZA is designed to grow beyond a single subject. Explore connected
                knowledge, discover new fields and build your own path.
              </p>
            </div>

            <div className="grid grid-cols-2 gap-3 sm:grid-cols-3">
              {subjects.map((subject) => (
                <div
                  key={subject}
                  className="rounded-xl border border-white/10 bg-slate-900 px-4 py-4 text-sm font-semibold text-slate-200"
                >
                  {subject}
                </div>
              ))}
            </div>
          </div>
        </div>
      </section>

      {/* Inspiration */}
      <section id="about" className="mx-auto max-w-7xl px-6 py-24">
        <div className="rounded-3xl border border-white/10 bg-white/[0.04] px-7 py-12 text-center sm:px-14">
          <div className="mx-auto max-w-3xl">
            <div className="text-4xl">🌍</div>
            <h2 className="mt-5 text-3xl font-black sm:text-5xl">
              Knowledge can change a life.
            </h2>
            <p className="mt-5 text-lg leading-8 text-slate-400">
              Our goal is to make high-quality learning more accessible, more
              practical and more inspiring—so that where someone begins does not
              determine how far they can go.
            </p>

            <div className="mt-8 flex flex-col justify-center gap-3 sm:flex-row">
              <a
                href="/register"
                className="rounded-xl bg-white px-7 py-4 font-bold text-slate-950 transition hover:bg-slate-200"
              >
                Create Your Account
              </a>
              <a
                href="/courses"
                className="rounded-xl border border-white/15 px-7 py-4 font-bold transition hover:bg-white/10"
              >
                Browse Learning
              </a>
            </div>
          </div>
        </div>
      </section>

      {/* Footer */}
      <footer className="border-t border-white/10">
        <div className="mx-auto flex max-w-7xl flex-col gap-5 px-6 py-8 text-sm text-slate-500 sm:flex-row sm:items-center sm:justify-between">
          <div>
            <span className="font-bold text-slate-300">VIRENZA</span>
            <span className="ml-2">Learn Without Limits.</span>
          </div>
          <div>© {new Date().getFullYear()} VIRENZA. Building the future of learning.</div>
        </div>
      </footer>
    </main>
  );
}
