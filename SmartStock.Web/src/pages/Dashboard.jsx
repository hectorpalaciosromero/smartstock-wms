import { useState, useEffect } from 'react';
import { dashboardAPI } from '../services/api';
import { Package, Layers, Warehouse as WarehouseIcon, AlertTriangle, XCircle, ArrowDownCircle, ArrowUpCircle, RefreshCw, DollarSign } from 'lucide-react';
import { BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip, ResponsiveContainer, PieChart, Pie, Cell, AreaChart, Area } from 'recharts';

const COLORS = ['#00D4FF', '#3B82F6', '#10B981', '#F59E0B', '#8B5CF6', '#EF4444', '#EC4899', '#14B8A6', '#F97316', '#6366F1'];

export default function Dashboard() {
  const [data, setData] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    loadDashboard();
  }, []);

  const loadDashboard = async () => {
    setLoading(true);
    setError(null);
    try {
      const res = await dashboardAPI.get();
      setData(res.data);
    } catch (err) {
      const status = err?.response?.status;
      const msg = status
        ? `Error ${status}: ${err.response?.data?.message ?? 'No se pudo cargar el dashboard.'}`
        : 'No se pudo conectar con el servidor. Verifica que la API esté activa.';
      setError(msg);
      console.error('[Dashboard] load error:', err);
    } finally {
      setLoading(false);
    }
  };

  if (loading) return <div className="loading-spinner"><div className="spinner"></div></div>;
  if (error || !data) return (
    <div className="empty-state">
      <h3>⚠️ Error al cargar dashboard</h3>
      <p style={{ fontSize: '0.85rem', color: 'var(--color-text-muted)', marginTop: '8px', maxWidth: '420px' }}>
        {error ?? 'No se recibieron datos del servidor.'}
      </p>
      <button
        className="btn btn-primary"
        style={{ marginTop: '20px' }}
        onClick={loadDashboard}
      >
        <RefreshCw size={16} /> Reintentar
      </button>
    </div>
  );

  const kpis = [
    { label: 'Total Productos', value: data.totalProducts, icon: Package, color: 'blue', sub: 'Activos en el sistema' },
    { label: 'Categorías', value: data.totalCategories, icon: Layers, color: 'purple', sub: 'Grupos de productos' },
    { label: 'Almacenes', value: data.totalWarehouses, icon: WarehouseIcon, color: 'cyan', sub: 'Ubicaciones activas' },
    { label: 'Stock Bajo', value: data.lowStockCount, icon: AlertTriangle, color: 'amber', sub: 'Requiere reposición' },
    { label: 'Sin Stock', value: data.outOfStockCount, icon: XCircle, color: 'red', sub: 'Productos agotados' },
    { label: 'Valor de Inventario', value: `S/. ${data.totalInventoryValue?.toLocaleString('es-PE', { minimumFractionDigits: 0 })}`, icon: DollarSign, color: 'green', sub: 'Valor total estimado' },
  ];

  return (
    <div>
      <div className="page-header">
        <h1>Dashboard</h1>
        <p>Resumen en tiempo real de tu inventario</p>
      </div>

      {/* KPI Cards */}
      <div className="kpi-grid">
        {kpis.map((kpi, i) => (
          <div key={i} className={`kpi-card ${kpi.color} animate-in`} style={{ animationDelay: `${i * 0.05}s` }}>
            <div className="kpi-header">
              <span className="kpi-label">{kpi.label}</span>
              <div className={`kpi-icon ${kpi.color}`}>
                <kpi.icon size={20} />
              </div>
            </div>
            <div className="kpi-value">{kpi.value}</div>
            <div className="kpi-sub">{kpi.sub}</div>
          </div>
        ))}
      </div>

      {/* Charts */}
      <div className="charts-grid">
        {/* Movement Trend */}
        <div className="chart-card">
          <h3>📊 Tendencia de Movimientos (7 días)</h3>
          <ResponsiveContainer width="100%" height={260}>
            <AreaChart data={data.movementTrend}>
              <defs>
                <linearGradient id="gradEntries" x1="0" y1="0" x2="0" y2="1">
                  <stop offset="0%" stopColor="#00D4FF" stopOpacity={0.3} />
                  <stop offset="100%" stopColor="#00D4FF" stopOpacity={0} />
                </linearGradient>
                <linearGradient id="gradExits" x1="0" y1="0" x2="0" y2="1">
                  <stop offset="0%" stopColor="#EF4444" stopOpacity={0.3} />
                  <stop offset="100%" stopColor="#EF4444" stopOpacity={0} />
                </linearGradient>
              </defs>
              <CartesianGrid strokeDasharray="3 3" stroke="rgba(255,255,255,0.05)" />
              <XAxis dataKey="date" tick={{ fill: '#64748B', fontSize: 12 }} axisLine={false} />
              <YAxis tick={{ fill: '#64748B', fontSize: 12 }} axisLine={false} />
              <Tooltip contentStyle={{ background: '#1E293B', border: '1px solid rgba(255,255,255,0.1)', borderRadius: '8px', color: '#F1F5F9' }} />
              <Area type="monotone" dataKey="entries" stroke="#00D4FF" fill="url(#gradEntries)" strokeWidth={2} name="Entradas" />
              <Area type="monotone" dataKey="exits" stroke="#EF4444" fill="url(#gradExits)" strokeWidth={2} name="Salidas" />
            </AreaChart>
          </ResponsiveContainer>
        </div>

        {/* Stock by Category */}
        <div className="chart-card">
          <h3>📦 Stock por Categoría</h3>
          <ResponsiveContainer width="100%" height={260}>
            <PieChart>
              <Pie
                data={data.stockByCategory?.filter(c => c.totalStock > 0)}
                cx="50%"
                cy="50%"
                outerRadius={90}
                innerRadius={50}
                dataKey="totalStock"
                nameKey="categoryName"
                paddingAngle={3}
                stroke="none"
              >
                {data.stockByCategory?.map((_, i) => (
                  <Cell key={i} fill={COLORS[i % COLORS.length]} />
                ))}
              </Pie>
              <Tooltip contentStyle={{ background: '#1E293B', border: '1px solid rgba(255,255,255,0.1)', borderRadius: '8px', color: '#F1F5F9' }} />
            </PieChart>
          </ResponsiveContainer>
          <div style={{ display: 'flex', flexWrap: 'wrap', gap: '8px', justifyContent: 'center', marginTop: '12px' }}>
            {data.stockByCategory?.filter(c => c.totalStock > 0).map((cat, i) => (
              <span key={i} style={{ display: 'flex', alignItems: 'center', gap: '4px', fontSize: '0.72rem', color: 'var(--color-text-secondary)' }}>
                <span style={{ width: '8px', height: '8px', borderRadius: '2px', background: COLORS[i % COLORS.length] }}></span>
                {cat.categoryName}
              </span>
            ))}
          </div>
        </div>
      </div>

      {/* Low Stock Alerts */}
      {data.lowStockProducts?.length > 0 && (
        <div className="table-container animate-in">
          <div className="table-header">
            <h3>⚠️ Alertas de Stock Bajo</h3>
            <span className="badge-status badge-warning">{data.lowStockProducts.length} productos</span>
          </div>
          <table>
            <thead>
              <tr>
                <th>Producto</th>
                <th>SKU</th>
                <th>Categoría</th>
                <th>Stock Actual</th>
                <th>Stock Mínimo</th>
                <th>Estado</th>
              </tr>
            </thead>
            <tbody>
              {data.lowStockProducts.map((p) => (
                <tr key={p.id}>
                  <td style={{ fontWeight: 600, color: 'var(--color-text-primary)' }}>{p.name}</td>
                  <td>{p.sku || '-'}</td>
                  <td>{p.categoryName}</td>
                  <td style={{ fontWeight: 700, color: p.currentStock === 0 ? '#EF4444' : '#F59E0B' }}>{p.currentStock}</td>
                  <td>{p.minStock}</td>
                  <td>
                    {p.currentStock === 0 
                      ? <span className="badge-status badge-danger">Sin stock</span>
                      : <span className="badge-status badge-warning">Bajo</span>
                    }
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}
      {/* Recent Movements */}
      {data.recentMovements?.length > 0 && (
        <div className="table-container animate-in" style={{ marginTop: '24px' }}>
          <div className="table-header">
            <h3>🔄 Últimos Movimientos</h3>
            <span className="badge-status badge-info">{data.recentMovements.length} recientes</span>
          </div>
          <table>
            <thead>
              <tr>
                <th>Tipo</th>
                <th>Producto</th>
                <th>SKU</th>
                <th>Almacén</th>
                <th>Cantidad</th>
                <th>Usuario</th>
                <th>Fecha</th>
              </tr>
            </thead>
            <tbody>
              {data.recentMovements.map((m) => (
                <tr key={m.id}>
                  <td>
                    {m.movementType === 'Entry'
                      ? <span className="badge-status badge-success">Entrada</span>
                      : m.movementType === 'Exit'
                        ? <span className="badge-status badge-danger">Salida</span>
                        : <span className="badge-status badge-warning">Ajuste</span>}
                  </td>
                  <td style={{ fontWeight: 600, color: 'var(--color-text-primary)' }}>{m.productName}</td>
                  <td style={{ fontFamily: 'monospace', fontSize: '0.8rem' }}>{m.productSKU || '-'}</td>
                  <td style={{ fontSize: '0.85rem' }}>{m.warehouseName}</td>
                  <td style={{ fontWeight: 700 }}>{m.quantity}</td>
                  <td style={{ fontSize: '0.82rem', color: 'var(--color-text-secondary)' }}>{m.userName || '-'}</td>
                  <td style={{ fontSize: '0.8rem', color: 'var(--color-text-muted)', whiteSpace: 'nowrap' }}>
                    {new Date(m.createdAt).toLocaleString('es-PE')}
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}
    </div>
  );
}
