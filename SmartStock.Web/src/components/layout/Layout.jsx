import { Outlet } from 'react-router-dom';
import { useState } from 'react';
import Sidebar from './Sidebar';
import Navbar from './Navbar';

export default function Layout() {
  const [collapsed, setCollapsed] = useState(false);

  return (
    <div className="app-layout">
      <Sidebar collapsed={collapsed} onToggle={() => setCollapsed(!collapsed)} />
      <Navbar collapsed={collapsed} onMobileToggle={() => setCollapsed(!collapsed)} />
      <main className={`main-content ${collapsed ? 'collapsed' : ''}`}>
        <div className="page-container">
          <Outlet />
        </div>
      </main>
    </div>
  );
}
