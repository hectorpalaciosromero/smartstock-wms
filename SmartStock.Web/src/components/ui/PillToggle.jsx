import { Archive } from 'lucide-react';

/**
 * Reusable pill-style toggle for archive/filter views.
 *
 * @param {Object} props
 * @param {string}  props.id       - Unique HTML id
 * @param {string}  props.label    - Display label (e.g. "Ver Archivados")
 * @param {boolean} props.checked  - Toggle state
 * @param {Function} props.onChange - Called with no args to toggle
 * @param {number}  [props.count]  - Optional count badge when checked
 * @param {React.ReactNode} [props.icon] - Optional override icon (defaults to Archive)
 */
export default function PillToggle({ id, label, checked, onChange, count, icon }) {
  const Icon = icon || Archive;

  return (
    <label
      htmlFor={id}
      style={{
        display: 'flex', alignItems: 'center', gap: '8px',
        cursor: 'pointer', userSelect: 'none',
        padding: '8px 14px', borderRadius: '10px',
        border: `1px solid ${checked ? 'var(--color-warning)' : 'var(--glass-border)'}`,
        background: checked ? 'rgba(245,158,11,0.08)' : 'var(--glass-bg)',
        transition: 'all 0.25s ease', fontSize: '0.875rem', fontWeight: 500,
        color: checked ? 'var(--color-warning)' : 'var(--color-text-secondary)',
      }}
    >
      <div
        id={id}
        role="switch"
        aria-checked={checked}
        tabIndex={0}
        onClick={onChange}
        onKeyDown={e => (e.key === ' ' || e.key === 'Enter') && onChange()}
        style={{
          width: '36px', height: '20px', borderRadius: '10px',
          background: checked ? 'var(--color-warning)' : 'var(--glass-border)',
          position: 'relative', transition: 'background 0.25s ease', flexShrink: 0,
        }}
      >
        <div style={{
          position: 'absolute', top: '3px',
          left: checked ? '18px' : '3px',
          width: '14px', height: '14px', borderRadius: '50%',
          background: '#fff', transition: 'left 0.25s ease',
          boxShadow: '0 1px 3px rgba(0,0,0,0.3)',
        }} />
      </div>
      <Icon size={14} />
      {label}
      {checked && count > 0 && (
        <span style={{
          background: 'var(--color-warning)', color: '#000',
          borderRadius: '99px', padding: '1px 7px', fontSize: '0.75rem', fontWeight: 700,
        }}>{count}</span>
      )}
    </label>
  );
}
