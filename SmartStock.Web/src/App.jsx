import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { AuthProvider, useAuth } from './context/AuthContext';
import Layout from './components/layout/Layout';
import RoleGuard from './components/ui/RoleGuard';
import Login from './pages/Login';
import Dashboard from './pages/Dashboard';
import Products from './pages/Products';
import Categories from './pages/Categories';
import Scanner from './pages/Scanner';
import Movements from './pages/Movements';
import Warehouses from './pages/Warehouses';
import Labels from './pages/Labels';
import Suppliers from './pages/Suppliers';
import Vision from './pages/Vision';
import Users from './pages/Users';
import Profile from './pages/Profile';
import ProductDetail from './pages/ProductDetail';

function ProtectedRoute({ children }) {
  const { isAuthenticated, loading } = useAuth();
  if (loading) return <div className="loading-spinner"><div className="spinner"></div></div>;
  return isAuthenticated ? children : <Navigate to="/login" />;
}

function AppRoutes() {
  const { isAuthenticated } = useAuth();

  return (
    <Routes>
      <Route path="/login" element={isAuthenticated ? <Navigate to="/" /> : <Login />} />
      <Route path="/" element={<ProtectedRoute><Layout /></ProtectedRoute>}>
        <Route index element={<Dashboard />} />
        <Route path="products" element={<Products />} />
        <Route path="products/:id" element={<ProductDetail />} />
        <Route path="categories" element={<Categories />} />
        <Route path="scanner" element={<Scanner />} />
        <Route path="vision" element={<Vision />} />
        <Route path="movements" element={<Movements />} />
        <Route path="warehouses" element={<Warehouses />} />
        <Route path="labels" element={<Labels />} />
        <Route path="suppliers" element={<Suppliers />} />
        <Route path="users" element={<RoleGuard roles={['Admin']}><Users /></RoleGuard>} />
        <Route path="profile" element={<Profile />} />
        {/* Catch-all: redirect unknown routes to dashboard */}
        <Route path="*" element={<Navigate to="/" replace />} />
      </Route>
    </Routes>
  );
}

export default function App() {
  return (
    <BrowserRouter>
      <AuthProvider>
        <AppRoutes />
      </AuthProvider>
    </BrowserRouter>
  );
}
