/**
 * Logo de SmartStock - Forma de "H"
 * 
 * Diseño vectorial propio. La idea es 2 pilares que simulan 
 * estanterías de almacén (con separaciones de niveles), 
 * conectados por un puente de datos con un nodo central.
 * 
 * Se adapta al tamaño del contenedor y usa colores sacados 
 * directamente del design system para no romper la armonía.
 */
import { useId } from 'react';

export default function SmartStockLogo({ size = 40, className = "" }) {
  // Generamos un ID único para evitar bugs feos con los gradientes del SVG
  // si es que renderizamos este componente 2 veces en la misma vista (ej: sidebar + navbar)
  const reactId = useId();
  const id = `ss-h-${reactId.replace(/:/g, '')}`;

  return (
    <svg
      width={size}
      height={size}
      viewBox="0 0 64 64"
      fill="none"
      xmlns="http://www.w3.org/2000/svg"
      className={className}
      role="img"
      aria-label="SmartStock Logo"
    >
      <defs>
        {/* Gradiente principal de las estanterías */}
        <linearGradient id={`${id}-pillar`} x1="0" y1="0" x2="0" y2="64" gradientUnits="userSpaceOnUse">
          <stop offset="0%" stopColor="#00D4FF" />
          <stop offset="50%" stopColor="#0891B2" />
          <stop offset="100%" stopColor="#0E7490" />
        </linearGradient>

        {/* Gradiente del puente de conexión */}
        <linearGradient id={`${id}-bridge`} x1="0" y1="32" x2="64" y2="32" gradientUnits="userSpaceOnUse">
          <stop offset="0%" stopColor="#0891B2" />
          <stop offset="50%" stopColor="#00D4FF" />
          <stop offset="100%" stopColor="#0891B2" />
        </linearGradient>

        {/* Sombra neon para el modo oscuro */}
        <filter id={`${id}-glow`} x="-30%" y="-30%" width="160%" height="160%">
          <feGaussianBlur in="SourceGraphic" stdDeviation="2.5" result="blur" />
          <feColorMatrix in="blur" type="matrix"
            values="0 0 0 0 0  0 0.83 0 0 0  0 0 1 0 0  0 0 0 0.35 0" />
          <feMerge>
            <feMergeNode />
            <feMergeNode in="SourceGraphic" />
          </feMerge>
        </filter>

        {/* Brillo del nodo central */}
        <radialGradient id={`${id}-node`} cx="50%" cy="50%" r="50%">
          <stop offset="0%" stopColor="#00D4FF" stopOpacity="1" />
          <stop offset="60%" stopColor="#00D4FF" stopOpacity="0.6" />
          <stop offset="100%" stopColor="#0891B2" stopOpacity="0" />
        </radialGradient>
      </defs>

      <g filter={`url(#${id}-glow)`}>
        {/* Pilar Izquierdo */}
        <rect x="8" y="6" width="14" height="52" rx="4" fill={`url(#${id}-pillar)`} />
        {/* Separadores que simulan los niveles del rack */}
        <line x1="11" y1="18" x2="19" y2="18" stroke="#0A0F1C" strokeWidth="1.5" strokeLinecap="round" opacity="0.45" />
        <line x1="11" y1="28" x2="19" y2="28" stroke="#0A0F1C" strokeWidth="1.5" strokeLinecap="round" opacity="0.35" />
        <line x1="11" y1="42" x2="19" y2="42" stroke="#0A0F1C" strokeWidth="1.5" strokeLinecap="round" opacity="0.35" />
        <line x1="11" y1="50" x2="19" y2="50" stroke="#0A0F1C" strokeWidth="1.5" strokeLinecap="round" opacity="0.45" />

        {/* Pilar Derecho */}
        <rect x="42" y="6" width="14" height="52" rx="4" fill={`url(#${id}-pillar)`} />
        {/* Más separadores */}
        <line x1="45" y1="18" x2="53" y2="18" stroke="#0A0F1C" strokeWidth="1.5" strokeLinecap="round" opacity="0.45" />
        <line x1="45" y1="28" x2="53" y2="28" stroke="#0A0F1C" strokeWidth="1.5" strokeLinecap="round" opacity="0.35" />
        <line x1="45" y1="42" x2="53" y2="42" stroke="#0A0F1C" strokeWidth="1.5" strokeLinecap="round" opacity="0.35" />
        <line x1="45" y1="50" x2="53" y2="50" stroke="#0A0F1C" strokeWidth="1.5" strokeLinecap="round" opacity="0.45" />

        {/* Puente de conexión (flujo de datos) */}
        <rect x="18" y="26" width="28" height="12" rx="3" fill={`url(#${id}-bridge)`} />

        {/* Trazos de circuito en el puente */}
        <line x1="21" y1="32" x2="27" y2="32" stroke="#0A0F1C" strokeWidth="1" strokeLinecap="round" opacity="0.3" />
        <line x1="37" y1="32" x2="43" y2="32" stroke="#0A0F1C" strokeWidth="1" strokeLinecap="round" opacity="0.3" />

        {/* Nodo al centro */}
        <circle cx="32" cy="32" r="4.5" fill={`url(#${id}-node)`} />
        <circle cx="32" cy="32" r="2" fill="#ffffff" opacity="0.95" />
      </g>

      {/* Efecto de pulso "Heartbeat" (CSS nativo) para simular escaneo en tiempo real */}
      <circle cx="32" cy="32" r="6" fill="none" stroke="#00D4FF" strokeWidth="0.8" opacity="0.2">
        <animate attributeName="r" values="5;8;5" dur="3s" repeatCount="indefinite" />
        <animate attributeName="opacity" values="0.25;0.05;0.25" dur="3s" repeatCount="indefinite" />
      </circle>
    </svg>
  );
}
