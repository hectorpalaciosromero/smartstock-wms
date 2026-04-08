import { createContext, useContext, useState, useCallback, useRef } from 'react';
import { CheckCircle, XCircle, AlertTriangle, Info, X } from 'lucide-react';

const ToastContext = createContext(null);

let _toastId = 0;

/**
 * Toast notification provider. Wrap your app with <ToastProvider>.
 * Then call useToast() to get { success, error, warning, info } functions.
 */
export function ToastProvider({ children }) {
  const [toasts, setToasts] = useState([]);
  const timersRef = useRef({});

  const removeToast = useCallback((id) => {
    clearTimeout(timersRef.current[id]);
    delete timersRef.current[id];
    setToasts(prev => prev.filter(t => t.id !== id));
  }, []);

  const addToast = useCallback((type, message, duration = 4000) => {
    const id = ++_toastId;
    setToasts(prev => [...prev, { id, type, message }]);
    timersRef.current[id] = setTimeout(() => removeToast(id), duration);
    return id;
  }, [removeToast]);

  const api = {
    success: (msg, dur) => addToast('success', msg, dur),
    error:   (msg, dur) => addToast('error',   msg, dur),
    warning: (msg, dur) => addToast('warning', msg, dur),
    info:    (msg, dur) => addToast('info',    msg, dur),
  };

  return (
    <ToastContext.Provider value={api}>
      {children}
      {/* Toast Container */}
      <div style={{
        position: 'fixed', top: '20px', right: '20px', zIndex: 9999,
        display: 'flex', flexDirection: 'column', gap: '10px',
        pointerEvents: 'none', maxWidth: '380px', width: '100%',
      }}>
        {toasts.map(t => (
          <ToastItem key={t.id} toast={t} onRemove={() => removeToast(t.id)} />
        ))}
      </div>
    </ToastContext.Provider>
  );
}

function ToastItem({ toast, onRemove }) {
  const config = {
    success: { icon: CheckCircle, color: '#10B981', bg: 'rgba(16,185,129,0.12)', border: 'rgba(16,185,129,0.3)' },
    error:   { icon: XCircle,     color: '#EF4444', bg: 'rgba(239,68,68,0.12)',  border: 'rgba(239,68,68,0.3)'  },
    warning: { icon: AlertTriangle, color: '#F59E0B', bg: 'rgba(245,158,11,0.12)', border: 'rgba(245,158,11,0.3)' },
    info:    { icon: Info,         color: '#3B82F6', bg: 'rgba(59,130,246,0.12)',  border: 'rgba(59,130,246,0.3)' },
  };
  const c = config[toast.type] || config.info;
  const Icon = c.icon;

  return (
    <div style={{
      pointerEvents: 'auto',
      display: 'flex', alignItems: 'flex-start', gap: '10px',
      padding: '12px 16px', borderRadius: '12px',
      background: 'var(--color-bg-card)', backdropFilter: 'blur(20px)',
      border: `1px solid ${c.border}`,
      boxShadow: '0 8px 32px rgba(0,0,0,0.3)',
      animation: 'slideUp 0.25s ease',
      fontSize: '0.85rem', color: 'var(--color-text-primary)',
    }}>
      <Icon size={18} color={c.color} style={{ flexShrink: 0, marginTop: '1px' }} />
      <span style={{ flex: 1, lineHeight: 1.4 }}>{toast.message}</span>
      <button
        onClick={onRemove}
        style={{
          background: 'none', border: 'none', cursor: 'pointer',
          color: 'var(--color-text-muted)', padding: '2px', flexShrink: 0,
        }}
      >
        <X size={14} />
      </button>
    </div>
  );
}

/**
 * Hook to show toast notifications.
 * @returns {{ success: Function, error: Function, warning: Function, info: Function }}
 */
export function useToast() {
  const ctx = useContext(ToastContext);
  if (!ctx) throw new Error('useToast must be used within <ToastProvider>');
  return ctx;
}
