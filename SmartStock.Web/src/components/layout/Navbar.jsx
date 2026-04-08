import { useState, useEffect, useRef } from 'react';
import { useAuth } from '../../context/AuthContext';
import { useNavigate } from 'react-router-dom';
import { Search, Bell, LogOut, Menu, AlertTriangle, User } from 'lucide-react';
import { productsAPI } from '../../services/api';

export default function Navbar({ collapsed, onMobileToggle }) {
  const { user, logout } = useAuth();
  const navigate = useNavigate();
  
  const [notifications, setNotifications] = useState([]);
  const [showNotifications, setShowNotifications] = useState(false);
  const notifRef = useRef(null);

  const [showProfile, setShowProfile] = useState(false);
  const profileRef = useRef(null);

  const [searchValue, setSearchValue] = useState('');

  useEffect(() => {
    if (user) loadNotifications();
  }, [user]);

  // Close dropdowns on outside click
  useEffect(() => {
    function handleClickOutside(event) {
      if (notifRef.current && !notifRef.current.contains(event.target)) setShowNotifications(false);
      if (profileRef.current && !profileRef.current.contains(event.target)) setShowProfile(false);
    }
    document.addEventListener("mousedown", handleClickOutside);
    return () => document.removeEventListener("mousedown", handleClickOutside);
  }, []);

  const loadNotifications = async () => {
    try {
      const res = await productsAPI.getAll({ lowStock: true, pageSize: 10, page: 1 });
      setNotifications(res.data?.items ?? []);
    } catch (err) {
      console.warn('Could not load stock notifications:', err?.response?.status ?? err.message);
    }
  };

  const handleSearch = (e) => {
    if (e.key === 'Enter' && searchValue.trim()) {
      navigate(`/products?search=${encodeURIComponent(searchValue.trim())}`);
      setSearchValue('');
    }
  };

  // Get avatar display — show image if avatarUrl exists, else first letter
  const getAvatar = (size = 'default') => {
    const s = size === 'small' ? { width: '32px', height: '32px', fontSize: '0.8rem' } : {};
    if (user?.avatarUrl) {
      return (
        <div className="navbar-avatar" style={{ ...s, padding: 0, overflow: 'hidden' }}>
          <img src={user.avatarUrl} alt={user.fullName} style={{ width: '100%', height: '100%', objectFit: 'cover', borderRadius: 'inherit' }} />
        </div>
      );
    }
    return <div className="navbar-avatar" style={s}>{user?.fullName?.charAt(0) || 'A'}</div>;
  };

  return (
    <header className={`navbar ${collapsed ? 'collapsed' : ''}`}>
      <div style={{ display: 'flex', alignItems: 'center', gap: '16px' }}>
        <button className="navbar-action-btn" onClick={onMobileToggle} style={{ display: 'none' }} id="mobile-menu-btn">
          <Menu size={18} />
        </button>
        <div className="navbar-search">
          <Search size={16} />
          <input 
            type="text" 
            placeholder="Buscar productos... (Enter)" 
            value={searchValue}
            onChange={(e) => setSearchValue(e.target.value)}
            onKeyDown={handleSearch}
          />
        </div>
      </div>

      <div className="navbar-actions">
        {/* Notifications */}
        <div style={{ position: 'relative' }} ref={notifRef}>
          <button 
            className="navbar-action-btn" 
            title="Notificaciones"
            onClick={() => { setShowNotifications(!showNotifications); setShowProfile(false); }}
          >
            <Bell size={18} />
            {notifications.length > 0 && <span className="badge">{notifications.length}</span>}
          </button>

          {showNotifications && (
            <div style={{
              position: 'absolute', top: '120%', right: 0, width: '320px',
              background: 'var(--color-bg-card)', border: '1px solid var(--glass-border)',
              borderRadius: 'var(--radius)', boxShadow: 'var(--shadow-card)',
              padding: '16px', zIndex: 100, animation: 'slideUp 0.15s ease'
            }}>
              <h3 style={{ fontSize: '0.9rem', marginBottom: '12px', paddingBottom: '8px', borderBottom: '1px solid var(--color-border)', display: 'flex', justifyContent: 'space-between' }}>
                Notificaciones 
                {notifications.length > 0 && <span className="badge-status badge-warning" style={{ fontSize: '0.65rem' }}>{notifications.length} Alertas</span>}
              </h3>
              <div style={{ maxHeight: '300px', overflowY: 'auto', display: 'flex', flexDirection: 'column', gap: '8px' }}>
                {notifications.length === 0 ? (
                  <p style={{ fontSize: '0.8rem', color: 'var(--color-text-muted)', textAlign: 'center', padding: '20px 0' }}>Sin notificaciones pendientes ✅</p>
                ) : (
                  notifications.map(n => (
                    <div key={`notif-${n.id}`} style={{ display: 'flex', gap: '10px', padding: '10px', borderRadius: '8px', background: 'rgba(245, 158, 11, 0.05)', border: '1px solid rgba(245, 158, 11, 0.2)' }}>
                      <AlertTriangle size={16} color="#F59E0B" style={{ flexShrink: 0, marginTop: '2px' }} />
                      <div>
                        <p style={{ fontSize: '0.8rem', fontWeight: 600, color: 'var(--color-text-primary)' }}>¡Stock en Estado Crítico!</p>
                        <p style={{ fontSize: '0.75rem', color: 'var(--color-text-secondary)', marginTop: '2px' }}>
                          El producto <strong>{n.name}</strong> está bajo el stock mínimo (Quedan {n.currentStock}).
                        </p>
                      </div>
                    </div>
                  ))
                )}
              </div>
            </div>
          )}
        </div>

        {/* User Menu */}
        <div style={{ position: 'relative' }} ref={profileRef}>
          <div className="navbar-user" onClick={() => { setShowProfile(!showProfile); setShowNotifications(false); }}>
            {getAvatar()}
            <div className="navbar-user-info">
              <div className="name">{user?.fullName || 'Admin'}</div>
              <div className="role">{user?.role || 'Administrador'}</div>
            </div>
          </div>

          {showProfile && (
            <div style={{
              position: 'absolute', top: '120%', right: 0, width: '200px',
              background: 'var(--color-bg-card)', border: '1px solid var(--glass-border)',
              borderRadius: 'var(--radius)', boxShadow: 'var(--shadow-card)',
              padding: '8px', zIndex: 100, animation: 'slideUp 0.1s ease',
              display: 'flex', flexDirection: 'column', gap: '4px'
            }}>
              <div style={{ padding: '8px 12px', borderBottom: '1px solid var(--color-border)', marginBottom: '4px' }}>
                <p style={{ fontSize: '0.85rem', fontWeight: 600, color: 'var(--color-text-primary)', whiteSpace: 'nowrap', overflow: 'hidden', textOverflow: 'ellipsis' }}>{user?.fullName}</p>
                <p style={{ fontSize: '0.75rem', color: 'var(--color-text-muted)', whiteSpace: 'nowrap', overflow: 'hidden', textOverflow: 'ellipsis' }}>{user?.email}</p>
              </div>
              
              <button className="btn btn-ghost" style={{ width: '100%', justifyContent: 'flex-start', border: 'none' }} onClick={() => { setShowProfile(false); navigate('/profile'); }}>
                <User size={16} /> Mi Perfil
              </button>
              
              <div style={{ height: '1px', background: 'var(--color-border)', margin: '4px 0' }}></div>
              
              <button className="btn btn-ghost" style={{ width: '100%', justifyContent: 'flex-start', border: 'none', color: 'var(--color-danger)' }} onClick={() => { setShowProfile(false); logout(); }}>
                <LogOut size={16} /> Cerrar Sesión
              </button>
            </div>
          )}
        </div>
      </div>
    </header>
  );
}
