
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { AuthProvider } from './context/AuthContext.jsx';
import { ProtectedRoute, PublicOnlyRoute } from './components/ProtectedRoute.jsx';

import Dashboard from './pages/Dashboard.jsx';
import Login from './pages/Login.jsx';
import Register from './pages/Register.jsx';

function PlaceholderPage({ title }) {
    return (
        <div style={{ padding: '40px', direction: 'rtl' }}>
            <h1>{title}</h1>
            <p>این صفحه در حال آماده‌سازی است.</p>
        </div>
    );
}

export default function App() {
    return (
        <AuthProvider>
            <BrowserRouter>
                <Routes>
                    <Route
                        path="/"
                        element={
                            <ProtectedRoute>
                                <Dashboard />
                            </ProtectedRoute>
                        }
                    />

                    <Route
                        path="/profile"
                        element={<ProtectedRoute><PlaceholderPage title="پروفایل من" /></ProtectedRoute>}
                    />

                    <Route
                        path="/portfolio"
                        element={<ProtectedRoute><PlaceholderPage title="نمونه‌کارها" /></ProtectedRoute>}
                    />

                    <Route
                        path="/reviews"
                        element={<ProtectedRoute><PlaceholderPage title="نظرات مشتریان" /></ProtectedRoute>}
                    />

                    <Route
                        path="/retention"
                        element={<ProtectedRoute><PlaceholderPage title="هشدارهای حفظ مشتری" /></ProtectedRoute>}
                    />

                    <Route
                        path="/settings"
                        element={<ProtectedRoute><PlaceholderPage title="تنظیمات" /></ProtectedRoute>}
                    />

                    <Route
                        path="/login"
                        element={
                            <PublicOnlyRoute>
                                <Login />
                            </PublicOnlyRoute>
                        }
                    />

                    <Route
                        path="/register"
                        element={
                            <PublicOnlyRoute>
                                <Register />
                            </PublicOnlyRoute>
                        }
                    />

                    <Route path="*" element={<Navigate to="/" replace />} />
                </Routes>
            </BrowserRouter>
        </AuthProvider>
    );
}
