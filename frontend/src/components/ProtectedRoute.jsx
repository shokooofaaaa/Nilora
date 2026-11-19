
import { Navigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext.jsx';

export function ProtectedRoute({ children }) {
    const { isAuthenticated, authLoading } = useAuth();

    if (authLoading) {
        return <div className="auth-loading">در حال بررسی وضعیت ورود...</div>;
    }

    return isAuthenticated
        ? children
        : <Navigate to="/login" replace />;
}

export function PublicOnlyRoute({ children }) {
    const { isAuthenticated, authLoading } = useAuth();

    if (authLoading) {
        return <div className="auth-loading">در حال بررسی وضعیت ورود...</div>;
    }

    return isAuthenticated
        ? <Navigate to="/" replace />
        : children;
}
