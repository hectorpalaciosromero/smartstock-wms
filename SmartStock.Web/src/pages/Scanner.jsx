import { useState, useEffect, useRef } from 'react';
import { Html5Qrcode } from 'html5-qrcode';
import { scanAPI, movementsAPI, warehousesAPI } from '../services/api';
import { useToast } from '../components/ui/Toast';
import { ScanBarcode, Camera, CameraOff, CheckCircle, XCircle, Plus, Minus, Package } from 'lucide-react';

export default function Scanner() {
  const [scanning, setScanning] = useState(false);
  const [scanResult, setScanResult] = useState(null);
  const [recentScans, setRecentScans] = useState([]);
  const [warehouses, setWarehouses] = useState([]);
  const [selectedWarehouse, setSelectedWarehouse] = useState('');
  const [quantity, setQuantity] = useState(1);
  const [movementType, setMovementType] = useState('Entry');
  const scannerRef = useRef(null);
  const html5QrCodeRef = useRef(null);
  const toast = useToast();

  useEffect(() => {
    loadWarehouses();
    loadRecentScans();
    return () => { stopScanner(); };
  }, []);

  const loadWarehouses = async () => {
    try { const res = await warehousesAPI.getAll(); setWarehouses(res.data); if (res.data.length) setSelectedWarehouse(res.data[0].id.toString()); } catch { }
  };

  const loadRecentScans = async () => {
    try { const res = await scanAPI.getRecent(10); setRecentScans(res.data); } catch { }
  };

  const startScanner = async () => {
    if (!scannerRef.current) return;
    try {
      const html5QrCode = new Html5Qrcode("scanner-viewport");
      html5QrCodeRef.current = html5QrCode;
      await html5QrCode.start(
        { facingMode: "environment" },
        { fps: 10, qrbox: { width: 250, height: 250 } },
        onScanSuccess,
        () => {}
      );
      setScanning(true);
    } catch (err) { 
        console.error('Scanner error:', err); 
        toast.error('No se pudo acceder a la cámara'); 
    }
  };

  const stopScanner = async () => {
    if (html5QrCodeRef.current) {
      try { await html5QrCodeRef.current.stop(); } catch { }
      html5QrCodeRef.current = null;
    }
    setScanning(false);
  };

  const onScanSuccess = async (decodedText, decodedResult) => {
    await stopScanner();
    try {
      const res = await scanAPI.process({ scanType: decodedResult.result?.format?.formatName || 'QR_CODE', rawData: decodedText, deviceInfo: navigator.userAgent.slice(0, 100) });
      setScanResult(res.data);
      loadRecentScans();
    } catch (err) { 
        toast.error('Error procesando escaneo'); 
    }
  };

  const handleRegisterMovement = async () => {
    if (!scanResult?.product || !selectedWarehouse) return;
    try {
      await movementsAPI.create({
        productId: scanResult.product.id,
        warehouseId: parseInt(selectedWarehouse),
        movementType,
        quantity,
        scanMethod: scanResult.scanType === 'QR_CODE' ? 'QRCode' : 'Barcode',
        notes: `Registrado por escaneo ${scanResult.scanType}`
      });
      toast.success(`${movementType === 'Entry' ? 'Entrada' : 'Salida'} registrada: ${quantity}x ${scanResult.product.name}`);
      setScanResult(null);
    } catch (err) { 
        toast.error(err.response?.data?.message || 'Error registrando movimiento'); 
    }
  };

  return (
    <div>
      <div className="page-header">
        <h1>Escáner QR / Código de Barras</h1>
        <p>Escanea productos para registrar entradas y salidas de inventario</p>
      </div>

      <div className="scanner-container">
        {/* Scanner Viewport */}
        <div>
          <div className={`scanner-viewport ${scanning ? 'active' : ''}`}>
            <div id="scanner-viewport" ref={scannerRef} style={{ width: '100%', height: '100%' }}>
              {!scanning && (
                <div style={{ textAlign: 'center', color: 'var(--color-text-muted)' }}>
                  <ScanBarcode size={48} style={{ marginBottom: '12px', opacity: 0.4 }} />
                  <p style={{ fontSize: '0.9rem' }}>Presiona "Iniciar Escáner" para activar la cámara</p>
                </div>
              )}
            </div>
          </div>
          <div style={{ display: 'flex', gap: '12px', marginTop: '16px' }}>
            {!scanning ? (
              <button className="btn btn-accent" onClick={startScanner} style={{ flex: 1, justifyContent: 'center' }}>
                <Camera size={18} /> Iniciar Escáner
              </button>
            ) : (
              <button className="btn btn-danger" onClick={stopScanner} style={{ flex: 1, justifyContent: 'center' }}>
                <CameraOff size={18} /> Detener
              </button>
            )}
          </div>
        </div>

        {/* Scan Result / Action Panel */}
        <div className="scanner-result">
          {scanResult ? (
            <div className="animate-in">
              <div style={{ display: 'flex', alignItems: 'center', gap: '12px', marginBottom: '20px' }}>
                {scanResult.productFound ? (
                  <><CheckCircle size={24} color="#10B981" /><h3 style={{ fontSize: '1.1rem' }}>Producto Encontrado</h3></>
                ) : (
                  <><XCircle size={24} color="#EF4444" /><h3 style={{ fontSize: '1.1rem' }}>Producto No Encontrado</h3></>
                )}
              </div>

              <div style={{ background: 'rgba(0,0,0,0.2)', borderRadius: '8px', padding: '12px', marginBottom: '16px', fontSize: '0.8rem', fontFamily: 'monospace' }}>
                <span style={{ color: 'var(--color-text-muted)' }}>Código escaneado: </span>
                <span style={{ color: 'var(--color-accent)' }}>{scanResult.rawData}</span>
              </div>

              {scanResult.product && (
                <>
                  <div style={{ borderBottom: '1px solid var(--glass-border)', paddingBottom: '16px', marginBottom: '16px' }}>
                    <h4 style={{ fontWeight: 700, marginBottom: '8px', fontSize: '1rem' }}>{scanResult.product.name}</h4>
                    <p style={{ fontSize: '0.83rem', color: 'var(--color-text-secondary)', marginBottom: '4px' }}>SKU: {scanResult.product.sku}</p>
                    <p style={{ fontSize: '0.83rem', color: 'var(--color-text-secondary)', marginBottom: '4px' }}>Categoría: {scanResult.product.categoryName}</p>
                    <p style={{ fontSize: '0.83rem', color: 'var(--color-text-secondary)', marginBottom: '4px' }}>
                      Stock: <span style={{ fontWeight: 700, color: scanResult.product.currentStock <= scanResult.product.minStock ? '#F59E0B' : '#10B981' }}>{scanResult.product.currentStock}</span>
                    </p>
                    <p style={{ fontSize: '0.95rem', fontWeight: 700, color: 'var(--color-accent)', marginTop: '8px' }}>S/. {scanResult.product.price?.toFixed(2)}</p>
                  </div>

                  {/* Movement Registration */}
                  <h4 style={{ fontSize: '0.9rem', marginBottom: '12px' }}>Registrar Movimiento</h4>
                  <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '12px', marginBottom: '12px' }}>
                    <div className="form-group" style={{ margin: 0 }}>
                      <label className="form-label">Tipo</label>
                      <select className="form-select" value={movementType} onChange={(e) => setMovementType(e.target.value)}>
                        <option value="Entry">📥 Entrada</option>
                        <option value="Exit">📤 Salida</option>
                      </select>
                    </div>
                    <div className="form-group" style={{ margin: 0 }}>
                      <label className="form-label">Cantidad</label>
                      <div style={{ display: 'flex', gap: '4px' }}>
                        <button className="btn btn-ghost btn-icon btn-sm" onClick={() => quantity > 1 && setQuantity(quantity - 1)}><Minus size={14} /></button>
                        <input className="form-input" type="number" value={quantity} onChange={(e) => setQuantity(Math.max(1, parseInt(e.target.value) || 1))} style={{ textAlign: 'center' }} />
                        <button className="btn btn-ghost btn-icon btn-sm" onClick={() => setQuantity(quantity + 1)}><Plus size={14} /></button>
                      </div>
                    </div>
                  </div>
                  <div className="form-group" style={{ margin: '0 0 16px 0' }}>
                    <label className="form-label">Almacén</label>
                    <select className="form-select" value={selectedWarehouse} onChange={(e) => setSelectedWarehouse(e.target.value)}>
                      {warehouses.map(w => <option key={w.id} value={w.id}>{w.name} ({w.code})</option>)}
                    </select>
                  </div>
                  <button className="btn btn-accent" onClick={handleRegisterMovement} style={{ width: '100%', justifyContent: 'center' }}>
                    {movementType === 'Entry' ? <Plus size={18} /> : <Minus size={18} />}
                    Registrar {movementType === 'Entry' ? 'Entrada' : 'Salida'}
                  </button>
                </>
              )}

              <button className="btn btn-ghost" onClick={() => { setScanResult(null); }} style={{ width: '100%', justifyContent: 'center', marginTop: '12px' }}>
                Escanear otro
              </button>
            </div>
          ) : (
            <div>
              <h3 style={{ fontSize: '1rem', marginBottom: '16px' }}>📋 Escaneos Recientes</h3>
              {recentScans.length === 0 ? (
                <div className="empty-state" style={{ padding: '30px' }}>
                  <ScanBarcode size={36} />
                  <p>Aún no hay escaneos</p>
                </div>
              ) : (
                <div style={{ display: 'flex', flexDirection: 'column', gap: '8px' }}>
                  {recentScans.map((scan) => (
                    <div key={scan.scanLogId} style={{ display: 'flex', alignItems: 'center', gap: '10px', padding: '10px', borderRadius: '8px', background: 'rgba(0,0,0,0.15)' }}>
                      {scan.productFound ? <CheckCircle size={16} color="#10B981" /> : <XCircle size={16} color="#EF4444" />}
                      <div style={{ flex: 1, minWidth: 0 }}>
                        <div style={{ fontSize: '0.82rem', fontWeight: 600, color: 'var(--color-text-primary)', overflow: 'hidden', textOverflow: 'ellipsis', whiteSpace: 'nowrap' }}>
                          {scan.product?.name || scan.rawData}
                        </div>
                        <div style={{ fontSize: '0.72rem', color: 'var(--color-text-muted)' }}>
                          {scan.scanType} · {new Date(scan.scannedAt).toLocaleString('es-PE')}
                        </div>
                      </div>
                    </div>
                  ))}
                </div>
              )}
            </div>
          )}
        </div>
      </div>
    </div>
  );
}
