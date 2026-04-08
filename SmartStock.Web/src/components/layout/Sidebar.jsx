import { NavLink, useLocation } from 'react-router-dom';
import { useState } from 'react';
import { useAuth } from '../../context/AuthContext';
import {
  LayoutDashboard, Package, ScanBarcode, Warehouse, ArrowLeftRight,
  Tag, Users, ChevronLeft, ChevronRight, QrCode, BrainCircuit, Shield
} from 'lucide-react';
import SmartStockLogo from '../ui/SmartStockLogo';

const baseNavItems = [
  { path: '/', icon: LayoutDashboard, label: 'Dashboard', section: 'General' },
  { path: '/products', icon: Package, label: 'Productos', section: 'General' },
  { path: '/categories', icon: Tag, label: 'Categorías', section: 'General' },
  { path: '/scanner', icon: ScanBarcode, label: 'Escáner', section: 'Operaciones' },
  { path: '/movements', icon: ArrowLeftRight, label: 'Movimientos', section: 'Operaciones' },
  { path: '/warehouses', icon: Warehouse, label: 'Almacenes', section: 'Operaciones' },
  { path: '/vision', icon: BrainCircuit, label: 'AI Vision', section: 'Operaciones' },
  { path: '/labels', icon: QrCode, label: 'Etiquetas', section: 'Herramientas' },
  { path: '/suppliers', icon: Users, label: 'Proveedores', section: 'Herramientas' },
];

export default function Sidebar({ collapsed, onToggle }) {
  const location = useLocation();
  const { user } = useAuth();

  const navItems = [...baseNavItems];
  if (user?.role === 'Admin') {
    navItems.push({ path: '/users', icon: Shield, label: 'Usuarios', section: 'Administración' });
  }

  const sections = [...new Set(navItems.map(item => item.section))];

  return (
    <aside className={`sidebar ${collapsed ? 'collapsed' : ''}`}>
      <div className="sidebar-header">
        <div className="sidebar-logo" style={{ background: 'transparent', padding: 0 }}>
          <SmartStockLogo size={36} />
        </div>
        <div className="sidebar-brand">
          <h2>SmartStock</h2>
          <span>Sistema WMS</span>
        </div>
      </div>

      <nav className="sidebar-nav">
        {sections.map(section => (
          <div key={section}>
            <div className="sidebar-section">
              <span className="sidebar-section-title">{section}</span>
            </div>
            {navItems.filter(item => item.section === section).map(item => (
              <NavLink
                key={item.path}
                to={item.path}
                className={({ isActive }) => `nav-item ${isActive ? 'active' : ''}`}
                end={item.path === '/'}
              >
                <item.icon size={20} />
                <span className="nav-item-text">{item.label}</span>
              </NavLink>
            ))}
          </div>
        ))}
      </nav>

      <div className="sidebar-toggle">
        <button onClick={onToggle}>
          {collapsed ? <ChevronRight size={18} /> : <ChevronLeft size={18} />}
          {!collapsed && <span>Colapsar</span>}
        </button>
      </div>
    </aside>
  );
}
