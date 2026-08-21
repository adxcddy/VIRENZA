import Link from "next/link";
import Navbar from "@/components/Navbar";
import Footer from "@/components/Footer";
import CourseCard from "@/components/CourseCard";
import SectionTitle from "@/components/SectionTitle";
import StatCard from "@/components/StatCard";

export default function Home() {
  return (
    <>
      <Navbar />

      <main>
        <section className="hero">
          <div className="hero-glow hero-glow-one" />
          <div className="hero-glow hero-glow-two" />

          <div className="container hero-grid">
            <div className="hero-content">
              <div className="hero-badge">
                <span>●</span> The future of learning
              </div>

              <h1>
                Learn today.
                <br />
                <em>Grow tomorrow.</em>
              </h1>

              <p>
                VIRENZA is a modern learning platform that brings courses,
                practical learning, assessments and progress tracking together
                in one intelligent experience.
              </p>

              <div className="hero-actions">
                <Link href="/register" className="button">
                  Start Learning →
                </Link>
                <Link href="/login" className="button button-outline">
                  Sign In
                </Link>
              </div>

              <div className="hero-trust">
                <span>✓ Learn at your pace</span>
                <span>✓ Track your progress</span>
                <span>✓ Build real skills</span>
              </div>
            </div>

            <div className="hero-panel">
              <div className="learning-window">
                <div className="window-top">
                  <div>
                    <span className="window-label">YOUR LEARNING</span>
                    <h3>Continue Learning</h3>
                  </div>
                  <span className="window-dot">●</span>
                </div>

                <div className="progress-card">
                  <div className="progress-icon">01</div>
                  <div className="progress-info">
                    <strong>Introduction to ICT</strong>
                    <span>Module 3 of 8</span>
                    <div className="progress-bar">
                      <i style={{ width: "64%" }} />
                    </div>
                  </div>
                  <b>64%</b>
                </div>

                <div className="mini-lessons">
                  <div>
                    <span className="lesson-number">01</span>
                    <span>
                      <strong>Digital Foundations</strong>
                      <small>Completed</small>
                    </span>
                    <b>✓</b>
                  </div>

                  <div>
                    <span className="lesson-number">02</span>
                    <span>
                      <strong>Computer Systems</strong>
                      <small>Completed</small>
                    </span>
                    <b>✓</b>
                  </div>

                  <div className="active-lesson">
                    <span className="lesson-number">03</span>
                    <span>
                      <strong>Networks & Internet</strong>
                      <small>Continue learning</small>
                    </span>
                    <b>→</b>
                  </div>
                </div>

                <Link href="/dashboard" className="window-button">
                  Open dashboard
                </Link>
              </div>
            </div>
          </div>
        </section>

        <section className="stats">
          <div className="container stats-grid">
            <StatCard value="24/7" label="Learn anytime" />
            <StatCard value="100%" label="Progress tracking" />
            <StatCard value="∞" label="Room to grow" />
            <StatCard value="1" label="Learning platform" />
          </div>
        </section>

        <section className="section" id="features">
          <div className="container">
            <SectionTitle
              eyebrow="WHY VIRENZA"
              title="Everything you need to keep learning."
              description="VIRENZA is designed around the complete learner journey — from discovering a course to completing it and measuring your progress."
            />

            <div className="feature-grid">
              <div className="feature-card">
                <div className="feature-number">01</div>
                <h3>Learn</h3>
                <p>
                  Access structured courses and lessons designed to make
                  difficult concepts easier to understand.
                </p>
              </div>

              <div className="feature-card">
                <div className="feature-number">02</div>
                <h3>Practice</h3>
                <p>
                  Reinforce your knowledge with assessments, quizzes,
                  assignments and practical activities.
                </p>
              </div>

              <div className="feature-card">
                <div className="feature-number">03</div>
                <h3>Grow</h3>
                <p>
                  Track your progress, identify areas for improvement and keep
                  building valuable skills.
                </p>
              </div>
            </div>
          </div>
        </section>

        <section className="section courses-section" id="courses">
          <div className="container">
            <SectionTitle
              eyebrow="EXPLORE"
              title="Start with a subject that matters."
              description="Build foundations, develop practical skills and prepare yourself for what's next."
            />

            <div className="course-grid">
              <CourseCard
                category="Technology"
                title="Information & Communication Technology"
                description="Build a strong foundation in modern ICT concepts and digital skills."
                lessons={12}
                level="Beginner"
              />

              <CourseCard
                category="Business"
                title="Digital Business Foundations"
                description="Understand the tools, concepts and skills behind modern digital business."
                lessons={10}
                level="Beginner"
              />

              <CourseCard
                category="Professional"
                title="Digital Skills & Productivity"
                description="Develop practical digital skills for school, work and everyday life."
                lessons={14}
                level="Intermediate"
              />
            </div>
          </div>
        </section>

        <section className="section about-section" id="about">
          <div className="container about-grid">
            <div>
              <span className="eyebrow">BUILT DIFFERENTLY</span>
              <h2>Education should move with you.</h2>
            </div>

            <div>
              <p>
                VIRENZA brings learning content, progress, assessments and
                learner tools into one connected platform.
              </p>
              <p>
                Whether you are starting from the basics or developing
                professional skills, the goal is simple: make learning
                practical, measurable and accessible.
              </p>

              <Link href="/register" className="text-link">
                Begin your journey →
              </Link>
            </div>
          </div>
        </section>

        <section className="cta-section">
          <div className="container cta-box">
            <span className="eyebrow">YOUR NEXT STEP</span>
            <h2>Ready to start learning?</h2>
            <p>Create your VIRENZA account and begin building your skills.</p>
            <Link href="/register" className="button">
              Create Free Account →
            </Link>
          </div>
        </section>
      </main>

      <Footer />
    </>
  );
}
