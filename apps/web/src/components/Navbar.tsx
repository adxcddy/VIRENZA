"use client";

import Link from "next/link";
import { useState } from "react";

export default function Navbar() {
  const [open, setOpen] = useState(false);

  return (
    <header className="site-header">
      <div className="container nav-inner">
        <Link href="/" className="brand">
          <span className="brand-mark">V</span>
          <span>VIRENZA</span>
        </Link>

        <nav className={`nav-links ${open ? "open" : ""}`}>
          <Link href="/#courses" onClick={() => setOpen(false)}>Courses</Link>
          <Link href="/#features" onClick={() => setOpen(false)}>Features</Link>
          <Link href="/#about" onClick={() => setOpen(false)}>About</Link>
          <Link href="/login" className="mobile-login" onClick={() => setOpen(false)}>
            Login
          </Link>
        </nav>

        <div className="nav-actions">
          <Link href="/login" className="nav-login">Login</Link>
          <Link href="/register" className="button button-small">Get Started</Link>
        </div>

        <button
          className="menu-button"
          onClick={() => setOpen(!open)}
          aria-label="Toggle navigation"
        >
          {open ? "×" : "☰"}
        </button>
      </div>
    </header>
  );
}
