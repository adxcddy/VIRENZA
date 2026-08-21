import Link from "next/link";

export default function Footer() {
  return (
    <footer className="footer">
      <div className="container footer-grid">
        <div>
          <div className="brand footer-brand">
            <span className="brand-mark">V</span>
            <span>VIRENZA</span>
          </div>
          <p>
            A modern learning platform designed to help learners build
            practical knowledge, skills and confidence.
          </p>
        </div>

        <div>
          <h4>Platform</h4>
          <Link href="/#courses">Courses</Link>
          <Link href="/#features">Features</Link>
          <Link href="/register">Get Started</Link>
        </div>

        <div>
          <h4>Account</h4>
          <Link href="/login">Login</Link>
          <Link href="/register">Register</Link>
          <Link href="/dashboard">Dashboard</Link>
        </div>

        <div>
          <h4>VIRENZA</h4>
          <Link href="/#about">About</Link>
          <span>Learn • Practice • Grow</span>
        </div>
      </div>

      <div className="container footer-bottom">
        <span>© {new Date().getFullYear()} VIRENZA. All rights reserved.</span>
        <span>Built for modern learning.</span>
      </div>
    </footer>
  );
}
