import { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { User, Phone, Mail, Lock } from 'lucide-react';
import AuthLayout from '../components/AuthLayout.jsx';
import Field from '../components/Field.jsx';
import { useAuth } from '../context/AuthContext.jsx';
import { AUTH_TEST_MODE } from '../config.js';

// قوانین اعتبارسنجی فرم ثبت‌نام (در حالت تست نادیده گرفته می‌شود)
function validate(f) {
  const e = {};
  if (f.fullName.trim().length < 3) e.fullName = 'نام و نام خانوادگی را وارد کنید.';
  if (!/^09\d{9}$/.test(f.mobile)) e.mobile = 'شماره موبایل معتبر نیست (مثال: 09123456789).';
  if (f.email && !/^\S+@\S+\.\S+$/.test(f.email)) e.email = 'ایمیل معتبر نیست.';
  if (f.password.length < 8) e.password = 'رمز عبور حداقل ۸ کاراکتر باشد.';
  if (f.confirm !== f.password) e.confirm = 'تکرار رمز عبور مطابقت ندارد.';
  if (!f.terms) e.terms = 'پذیرش قوانین الزامی است.';
  return e;
}

// صفحه ثبت‌نام
export default function Register() {
  const { login } = useAuth();
  const navigate = useNavigate();
  const [form, setForm] = useState({ fullName: '', mobile: '', email: '', password: '', confirm: '', terms: false });
  const [errors, setErrors] = useState({});

  const onChange = (e) => {
    const { name, value, type, checked } = e.target;
    setForm({ ...form, [name]: type === 'checkbox' ? checked : value });
  };

  const onSubmit = (e) => {
    e.preventDefault();
    const errs = AUTH_TEST_MODE ? {} : validate(form);
    setErrors(errs);
    if (Object.keys(errs).length) return;
    // TODO: اتصال به بک‌اند:
    // await fetch('/api/auth/register', { method: 'POST', body: JSON.stringify(form) });
    login({ name: form.fullName.trim() || 'کاربر تست' });
    navigate('/', { replace: true });
  };

  return (
    <AuthLayout
      title="ایجاد حساب کاربری"
      subtitle="چند قدم تا شروع مسیر رشد کسب‌وکار شما"
      footer={<>قبلاً ثبت‌نام کرده‌اید؟ <Link to="/login">وارد شوید</Link></>}
    >
      <form className="auth-form" onSubmit={onSubmit} noValidate>
        <Field icon={User} name="fullName" placeholder="نام و نام خانوادگی" autoComplete="name"
               value={form.fullName} onChange={onChange} error={errors.fullName} />
        <Field icon={Phone} name="mobile" placeholder="شماره موبایل" inputMode="numeric" autoComplete="tel"
               value={form.mobile} onChange={onChange} error={errors.mobile} />
        <Field icon={Mail} type="email" name="email" placeholder="ایمیل (اختیاری)" autoComplete="email"
               value={form.email} onChange={onChange} error={errors.email} />
        <Field icon={Lock} type="password" name="password" placeholder="رمز عبور" autoComplete="new-password"
               value={form.password} onChange={onChange} error={errors.password} />
        <Field icon={Lock} type="password" name="confirm" placeholder="تکرار رمز عبور" autoComplete="new-password"
               value={form.confirm} onChange={onChange} error={errors.confirm} />
        <label className="check">
          <input type="checkbox" name="terms" checked={form.terms} onChange={onChange} />
          <span>قوانین و مقررات نیلورا را می‌پذیرم</span>
        </label>
        {errors.terms && <span className="field-error">{errors.terms}</span>}
        <button type="submit" className="auth-btn">ثبت‌نام</button>
        {AUTH_TEST_MODE && <p className="test-note">حالت تست فعال است: با زدن «ثبت‌نام» مستقیم به داشبورد می‌روید.</p>}
      </form>
    </AuthLayout>
  );
}
