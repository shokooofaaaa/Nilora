// کارت سفید عمومی با عنوان و آیکن (آیکن سمت راست عنوان قرار می‌گیرد)
export default function Card({ title, icon, className = '', children }) {
  return (
    <section className={`card ${className}`}>
      {title && (
        <h2 className="card-title">
          <span className="card-title-icon">{icon}</span>
          {title}
        </h2>
      )}
      {children}
    </section>
  );
}
