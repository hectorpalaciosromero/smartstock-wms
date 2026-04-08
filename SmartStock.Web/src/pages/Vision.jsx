import { useState, useRef, useEffect } from 'react';
import * as tf from '@tensorflow/tfjs';
import * as mobilenet from '@tensorflow-models/mobilenet';
import { Camera, CameraOff, Scan, AlertTriangle, CheckCircle, BrainCircuit } from 'lucide-react';
import { productsAPI } from '../services/api';
import { useToast } from '../components/ui/Toast';

export default function Vision() {
  const [model, setModel] = useState(null);
  const [loadingModel, setLoadingModel] = useState(true);
  const [isCameraActive, setIsCameraActive] = useState(false);
  const [predictions, setPredictions] = useState([]);
  const [relatedProducts, setRelatedProducts] = useState([]);
  const [analyzing, setAnalyzing] = useState(false);
  const videoRef = useRef(null);
  const toast = useToast();

  useEffect(() => {
    loadModel();
    return () => stopCamera();
  }, []);

  const loadModel = async () => {
    try {
      await tf.ready();
      const loadedModel = await mobilenet.load({ version: 2, alpha: 1.0 });
      setModel(loadedModel);
    } catch (err) {
      console.error('Error cargando el modelo AI:', err);
    } finally {
      setLoadingModel(false);
    }
  };

  const startCamera = async () => {
    try {
      const stream = await navigator.mediaDevices.getUserMedia({
        video: { facingMode: 'environment', width: { ideal: 640 }, height: { ideal: 480 } }
      });
      if (videoRef.current) {
        videoRef.current.srcObject = stream;
        videoRef.current.play();
        setIsCameraActive(true);
      }
    } catch (err) {
      toast.error('No se pudo acceder a la cámara. Revisa los permisos.');
    }
  };

  const stopCamera = () => {
    if (videoRef.current && videoRef.current.srcObject) {
      const tracks = videoRef.current.srcObject.getTracks();
      tracks.forEach(track => track.stop());
      videoRef.current.srcObject = null;
    }
    setIsCameraActive(false);
    setPredictions([]);
  };

  const analyzeFrame = async () => {
    if (!model || !videoRef.current || !isCameraActive) return;
    
    setAnalyzing(true);
    try {
      // Tomar predicción usando MobileNet
      const results = await model.classify(videoRef.current, 3);
      setPredictions(results);
      
      // Simular búsqueda de productos relacionados basados en las etiquetas encontradas
      if (results.length > 0) {
        const topTerm = results[0].className.split(',')[0].toLowerCase();
        searchRelatedProductsInDB(topTerm);
      }
    } catch (err) {
      console.error('Error en predicción:', err);
    } finally {
      setAnalyzing(false);
    }
  };

  const searchRelatedProductsInDB = async (searchTerm) => {
    try {
      // Buscar en memoria para la demo (idealmente esto pasaría al backend)
      const res = await productsAPI.getAll({ pageSize: 100 });
      const matches = res.data.items.filter(p => 
        p.name.toLowerCase().includes(searchTerm) || 
        p.categoryName.toLowerCase().includes(searchTerm) ||
        // Mapeo básico de algunos términos en inglés del modelo a español
        (searchTerm.includes('computer') && p.categoryName.toLowerCase().includes('computación')) ||
        (searchTerm.includes('monitor') && p.categoryName.toLowerCase().includes('monitor')) ||
        (searchTerm.includes('printer') && p.name.toLowerCase().includes('impresora')) ||
        (searchTerm.includes('laptop') && p.name.toLowerCase().includes('laptop')) ||
        (searchTerm.includes('phone') && p.name.toLowerCase().includes('móvil'))
      );
      setRelatedProducts(matches.slice(0, 3));
    } catch (err) {
      console.error(err);
    }
  };

  return (
    <div>
      <div className="page-header" style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start' }}>
        <div>
          <h1 style={{ display: 'flex', alignItems: 'center', gap: '8px' }}>
            <BrainCircuit color="var(--color-accent)" /> 
            Smart Vision Classifier
          </h1>
          <p>Identificación automática de productos mediante Inteligencia Artificial</p>
        </div>
      </div>

      <div className="scanner-container">
        {/* Panel de Cámara */}
        <div className="card" style={{ padding: '0', overflow: 'hidden' }}>
          <div style={{ position: 'relative', width: '100%', aspectRatio: '4/3', background: '#000', display: 'flex', alignItems: 'center', justifyContent: 'center' }}>
            <video 
              ref={videoRef} 
              style={{ width: '100%', height: '100%', objectFit: 'cover', display: isCameraActive ? 'block' : 'none' }} 
              muted 
              playsInline
            />
            
            {!isCameraActive && (
              <div style={{ textAlign: 'center', color: 'var(--color-text-muted)' }}>
                <Camera size={48} style={{ marginBottom: '16px', opacity: 0.5, margin: '0 auto' }} />
                <p>Cámara apagada</p>
              </div>
            )}

            {isCameraActive && analyzing && (
              <div style={{ position: 'absolute', inset: 0, border: '3px solid var(--color-accent)', boxShadow: 'inset 0 0 20px rgba(0,212,255,0.4)', pointerEvents: 'none', animation: 'pulse 1.5s infinite' }}>
                <div style={{ position: 'absolute', top: '10px', right: '10px', background: 'rgba(0,0,0,0.6)', padding: '4px 8px', borderRadius: '4px', fontSize: '0.7rem', display: 'flex', alignItems: 'center', gap: '6px' }}>
                  <div className="spinner" style={{ width: '12px', height: '12px', borderWidth: '2px' }}></div>
                  Analizando
                </div>
              </div>
            )}
          </div>

          <div style={{ padding: '20px', display: 'flex', gap: '12px', background: 'var(--color-bg-card)' }}>
            {!isCameraActive ? (
              <button className="btn btn-accent" onClick={startCamera} style={{ flex: 1, justifyContent: 'center' }} disabled={loadingModel}>
                {loadingModel ? 'Cargando Modelo AI...' : <><Camera size={18} /> Activar Cámara</>}
              </button>
            ) : (
              <>
                <button className="btn btn-primary" onClick={analyzeFrame} style={{ flex: 2, justifyContent: 'center' }} disabled={analyzing}>
                  <Scan size={18} /> {analyzing ? 'Procesando...' : 'Analizar Producto'}
                </button>
                <button className="btn btn-danger" onClick={stopCamera} style={{ flex: 1, justifyContent: 'center' }}>
                  <CameraOff size={18} /> Detener
                </button>
              </>
            )}
          </div>
        </div>

        {/* Panel de Resultados */}
        <div style={{ display: 'flex', flexDirection: 'column', gap: '20px' }}>
          
          <div className="card">
            <h3 style={{ fontSize: '1rem', marginBottom: '16px', display: 'flex', alignItems: 'center', gap: '8px' }}>
              <BrainCircuit size={18} color="var(--color-accent)" />
              Predicciones del Modelo (TensorFlow)
            </h3>
            
            {predictions.length > 0 ? (
              <div style={{ display: 'flex', flexDirection: 'column', gap: '12px' }}>
                {predictions.map((p, i) => (
                  <div key={i} style={{ background: 'rgba(0,0,0,0.2)', padding: '12px', borderRadius: '8px', borderLeft: i === 0 ? '3px solid var(--color-success)' : '3px solid var(--color-border)' }}>
                    <div style={{ display: 'flex', justifyContent: 'space-between', marginBottom: '4px' }}>
                      <span style={{ fontWeight: 600, textTransform: 'capitalize' }}>{p.className.split(',')[0]}</span>
                      <span style={{ color: p.probability > 0.6 ? 'var(--color-success)' : 'var(--color-warning)', fontWeight: 700 }}>
                        {(p.probability * 100).toFixed(1)}%
                      </span>
                    </div>
                    <div style={{ width: '100%', background: 'rgba(255,255,255,0.05)', height: '6px', borderRadius: '3px', overflow: 'hidden' }}>
                      <div style={{ width: `${p.probability * 100}%`, background: p.probability > 0.6 ? 'var(--color-success)' : 'var(--color-warning)', height: '100%' }}></div>
                    </div>
                  </div>
                ))}
              </div>
            ) : (
              <div className="empty-state">
                <Scan size={32} />
                <p>Captura una imagen para ver los resultados de la red neuronal</p>
              </div>
            )}
          </div>

          {predictions.length > 0 && (
            <div className="card animate-in" style={{ borderColor: 'rgba(0,212,255,0.2)' }}>
              <h3 style={{ fontSize: '1rem', marginBottom: '16px', display: 'flex', alignItems: 'center', gap: '8px' }}>
                <CheckCircle size={18} color="var(--color-success)" />
                Coincidencias en Inventario
              </h3>

              {relatedProducts.length > 0 ? (
                <div style={{ display: 'flex', flexDirection: 'column', gap: '10px' }}>
                  {relatedProducts.map(prod => (
                    <div key={prod.id} style={{ padding: '12px', background: 'rgba(0,212,255,0.05)', borderRadius: '8px', border: '1px solid rgba(0,212,255,0.1)' }}>
                      <h4 style={{ fontSize: '0.9rem', fontWeight: 600, color: 'var(--color-text-primary)' }}>{prod.name}</h4>
                      <div style={{ display: 'flex', justifyContent: 'space-between', fontSize: '0.8rem', color: 'var(--color-text-secondary)', marginTop: '4px' }}>
                        <span>SKU: {prod.sku}</span>
                        <span>Stock: <strong style={{color:'var(--color-text-primary)'}}>{prod.currentStock}</strong></span>
                      </div>
                    </div>
                  ))}
                </div>
              ) : (
                <div style={{ display: 'flex', gap: '12px', alignItems: 'center', color: 'var(--color-warning)', background: 'rgba(245, 158, 11, 0.1)', padding: '12px', borderRadius: '8px' }}>
                  <AlertTriangle size={24} />
                  <p style={{ fontSize: '0.85rem' }}>No se encontraron productos en el inventario actual que coincidan exactamente con la predicción visual.</p>
                </div>
              )}
            </div>
          )}

        </div>
      </div>
    </div>
  );
}
