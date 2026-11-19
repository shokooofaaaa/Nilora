import { Home, User, Image as ImageIcon, Star, Bell, Settings, LogOut } from 'lucide-react';
import Logo from './Logo.jsx';
import { useAuth } from '../context/AuthContext.jsx';

// آیتم‌های منو؛ برای مسیرها می‌توانید بعداً از react-router استفاده کنید
const items = [
  { label: 'داشبورد', icon: Home, active: true },
  { label: 'پروفایل من', icon: User },
  { label: 'نمونه‌کارها', icon: ImageIcon },
  { label: 'نظرات مشتریان', icon: Star },
  { label: 'هشدارهای حفظ مشتری', icon: Bell, small: true },
  { label: 'تنظیمات', icon: Settings },
];

// نوار کناری سمت راست: لوگو + منو + دکمه خروج + کارت تبلیغاتی پایین
export default function Sidebar() {
  const { logout } = useAuth(); // خروج از حساب
  return (
    <aside className="sidebar">
      <Logo />

      <nav className="menu">
        {items.map(({ label, icon: Icon, active, small }) => (
          <a key={label} href="#" className={`menu-item${active ? ' active' : ''}${small ? ' small' : ''}`}
             onClick={(e) => e.preventDefault()}>
            <Icon size={18} />
            <span>{label}</span>
          </a>
        ))}
        {/* دکمه خروج: بعد از زدن، به صفحه ورود برمی‌گردید */}
        <button type="button" className="menu-item logout" onClick={logout}>
          <LogOut size={18} />
          <span>خروج از حساب</span>
        </button>
      </nav>

      <div className="promo">
        <span className="promo-heart">♥</span>
        <p>با نیلورا،<br />اعتماد مشتریانت را<br />بیشتر کنید...</p>
      </div>
    </aside>
  );
}
