import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import { Eye, EyeOff } from 'lucide-react';
import SmartStockLogo from '../components/ui/SmartStockLogo';

export default function Login() {
  const [isRegister, setIsRegister] = useState(false);
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [fullName, setFullName] = useState('');
  const [showPassword, setShowPassword] = useState(false);
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);
  const { login, register } = useAuth();
  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');
    setLoading(true);
    try {
      if (isRegister) {
        await register(fullName, email, password);
      } else {
        await login(email, password);
      }
      navigate('/');
    } catch (err) {
      setError(err.response?.data?.message || 'Error de autenticación');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="login-container">
      <div className="login-card animate-in">
        <div className="login-logo">
          <div style={{ padding: '0', marginBottom: '8px', display: 'flex', justifyContent: 'center' }}>
            <SmartStockLogo size={70} />
          </div>
          <h1>Smart<span>Stock</span></h1>
        </div>

        <p style={{ textAlign: 'center', color: 'var(--color-text-secondary)', marginBottom: '28px', fontSize: '0.9rem' }}>
          {isRegister ? 'Crear cuenta nueva' : 'Inicia sesión para continuar'}
        </p>

        {error && (
          <div style={{ background: 'rgba(239, 68, 68, 0.1)', border: '1px solid rgba(239, 68, 68, 0.3)', borderRadius: '8px', padding: '12px', marginBottom: '20px', color: '#EF4444', fontSize: '0.85rem' }}>
            {error}
          </div>
        )}

        <form onSubmit={handleSubmit}>
          {isRegister && (
            <div className="form-group">
              <label className="form-label">Nombre Completo</label>
              <input 
                className="form-input" 
                type="text" 
                placeholder="Juan Pérez"
                value={fullName} 
                onChange={(e) => setFullName(e.target.value)} 
                required 
              />
            </div>
          )}

          <div className="form-group">
            <label className="form-label">Email</label>
            <input 
              className="form-input" 
              type="email" 
              placeholder="correo@empresa.com" 
              value={email} 
              onChange={(e) => setEmail(e.target.value)} 
              required 
            />
          </div>

          <div className="form-group">
            <label className="form-label">Contraseña</label>
            <div style={{ position: 'relative' }}>
              <input 
                className="form-input" 
                type={showPassword ? 'text' : 'password'} 
                placeholder="••••••••"
                value={password} 
                onChange={(e) => setPassword(e.target.value)} 
                required 
                minLength={6}
              />
              <button 
                type="button" 
                onClick={() => setShowPassword(!showPassword)}
                style={{ position: 'absolute', right: '12px', top: '50%', transform: 'translateY(-50%)', background: 'none', border: 'none', color: 'var(--color-text-muted)', cursor: 'pointer' }}
              >
                {showPassword ? <EyeOff size={18} /> : <Eye size={18} />}
              </button>
            </div>
          </div>

          <button className="btn btn-accent" type="submit" disabled={loading} style={{ width: '100%', justifyContent: 'center', marginTop: '8px', padding: '12px' }}>
            {loading ? 'Procesando...' : (isRegister ? 'Crear Cuenta' : 'Iniciar Sesión')}
          </button>
        </form>

        <div style={{ textAlign: 'center', marginTop: '24px' }}>
          <button 
            onClick={() => { setIsRegister(!isRegister); setError(''); }} 
            style={{ background: 'none', border: 'none', color: 'var(--color-accent)', cursor: 'pointer', fontSize: '0.85rem', fontFamily: 'var(--font-family)' }}
          >
            {isRegister ? '¿Ya tienes cuenta? Inicia sesión' : '¿No tienes cuenta? Regístrate'}
          </button>
        </div>

        {!isRegister && (
          <div
            style={{
              marginTop: '24px',
              padding: '14px 16px',
              background: 'rgba(0, 212, 255, 0.06)',
              borderRadius: '10px',
              border: '1px solid rgba(0, 212, 255, 0.18)',
            }}
          >
            <p style={{ fontSize: '0.72rem', color: 'var(--color-text-muted)', marginBottom: '10px', textTransform: 'uppercase', letterSpacing: '0.08em', fontWeight: 600 }}>
              Acceso Rápido
            </p>
            <div style={{ display: 'flex', gap: '8px' }}>
              <button
                type="button"
                onClick={() => { setEmail('admin@smartstock.pe'); setPassword('Admin123!'); }}
                style={{
                  flex: 1,
                  padding: '8px',
                  background: 'rgba(0, 212, 255, 0.1)',
                  border: '1px solid rgba(0, 212, 255, 0.25)',
                  borderRadius: '7px',
                  color: 'var(--color-accent)',
                  fontSize: '0.78rem',
                  fontWeight: 600,
                  cursor: 'pointer',
                  fontFamily: 'var(--font-family)',
                  transition: 'background 0.2s',
                }}
                onMouseEnter={e => e.target.style.background = 'rgba(0, 212, 255, 0.18)'}
                onMouseLeave={e => e.target.style.background = 'rgba(0, 212, 255, 0.1)'}
              >
                👤 Admin
              </button>
            </div>
            <p style={{ fontSize: '0.7rem', color: 'var(--color-text-muted)', marginTop: '8px', opacity: 0.7 }}>
              Haz clic para rellenar las credenciales automáticamente
            </p>
          </div>
        )}
      </div>
    </div>
  );
}

