import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { AuthProvider } from './context/AuthContext.jsx';
import { ProtectedRoute, PublicOnlyRoute } from './components/ProtectedRoute.jsx';
import Dashboard from './pages/Dashboard.jsx';
import Login from './pages/Login.jsx';
import Register from './pages/Register.jsx';

// مسیرها (Routes):
//  /          → داشبورد (فقط برای کاربر واردشده؛ وگرنه می‌رود به /login)
//  /login     → ورود
//  /register  → ثبت‌نام
export default function App() {
  return (
    <AuthProvider>
      <BrowserRouter>
        <Routes>
          <Route path="/" element={<ProtectedRoute><Dashboard /></ProtectedRoute>} />
          <Route path="/login" element={<PublicOnlyRoute><Login /></PublicOnlyRoute>} />
          <Route path="/register" element={<PublicOnlyRoute><Register /></PublicOnlyRoute>} />
          <Route path="*" element={<Navigate to="/" replace />} />
        </Routes>
      </BrowserRouter>
    </AuthProvider>
  );
}
