import { BarChart3, TrendingUp, Cpu, Trophy, Lightbulb, MessageCircle, Bell } from 'lucide-react';
import Sidebar from '../components/Sidebar.jsx';
import Header from '../components/Header.jsx';
import StatCards from '../components/StatCards.jsx';
import Card from '../components/Card.jsx';
import { RatingBars, TrendChart, Donut } from '../components/Charts.jsx';
import { Strengths, Improvements, Reviews, Alerts } from '../components/Panels.jsx';
import * as data from '../data/dashboardData.js';

/*
  اتصال به بک‌اند (راهنما):
  ۱) داده‌ی دیتا را با useState/useEffect و fetch('/api/dashboard') بگیرید
  ۲) ساختار پاسخ API را مثل export های dashboardData.js بسازید (user, stats, ...)
  ۳) به‌جای `data.xxx` از state استفاده کنید.
*/
export default function Dashboard() {
  return (
    <div className="layout">
      {/* ===== نوار کناری (سمت راست) ===== */}
      <Sidebar />

      <main className="main">
        {/* ===== هدر ===== */}
        <Header user={data.user} />

        {/* ===== کارت‌های آماری ===== */}
        <StatCards stats={data.stats} />

        {/* ===== ردیف نمودارها (ترتیب از راست به چپ) ===== */}
        <div className="row row-charts">
          <Card title="تحلیل موضوعی نظرات" icon={<span className="ai-chip"><Cpu size={16} /></span>}>
            <Donut data={data.topics} />
          </Card>
          <Card title="روند رضایت مشتریان" icon={<TrendingUp size={18} color="#d6348a" />}>
            <TrendChart data={data.satisfactionTrend} />
          </Card>
          <Card title="توزیع امتیازها" icon={<BarChart3 size={18} color="#b0489a" />}>
            <RatingBars data={data.ratingDistribution} />
          </Card>
        </div>

        {/* ===== ردیف پایین ===== */}
        <div className="row row-bottom">
          <Card title="هشدارهای مهم" icon={<Bell size={20} color="#b0306f" />} className="card-alerts">
            <Alerts items={data.alerts} onAction={(a) => console.log('alert action:', a.id)} />
          </Card>

          <Card title="آخرین نظرات مشتریان" icon={<MessageCircle size={20} color="#8a5ac8" />} className="card-reviews">
            <Reviews items={data.reviews} />
            <a href="#" className="see-all" onClick={(e) => e.preventDefault()}>مشاهده همه ←</a>
          </Card>

          <div className="stack">
            <Card title="نقاط قوت شما" icon={<Trophy size={20} color="#c9962b" />}>
              <Strengths items={data.strengths} />
            </Card>
            <Card title="زمینه‌های نیازمند بهبود" icon={<Lightbulb size={20} color="#f0a030" />} className="card-improve">
              <Improvements items={data.improvements} />
            </Card>
          </div>
        </div>
      </main>
    </div>
  );
}
