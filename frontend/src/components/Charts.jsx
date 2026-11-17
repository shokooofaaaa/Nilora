import { toFa } from '../utils/format';

// ---------- نمودار ستونی توزیع امتیازها ----------
// ستاره ۱ سمت چپ و ستاره ۵ سمت راست است (مثل طرح)
export function RatingBars({ data }) {
  const max = Math.max(...data.map((d) => d.percent));
  return (
    <div className="bars" dir="ltr">
      {data.map((d, i) => (
        <div className="bar-col" key={d.stars}>
          <span className="bar-percent">{toFa(d.percent)}٪</span>
          <div className="bar" style={{ height: `${(d.percent / max) * 78}%`, opacity: 0.45 + i * 0.14 }} />
          <span className="bar-label" dir="rtl">{toFa(d.stars)} ستاره</span>
        </div>
      ))}
    </div>
  );
}

// ---------- نمودار خطی روند رضایت (SVG خالص، بدون کتابخانه) ----------
export function TrendChart({ data, min = 3, max = 5 }) {
  const W = 400, H = 210, L = 34, R = 10, T = 24, B = 26;
  const pw = W - L - R, ph = H - T - B;
  const x = (i) => L + ((i + 0.5) * pw) / data.length;
  const y = (v) => T + ph - ((v - min) / (max - min)) * ph;
  const pts = data.map((d, i) => [x(i), y(d.value)]);
  const line = pts.map((p) => p.join(',')).join(' ');
  const area = `${L},${T + ph} ${line} ${pts[pts.length - 1][0]},${T + ph}`;
  const ticks = [5, 4.5, 4, 3.5, 3];
  const [lx, ly] = pts[pts.length - 1];

  return (
    <svg viewBox={`0 0 ${W} ${H}`} className="trend" direction="ltr" role="img" aria-label="روند رضایت مشتریان">
      <defs>
        <linearGradient id="areaFill" x1="0" y1="0" x2="0" y2="1">
          <stop offset="0" stopColor="#e0489a" stopOpacity="0.22" />
          <stop offset="1" stopColor="#e0489a" stopOpacity="0.02" />
        </linearGradient>
      </defs>
      {/* خطوط افقی و برچسب محور عمودی */}
      {ticks.map((t) => (
        <g key={t}>
          <line x1={L} x2={W - R} y1={y(t)} y2={y(t)} stroke="#f0dbe8" />
          <text x={L - 8} y={y(t) + 4} textAnchor="end" className="axis">{toFa(t)}</text>
        </g>
      ))}
      {/* خطوط عمودی و برچسب روزها */}
      {data.map((d, i) => (
        <g key={d.day}>
          <line x1={L + (i * pw) / data.length} x2={L + (i * pw) / data.length} y1={T} y2={T + ph} stroke="#f0dbe8" />
          <text x={x(i)} y={H - 6} textAnchor="middle" className="axis">{d.day}</text>
        </g>
      ))}
      <line x1={W - R} x2={W - R} y1={T} y2={T + ph} stroke="#f0dbe8" />
      <polygon points={area} fill="url(#areaFill)" />
      <polyline points={line} fill="none" stroke="#d6348a" strokeWidth="1.8" strokeLinejoin="round" />
      {pts.map(([px, py], i) => (
        <circle key={i} cx={px} cy={py} r={i === pts.length - 1 ? 4.5 : 3.5} fill="#d6348a" />
      ))}
      {/* حباب مقدار آخر */}
      <rect x={lx - 26} y={ly - 38} width="52" height="24" rx="12" fill="#fff" stroke="#e7cfdd" />
      <text x={lx} y={ly - 22} textAnchor="middle" className="bubble">{toFa(data[data.length - 1].value)}</text>
    </svg>
  );
}

// ---------- نمودار دونات تحلیل موضوعی ----------
export function Donut({ data }) {
  const r = 40, C = 2 * Math.PI * r;
  let offset = 0;
  return (
    <div className="donut-wrap">
      <ul className="legend">
        {data.map((d) => (
          <li key={d.label}>
            <span className="dot" style={{ background: d.color }} />
            <span className="legend-label">{d.label}</span>
            <span className="legend-percent">{toFa(d.percent)}٪</span>
          </li>
        ))}
      </ul>
      <svg viewBox="0 0 120 120" className="donut" role="img" aria-label="تحلیل موضوعی نظرات">
        <g transform="rotate(-90 60 60)">
          {data.map((d) => {
            const len = (d.percent / 100) * C;
            const el = (
              <circle key={d.label} cx="60" cy="60" r={r} fill="none" stroke={d.color} strokeWidth="24"
                      strokeDasharray={`${len} ${C - len}`} strokeDashoffset={-offset} />
            );
            offset += len;
            return el;
          })}
        </g>
      </svg>
    </div>
  );
}
