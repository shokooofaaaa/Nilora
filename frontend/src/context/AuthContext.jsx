import { createContext, useContext, useState, useCallback, useMemo } from 'react';

// وضعیت ورود کاربر در کل برنامه. فعلاً در localStorage ذخیره می‌شود (حالت تست).
// بعداً به‌جای آن می‌توانید توکن واقعی (JWT) بک‌اند را ذخیره کنید.
const STORAGE_KEY = 'nilora_auth';
const AuthContext = createContext(null);

function readStored() {
  try {
    const raw = localStorage.getItem(STORAGE_KEY);
    return raw ? JSON.parse(raw) : null;
  } catch {
    return null; // اگر localStorage در دسترس نبود یا داده خراب بود
  }
}

export function AuthProvider({ children }) {
  const [user, setUser] = useState(readStored);

  // ورود: اطلاعات کاربر را نگه می‌دارد
  const login = useCallback((userData) => {
    setUser(userData);
    try { localStorage.setItem(STORAGE_KEY, JSON.stringify(userData)); } catch { /* نادیده */ }
  }, []);

  // خروج
  const logout = useCallback(() => {
    setUser(null);
    try { localStorage.removeItem(STORAGE_KEY); } catch { /* نادیده */ }
  }, []);

  const value = useMemo(() => ({ user, isAuthenticated: !!user, login, logout }), [user, login, logout]);
  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}

export function useAuth() {
  const ctx = useContext(AuthContext);
  if (!ctx) throw new Error('useAuth باید داخل AuthProvider استفاده شود');
  return ctx;
}
