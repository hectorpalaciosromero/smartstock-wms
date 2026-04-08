import { Component } from 'react';
import { AlertTriangle, RefreshCw } from 'lucide-react';

export default class ErrorBoundary extends Component {
  constructor(props) {
    super(props);
    this.state = { hasError: false, error: null };
  }

  static getDerivedStateFromError(error) {
    return { hasError: true, error };
  }

  componentDidCatch(error, errorInfo) {
    console.error('ErrorBoundary caught:', error, errorInfo);
  }

  render() {
    if (this.state.hasError) {
      return (
        <div style={{
          display: 'flex',
          alignItems: 'center',
          justifyContent: 'center',
          minHeight: '100vh',
          background: 'var(--color-bg)',
          color: 'var(--color-text-primary)',
          fontFamily: 'var(--font-family)'
        }}>
          <div style={{
            textAlign: 'center',
            maxWidth: '420px',
            padding: '40px',
            background: 'var(--color-bg-card)',
            borderRadius: '16px',
            border: '1px solid var(--glass-border)'
          }}>
            <AlertTriangle size={48} color="#F59E0B" style={{ marginBottom: '16px' }} />
            <h2 style={{ fontSize: '1.3rem', marginBottom: '8px' }}>Algo salió mal</h2>
            <p style={{ fontSize: '0.85rem', color: 'var(--color-text-secondary)', marginBottom: '24px' }}>
              Ocurrió un error inesperado. Por favor recarga la página.
            </p>
            <button
              className="btn btn-accent"
              onClick={() => window.location.reload()}
              style={{ margin: '0 auto', display: 'inline-flex', gap: '8px', alignItems: 'center' }}
            >
              <RefreshCw size={16} /> Recargar Página
            </button>
          </div>
        </div>
      );
    }

    return this.props.children;
  }
}
