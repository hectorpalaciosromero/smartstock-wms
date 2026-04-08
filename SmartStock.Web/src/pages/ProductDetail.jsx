import { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { productsAPI, movementsAPI } from '../services/api';
import { useToast } from '../components/ui/Toast';
import { ArrowLeft, Box, PackageOpen, Layers, Barcode, Tag, ArrowUpCircle, ArrowDownCircle, RefreshCw, ArrowLeftRight, Download, Scan } from 'lucide-react';
import PillToggle from '../components/ui/PillToggle';

export default function ProductDetail() {
  const { id } = useParams();
  const navigate = useNavigate();
  const toast = useToast();

  const [product, setProduct] = useState(null);
  const [movements, setMovements] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    loadData();
  }, [id]);

  const loadData = async () => {
    setLoading(true);
    try {
      // 1. Cargar Producto
      const pRes = await productsAPI.getById(id);
      setProduct(pRes.data);

      // 2. Cargar Movimientos Recientes (10 max, sin paginación aquí)
      const mRes = await movementsAPI.getAll({ productId: id, page: 1, pageSize: 15 });
      setMovements(mRes.data.items);
    } catch {
      toast.error('Error cargando detalles del producto');
      navigate('/products', { replace: true });
    } finally {
      setLoading(false);
    }
  };

  const handleStatusChange = async (newStatus) => {
    try {
      if (newStatus) await productsAPI.restore(id);
      else await productsAPI.delete(id);
      setProduct((prev) => ({ ...prev, isActive: newStatus }));
      toast.success(`Producto ${newStatus ? 'Desarchivado' : 'Archivado'}`);
    } catch (err) {
      toast.error('Error cambiando estado');
    }
  };

  const generateQRUrl = (text) => `https://api.qrserver.com/v1/create-qr-code/?size=200x200&data=${encodeURIComponent(text)}`;

  const getTypeIcon = (type) => {
    switch (type) {
      case 'Entry':      return <ArrowDownCircle size={16} color="#10B981" />;
      case 'Exit':       return <ArrowUpCircle   size={16} color="#EF4444" />;
      case 'Adjustment': return <RefreshCw       size={16} color="#F59E0B" />;
      default:           return <ArrowLeftRight  size={16} color="#3B82F6" />;
    }
  };

  if (loading) return <div className="loading-spinner"><div className="spinner"></div></div>;
  if (!product) return null;

  return (
    <div style={{ maxWidth: '1000px', margin: '0 auto', paddingBottom: '40px' }}>
      
      {/* ── HEADER CON BOTON ATRAS ── */}
      <div style={{ display: 'flex', alignItems: 'center', gap: '16px', marginBottom: '32px' }}>
        <button 
          className="btn btn-ghost btn-icon" 
          onClick={() => navigate('/products')}
          style={{ width: '40px', height: '40px', background: 'var(--color-bg-secondary)' }}
          title="Volver al Catálogo"
        >
          <ArrowLeft size={18} />
        </button>
        <div style={{ flex: 1 }}>
          <div style={{ display: 'flex', alignItems: 'center', gap: '12px' }}>
            <h1 style={{ margin: 0, fontSize: '1.8rem' }}>{product.name}</h1>
            <PillToggle active={product.isActive} onChange={handleStatusChange} />
          </div>
          <div style={{ display: 'flex', gap: '16px', color: 'var(--color-text-secondary)', marginTop: '6px', fontSize: '0.9rem' }}>
            <span style={{ display: 'flex', alignItems: 'center', gap: '6px' }}><Barcode size={15} /> {product.sku}</span>
            <span style={{ display: 'flex', alignItems: 'center', gap: '6px' }}><Layers size={15} /> {product.categoryName}</span>
          </div>
        </div>
      </div>

      <div style={{ display: 'grid', gridTemplateColumns: 'minmax(300px, 1fr) 2.5fr', gap: '24px', alignItems: 'start' }}>
        
        {/* ── COLUMNA IZQUIERDA: TARJETA DEL PRODUCTO / ETIQUETA QR ── */}
        <div style={{ display: 'flex', flexDirection: 'column', gap: '24px' }}>
          <div className="card animate-in" style={{ padding: '0', overflow: 'hidden' }}>
            {product.imageUrl ? (
              <div style={{ width: '100%', aspectRatio: '1', background: '#000' }}>
                <img src={product.imageUrl} alt={product.name} style={{ width: '100%', height: '100%', objectFit: 'cover' }} />
              </div>
            ) : (
              <div style={{ width: '100%', aspectRatio: '1', background: 'var(--color-bg-secondary)', display: 'flex', alignItems: 'center', justifyContent: 'center' }}>
                <Box size={64} style={{ opacity: 0.1 }} />
              </div>
            )}
            <div style={{ padding: '20px' }}>
              <div style={{ fontSize: '1.6rem', fontWeight: 800, color: 'var(--color-accent)', marginBottom: '4px' }}>
                S/. {product.price?.toFixed(2)}
              </div>
              <p style={{ color: 'var(--color-text-secondary)', fontSize: '0.85rem', lineHeight: 1.5, marginBottom: '16px' }}>
                {product.description || 'Sin descripción detallada.'}
              </p>

              {product.supplierName && (
                <div style={{ fontSize: '0.85rem', color: 'var(--color-text-muted)', paddingTop: '16px', borderTop: '1px solid var(--glass-border)' }}>
                  <strong style={{ color: 'var(--color-text-primary)' }}>Proveedor:</strong> {product.supplierName}
                </div>
              )}
            </div>
          </div>

          <div className="card animate-in" style={{ textAlign: 'center' }}>
            <h3 style={{ fontSize: '0.95rem', fontWeight: 600, marginBottom: '16px', display: 'flex', alignItems: 'center', justifyContent: 'center', gap: '6px' }}>
              <Scan size={16} color="var(--color-accent)"/> Identificación Única
            </h3>
            <img src={generateQRUrl(product.barcode || product.qrCode || product.sku)} alt="QR Code" style={{ width: '140px', height: '140px', margin: '0 auto 12px' }} />
            <div style={{ background: 'var(--color-bg-primary)', padding: '6px', borderRadius: '6px', fontFamily: 'monospace', fontSize: '0.9rem', letterSpacing: '1px' }}>
              {product.barcode || 'N/A'}
            </div>
          </div>
        </div>

        {/* ── COLUMNA DERECHA: KPIs Y MOVIMIENTOS ── */}
        <div style={{ display: 'flex', flexDirection: 'column', gap: '24px' }}>
          
          {/* KPIs */}
          <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '16px' }}>
            <div className="card animate-in" style={{ padding: '24px', display: 'flex', alignItems: 'center', gap: '20px' }}>
              <div style={{ width: '56px', height: '56px', borderRadius: '16px', background: 'rgba(16, 185, 129, 0.1)', display: 'flex', alignItems: 'center', justifyContent: 'center', flexShrink: 0 }}>
                <PackageOpen size={28} color="#10B981" />
              </div>
              <div>
                <div style={{ fontSize: '0.8rem', color: 'var(--color-text-muted)', textTransform: 'uppercase', letterSpacing: '1px', fontWeight: 600, marginBottom: '4px' }}>Stock Actual</div>
                <div style={{ fontSize: '2.5rem', fontWeight: 800, lineHeight: 1, color: product.currentStock <= product.minStock ? '#F59E0B' : '#10B981' }}>
                  {product.currentStock}
                </div>
              </div>
            </div>

            <div className="card animate-in" style={{ padding: '24px', display: 'flex', alignItems: 'center', gap: '20px' }}>
              <div style={{ width: '56px', height: '56px', borderRadius: '16px', background: 'rgba(239, 68, 68, 0.1)', display: 'flex', alignItems: 'center', justifyContent: 'center', flexShrink: 0 }}>
                <Tag size={28} color="#EF4444" />
              </div>
              <div>
                <div style={{ fontSize: '0.8rem', color: 'var(--color-text-muted)', textTransform: 'uppercase', letterSpacing: '1px', fontWeight: 600, marginBottom: '4px' }}>Stock Mínimo (Alerta)</div>
                <div style={{ fontSize: '2.5rem', fontWeight: 800, lineHeight: 1, color: '#EF4444' }}>
                  {product.minStock}
                </div>
              </div>
            </div>
          </div>

          {/* Historial de Movimientos */}
          <div className="card animate-in" style={{ flex: 1, display: 'flex', flexDirection: 'column' }}>
            <h3 style={{ fontSize: '1.05rem', fontWeight: 600, marginBottom: '16px', borderBottom: '1px solid var(--glass-border)', paddingBottom: '16px' }}>
              Historial de Transacciones (Últimas 15)
            </h3>
            
            {movements.length === 0 ? (
              <div className="empty-state" style={{ flex: 1 }}>
                <ArrowLeftRight size={32} />
                <h4 style={{ fontSize: '1rem', marginTop: '12px' }}>Sin movimientos</h4>
                <p>Este producto no tiene historial de flujo en almacén.</p>
              </div>
            ) : (
              <div className="table-container" style={{ margin: 0 }}>
                <table style={{ margin: 0 }}>
                  <thead>
                    <tr>
                      <th style={{ background: 'transparent' }}>Tipo</th>
                      <th style={{ background: 'transparent' }}>Fecha</th>
                      <th style={{ background: 'transparent' }}>Almacén</th>
                      <th style={{ background: 'transparent' }}>Cant.</th>
                      <th style={{ background: 'transparent' }}>Usuario</th>
                    </tr>
                  </thead>
                  <tbody>
                    {movements.map(m => (
                      <tr key={m.id}>
                        <td>
                          <span style={{ display: 'flex', alignItems: 'center', gap: '6px' }}>
                            {getTypeIcon(m.movementType)}
                            <span style={{ fontSize: '0.85rem' }}>
                              {m.movementType === 'Entry' ? 'Ingreso' : m.movementType === 'Exit' ? 'Salida' : 'Ajuste'}
                            </span>
                          </span>
                        </td>
                        <td style={{ fontSize: '0.85rem', color: 'var(--color-text-secondary)', whiteSpace: 'nowrap' }}>
                          {new Date(m.createdAt).toLocaleString('es-PE')}
                        </td>
                        <td style={{ fontSize: '0.85rem' }}>{m.warehouseName}</td>
                        <td style={{ fontWeight: 700, fontSize: '0.9rem' }}>{m.quantity}</td>
                        <td style={{ fontSize: '0.85rem', color: 'var(--color-text-muted)' }}>{m.userName || 'Sistema'}</td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              </div>
            )}
          </div>
        </div>

      </div>
    </div>
  );
}
