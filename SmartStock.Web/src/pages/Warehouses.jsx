import { useState, useEffect } from 'react';
import { warehousesAPI } from '../services/api';
import { useAuth } from '../context/AuthContext';
import { useToast } from '../components/ui/Toast';
import PillToggle from '../components/ui/PillToggle';
import ArchiveStatsBar from '../components/ui/ArchiveStatsBar';
import ConfirmModal from '../components/ui/ConfirmModal';
import { Warehouse as WIcon, Plus, X, MapPin, Edit2, RotateCcw } from 'lucide-react';

export default function Warehouses() {
  const [warehouses, setWarehouses]       = useState([]);
  const [loading, setLoading]             = useState(true);
  const [showModal, setShowModal]         = useState(false);
  const [isEditing, setIsEditing]         = useState(false);
  const [actionLoading, setActionLoading] = useState(false);
  const [showArchived, setShowArchived]   = useState(false);
  const [restoringId, setRestoringId]     = useState(null);
  const [form, setForm] = useState({ id: null, name: '', location: '', code: '' });
  const [confirmModal, setConfirmModal]   = useState({ open: false, id: null, name: '' });

  const { user } = useAuth();
  const toast = useToast();
  const canEdit = user?.role === 'Admin' || user?.role === 'Supervisor';

  useEffect(() => { load(); }, [showArchived]);

  const load = async () => {
    setLoading(true);
    try {
      const params = showArchived ? { includeInactive: true } : undefined;
      const r = await warehousesAPI.getAll(params);
      setWarehouses(r.data);
    } catch {
      toast.error('Error cargando almacenes');
    } finally {
      setLoading(false);
    }
  };

  const openNewModal = () => { setIsEditing(false); setForm({ id: null, name: '', location: '', code: '' }); setShowModal(true); };
  const openEditModal = (w) => { setIsEditing(true); setForm({ id: w.id, name: w.name, location: w.location || '', code: w.code }); setShowModal(true); };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setActionLoading(true);
    const payload = { name: form.name, location: form.location || null, code: form.code };
    try {
      if (isEditing) {
        await warehousesAPI.update(form.id, payload);
        toast.success('Almacén actualizado');
      } else {
        await warehousesAPI.create(payload);
        toast.success('Almacén creado exitosamente');
      }
      setShowModal(false);
      load();
    } catch (err) {
      toast.error(err.response?.data?.message || 'Error guardando almacén');
    } finally {
      setActionLoading(false);
    }
  };

  const handleArchiveConfirm = () => {
    setConfirmModal(prev => ({ ...prev, open: false }));
    (async () => {
      try {
        await warehousesAPI.delete(confirmModal.id);
        toast.success(`Almacén "${confirmModal.name}" archivado`);
        load();
      } catch (err) {
        toast.error(err.response?.data?.message || 'Error al archivar almacén');
      }
    })();
  };

  const handleRestore = async (id, name) => {
    setRestoringId(id);
    try {
      await warehousesAPI.restore(id);
      toast.success(`Almacén "${name}" restaurado`);
      load();
    } catch (err) {
      toast.error(err.response?.data?.message || 'Error al restaurar almacén');
    } finally {
      setRestoringId(null);
    }
  };

  // Close modal on Escape
  useEffect(() => {
    const handleKey = (e) => { if (e.key === 'Escape') setShowModal(false); };
    if (showModal) { document.addEventListener('keydown', handleKey); return () => document.removeEventListener('keydown', handleKey); }
  }, [showModal]);

  const activeCount   = warehouses.filter(w => w.isActive).length;
  const archivedCount = warehouses.filter(w => !w.isActive).length;

  return (
    <div>
      <div className="page-header" style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start', flexWrap: 'wrap', gap: '12px' }}>
        <div><h1>Almacenes</h1><p>Gestión de ubicaciones de inventario</p></div>
        {canEdit && (
          <div style={{ display: 'flex', alignItems: 'center', gap: '12px' }}>
            <PillToggle id="toggle-archived" label="Ver Archivados" checked={showArchived} onChange={() => setShowArchived(p => !p)} count={showArchived ? archivedCount : 0} />
            <button className="btn btn-accent" onClick={openNewModal} id="btn-nuevo-almacen"><Plus size={18} /> Nuevo Almacén</button>
          </div>
        )}
      </div>

      {!loading && showArchived && <ArchiveStatsBar activeCount={activeCount} archivedCount={archivedCount} />}

      {loading ? (
        <div className="loading-spinner"><div className="spinner" /></div>
      ) : (
        <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fill, minmax(300px, 1fr))', gap: '16px' }}>
          {warehouses.map((w, i) => {
            const isInactive = !w.isActive;
            const isRestoring = restoringId === w.id;
            return (
              <div key={w.id} className="card animate-in" style={{ animationDelay: `${i * 0.05}s`, opacity: isInactive ? 0.55 : 1, filter: isInactive ? 'grayscale(0.4)' : 'none', transition: 'opacity 0.3s, filter 0.3s', position: 'relative', borderColor: isInactive ? 'rgba(245,158,11,0.25)' : undefined }}>
                {isInactive && (
                  <span style={{ position: 'absolute', top: '10px', right: '10px', background: 'rgba(245,158,11,0.15)', color: 'var(--color-warning)', border: '1px solid rgba(245,158,11,0.35)', borderRadius: '99px', padding: '2px 9px', fontSize: '0.7rem', fontWeight: 700, letterSpacing: '0.04em', textTransform: 'uppercase' }}>Archivado</span>
                )}
                <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start', marginBottom: '16px' }}>
                  <div style={{ display: 'flex', alignItems: 'center', gap: '12px' }}>
                    <div className={`kpi-icon ${isInactive ? '' : 'blue'}`} style={isInactive ? { background: 'var(--glass-border)' } : {}}>
                      <WIcon size={20} />
                    </div>
                    <div>
                      <h3 style={{ fontSize: '1.05rem', fontWeight: 600, color: isInactive ? 'var(--color-text-secondary)' : undefined }}>{w.name}</h3>
                      <div style={{ display: 'flex', alignItems: 'center', gap: '4px', color: 'var(--color-text-muted)', fontSize: '0.8rem' }}>
                        <MapPin size={12} /> {w.location || 'Sin ubicación'}
                      </div>
                    </div>
                  </div>
                  {canEdit && (
                    <div style={{ display: 'flex', gap: '4px', marginTop: isInactive ? '28px' : 0 }}>
                      {isInactive ? (
                        <button className="btn btn-ghost btn-sm" style={{ padding: '6px 12px', gap: '6px', color: 'var(--color-success)', border: '1px solid rgba(34,197,94,0.3)', borderRadius: '8px', display: 'flex', alignItems: 'center', opacity: isRestoring ? 0.6 : 1 }} onClick={() => handleRestore(w.id, w.name)} disabled={isRestoring} title="Restaurar almacén" id={`btn-restore-${w.id}`}>
                          <RotateCcw size={14} style={{ animation: isRestoring ? 'spin 1s linear infinite' : 'none' }} />
                          {isRestoring ? 'Restaurando…' : 'Restaurar'}
                        </button>
                      ) : (
                        <>
                          <button className="btn btn-ghost btn-sm" style={{ padding: '6px' }} onClick={() => openEditModal(w)} title="Editar" id={`btn-edit-${w.id}`}><Edit2 size={15} /></button>
                          <button className="btn btn-ghost btn-sm" style={{ padding: '6px', color: 'var(--color-warning)' }} onClick={() => setConfirmModal({ open: true, id: w.id, name: w.name })} title="Archivar" id={`btn-archive-${w.id}`}><WIcon size={15} /></button>
                        </>
                      )}
                    </div>
                  )}
                </div>
                <div style={{ display: 'flex', justifyContent: 'space-between', paddingTop: '14px', borderTop: '1px solid var(--glass-border)' }}>
                  <span style={{ fontFamily: 'monospace', fontSize: '0.85rem', color: isInactive ? 'var(--color-text-muted)' : 'var(--color-accent)', fontWeight: 600 }}>{w.code}</span>
                  <span style={{ fontSize: '0.82rem', color: 'var(--color-text-secondary)' }}>{w.totalMovements} mov.</span>
                </div>
              </div>
            );
          })}
          {warehouses.length === 0 && (
            <div style={{ gridColumn: '1 / -1', textAlign: 'center', padding: '60px 0', color: 'var(--color-text-muted)' }}>
              <WIcon size={40} style={{ opacity: 0.3, marginBottom: '12px' }} />
              <p style={{ fontSize: '0.95rem' }}>{showArchived ? 'No hay almacenes archivados.' : 'No hay almacenes activos.'}</p>
              {canEdit && !showArchived && (
                <button className="btn btn-accent" style={{ marginTop: '16px' }} onClick={openNewModal}><Plus size={16} /> Crear primer almacén</button>
              )}
            </div>
          )}
        </div>
      )}

      <ConfirmModal open={confirmModal.open} title="Archivar Almacén" message={`¿Deseas archivar el almacén "${confirmModal.name}"? No se podrán registrar movimientos mientras esté archivado.`} confirmText="Archivar" variant="warning" onConfirm={handleArchiveConfirm} onCancel={() => setConfirmModal({ open: false, id: null, name: '' })} />

      {showModal && (
        <div className="modal-overlay" onClick={() => setShowModal(false)}>
          <div className="modal" onClick={e => e.stopPropagation()}>
            <div className="modal-header">
              <h2>{isEditing ? 'Editar Almacén' : 'Nuevo Almacén'}</h2>
              <button className="modal-close" onClick={() => setShowModal(false)}><X size={16} /></button>
            </div>
            <form onSubmit={handleSubmit}>
              <div className="form-group"><label className="form-label">Nombre *</label><input className="form-input" value={form.name} onChange={e => setForm({ ...form, name: e.target.value })} required disabled={actionLoading} /></div>
              <div className="form-group"><label className="form-label">Ubicación</label><input className="form-input" value={form.location} onChange={e => setForm({ ...form, location: e.target.value })} disabled={actionLoading} /></div>
              <div className="form-group"><label className="form-label">Código *</label><input className="form-input" value={form.code} onChange={e => setForm({ ...form, code: e.target.value })} required disabled={actionLoading} /></div>
              <div className="modal-actions">
                <button type="button" className="btn btn-ghost" onClick={() => setShowModal(false)} disabled={actionLoading}>Cancelar</button>
                <button type="submit" className="btn btn-accent" disabled={actionLoading} id="btn-submit-almacen">{actionLoading ? 'Guardando…' : (isEditing ? 'Actualizar' : 'Crear')}</button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
}
