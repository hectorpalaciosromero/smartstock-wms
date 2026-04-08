import { Navigate } from 'react-router-dom';
import { useAuth } from '../../context/AuthContext';
import { ShieldAlert } from 'lucide-react';

/**
 * Protects routes by user role. Renders children only if the current user
 * has one of the allowed roles.
 *
 * @param {Object} props
 * @param {string[]} props.roles   - Array of allowed role names (e.g. ['Admin', 'Supervisor'])
 * @param {React.ReactNode} props.children
 * @param {string} [props.fallback] - Path to redirect to (default: '/')
 */
export default function RoleGuard({ roles, children, fallback = '/' }) {
  const { user } = useAuth();

  if (!user) return <Navigate to="/login" />;

  if (!roles.includes(user.role)) {
    return (
      <div className="empty-state" style={{ minHeight: '60vh' }}>
        <ShieldAlert size={48} color="var(--color-danger)" />
        <h3>Acceso Denegado</h3>
        <p style={{ color: 'var(--color-text-secondary)', fontSize: '0.9rem' }}>
          No tienes permisos para acceder a esta sección.
          Se requiere rol: <strong>{roles.join(' o ')}</strong>
        </p>
        <button className="btn btn-primary" onClick={() => window.history.back()} style={{ marginTop: '16px' }}>
          Volver
        </button>
      </div>
    );
  }

  return children;
}
