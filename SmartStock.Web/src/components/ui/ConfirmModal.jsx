import { useEffect, useCallback } from 'react';
import { AlertTriangle, X } from 'lucide-react';

/**
 * Professional confirm dialog replacing window.confirm().
 *
 * @param {Object} props
 * @param {boolean}  props.open        - Whether the modal is visible
 * @param {string}   props.title       - Modal title
 * @param {string}   props.message     - Confirmation message body
 * @param {string}   [props.confirmText] - Confirm button text (default "Confirmar")
 * @param {string}   [props.cancelText]  - Cancel button text (default "Cancelar")
 * @param {string}   [props.variant]     - "danger" | "warning" | "info" (default "danger")
 * @param {Function} props.onConfirm   - Called when user confirms
 * @param {Function} props.onCancel    - Called when user cancels
 * @param {boolean}  [props.loading]   - If true, disable confirm button and show loading state
 */
export default function ConfirmModal({
  open, title, message, confirmText = 'Confirmar', cancelText = 'Cancelar',
  variant = 'danger', onConfirm, onCancel, loading = false,
}) {
  const handleKeyDown = useCallback((e) => {
    if (e.key === 'Escape') onCancel();
  }, [onCancel]);

  useEffect(() => {
    if (open) {
      document.addEventListener('keydown', handleKeyDown);
      return () => document.removeEventListener('keydown', handleKeyDown);
    }
  }, [open, handleKeyDown]);

  if (!open) return null;

  const colors = {
    danger:  { bg: 'rgba(239,68,68,0.1)',  border: 'rgba(239,68,68,0.3)',  icon: '#EF4444', btn: 'btn-danger'  },
    warning: { bg: 'rgba(245,158,11,0.1)', border: 'rgba(245,158,11,0.3)', icon: '#F59E0B', btn: 'btn-warning' },
    info:    { bg: 'rgba(59,130,246,0.1)',  border: 'rgba(59,130,246,0.3)', icon: '#3B82F6', btn: 'btn-primary' },
  };
  const c = colors[variant] || colors.danger;

  return (
    <div className="modal-overlay" onClick={onCancel}>
      <div className="modal" onClick={e => e.stopPropagation()} style={{ maxWidth: '420px' }}>
        <div className="modal-header">
          <h2 style={{ display: 'flex', alignItems: 'center', gap: '8px' }}>
            <AlertTriangle size={20} color={c.icon} />
            {title}
          </h2>
          <button className="modal-close" onClick={onCancel}><X size={16} /></button>
        </div>
        <div style={{ padding: '4px 0 20px', fontSize: '0.9rem', color: 'var(--color-text-secondary)', lineHeight: 1.6 }}>
          {message}
        </div>
        <div className="modal-actions">
          <button className="btn btn-ghost" onClick={onCancel} disabled={loading}>{cancelText}</button>
          <button className={`btn ${c.btn}`} onClick={onConfirm} disabled={loading}>
            {loading ? 'Procesando…' : confirmText}
          </button>
        </div>
      </div>
    </div>
  );
}
