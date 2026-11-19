import { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { User, Lock } from 'lucide-react';
import AuthLayout from '../components/AuthLayout.jsx';
import Field from '../components/Field.jsx';
import { useAuth } from '../context/AuthContext.jsx';
import { AUTH_TEST_MODE } from '../config.js';

// صفحه ورود
export default function Login() {
  const { login } = useAuth();
  const navigate = useNavigate();
  const [form, setForm] = useState({ identifier: '', password: '' });
  const [error, setError] = useState('');

  const onChange = (e) => setForm({ ...form, [e.target.name]: e.target.value });

  const onSubmit = (e) => {
    e.preventDefault();
    setError('');
    // در حالت تست اعتبارسنجی انجام نمی‌شود
    if (!AUTH_TEST_MODE && (!form.identifier.trim() || !form.password)) {
      setError('شماره موبایل/ایمیل و رمز عبور را وارد کنید.');
      return;
    }
    // TODO: اتصال به بک‌اند:
    // const res = await fetch('/api/auth/login', { method: 'POST', body: JSON.stringify(form) });
    // اگر موفق بود، توکن/اطلاعات کاربر را به login بدهید.
    login({ name: form.identifier.trim() || 'کاربر تست' });
    navigate('/', { replace: true });
  };

  return (
    <AuthLayout
      title="خوش آمدید 💗"
      subtitle="برای ورود به پنل کاربری، اطلاعات خود را وارد کنید"
      footer={<>حساب کاربری ندارید؟ <Link to="/register">ثبت‌نام کنید</Link></>}
    >
      <form className="auth-form" onSubmit={onSubmit} noValidate>
        <Field icon={User} name="identifier" placeholder="شماره موبایل یا ایمیل" autoComplete="username"
               value={form.identifier} onChange={onChange} />
        <Field icon={Lock} type="password" name="password" placeholder="رمز عبور" autoComplete="current-password"
               value={form.password} onChange={onChange} />
        <div className="auth-row">
          <label className="check"><input type="checkbox" /> مرا به خاطر بسپار</label>
          <a href="#" onClick={(e) => e.preventDefault()}>فراموشی رمز عبور؟</a>
        </div>
        {error && <div className="auth-error">{error}</div>}
        <button type="submit" className="auth-btn">ورود</button>
        {AUTH_TEST_MODE && <p className="test-note">حالت تست فعال است: با زدن «ورود» مستقیم به داشبورد می‌روید.</p>}
      </form>
    </AuthLayout>
  );
}
