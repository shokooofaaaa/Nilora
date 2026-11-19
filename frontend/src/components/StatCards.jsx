import { Heart, Check, MessageCircle, Star, ArrowUp } from 'lucide-react';
import { toFa } from '../utils/format';

// آیکن هر کارت (با کلید icon در دیتا)
const icons = {
  heart: <Heart size={34} fill="#f08a8a" color="#f08a8a" />,
  check: <span className="check-circle"><Check size={20} strokeWidth={3} color="#fff" /></span>,
  chat: <MessageCircle size={36} fill="#a56bd6" color="#a56bd6" />,
  star: <Star size={34} fill="#d6348a" color="#d6348a" />,
};

// چهار کارت آماری بالای صفحه
export default function StatCards({ stats }) {
  return (
    <section className="stats">
      {stats.map((s) => (
        <article key={s.id} className={`stat tone-${s.tone}`}>
          <div className="stat-text">
            <div className="stat-value">{toFa(s.value)}{s.suffix}</div>
            <div className="stat-label">{s.label}</div>
            <div className="stat-delta"><ArrowUp size={12} /> {s.delta}</div>
          </div>
          <div className="stat-icon">{icons[s.icon]}</div>
        </article>
      ))}
    </section>
  );
}
