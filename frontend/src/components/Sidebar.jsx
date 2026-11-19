
import {
    Home,
    User,
    Image as ImageIcon,
    Star,
    Bell,
    Settings,
    LogOut,
} from 'lucide-react';

import { NavLink } from 'react-router-dom';
import Logo from './Logo.jsx';
import { useAuth } from '../context/AuthContext.jsx';

const items = [
    { label: 'داشبورد', icon: Home, path: '/' },
    { label: 'پروفایل من', icon: User, path: '/profile' },
    { label: 'نمونه‌کارها', icon: ImageIcon, path: '/portfolio' },
    { label: 'نظرات مشتریان', icon: Star, path: '/reviews' },
    {
        label: 'هشدارهای حفظ مشتری',
        icon: Bell,
        path: '/retention',
        small: true,
    },
    { label: 'تنظیمات', icon: Settings, path: '/settings' },
];

export default function Sidebar() {
    const { logout } = useAuth();

    return (
        <aside className="sidebar">
            <Logo />

            <nav className="menu">
                {items.map(({ label, icon: Icon, path, small }) => (
                    <NavLink
                        key={path}
                        to={path}
                        end={path === '/'}
                        className={({ isActive }) =>
                            `menu-item${isActive ? ' active' : ''}${small ? ' small' : ''}`
                        }
                    >
                        <Icon size={18} />
                        <span>{label}</span>
                    </NavLink>
                ))}

                <button
                    type="button"
                    className="menu-item logout"
                    onClick={logout}
                >
                    <LogOut size={18} />
                    <span>خروج از حساب</span>
                </button>
            </nav>

            <div className="promo">
                <span className="promo-heart">♥</span>
                <p>
                    با نیلورا،
                    <br />
                    اعتماد مشتریانت را
                    <br />
                    بیشتر کنید...
                </p>
            </div>
        </aside>
    );
}
