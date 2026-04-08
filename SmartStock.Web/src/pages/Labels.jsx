import { useState } from 'react';
import { productsAPI } from '../services/api';
import { useToast } from '../components/ui/Toast';
import { QrCode, Download, Search } from 'lucide-react';

export default function Labels() {
  const [barcode, setBarcode] = useState('');
  const [product, setProduct] = useState(null);
  const toast = useToast();

  const searchProduct = async () => {
    setProduct(null);
    try {
      const res = await productsAPI.getByBarcode(barcode);
      setProduct(res.data);
      toast.success('Producto encontrado');
    } catch { 
      toast.error('Producto no encontrado'); 
    }
  };

  const generateQRUrl = (text) => `https://api.qrserver.com/v1/create-qr-code/?size=200x200&data=${encodeURIComponent(text)}`;

  return (
    <div>
      <div className="page-header">
        <h1>Generador de Etiquetas</h1>
        <p>Genera etiquetas con código QR para tus productos</p>
      </div>

      <div className="card" style={{ maxWidth: '600px', marginBottom: '24px' }}>
        <h3 style={{ marginBottom: '16px', fontSize: '0.95rem' }}>Buscar Producto</h3>
        <div style={{ display: 'flex', gap: '12px' }}>
          <input className="form-input" placeholder="Código de barras o QR..." value={barcode} onChange={e => setBarcode(e.target.value)} onKeyDown={e => e.key === 'Enter' && searchProduct()} />
          <button className="btn btn-accent" onClick={searchProduct}><Search size={18} /> Buscar</button>
        </div>
      </div>

      {product && (
        <div className="card animate-in" style={{ maxWidth: '600px' }}>
          <h3 style={{ marginBottom: '20px' }}>Etiqueta Generada</h3>
          <div style={{ background: 'white', borderRadius: '12px', padding: '24px', color: '#000', textAlign: 'center' }}>
            <h2 style={{ fontSize: '1.2rem', fontWeight: 700, marginBottom: '4px' }}>{product.name}</h2>
            <p style={{ fontSize: '0.85rem', color: '#666', marginBottom: '16px' }}>SKU: {product.sku}</p>
            <img src={generateQRUrl(product.barcode || product.qrCode || product.sku)} alt="QR Code" style={{ width: '160px', height: '160px', margin: '0 auto 12px' }} />
            <p style={{ fontFamily: 'monospace', fontSize: '1.1rem', fontWeight: 600, letterSpacing: '2px' }}>{product.barcode}</p>
            <p style={{ fontSize: '1.3rem', fontWeight: 800, marginTop: '12px' }}>S/. {product.price?.toFixed(2)}</p>
            <p style={{ fontSize: '0.75rem', color: '#999', marginTop: '8px' }}>{product.categoryName} | SmartStock WMS</p>
          </div>
          <button className="btn btn-primary" onClick={() => window.print()} style={{ width: '100%', justifyContent: 'center', marginTop: '16px' }}>
            <Download size={18} /> Imprimir Etiqueta
          </button>
        </div>
      )}
    </div>
  );
}
