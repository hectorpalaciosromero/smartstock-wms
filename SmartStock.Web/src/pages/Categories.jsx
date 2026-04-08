import { useState, useEffect } from 'react';
import { categoriesAPI } from '../services/api';
import { useAuth } from '../context/AuthContext';
import { useToast } from '../components/ui/Toast';
import PillToggle from '../components/ui/PillToggle';
import ArchiveStatsBar from '../components/ui/ArchiveStatsBar';
import ConfirmModal from '../components/ui/ConfirmModal';
import { Tag, Plus, X, Edit2, Trash2, AlertTriangle, RotateCcw } from 'lucide-react';

export default function Categories() {
  const [categories, setCategories] = useState([]);
  const [loading, setLoading] = useState(true);
  const [showModal, setShowModal] = useState(false);
  const [isEditing, setIsEditing] = useState(false);
  const [form, setForm] = useState({ id: null, name: '', description: '' });
  const [actionLoading, setActionLoading] = useState(false);
  const [showArchived, setShowArchived] = useState(false);
  const [confirmModal, setConfirmModal] = useState({ open: false, id: null, name: '' });

  const { user } = useAuth();
  const toast = useToast();
  const canEdit = user?.role === 'Admin' || user?.role === 'Supervisor';

  useEffect(() => { loadCategories(); }, [showArchived]);

  const loadCategories = async () => {
    setLoading(true);
    try { 
      const params = showArchived ? { includeInactive: true } : undefined;
      const res = await categoriesAPI.getAll(params); 
      setCategories(res.data); 
    } catch {
      toast.error('Error cargando categorías');
    }
    finally { setLoading(false); }
  };

  const openNewModal = () => {
    setIsEditing(false);
    setForm({ id: null, name: '', description: '' });
    setShowModal(true);
  };

  const openEditModal = (cat) => {
    setIsEditing(true);
    setForm({ id: cat.id, name: cat.name, description: cat.description || '' });
    setShowModal(true);
  };

  const handleDeleteConfirm = () => {
    setConfirmModal(prev => ({ ...prev, open: false }));
    (async () => {
      try {
        await categoriesAPI.delete(confirmModal.id);
        toast.success(`Categoría "${confirmModal.name}" archivada`);
        loadCategories();
      } catch {
        toast.error('Error al archivar categoría');
      }
    })();
  };

  const handleRestore = async (id, name) => {
    try {
      await categoriesAPI.restore(id);
      toast.success(`Categoría "${name}" restaurada`);
      loadCategories();
    } catch {
      toast.error('Error al restaurar categoría');
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setActionLoading(true);
    try { 
      if (isEditing) {
        await categoriesAPI.update(form.id, { name: form.name, description: form.description, imageUrl: '' });
        toast.success('Categoría actualizada');
      } else {
        await categoriesAPI.create({ name: form.name, description: form.description, imageUrl: '' });
        toast.success('Categoría creada exitosamente');
      }
      setShowModal(false); 
      loadCategories(); 
    } catch (err) {
      toast.error(err.response?.data?.message || 'Error guardando categoría');
    } finally {
      setActionLoading(false);
    }
  };

  // Close modal on Escape
  useEffect(() => {
    const handleKey = (e) => { if (e.key === 'Escape') setShowModal(false); };
    if (showModal) { document.addEventListener('keydown', handleKey); return () => document.removeEventListener('keydown', handleKey); }
  }, [showModal]);

  const activeCount = categories.filter(c => c.isActive).length;
  const archivedCount = categories.length - activeCount;

  return (
    <div>
      <div className="page-header" style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start' }}>
        <div><h1>Categorías</h1><p>Organización de productos por tipo</p></div>
        {canEdit && (
          <div style={{ display: 'flex', gap: '12px', alignItems: 'center' }}>
            <PillToggle
              id="toggle-archived-cat"
              label="Ver Archivados"
              checked={showArchived}
              onChange={() => setShowArchived(p => !p)}
              count={showArchived ? archivedCount : 0}
            />
            <button className="btn btn-accent" onClick={openNewModal}><Plus size={18} /> Nueva Categoría</button>
          </div>
        )}
      </div>

      {!loading && showArchived && <ArchiveStatsBar activeCount={activeCount} archivedCount={archivedCount} activeLabel="activas" archivedLabel="archivadas" />}

      {loading ? <div className="loading-spinner"><div className="spinner"></div></div> : (
        <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fill, minmax(280px, 1fr))', gap: '16px' }}>
          {categories.map((c, i) => (
            <div key={c.id} className="card animate-in" style={{ animationDelay: `${i * 0.05}s`, position: 'relative', opacity: c.isActive ? 1 : 0.65, filter: c.isActive ? 'none' : 'grayscale(100%)' }}>
              {canEdit && (
                <div style={{ position: 'absolute', top: '16px', right: '16px', display: 'flex', gap: '8px' }}>
                  {c.isActive ? (
                    <>
                      <button className="btn btn-ghost" style={{ padding: '6px' }} onClick={() => openEditModal(c)} title="Editar"><Edit2 size={14} /></button>
                      <button className="btn btn-ghost" style={{ padding: '6px', color: 'var(--color-danger)' }} onClick={() => setConfirmModal({ open: true, id: c.id, name: c.name })} title="Archivar"><Trash2 size={14} /></button>
                    </>
                  ) : (
                    <button className="btn btn-ghost" style={{ padding: '6px', color: 'var(--color-success)' }} onClick={() => handleRestore(c.id, c.name)} title="Restaurar Categoría"><RotateCcw size={14} /></button>
                  )}
                </div>
              )}
              <div style={{ display: 'flex', alignItems: 'center', gap: '12px', marginBottom: '12px' }}>
                <div className="kpi-icon cyan"><Tag size={20} /></div>
                <div>
                  <h3 style={{ fontSize: '1rem', fontWeight: 600, paddingRight: '60px' }}>{c.name}</h3>
                  <p style={{ fontSize: '0.78rem', color: 'var(--color-text-muted)', minHeight: '36px' }}>{c.description || 'Sin descripción'}</p>
                </div>
              </div>
              <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', paddingTop: '12px', borderTop: '1px solid var(--glass-border)' }}>
                <span style={{ fontSize: '0.82rem', color: 'var(--color-text-secondary)' }}>{c.productCount} productos</span>
                <span className={`badge-status ${c.isActive ? 'badge-success' : 'badge-danger'}`}>{c.isActive ? 'Activa' : 'Inactiva'}</span>
              </div>
            </div>
          ))}
          {categories.length === 0 && (
            <div className="empty-state" style={{ gridColumn: '1 / -1' }}>
              <AlertTriangle size={32} />
              <h3>No hay categorías registradas</h3>
              <p>Organiza tus productos creando categorías</p>
              {canEdit && (
                <button className="btn btn-accent" style={{ marginTop: '16px' }} onClick={openNewModal}><Plus size={16} /> Crear primera categoría</button>
              )}
            </div>
          )}
        </div>
      )}

      {/* Confirm Archive Modal */}
      <ConfirmModal
        open={confirmModal.open}
        title="Archivar Categoría"
        message={`¿Archivar la categoría "${confirmModal.name}"? Sus productos quedarán sin categoría visible en filtros.`}
        confirmText="Archivar"
        variant="warning"
        onConfirm={handleDeleteConfirm}
        onCancel={() => setConfirmModal({ open: false, id: null, name: '' })}
      />

      {showModal && (
        <div className="modal-overlay" onClick={() => setShowModal(false)}>
          <div className="modal" onClick={(e) => e.stopPropagation()}>
            <div className="modal-header">
              <h2>{isEditing ? 'Editar Categoría' : 'Nueva Categoría'}</h2>
              <button className="modal-close" onClick={() => setShowModal(false)}><X size={16} /></button>
            </div>
            <form onSubmit={handleSubmit}>
              <div className="form-group"><label className="form-label">Nombre *</label><input className="form-input" value={form.name} onChange={(e) => setForm({ ...form, name: e.target.value })} required placeholder="Nombre de categoría" disabled={actionLoading} /></div>
              <div className="form-group"><label className="form-label">Descripción</label><input className="form-input" value={form.description} onChange={(e) => setForm({ ...form, description: e.target.value })} placeholder="Descripción opcional" disabled={actionLoading} /></div>
              <div className="modal-actions"><button type="button" className="btn btn-ghost" onClick={() => setShowModal(false)} disabled={actionLoading}>Cancelar</button><button type="submit" className="btn btn-accent" disabled={actionLoading}>{actionLoading ? 'Guardando...' : (isEditing ? 'Actualizar' : 'Crear')}</button></div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
}
