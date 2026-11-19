import { Navigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext.jsx';

// صفحات خصوصی (مثل داشبورد): اگر کاربر وارد نشده باشد به /login می‌رود
export function ProtectedRoute({ children }) {
  const { isAuthenticated } = useAuth();
  return isAuthenticated ? children : <Navigate to="/login" replace />;
}

// صفحات عمومی (ورود/ثبت‌نام): اگر کاربر قبلاً وارد شده باشد مستقیم به داشبورد می‌رود
export function PublicOnlyRoute({ children }) {
  const { isAuthenticated } = useAuth();
  return isAuthenticated ? <Navigate to="/" replace /> : children;
}
