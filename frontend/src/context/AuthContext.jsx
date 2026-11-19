
import {
    createContext,
    useContext,
    useState,
    useCallback,
    useMemo,
    useEffect,
} from 'react';

import { authApi } from '../utils/api.js';

const STORAGE_KEY = 'nilora_auth';

const AuthContext = createContext(null);

function readStored() {
    try {
        const raw = localStorage.getItem(STORAGE_KEY);
        return raw ? JSON.parse(raw) : null;
    } catch {
        return null;
    }
}

export function AuthProvider({ children }) {
    const [user, setUser] = useState(null);
    const [token, setToken] = useState(null);
    const [authLoading, setAuthLoading] = useState(true);

    // بررسی نشست قبلی هنگام باز شدن برنامه
    useEffect(() => {
        let cancelled = false;

        async function restoreSession() {
            const stored = readStored();

            if (!stored?.token) {
                localStorage.removeItem(STORAGE_KEY);
                if (!cancelled) setAuthLoading(false);
                return;
            }

            try {
                const currentUser = await authApi.getMe(stored.token);

                if (!cancelled) {
                    setToken(stored.token);
                    setUser(currentUser);
                }
            } catch {
                localStorage.removeItem(STORAGE_KEY);

                if (!cancelled) {
                    setToken(null);
                    setUser(null);
                }
            } finally {
                if (!cancelled) setAuthLoading(false);
            }
        }

        restoreSession();

        return () => {
            cancelled = true;
        };
    }, []);

    // ورود یا ثبت‌نام با پاسخ واقعی بک‌اند
    const login = useCallback(async (authResponse) => {
        if (!authResponse?.token) {
            throw new Error('توکن ورود از سرور دریافت نشد.');
        }

        const currentUser = await authApi.getMe(authResponse.token);

        const session = {
            token: authResponse.token,
            user: currentUser,
            expiresAtUtc: authResponse.expiresAtUtc,
        };

        localStorage.setItem(STORAGE_KEY, JSON.stringify(session));

        setToken(authResponse.token);
        setUser(currentUser);

        return currentUser;
    }, []);

    // خروج از حساب
    const logout = useCallback(() => {
        localStorage.removeItem(STORAGE_KEY);
        setToken(null);
        setUser(null);
    }, []);

    const value = useMemo(
        () => ({
            user,
            token,
            authLoading,
            isAuthenticated: !!user && !!token,
            login,
            logout,
        }),
        [user, token, authLoading, login, logout]
    );

    return (
        <AuthContext.Provider value={value}>
            {children}
        </AuthContext.Provider>
    );
}

export function useAuth() {
    const ctx = useContext(AuthContext);

    if (!ctx) {
        throw new Error('useAuth باید داخل AuthProvider استفاده شود');
    }

    return ctx;
}
