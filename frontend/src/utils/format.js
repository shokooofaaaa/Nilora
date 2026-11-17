// تبدیل اعداد انگلیسی به فارسی (مثلاً 4.75 → ۴.۷۵)
export const toFa = (v) => String(v).replace(/\d/g, (d) => '۰۱۲۳۴۵۶۷۸۹'[d]);
