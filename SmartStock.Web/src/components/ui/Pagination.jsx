/**
 * Smart pagination component with max 7 visible buttons.
 *
 * @param {Object} props
 * @param {number} props.page       - Current page (1-indexed)
 * @param {number} props.totalPages - Total number of pages
 * @param {Function} props.onChange - Called with the new page number
 */
export default function Pagination({ page, totalPages, onChange }) {
  if (totalPages <= 1) return null;

  const maxButtons = 7;
  let start, end;

  if (totalPages <= maxButtons) {
    start = 1;
    end = totalPages;
  } else {
    start = Math.max(1, page - 3);
    end = Math.min(totalPages, start + maxButtons - 1);
    if (end - start < maxButtons - 1) {
      start = Math.max(1, end - maxButtons + 1);
    }
  }

  const range = [];
  for (let i = start; i <= end; i++) range.push(i);

  return (
    <div style={{ display: 'flex', justifyContent: 'center', gap: '8px', padding: '16px' }}>
      <button className="btn btn-sm btn-ghost" disabled={page === 1} onClick={() => onChange(1)}>«</button>
      <button className="btn btn-sm btn-ghost" disabled={page === 1} onClick={() => onChange(page - 1)}>‹</button>
      {range.map(i => (
        <button
          key={i}
          className={`btn btn-sm ${page === i ? 'btn-accent' : 'btn-ghost'}`}
          onClick={() => onChange(i)}
        >
          {i}
        </button>
      ))}
      <button className="btn btn-sm btn-ghost" disabled={page === totalPages} onClick={() => onChange(page + 1)}>›</button>
      <button className="btn btn-sm btn-ghost" disabled={page === totalPages} onClick={() => onChange(totalPages)}>»</button>
    </div>
  );
}
