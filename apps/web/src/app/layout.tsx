import type { Metadata } from "next";
import "./globals.css";

export const metadata: Metadata = {
  title: "VIRENZA",
  description: "Learn Without Limits",
};

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="en">
      <body>{children}</body>
    </html>
  );
}
