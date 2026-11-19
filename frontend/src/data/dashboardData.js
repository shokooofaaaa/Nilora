// =====================================================================
// داده‌های نمونه (Mock). وقتی بک‌اند آماده شد، فقط همین فایل را با
// فراخوانی API جایگزین کنید (به توضیح بالای App.jsx نگاه کنید).
// =====================================================================

// اطلاعات کاربر واردشده
export const user = { name: 'سارا احمدی', role: 'ناخن‌کار حرفه‌ای', avatar: '', notifications: 3 };

// کارت‌های آماری بالای صفحه. tone یکی از: orange | green | purple | pink
export const stats = [
  { id: 'return', label: 'بازگشت مشتریان', value: 89, suffix: '٪', delta: '+۷٪', tone: 'orange', icon: 'heart' },
  { id: 'satisfaction', label: 'رضایت مشتریان', value: 92, suffix: '٪', delta: '+۵٪', tone: 'green', icon: 'check' },
  { id: 'count', label: 'تعداد کل نظرات', value: 24, suffix: '', delta: '+۱۲٪', tone: 'purple', icon: 'chat' },
  { id: 'avg', label: 'میانگین امتیاز شما', value: 4.75, suffix: '', delta: '+۰.۲', tone: 'pink', icon: 'star' },
];

// توزیع امتیازها (درصد هر ستاره)
export const ratingDistribution = [
  { stars: 1, percent: 4 }, { stars: 2, percent: 8 }, { stars: 3, percent: 12 },
  { stars: 4, percent: 33 }, { stars: 5, percent: 43 },
];

// روند رضایت هفته (از شنبه تا جمعه)
export const satisfactionTrend = [
  { day: 'شنبه', value: 3.7 }, { day: 'یکشنبه', value: 4.2 }, { day: 'دوشنبه', value: 3.9 },
  { day: 'سه‌شنبه', value: 4.4 }, { day: 'چهارشنبه', value: 4.6 }, { day: 'پنجشنبه', value: 4.4 },
  { day: 'جمعه', value: 4.75 },
];

// تحلیل موضوعی نظرات (مجموع = ۱۰۰)
export const topics = [
  { label: 'کیفیت و دوام', percent: 32, color: '#a2307a' },
  { label: 'بهداشت و تمیزی', percent: 25, color: '#e07ab8' },
  { label: 'رفتار و ارتباط', percent: 20, color: '#f4aad6' },
  { label: 'زمان انتظار', percent: 13, color: '#e3bdf3' },
  { label: 'قیمت و ارزش', percent: 10, color: '#b25ec4' },
];

export const strengths = ['کیفیت بالای خدمات', 'رفتار حرفه‌ای و صمیمی', 'تمیزی و رعایت بهداشت'];
export const improvements = ['کاهش زمان انتظار', 'شفافیت بیشتر در قیمت‌ها'];

// آخرین نظرات؛ rating عدد ۱ تا ۵
export const reviews = [
  { id: 1, time: '۲ ساعت پیش', rating: 5, text: 'عالی‌بود! خیلی راضی هستم', emoji: '❤️' },
  { id: 2, time: '۵ ساعت پیش', rating: 4, text: 'کیفیت کار فوق‌العاده بود.', emoji: '' },
  { id: 3, time: '۱ روز پیش', rating: 4, text: 'ممنون از برخورد خوبتون', emoji: '🌸' },
];

// هشدارها: type = danger | suggest
export const alerts = [
  { id: 1, type: 'danger', title: 'کاهش مراجعه مشتری «مینا کریمی»', desc: 'آخرین مراجعه: ۶۰ روز پیش', button: 'مشاهده جزئیات' },
  { id: 2, type: 'suggest', title: 'پیشنهاد بهبود', desc: 'افزایش تنوع طرح‌های ژورنالی', button: 'مشاهده پیشنهاد' },
];
