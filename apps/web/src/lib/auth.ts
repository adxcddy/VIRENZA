export const TOKEN_KEY = "virenza_token";
export const USER_KEY = "virenza_user";

export type AuthUser = {
  userId: string;
  firstName: string;
  lastName: string;
  email: string;
  role: string;
  expiresAt?: string;
};

export type AuthResponse = {
  token: string;
  userId: string;
  firstName: string;
  lastName: string;
  email: string;
  role: string;
  expiresAt: string;
};

export function saveAuth(response: AuthResponse) {
  if (typeof window === "undefined") return;

  localStorage.setItem(TOKEN_KEY, response.token);

  localStorage.setItem(
    USER_KEY,
    JSON.stringify({
      userId: response.userId,
      firstName: response.firstName,
      lastName: response.lastName,
      email: response.email,
      role: response.role,
      expiresAt: response.expiresAt,
    }),
  );
}

export function getToken() {
  if (typeof window === "undefined") return null;
  return localStorage.getItem(TOKEN_KEY);
}

export function getStoredUser(): AuthUser | null {
  if (typeof window === "undefined") return null;

  const raw = localStorage.getItem(USER_KEY);

  if (!raw) return null;

  try {
    return JSON.parse(raw) as AuthUser;
  } catch {
    return null;
  }
}

export function clearAuth() {
  if (typeof window === "undefined") return;

  localStorage.removeItem(TOKEN_KEY);
  localStorage.removeItem(USER_KEY);
}

export function authHeaders() {
  const token = getToken();

  return {
    "Content-Type": "application/json",
    ...(token ? { Authorization: `Bearer ${token}` } : {}),
  };
}
