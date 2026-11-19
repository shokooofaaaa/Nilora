import { Bell, ChevronDown, CalendarDays, ChevronsUpDown, Heart } from 'lucide-react';
import { toFa } from '../utils/format';

// هدر: سمت راست خوش‌آمدگویی، سمت چپ فیلتر زمانی، زنگ اعلان و پروفایل
export default function Header({ user }) {
  return (
    <header className="header">
      <div className="greeting">
        <h1><Heart size={14} fill="currentColor" className="greet-heart" /> سلام {user.name.split(' ')[0]} جان!</h1>
        <p>خلاصه عملکرد شما در یک نگاه</p>
      </div>

      <div className="header-tools">
        {/* فیلتر بازه زمانی */}
        <button className="pill" type="button">
          <CalendarDays size={16} />
          <span>۷ روز گذشته</span>
          <ChevronsUpDown size={14} />
        </button>

        {/* زنگ اعلان‌ها با تعداد */}
        <button className="bell" type="button" aria-label="اعلان‌ها">
          <Bell size={22} />
          <span className="badge">{toFa(user.notifications)}</span>
        </button>

        {/* پروفایل کاربر */}
        <div className="profile">
          <div className="avatar-lg">{user.avatar ? <img src={user.avatar} alt="" /> : user.name[0]}</div>
          <div className="profile-text">
            <strong>{user.name}</strong>
            <small>{user.role}</small>
          </div>
          <ChevronDown size={16} />
        </div>
      </div>
    </header>
  );
}
