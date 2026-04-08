import { useState, useEffect } from 'react';
import { useSearchParams, useNavigate } from 'react-router-dom';
import { productsAPI, categoriesAPI, suppliersAPI } from '../services/api';
import { useAuth } from '../context/AuthContext';
import { useToast } from '../components/ui/Toast';
import useDebounce from '../hooks/useDebounce';
import PillToggle from '../components/ui/PillToggle';
import Pagination from '../components/ui/Pagination';
import ConfirmModal from '../components/ui/ConfirmModal';
import { Plus, Search, Edit, Trash2, X, Package, AlertTriangle, UploadCloud, Loader2, DownloadCloud, RotateCcw, Eye, CheckSquare } from 'lucide-react';

export default function Products() {
  const [products, setProducts] = useState([]);
  const [categories, setCategories] = useState([]);
  const [suppliers, setSuppliers] = useState([]);
  const [loading, setLoading] = useState(true);
  const [search, setSearch] = useState('');
  const [categoryFilter, setCategoryFilter] = useState('');
  const [lowStockFilter, setLowStockFilter] = useState(false);
  const [page, setPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);
  const [showModal, setShowModal] = useState(false);
  const [editProduct, setEditProduct] = useState(null);
  const [uploadingImage, setUploadingImage] = useState(false);
  const [exporting, setExporting] = useState(false);
  const [showArchived, setShowArchived] = useState(false);
  const [form, setForm] = useState({ name: '', sku: '', barcode: '', description: '', price: '', currentStock: '', minStock: '5', categoryId: '', supplierId: '', imageUrl: '' });

  // Batch actions
  const [selectedIds, setSelectedIds] = useState([]);
  const [batching, setBatching] = useState(false);

  // Confirm modal state
  const [confirmModal, setConfirmModal] = useState({ open: false, id: null, name: '' });

  const { user } = useAuth();
  const toast = useToast();
  const navigate = useNavigate();
  const canEdit = user?.role === 'Admin' || user?.role === 'Supervisor' || user?.role === 'Almacenero';

  // Read search query param from URL (global search from Navbar)
  const [searchParams, setSearchParams] = useSearchParams();
  useEffect(() => {
    const urlSearch = searchParams.get('search');
    if (urlSearch) {
      setSearch(urlSearch);
      setSearchParams({}, { replace: true });
    }
  }, [searchParams, setSearchParams]);

  const debouncedSearch = useDebounce(search, 400);

  useEffect(() => { setPage(1); setSelectedIds([]); }, [debouncedSearch, categoryFilter, lowStockFilter, showArchived]);
  useEffect(() => { loadProducts(); }, [page, debouncedSearch, categoryFilter, lowStockFilter, showArchived]);
  useEffect(() => { loadCategories(); loadSuppliers(); }, []);

  const loadProducts = async () => {
    setLoading(true);
    try {
      const params = { page, pageSize: 12, search: debouncedSearch || undefined, categoryId: categoryFilter || undefined, lowStock: lowStockFilter || undefined, includeInactive: showArchived || undefined };
      const res = await productsAPI.getAll(params);
      setProducts(res.data.items);
      setTotalPages(res.data.totalPages);
    } catch (err) {
      toast.error('Error cargando productos');
    } finally {
      setLoading(false);
    }
  };

  const loadCategories = async () => {
    try { const res = await categoriesAPI.getAll(); setCategories(res.data); } catch { }
  };

  const loadSuppliers = async () => {
    try { const res = await suppliersAPI.getAll(); setSuppliers(res.data); } catch { }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    const data = { ...form, price: parseFloat(form.price), currentStock: parseInt(form.currentStock), minStock: parseInt(form.minStock), categoryId: parseInt(form.categoryId), supplierId: form.supplierId ? parseInt(form.supplierId) : null, imageUrl: form.imageUrl || null };
    try {
      if (editProduct) {
        await productsAPI.update(editProduct.id, data);
        toast.success('Producto actualizado exitosamente');
      } else {
        await productsAPI.create(data);
        toast.success('Producto creado exitosamente');
      }
      setShowModal(false); setEditProduct(null);
      resetForm();
      loadProducts();
    } catch (err) {
      toast.error(err.response?.data?.message || 'Error guardando producto');
    }
  };

  const resetForm = () => setForm({ name: '', sku: '', barcode: '', description: '', price: '', currentStock: '', minStock: '5', categoryId: '', supplierId: '', imageUrl: '' });

  const handleEdit = (product) => {
    setEditProduct(product);
    setForm({ name: product.name, sku: product.sku || '', barcode: product.barcode || '', description: product.description || '', price: product.price.toString(), currentStock: product.currentStock.toString(), minStock: product.minStock.toString(), categoryId: product.categoryId?.toString() || '', supplierId: product.supplierId?.toString() || '', imageUrl: product.imageUrl || '' });
    setShowModal(true);
  };

  const handleImageUpload = async (e) => {
    const file = e.target.files[0];
    if (!file) return;
    setUploadingImage(true);
    const formData = new FormData();
    formData.append('file', file);
    formData.append('upload_preset', import.meta.env.VITE_CLOUDINARY_UPLOAD_PRESET || 'smartstock');
    formData.append('cloud_name', import.meta.env.VITE_CLOUDINARY_CLOUD_NAME || 'dwfdo3rhu');
    try {
      const res = await fetch(`https://api.cloudinary.com/v1_1/${import.meta.env.VITE_CLOUDINARY_CLOUD_NAME || 'dwfdo3rhu'}/image/upload`, { method: 'POST', body: formData });
      const data = await res.json();
      if (data.secure_url) {
        setForm(f => ({ ...f, imageUrl: data.secure_url }));
        toast.success('Imagen subida correctamente');
      }
    } catch {
      toast.error('Error subiendo imagen a Cloudinary');
    } finally {
      setUploadingImage(false);
    }
  };

  const handleDeleteConfirm = () => {
    setConfirmModal(prev => ({ ...prev, open: false }));
    (async () => {
      try {
        await productsAPI.delete(confirmModal.id);
        toast.success(`Producto "${confirmModal.name}" archivado`);
        loadProducts();
      } catch {
        toast.error('Error archivando producto');
      }
    })();
  };

  const handleRestore = async (id, name) => {
    try {
      await productsAPI.restore(id);
      toast.success(`Producto "${name}" restaurado`);
      loadProducts();
    } catch {
      toast.error('Error restaurando producto');
    }
  };

  const handleExport = async () => {
    setExporting(true);
    try {
      const params = { search: debouncedSearch || undefined, categoryId: categoryFilter || undefined, lowStock: lowStockFilter || undefined };
      const res = await productsAPI.export(params);
      const url = window.URL.createObjectURL(new Blob([res.data], { type: 'text/csv' }));
      const link = document.createElement('a');
      link.href = url;
      link.setAttribute('download', `Inventario_SmartStock_${new Date().toISOString().split('T')[0]}.csv`);
      document.body.appendChild(link);
      link.click();
      link.remove();
      toast.success('Inventario exportado exitosamente');
    } catch {
      toast.error('Error descargando el inventario CSV');
    } finally {
      setExporting(false);
    }
  };

  const handleSelectAll = (e) => {
    if (e.target.checked) setSelectedIds(products.map(p => p.id));
    else setSelectedIds([]);
  };

  const handleSelect = (id) => {
    setSelectedIds(prev => prev.includes(id) ? prev.filter(x => x !== id) : [...prev, id]);
  };

  const handleBatchOperation = async (actionStr) => {
    if (selectedIds.length === 0) return;
    setBatching(true);
    try {
      const res = await productsAPI.batchAction({ productIds: selectedIds, action: actionStr });
      toast.success(res.data.message || `Operación exitosa en ${selectedIds.length} elementos`);
      setSelectedIds([]);
      loadProducts();
    } catch (err) {
      toast.error(err.response?.data?.message || 'Error ejecutando operación masiva');
    } finally {
      setBatching(false);
    }
  };

  useEffect(() => {
    const handleKey = (e) => { if (e.key === 'Escape') setShowModal(false); };
    if (showModal) { document.addEventListener('keydown', handleKey); return () => document.removeEventListener('keydown', handleKey); }
  }, [showModal]);

  return (
    <div>
      <div className="page-header" style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start' }}>
        <div>
          <h1>Productos</h1>
          <p>Gestión del catálogo de productos e inventario</p>
        </div>
        <div style={{ display: 'flex', gap: '12px', flexWrap: 'wrap', alignItems: 'center' }}>
          {canEdit && (
            <>
              <PillToggle
                id="toggle-archived-prod"
                label="Ver Archivados"
                checked={showArchived}
                onChange={() => setShowArchived(p => !p)}
              />
              <button className="btn btn-ghost" onClick={handleExport} disabled={exporting}>
                {exporting ? <Loader2 size={18} className="spinner" /> : <DownloadCloud size={18} />} Exportar
              </button>
              <button className="btn btn-accent" onClick={() => { setEditProduct(null); resetForm(); setShowModal(true); }}>
                <Plus size={18} /> Nuevo Producto
              </button>
            </>
          )}
        </div>
      </div>

      <div className="table-container" style={{ marginBottom: '24px', position: 'relative' }}>
        <div className="table-header">
          <div style={{ display: 'flex', gap: '12px', alignItems: 'center', flex: 1, flexWrap: 'wrap' }}>
            <div className="navbar-search" style={{ flex: 1, maxWidth: '300px' }}>
              <Search size={16} />
              <input type="text" placeholder="Buscar por nombre, SKU, barcode..." value={search} onChange={(e) => setSearch(e.target.value)} />
            </div>
            <select className="form-select" style={{ width: 'auto', minWidth: '160px' }} value={categoryFilter} onChange={(e) => { setCategoryFilter(e.target.value); }}>
              <option value="">Todas las categorías</option>
              {categories.map(c => <option key={c.id} value={c.id}>{c.name}</option>)}
            </select>
            <label style={{ display: 'flex', alignItems: 'center', gap: '6px', fontSize: '0.85rem', color: 'var(--color-text-secondary)', cursor: 'pointer' }}>
              <input type="checkbox" checked={lowStockFilter} onChange={(e) => { setLowStockFilter(e.target.checked); }} />
              <AlertTriangle size={14} /> Solo stock bajo
            </label>
          </div>
        </div>

        {loading ? (
          <div className="loading-spinner"><div className="spinner"></div></div>
        ) : products.length === 0 ? (
          <div className="empty-state">
            <Package size={48} />
            <h3>No se encontraron productos</h3>
            <p>Intenta con otros filtros o crea un nuevo producto</p>
            {canEdit && !search && !categoryFilter && !lowStockFilter && (
              <button className="btn btn-accent" style={{ marginTop: '16px' }} onClick={() => { setEditProduct(null); resetForm(); setShowModal(true); }}>
                <Plus size={16} /> Crear primer producto
              </button>
            )}
          </div>
        ) : (
          <>
            <div style={{ overflowX: 'auto' }}>
              <table>
                <thead>
                  <tr>
                    {canEdit && (
                      <th style={{ width: '40px', textAlign: 'center' }}>
                        <input type="checkbox" checked={selectedIds.length === products.length && products.length > 0} onChange={handleSelectAll} style={{ cursor: 'pointer' }} />
                      </th>
                    )}
                    <th>Imagen</th>
                    <th>Producto</th>
                    <th>SKU</th>
                    <th>Categoría</th>
                    <th>Precio</th>
                    <th>Stock</th>
                    <th>Estado</th>
                    <th>Acciones</th>
                  </tr>
                </thead>
                <tbody>
                  {products.map(p => (
                    <tr key={p.id} style={{ opacity: p.isActive ? 1 : 0.55, background: selectedIds.includes(p.id) ? 'rgba(0, 212, 255, 0.05)' : 'transparent' }}>
                      {canEdit && (
                        <td style={{ textAlign: 'center' }}>
                          <input type="checkbox" checked={selectedIds.includes(p.id)} onChange={() => handleSelect(p.id)} style={{ cursor: 'pointer' }} />
                        </td>
                      )}
                      <td>
                        {p.imageUrl ? (
                          <img src={p.imageUrl} alt={p.name} style={{ width: '40px', height: '40px', borderRadius: '8px', objectFit: 'cover' }} />
                        ) : (
                          <div style={{ width: '40px', height: '40px', borderRadius: '8px', background: 'var(--color-bg-secondary)', display: 'flex', alignItems: 'center', justifyContent: 'center', color: 'var(--color-text-muted)' }}>
                            <Package size={20} />
                          </div>
                        )}
                      </td>
                      <td style={{ fontWeight: 600, color: 'var(--color-text-primary)' }}>{p.name}</td>
                      <td style={{ fontFamily: 'monospace', fontSize: '0.8rem' }}>{p.sku || '-'}</td>
                      <td><span className="badge-status badge-info">{p.categoryName}</span></td>
                      <td>S/. {p.price?.toFixed(2)}</td>
                      <td style={{ fontWeight: 700, color: p.isLowStock ? (p.currentStock === 0 ? '#EF4444' : '#F59E0B') : '#10B981' }}>{p.currentStock}</td>
                      <td>
                        {p.isActive === false ? <span className="badge-status badge-danger">Archivado</span>
                          : p.currentStock === 0 ? <span className="badge-status badge-danger">Sin stock</span>
                          : p.isLowStock ? <span className="badge-status badge-warning">Stock bajo</span>
                          : <span className="badge-status badge-success">OK</span>}
                      </td>
                      <td>
                        <div style={{ display: 'flex', gap: '6px' }}>
                          <button className="btn btn-ghost btn-icon btn-sm" onClick={() => navigate(`/products/${p.id}`)} title="Ver detalles"><Eye size={15} color="var(--color-text-secondary)" /></button>
                          {canEdit && (
                            p.isActive ? (
                              <>
                                <button className="btn btn-ghost btn-icon btn-sm" onClick={() => handleEdit(p)} title="Editar"><Edit size={15} /></button>
                                <button className="btn btn-danger btn-icon btn-sm" onClick={() => setConfirmModal({ open: true, id: p.id, name: p.name })} title="Archivar"><Trash2 size={15} /></button>
                              </>
                            ) : (
                              <button className="btn btn-ghost btn-sm" style={{ padding: '6px', color: 'var(--color-success)' }} onClick={() => handleRestore(p.id, p.name)} title="Restaurar Producto">
                                <RotateCcw size={15} />
                              </button>
                            )
                          )}
                        </div>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>

            <Pagination page={page} totalPages={totalPages} onChange={setPage} />
          </>
        )}
      </div>

      {/* ── BATCH ACTION TOOLBAR ── */}
      {selectedIds.length > 0 && (
        <div className="animate-in" style={{
          position: 'fixed', bottom: '30px', left: '50%', transform: 'translateX(-50%)',
          background: 'var(--color-bg-card)', border: '1px solid var(--color-accent)', boxShadow: '0 8px 32px rgba(0,0,0,0.4)', borderRadius: '12px', padding: '12px 24px', zIndex: 100, display: 'flex', alignItems: 'center', gap: '24px'
        }}>
          <div style={{ display: 'flex', alignItems: 'center', gap: '8px', color: 'var(--color-text-primary)' }}>
            <CheckSquare size={18} color="var(--color-accent)" />
            <span style={{ fontWeight: 600 }}>{selectedIds.length} productos seleccionados</span>
          </div>
          <div style={{ borderLeft: '1px solid var(--glass-border)', height: '24px' }}></div>
          <div style={{ display: 'flex', gap: '12px' }}>
            <button className="btn btn-danger btn-sm" onClick={() => handleBatchOperation('Archive')} disabled={batching}>
              {batching ? <Loader2 size={16} className="spinner" /> : <Trash2 size={16} />} Archivar
            </button>
            <button className="btn btn-ghost btn-sm" style={{ color: 'var(--color-success)' }} onClick={() => handleBatchOperation('Restore')} disabled={batching}>
              {batching ? <Loader2 size={16} className="spinner" /> : <RotateCcw size={16} />} Restaurar
            </button>
          </div>
        </div>
      )}

      {/* Confirm Archive Modal */}
      <ConfirmModal
        open={confirmModal.open}
        title="Archivar Producto"
        message={`¿Archivar "${confirmModal.name}"? Desaparecerá de las métricas principales pero podrás restaurarlo después.`}
        confirmText="Archivar"
        variant="warning"
        onConfirm={handleDeleteConfirm}
        onCancel={() => setConfirmModal({ open: false, id: null, name: '' })}
      />

      {/* Create/Edit Modal */}
      {showModal && (
        <div className="modal-overlay" onClick={() => setShowModal(false)}>
          <div className="modal" onClick={(e) => e.stopPropagation()}>
            <div className="modal-header">
              <h2>{editProduct ? 'Editar Producto' : 'Nuevo Producto'}</h2>
              <button className="modal-close" onClick={() => setShowModal(false)}><X size={16} /></button>
            </div>
            <form onSubmit={handleSubmit}>
              <div style={{ display: 'flex', flexDirection: 'column', alignItems: 'center', marginBottom: '20px' }}>
                <div style={{ position: 'relative', width: '120px', height: '120px', borderRadius: '12px', overflow: 'hidden', backgroundColor: 'var(--color-bg-secondary)', display: 'flex', alignItems: 'center', justifyContent: 'center', border: '2px dashed var(--color-border)', cursor: 'pointer' }} onClick={() => document.getElementById('imageUpload').click()}>
                  {uploadingImage ? (
                    <Loader2 size={32} className="spinner" style={{ color: 'var(--color-accent)' }} />
                  ) : form.imageUrl ? (
                    <img src={form.imageUrl} alt="Preview" style={{ width: '100%', height: '100%', objectFit: 'cover' }} />
                  ) : (
                    <div style={{ display: 'flex', flexDirection: 'column', alignItems: 'center', color: 'var(--color-text-muted)' }}>
                      <UploadCloud size={32} />
                      <span style={{ fontSize: '0.7rem', marginTop: '4px' }}>Subir Foto</span>
                    </div>
                  )}
                </div>
                <input id="imageUpload" type="file" accept="image/*" style={{ display: 'none' }} onChange={handleImageUpload} disabled={uploadingImage} />
                {form.imageUrl && (
                  <button type="button" className="btn btn-ghost btn-sm" style={{ marginTop: '8px', color: 'var(--color-danger)' }} onClick={() => setForm(f => ({...f, imageUrl: ''}))}>Eliminar foto</button>
                )}
              </div>

              <div className="form-group">
                <label className="form-label">Nombre *</label>
                <input className="form-input" value={form.name} onChange={(e) => setForm({ ...form, name: e.target.value })} required placeholder="Terminal Móvil TC52x" />
              </div>
              <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '16px' }}>
                <div className="form-group"><label className="form-label">SKU</label><input className="form-input" value={form.sku} onChange={(e) => setForm({ ...form, sku: e.target.value })} placeholder="ZBR-TC52X" /></div>
                <div className="form-group"><label className="form-label">Código de Barras</label><input className="form-input" value={form.barcode} onChange={(e) => setForm({ ...form, barcode: e.target.value })} placeholder="7501234567890" /></div>
              </div>
              <div className="form-group"><label className="form-label">Descripción</label><input className="form-input" value={form.description} onChange={(e) => setForm({ ...form, description: e.target.value })} placeholder="Descripción del producto..." /></div>
              <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr 1fr', gap: '16px' }}>
                <div className="form-group"><label className="form-label">Precio (S/.)</label><input className="form-input" type="number" step="0.01" value={form.price} onChange={(e) => setForm({ ...form, price: e.target.value })} required placeholder="0.00" /></div>
                <div className="form-group"><label className="form-label">Stock Inicial</label><input className="form-input" type="number" value={form.currentStock} onChange={(e) => setForm({ ...form, currentStock: e.target.value })} required placeholder="0" /></div>
                <div className="form-group"><label className="form-label">Stock Mínimo</label><input className="form-input" type="number" value={form.minStock} onChange={(e) => setForm({ ...form, minStock: e.target.value })} required placeholder="5" /></div>
              </div>
              <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '16px' }}>
                <div className="form-group">
                  <label className="form-label">Categoría *</label>
                  <select className="form-select" value={form.categoryId} onChange={(e) => setForm({ ...form, categoryId: e.target.value })} required>
                    <option value="">Seleccionar...</option>
                    {categories.map(c => <option key={c.id} value={c.id}>{c.name}</option>)}
                  </select>
                </div>
                <div className="form-group">
                  <label className="form-label">Proveedor</label>
                  <select className="form-select" value={form.supplierId || ''} onChange={(e) => setForm({ ...form, supplierId: e.target.value })}>
                    <option value="">Sin proveedor</option>
                    {suppliers.map(s => <option key={s.id} value={s.id}>{s.companyName}</option>)}
                  </select>
                </div>
              </div>
              <div className="modal-actions">
                <button type="button" className="btn btn-ghost" onClick={() => setShowModal(false)}>Cancelar</button>
                <button type="submit" className="btn btn-accent">{editProduct ? 'Guardar Cambios' : 'Crear Producto'}</button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
}
