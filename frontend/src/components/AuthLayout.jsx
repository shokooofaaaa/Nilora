import '../pages/Auth.css';

// قالب مشترک صفحه ورود و ثبت‌نام: پس‌زمینه صورتی + کارت وسط صفحه + لوگو
export default function AuthLayout({ title, subtitle, children, footer }) {
  return (
    <div className="auth-page">
      <div className="auth-card">
        <div className="auth-logo">
          <svg viewBox="0 0 64 64" width="56" height="56" aria-hidden="true">
            <path d="M34 6c-12 0-20 9-18 22 1 8-3 12-3 20 0 6 5 10 10 10 4 0 6-3 6-7 0-5 3-7 7-9 8-4 10-14 6-24-2-8-5-12-8-12z" fill="#b0306f" />
            <circle cx="16" cy="22" r="7" fill="#e24b94" />
          </svg>
          <div className="auth-brand">نیلورا</div>
          <div className="auth-slogan">اعتماد، ماندگاری، رشد</div>
        </div>
        <h1 className="auth-title">{title}</h1>
        <p className="auth-sub">{subtitle}</p>
        {children}
      </div>
      <p className="auth-foot">{footer}</p>
    </div>
  );
}
