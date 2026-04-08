import { useState, useEffect } from 'react';
import { suppliersAPI } from '../services/api';
import { useAuth } from '../context/AuthContext';
import { useToast } from '../components/ui/Toast';
import PillToggle from '../components/ui/PillToggle';
import ArchiveStatsBar from '../components/ui/ArchiveStatsBar';
import ConfirmModal from '../components/ui/ConfirmModal';
import { Users, Plus, X, Phone, Mail, MapPin, Edit2, Trash2, RotateCcw } from 'lucide-react';

export default function Suppliers() {
  const [suppliers, setSuppliers] = useState([]);
  const [loading, setLoading] = useState(true);
  const [showModal, setShowModal] = useState(false);
  const [isEditing, setIsEditing] = useState(false);
  const [actionLoading, setActionLoading] = useState(false);
  const [form, setForm] = useState({ id: null, companyName: '', contactName: '', phone: '', email: '', address: '' });
  const [showArchived, setShowArchived] = useState(false);
  const [confirmModal, setConfirmModal] = useState({ open: false, id: null, name: '' });

  const { user } = useAuth();
  const toast = useToast();
  const canEdit = user?.role === 'Admin' || user?.role === 'Supervisor';

  useEffect(() => { load(); }, [showArchived]);

  const load = async () => {
    setLoading(true);
    try {
      const params = showArchived ? { includeInactive: true } : undefined;
      const r = await suppliersAPI.getAll(params);
      setSuppliers(r.data);
    } catch {
      toast.error('Error cargando proveedores');
    } finally {
      setLoading(false);
    }
  };

  const openNewModal = () => { setIsEditing(false); setForm({ id: null, companyName: '', contactName: '', phone: '', email: '', address: '' }); setShowModal(true); };
  const openEditModal = (s) => { setIsEditing(true); setForm({ id: s.id, companyName: s.companyName, contactName: s.contactName || '', phone: s.phone || '', email: s.email || '', address: s.address || '' }); setShowModal(true); };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setActionLoading(true);
    const payload = { companyName: form.companyName, contactName: form.contactName || null, phone: form.phone || null, email: form.email || null, address: form.address || null };
    try {
      if (isEditing) { await suppliersAPI.update(form.id, payload); toast.success('Proveedor actualizado'); }
      else { await suppliersAPI.create(payload); toast.success('Proveedor creado exitosamente'); }
      setShowModal(false);
      load();
    } catch (err) {
      toast.error(err.response?.data?.message || 'Error guardando proveedor');
    } finally {
      setActionLoading(false);
    }
  };

  const handleDeleteConfirm = () => {
    setConfirmModal(prev => ({ ...prev, open: false }));
    (async () => {
      try {
        await suppliersAPI.delete(confirmModal.id);
        toast.success(`Proveedor "${confirmModal.name}" archivado`);
        load();
      } catch {
        toast.error('Error al archivar proveedor');
      }
    })();
  };

  const handleRestore = async (id, name) => {
    try {
      await suppliersAPI.restore(id);
      toast.success(`Proveedor "${name}" restaurado`);
      load();
    } catch {
      toast.error('Error al restaurar proveedor');
    }
  };

  useEffect(() => {
    const handleKey = (e) => { if (e.key === 'Escape') setShowModal(false); };
    if (showModal) { document.addEventListener('keydown', handleKey); return () => document.removeEventListener('keydown', handleKey); }
  }, [showModal]);

  const activeCount = suppliers.filter(s => s.isActive).length;
  const archivedCount = suppliers.length - activeCount;

  return (
    <div>
      <div className="page-header" style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start' }}>
        <div><h1>Proveedores</h1><p>Gestión de proveedores</p></div>
        {canEdit && (
          <div style={{ display: 'flex', gap: '12px', alignItems: 'center' }}>
            <PillToggle id="toggle-archived-sup" label="Ver Archivados" checked={showArchived} onChange={() => setShowArchived(p => !p)} count={showArchived ? archivedCount : 0} />
            <button className="btn btn-accent" onClick={openNewModal}><Plus size={18} /> Nuevo Proveedor</button>
          </div>
        )}
      </div>

      {!loading && showArchived && <ArchiveStatsBar activeCount={activeCount} archivedCount={archivedCount} />}

      {loading ? <div className="loading-spinner"><div className="spinner"></div></div> : (
        <div className="table-container">
          {suppliers.length === 0 ? (
            <div className="empty-state">
              <Users size={48} />
              <h3>Sin proveedores registrados</h3>
              <p>Agrega tu primer proveedor para comenzar</p>
              {canEdit && <button className="btn btn-accent" style={{ marginTop: '16px' }} onClick={openNewModal}><Plus size={16} /> Agregar proveedor</button>}
            </div>
          ) : (
            <div style={{ overflowX: 'auto' }}>
              <table>
                <thead>
                  <tr>
                    <th>Empresa</th><th>Contacto</th><th>Teléfono</th><th>Email</th><th>Dirección</th><th>Productos</th>
                    {canEdit && <th>Acciones</th>}
                  </tr>
                </thead>
                <tbody>
                  {suppliers.map(s => (
                    <tr key={s.id} style={{ opacity: s.isActive ? 1 : 0.55 }}>
                      <td style={{ fontWeight: 600, color: 'var(--color-text-primary)' }}>{s.companyName}</td>
                      <td>{s.contactName || '-'}</td>
                      <td><span style={{ display: 'flex', alignItems: 'center', gap: '4px' }}><Phone size={13} />{s.phone || '-'}</span></td>
                      <td><span style={{ display: 'flex', alignItems: 'center', gap: '4px' }}><Mail size={13} />{s.email || '-'}</span></td>
                      <td><span style={{ display: 'flex', alignItems: 'center', gap: '4px', fontSize: '0.85rem', color: s.address ? 'var(--color-text-secondary)' : 'var(--color-text-muted)' }}><MapPin size={13} />{s.address || 'Sin dirección'}</span></td>
                      <td><span className="badge-status badge-info">{s.productCount}</span></td>
                      {canEdit && (
                        <td>
                          <div style={{ display: 'flex', gap: '6px' }}>
                            {s.isActive ? (
                              <>
                                <button className="btn btn-ghost btn-sm" style={{ padding: '6px' }} onClick={() => openEditModal(s)} title="Editar"><Edit2 size={14} /></button>
                                <button className="btn btn-ghost btn-sm" style={{ padding: '6px', color: 'var(--color-danger)' }} onClick={() => setConfirmModal({ open: true, id: s.id, name: s.companyName })} title="Archivar"><Trash2 size={14} /></button>
                              </>
                            ) : (
                              <button className="btn btn-ghost btn-sm" style={{ padding: '6px', color: 'var(--color-success)' }} onClick={() => handleRestore(s.id, s.companyName)} title="Restaurar Proveedor"><RotateCcw size={14} /></button>
                            )}
                          </div>
                        </td>
                      )}
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          )}
        </div>
      )}

      <ConfirmModal open={confirmModal.open} title="Archivar Proveedor" message={`¿Estás seguro de archivar al proveedor "${confirmModal.name}"?`} confirmText="Archivar" variant="warning" onConfirm={handleDeleteConfirm} onCancel={() => setConfirmModal({ open: false, id: null, name: '' })} />

      {showModal && (
        <div className="modal-overlay" onClick={() => setShowModal(false)}>
          <div className="modal" onClick={e => e.stopPropagation()}>
            <div className="modal-header">
              <h2>{isEditing ? 'Editar Proveedor' : 'Nuevo Proveedor'}</h2>
              <button className="modal-close" onClick={() => setShowModal(false)}><X size={16} /></button>
            </div>
            <form onSubmit={handleSubmit}>
              <div className="form-group"><label className="form-label">Empresa *</label><input className="form-input" value={form.companyName} onChange={e => setForm({...form, companyName: e.target.value})} required placeholder="Nombre de la empresa" disabled={actionLoading} /></div>
              <div className="form-group"><label className="form-label">Contacto</label><input className="form-input" value={form.contactName} onChange={e => setForm({...form, contactName: e.target.value})} placeholder="Nombre de contacto" disabled={actionLoading} /></div>
              <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '16px' }}>
                <div className="form-group"><label className="form-label">Teléfono</label><input className="form-input" value={form.phone} onChange={e => setForm({...form, phone: e.target.value})} placeholder="+51 999 999 999" disabled={actionLoading} /></div>
                <div className="form-group"><label className="form-label">Email</label><input className="form-input" type="email" value={form.email} onChange={e => setForm({...form, email: e.target.value})} placeholder="contacto@empresa.com" disabled={actionLoading} /></div>
              </div>
              <div className="form-group"><label className="form-label">Dirección</label><input className="form-input" value={form.address} onChange={e => setForm({...form, address: e.target.value})} placeholder="Av. Principal 123, Lima, Perú" disabled={actionLoading} /></div>
              <div className="modal-actions">
                <button type="button" className="btn btn-ghost" onClick={() => setShowModal(false)} disabled={actionLoading}>Cancelar</button>
                <button type="submit" className="btn btn-accent" disabled={actionLoading}>{actionLoading ? 'Guardando...' : (isEditing ? 'Actualizar' : 'Crear')}</button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
}
