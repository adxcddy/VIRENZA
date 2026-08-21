import { authHeaders } from "@/lib/auth";

export async function apiFetch<T>(
  path: string,
  options: RequestInit = {},
): Promise<T> {
  const response = await fetch(`/api/backend${path}`, {
    ...options,
    headers: {
      ...authHeaders(),
      ...(options.headers || {}),
    },
  });

  if (!response.ok) {
    const text = await response.text();
    throw new Error(text || `Request failed: ${response.status}`);
  }

  const contentType = response.headers.get("content-type") || "";

  if (contentType.includes("application/json")) {
    return response.json();
  }

  return undefined as T;
}

export async function login(email: string, password: string) {
  const response = await fetch("/api/backend/auth/login", {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify({
      email,
      password,
    }),
  });

  if (!response.ok) {
    const text = await response.text();
    throw new Error(text || "Invalid email or password.");
  }

  const data = await response.json();

  localStorage.setItem("virenza_token", data.token);
  localStorage.setItem("virenza_user", JSON.stringify(data));

  return data;
}

export function logout() {
  localStorage.removeItem("virenza_token");
  localStorage.removeItem("virenza_user");
}
