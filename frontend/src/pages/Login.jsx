
import { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { User, Lock } from 'lucide-react';
import AuthLayout from '../components/AuthLayout.jsx';
import Field from '../components/Field.jsx';
import { useAuth } from '../context/AuthContext.jsx';
import { authApi } from '../utils/api.js';

export default function Login() {
    const { login } = useAuth();
    const navigate = useNavigate();

    const [form, setForm] = useState({
        identifier: '',
        password: '',
    });

    const [error, setError] = useState('');
    const [loading, setLoading] = useState(false);

    const onChange = (e) => {
        setForm((previous) => ({
            ...previous,
            [e.target.name]: e.target.value,
        }));
    };

    const onSubmit = async (e) => {
        e.preventDefault();
        setError('');

        const phoneNumber = form.identifier.trim();

        if (!/^09\d{9}$/.test(phoneNumber)) {
            setError('شماره موبایل معتبر نیست.');
            return;
        }

        if (!form.password) {
            setError('رمز عبور را وارد کنید.');
            return;
        }

        setLoading(true);

        try {
            const response = await authApi.login({
                phoneNumber,
                password: form.password,
            });

            await login(response);

            navigate('/', { replace: true });
        } catch (error) {
            setError(error.message || 'ورود انجام نشد.');
        } finally {
            setLoading(false);
        }
    };

    return (
        <AuthLayout
            title="خوش آمدید 💗"
            subtitle="برای ورود به پنل کاربری، اطلاعات خود را وارد کنید"
            footer={
                <>
                    حساب کاربری ندارید؟ <Link to="/register">ثبت‌نام کنید</Link>
                </>
            }
        >
            <form className="auth-form" onSubmit={onSubmit} noValidate>
                <Field
                    icon={User}
                    name="identifier"
                    placeholder="شماره موبایل"
                    autoComplete="username"
                    inputMode="numeric"
                    value={form.identifier}
                    onChange={onChange}
                />

                <Field
                    icon={Lock}
                    type="password"
                    name="password"
                    placeholder="رمز عبور"
                    autoComplete="current-password"
                    value={form.password}
                    onChange={onChange}
                />

                <div className="auth-row">
                    <label className="check">
                        <input type="checkbox" />
                        مرا به خاطر بسپار
                    </label>

                    <a href="#" onClick={(e) => e.preventDefault()}>
                        فراموشی رمز عبور؟
                    </a>
                </div>

                {error && (
                    <div className="auth-error">{error}</div>
                )}

                <button
                    type="submit"
                    className="auth-btn"
                    disabled={loading}
                >
                    {loading ? 'در حال ورود...' : 'ورود'}
                </button>
            </form>
        </AuthLayout>
    );
}
