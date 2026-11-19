import { useState } from 'react';
import { Eye, EyeOff } from 'lucide-react';

// فیلد ورودی با آیکن، پیام خطا و (برای رمز عبور) دکمه نمایش/مخفی کردن
export default function Field({ icon: Icon, type = 'text', error, ...props }) {
  const [show, setShow] = useState(false);
  const isPass = type === 'password';
  return (
    <div className="field-wrap">
      <div className={`field${error ? ' has-error' : ''}`}>
        {Icon && <Icon size={18} className="field-icon" />}
        <input type={isPass && show ? 'text' : type} {...props} />
        {isPass && (
          <button type="button" className="field-eye" onClick={() => setShow((s) => !s)}
                  aria-label={show ? 'مخفی کردن رمز' : 'نمایش رمز'}>
            {show ? <EyeOff size={18} /> : <Eye size={18} />}
          </button>
        )}
      </div>
      {error && <span className="field-error">{error}</span>}
    </div>
  );
}
