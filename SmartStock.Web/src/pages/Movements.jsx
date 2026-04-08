import { useState, useEffect } from 'react';
import { movementsAPI, warehousesAPI } from '../services/api';
import { useToast } from '../components/ui/Toast';
import Pagination from '../components/ui/Pagination';
import { ArrowLeftRight, ArrowDownCircle, ArrowUpCircle, RefreshCw, Filter, Download } from 'lucide-react';

export default function Movements() {
  const [movements, setMovements]   = useState([]);
  const [warehouses, setWarehouses] = useState([]);
  const [loading, setLoading]       = useState(true);
  const [exporting, setExporting]   = useState(false);
  const [page, setPage]             = useState(1);
  const [totalPages, setTotalPages] = useState(1);
  const [totalCount, setTotalCount] = useState(0);

  const [warehouseFilter, setWarehouseFilter] = useState('');
  const [typeFilter, setTypeFilter]           = useState('');

  const toast = useToast();

  useEffect(() => { loadWarehouses(); }, []);
  useEffect(() => { loadMovements(); }, [page, warehouseFilter, typeFilter]);

  const loadWarehouses = async () => {
    try { const res = await warehousesAPI.getAll(); setWarehouses(res.data); } catch { /* non-critical */ }
  };

  const loadMovements = async () => {
    setLoading(true);
    try {
      const params = {
        page, pageSize: 20,
        warehouseId: warehouseFilter || undefined,
        movementType: typeFilter || undefined,
      };
      const res = await movementsAPI.getAll(params);
      setMovements(res.data.items);
      setTotalPages(res.data.totalPages);
      setTotalCount(res.data.totalCount);
    } catch {
      toast.error('Error cargando movimientos');
    } finally {
      setLoading(false);
    }
  };

  const handleFilterChange = (setter) => (e) => { setter(e.target.value); setPage(1); };

  const handleExport = async () => {
    setExporting(true);
    toast.success('Generando reporte CSV...', { duration: 2000 });
    try {
      const params = {
        warehouseId: warehouseFilter || undefined,
        movementType: typeFilter || undefined,
      };
      const res = await movementsAPI.export(params);
      
      const url = window.URL.createObjectURL(new Blob([res.data]));
      const a = document.createElement('a');
      a.href = url;
      a.setAttribute('download', `Movimientos_SmartStock_${new Date().toISOString().split('T')[0]}.csv`);
      document.body.appendChild(a);
      a.click();
      a.remove();
    } catch {
      toast.error('Error exportando reporte');
    } finally {
      setExporting(false);
    }
  };

  const getTypeIcon = (type) => {
    switch (type) {
      case 'Entry':      return <ArrowDownCircle size={16} color="#10B981" />;
      case 'Exit':       return <ArrowUpCircle   size={16} color="#EF4444" />;
      case 'Adjustment': return <RefreshCw       size={16} color="#F59E0B" />;
      default:           return <ArrowLeftRight  size={16} color="#3B82F6" />;
    }
  };

  const getTypeBadge = (type) => {
    switch (type) {
      case 'Entry':      return <span className="badge-status badge-success">Entrada</span>;
      case 'Exit':       return <span className="badge-status badge-danger">Salida</span>;
      case 'Adjustment': return <span className="badge-status badge-warning">Ajuste</span>;
      default:           return <span className="badge-status badge-info">Transferencia</span>;
    }
  };

  const hasFilters = warehouseFilter || typeFilter;

  return (
    <div>
      <div className="page-header" style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start' }}>
        <div>
          <h1>Movimientos de Inventario</h1>
          <p>Historial de entradas, salidas y ajustes de stock</p>
        </div>
        <button className="btn btn-accent" onClick={handleExport} disabled={exporting || totalCount === 0}>
          <Download size={18} /> {exporting ? 'Exportando...' : 'Exportar CSV'}
        </button>
      </div>

      <div className="table-container">
        <div className="table-header" style={{ flexWrap: 'wrap', gap: '12px' }}>
          <h3 style={{ display: 'flex', alignItems: 'center', gap: '8px', margin: 0 }}>
            <ArrowLeftRight size={18} style={{ display: 'inline', verticalAlign: 'middle' }} />
            Registro de Movimientos
            {totalCount > 0 && <span className="badge-status badge-info" style={{ fontWeight: 700 }}>{totalCount}</span>}
          </h3>
          <div style={{ display: 'flex', gap: '10px', alignItems: 'center', flexWrap: 'wrap' }}>
            <Filter size={15} color="var(--color-text-muted)" />
            <select className="form-input" style={{ padding: '7px 12px', height: 'auto', width: 'auto', minWidth: '160px', fontSize: '0.85rem' }} value={warehouseFilter} onChange={handleFilterChange(setWarehouseFilter)}>
              <option value="">Todos los almacenes</option>
              {warehouses.map(w => <option key={w.id} value={w.id}>{w.name}</option>)}
            </select>
            <select className="form-input" style={{ padding: '7px 12px', height: 'auto', width: 'auto', minWidth: '140px', fontSize: '0.85rem' }} value={typeFilter} onChange={handleFilterChange(setTypeFilter)}>
              <option value="">Todos los tipos</option>
              <option value="Entry">Entradas</option>
              <option value="Exit">Salidas</option>
              <option value="Adjustment">Ajustes</option>
            </select>
            {hasFilters && (
              <button className="btn btn-ghost btn-sm" style={{ fontSize: '0.8rem', padding: '6px 10px' }} onClick={() => { setWarehouseFilter(''); setTypeFilter(''); setPage(1); }}>Limpiar filtros</button>
            )}
          </div>
        </div>

        {loading ? (
          <div className="loading-spinner"><div className="spinner" /></div>
        ) : movements.length === 0 ? (
          <div className="empty-state">
            <ArrowLeftRight size={48} />
            <h3>Sin movimientos{hasFilters ? ' con estos filtros' : ' registrados'}</h3>
            <p>{hasFilters ? 'Prueba ajustando los filtros' : 'Escanea un producto para registrar un movimiento'}</p>
          </div>
        ) : (
          <>
            <div style={{ overflowX: 'auto' }}>
              <table>
                <thead><tr><th>Tipo</th><th>Producto</th><th>SKU</th><th>Almacén</th><th>Cantidad</th><th>Método</th><th>Usuario</th><th>Notas</th><th>Fecha</th></tr></thead>
                <tbody>
                  {movements.map(m => (
                    <tr key={m.id}>
                      <td>{getTypeBadge(m.movementType)}</td>
                      <td style={{ fontWeight: 600, color: 'var(--color-text-primary)' }}>{m.productName}</td>
                      <td style={{ fontFamily: 'monospace', fontSize: '0.8rem' }}>{m.productSKU || '-'}</td>
                      <td style={{ fontSize: '0.85rem' }}>{m.warehouseName}</td>
                      <td style={{ fontWeight: 700 }}>
                        <span style={{ display: 'flex', alignItems: 'center', gap: '5px' }}>{getTypeIcon(m.movementType)}{m.quantity}</span>
                      </td>
                      <td><span className="badge-status badge-info">{m.scanMethod}</span></td>
                      <td style={{ fontSize: '0.82rem', color: 'var(--color-text-secondary)' }}>{m.userName || '-'}</td>
                      <td style={{ fontSize: '0.8rem', color: 'var(--color-text-muted)', maxWidth: '160px', overflow: 'hidden', textOverflow: 'ellipsis', whiteSpace: 'nowrap' }}>{m.notes || '-'}</td>
                      <td style={{ fontSize: '0.8rem', color: 'var(--color-text-muted)', whiteSpace: 'nowrap' }}>{new Date(m.createdAt).toLocaleString('es-PE')}</td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
            <Pagination page={page} totalPages={totalPages} onChange={setPage} />
          </>
        )}
      </div>
    </div>
  );
}
