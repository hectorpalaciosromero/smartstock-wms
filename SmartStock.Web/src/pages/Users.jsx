import { useState, useEffect } from 'react';
import { authAPI } from '../services/api';
import { useAuth } from '../context/AuthContext';
import { useToast } from '../components/ui/Toast';
import ConfirmModal from '../components/ui/ConfirmModal';
import { Users as UsersIcon, Shield, Search, Loader2, Trash2, RotateCcw } from 'lucide-react';

export default function Users() {
  const [users, setUsers] = useState([]);
  const [loading, setLoading] = useState(true);
  const [updatingId, setUpdatingId] = useState(null);
  const [search, setSearch] = useState('');
  
  // Confirm Modal state
  const [confirmModal, setConfirmModal] = useState({ open: false, title: '', message: '', action: null, variant: 'danger' });

  const { user: currentUser } = useAuth();
  const toast = useToast();

  useEffect(() => { loadUsers(); }, []);

  const loadUsers = async () => {
    setLoading(true);
    try {
      const res = await authAPI.getUsers();
      setUsers(res.data);
    } catch (err) {
      toast.error('Error loading users');
    } finally {
      setLoading(false);
    }
  };

  const executeAction = async () => {
    if (!confirmModal.action) return;
    try {
      await confirmModal.action();
      await loadUsers();
    } catch (err) {
      toast.error(err.response?.data?.message || 'Error realizando acción');
    } finally {
      setConfirmModal({ open: false, title: '', message: '', action: null });
      setUpdatingId(null);
    }
  };

  const handleRoleChange = (userId, newRole) => {
    setConfirmModal({
      open: true,
      title: 'Cambiar Rol',
      message: `¿Estás seguro de cambiar el rol de este usuario a ${newRole}?`,
      variant: 'warning',
      action: async () => {
        setUpdatingId(userId);
        await authAPI.updateRole(userId, { role: newRole });
        toast.success(`Rol cambiado a ${newRole}`);
      }
    });
  };

  const handleToggleStatus = (userId, isActive) => {
    setConfirmModal({
      open: true,
      title: isActive ? 'Desactivar Usuario' : 'Activar Usuario',
      message: `¿Estás seguro de ${isActive ? 'desactivar' : 'activar'} este usuario?`,
      variant: isActive ? 'danger' : 'success',
      action: async () => {
        setUpdatingId(userId);
        await authAPI.toggleStatus(userId);
        toast.success(`Usuario ${isActive ? 'desactivado' : 'activado'}`);
      }
    });
  };

  const filteredUsers = users.filter(u => 
    u.fullName?.toLowerCase().includes(search.toLowerCase()) || 
    u.email?.toLowerCase().includes(search.toLowerCase())
  );

  return (
    <div>
      <div className="page-header" style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start' }}>
        <div>
          <h1>Gestión de Personal</h1>
          <p>Supervisa las cuentas de usuario y sus niveles de acceso</p>
        </div>
      </div>

      <div className="table-container" style={{ marginBottom: '24px' }}>
        <div className="table-header" style={{ display: 'flex', gap: '12px' }}>
          <div className="navbar-search" style={{ flex: 1, maxWidth: '300px' }}>
            <Search size={16} />
            <input type="text" placeholder="Buscar por nombre o correo..." value={search} onChange={(e) => setSearch(e.target.value)} />
          </div>
        </div>

        {loading ? (
          <div className="loading-spinner"><div className="spinner"></div></div>
        ) : (
          <div style={{ overflowX: 'auto' }}>
            <table className="table">
              <thead><tr><th>Usuario</th><th>Identificación</th><th>Estado</th><th>Rol Actual</th><th>Asignar Cargo</th></tr></thead>
              <tbody>
                {filteredUsers.map(u => (
                  <tr key={u.id}>
                    <td>
                      <div style={{ display: 'flex', alignItems: 'center', gap: '12px' }}>
                        {u.avatarUrl ? (
                          <img src={u.avatarUrl} alt="" className="navbar-avatar" style={{ padding: 0, objectFit: 'cover' }} />
                        ) : (
                          <div className="navbar-avatar">{u.fullName?.charAt(0) || u.email?.charAt(0) || 'U'}</div>
                        )}
                        <div style={{ fontWeight: 500 }}>{u.fullName || 'Sin Nombre'}</div>
                      </div>
                    </td>
                    <td><span style={{ fontSize: '0.9rem', color: 'var(--color-text-secondary)' }}>{u.email}</span></td>
                    <td>{u.isActive ? <span className="badge-status badge-success">Activo</span> : <span className="badge-status badge-danger">Inactivo</span>}</td>
                    <td>
                      <div style={{ display: 'flex', alignItems: 'center', gap: '6px' }}>
                        <Shield size={14} style={{ color: u.role === 'Admin' ? 'var(--color-danger)' : u.role === 'Supervisor' ? 'var(--color-warning)' : 'var(--color-info)' }} />
                        <span style={{ fontSize: '0.85rem', fontWeight: 600 }}>{u.role}</span>
                      </div>
                    </td>
                    <td>
                      <div style={{ display: 'flex', alignItems: 'center', gap: '8px' }}>
                        <select className="form-input" style={{ padding: '6px 12px', height: 'auto', width: '140px', fontSize: '0.85rem' }} value={u.role} onChange={(e) => handleRoleChange(u.id, e.target.value)} disabled={updatingId === u.id || u.id === currentUser.id}>
                          <option value="Admin">Administrador</option>
                          <option value="Supervisor">Supervisor</option>
                          <option value="Almacenero">Almacenero</option>
                          <option value="Viewer">Solo Lectura</option>
                        </select>
                        <button className="btn btn-ghost btn-icon btn-sm" style={{ padding: '6px' }} title={u.isActive ? "Desactivar Usuario" : "Activar Usuario"} onClick={() => handleToggleStatus(u.id, u.isActive)} disabled={updatingId === u.id || u.id === currentUser.id}>
                          {u.isActive ? <Trash2 size={16} color="var(--color-danger)" /> : <RotateCcw size={16} color="var(--color-success)" />}
                        </button>
                        {updatingId === u.id && <Loader2 size={16} className="spinner" />}
                      </div>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
            
            {filteredUsers.length === 0 && (
              <div className="empty-state">
                <UsersIcon size={48} />
                <h3>No se encontraron usuarios</h3>
                <p>Intenta con otros términos de búsqueda.</p>
              </div>
            )}
          </div>
        )}
      </div>

      <ConfirmModal
        open={confirmModal.open}
        title={confirmModal.title}
        message={confirmModal.message}
        variant={confirmModal.variant}
        onConfirm={executeAction}
        onCancel={() => setConfirmModal({ open: false, title: '', message: '', action: null })}
      />
    </div>
  );
}
