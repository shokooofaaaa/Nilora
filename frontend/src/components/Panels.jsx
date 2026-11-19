import { Check, TriangleAlert, Star, User } from 'lucide-react';
import { toFa } from '../utils/format';

// لیست نقاط قوت (تیک سبز)
export function Strengths({ items }) {
  return (
    <ul className="check-list">
      {items.map((t) => (
        <li key={t}><span className="round ok"><Check size={13} strokeWidth={3} /></span>{t}</li>
      ))}
    </ul>
  );
}

// لیست زمینه‌های نیازمند بهبود (علامت تعجب نارنجی)
export function Improvements({ items }) {
  return (
    <ul className="check-list">
      {items.map((t) => (
        <li key={t}><span className="round warn">!</span>{t}</li>
      ))}
    </ul>
  );
}

// ستاره‌های امتیاز (۵ ستاره؛ پرشده‌ها زرد)
function Stars({ rating }) {
  return (
    <span className="stars" dir="ltr" aria-label={`${toFa(rating)} از ۵`}>
      {[1, 2, 3, 4, 5].map((n) => (
        <Star key={n} size={14} fill={n <= rating ? '#f5b400' : '#f3dfa8'} color={n <= rating ? '#f5b400' : '#f3dfa8'} />
      ))}
    </span>
  );
}

// آخرین نظرات مشتریان
export function Reviews({ items }) {
  return (
    <ul className="reviews">
      {items.map((r) => (
        <li key={r.id}>
          <div className="review-avatar"><User size={22} /></div>
          <div className="review-body">
            <Stars rating={r.rating} />
            <p>{r.text} {r.emoji}</p>
          </div>
          <time>{r.time}</time>
        </li>
      ))}
    </ul>
  );
}

// کارت‌های هشدار و پیشنهاد
export function Alerts({ items, onAction }) {
  return (
    <div className="alerts">
      {items.map((a) => (
        <div key={a.id} className={`alert ${a.type}`}>
          <div className="alert-head">
            {a.type === 'danger' && <span className="alert-icon"><TriangleAlert size={20} fill="#e0364f" color="#fff" /></span>}
            <strong>{a.title}</strong>
          </div>
          <p>{a.desc}</p>
          <button type="button" className="alert-btn" onClick={() => onAction?.(a)}>{a.button}</button>
        </div>
      ))}
    </div>
  );
}
