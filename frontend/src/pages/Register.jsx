
import { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { User, Phone, Mail, Lock } from 'lucide-react';
import AuthLayout from '../components/AuthLayout.jsx';
import Field from '../components/Field.jsx';
import { useAuth } from '../context/AuthContext.jsx';
import { authApi } from '../utils/api.js';

function validate(f) {
    const e = {};
    const nameParts = f.fullName.trim().split(/\s+/);

    if (nameParts.length < 2) {
        e.fullName = 'نام و نام خانوادگی را وارد کنید.';
    }

    if (!/^09\d{9}$/.test(f.mobile)) {
        e.mobile = 'شماره موبایل معتبر نیست (مثال: 09123456789).';
    }

    if (f.password.length < 8) {
        e.password = 'رمز عبور حداقل ۸ کاراکتر باشد.';
    }

    if (f.confirm !== f.password) {
        e.confirm = 'تکرار رمز عبور مطابقت ندارد.';
    }

    if (!f.terms) {
        e.terms = 'پذیرش قوانین الزامی است.';
    }

    return e;
}

export default function Register() {
    const { login } = useAuth();
    const navigate = useNavigate();

    const [form, setForm] = useState({
        fullName: '',
        mobile: '',
        email: '',
        password: '',
        confirm: '',
        terms: false,
    });

    const [errors, setErrors] = useState({});
    const [serverError, setServerError] = useState('');
    const [loading, setLoading] = useState(false);

    const onChange = (e) => {
        const { name, value, type, checked } = e.target;

        setForm((previous) => ({
            ...previous,
            [name]: type === 'checkbox' ? checked : value,
        }));
    };

    const onSubmit = async (e) => {
        e.preventDefault();

        setServerError('');

        const validationErrors = validate(form);
        setErrors(validationErrors);

        if (Object.keys(validationErrors).length > 0) return;

        const nameParts = form.fullName.trim().split(/\s+/);
        const firstName = nameParts.shift();
        const lastName = nameParts.join(' ');

        setLoading(true);

        try {
            const response = await authApi.register({
                phoneNumber: form.mobile.trim(),
                password: form.password,
                firstName,
                lastName,
            });

            await login(response);

            navigate('/', { replace: true });
        } catch (error) {
            setServerError(error.message || 'ثبت‌نام انجام نشد.');
        } finally {
            setLoading(false);
        }
    };

    return (
        <AuthLayout
            title="ایجاد حساب کاربری"
            subtitle="چند قدم تا شروع مسیر رشد کسب‌وکار شما"
            footer={
                <>
                    قبلاً ثبت‌نام کرده‌اید؟ <Link to="/login">وارد شوید</Link>
                </>
            }
        >
            <form className="auth-form" onSubmit={onSubmit} noValidate>
                <Field
                    icon={User}
                    name="fullName"
                    placeholder="نام و نام خانوادگی"
                    autoComplete="name"
                    value={form.fullName}
                    onChange={onChange}
                    error={errors.fullName}
                />

                <Field
                    icon={Phone}
                    name="mobile"
                    placeholder="شماره موبایل"
                    inputMode="numeric"
                    autoComplete="tel"
                    value={form.mobile}
                    onChange={onChange}
                    error={errors.mobile}
                />

                <Field
                    icon={Mail}
                    type="email"
                    name="email"
                    placeholder="ایمیل (اختیاری)"
                    autoComplete="email"
                    value={form.email}
                    onChange={onChange}
                />

                <Field
                    icon={Lock}
                    type="password"
                    name="password"
                    placeholder="رمز عبور"
                    autoComplete="new-password"
                    value={form.password}
                    onChange={onChange}
                    error={errors.password}
                />

                <Field
                    icon={Lock}
                    type="password"
                    name="confirm"
                    placeholder="تکرار رمز عبور"
                    autoComplete="new-password"
                    value={form.confirm}
                    onChange={onChange}
                    error={errors.confirm}
                />

                <label className="check">
                    <input
                        type="checkbox"
                        name="terms"
                        checked={form.terms}
                        onChange={onChange}
                    />
                    <span>قوانین و مقررات نیلورا را می‌پذیرم</span>
                </label>

                {errors.terms && (
                    <span className="field-error">{errors.terms}</span>
                )}

                {serverError && (
                    <div className="auth-error">{serverError}</div>
                )}

                <button
                    type="submit"
                    className="auth-btn"
                    disabled={loading}
                >
                    {loading ? 'در حال ثبت‌نام...' : 'ثبت‌نام'}
                </button>
            </form>
        </AuthLayout>
    );
}
