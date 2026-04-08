import { createContext, useContext, useState, useEffect, useCallback } from 'react';
import { authAPI } from '../services/api';

const AuthContext = createContext(null);

export function AuthProvider({ children }) {
  const [user, setUser] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const stored = localStorage.getItem('smartstock_user');
    const token = localStorage.getItem('smartstock_token');

    if (stored && token) {
      try {
        const userData = JSON.parse(stored);
        if (userData.expiration && new Date(userData.expiration) < new Date()) {
          localStorage.removeItem('smartstock_user');
          localStorage.removeItem('smartstock_token');
        } else {
          setUser(userData);
        }
      } catch {
        localStorage.removeItem('smartstock_user');
        localStorage.removeItem('smartstock_token');
      }
    }
    setLoading(false);
  }, []);

  // Listen for storage changes (logout from another tab / 401 interceptor)
  useEffect(() => {
    const handleStorageChange = () => {
      const token = localStorage.getItem('smartstock_token');
      if (!token) setUser(null);
    };
    window.addEventListener('storage', handleStorageChange);
    return () => window.removeEventListener('storage', handleStorageChange);
  }, []);

  const _persistUser = (data) => {
    localStorage.setItem('smartstock_token', data.token);
    localStorage.setItem('smartstock_user', JSON.stringify(data));
    setUser(data);
  };

  const login = async (email, password) => {
    const { data } = await authAPI.login({ email, password });
    _persistUser(data);
    return data;
  };

  const register = async (fullName, email, password) => {
    const { data } = await authAPI.register({ fullName, email, password });
    _persistUser(data);
    return data;
  };

  /** Update own profile — syncs user state & localStorage with latest data from API */
  const updateProfile = useCallback(async (profileData) => {
    const { data } = await authAPI.updateProfile(profileData);
    // Merge new profile fields into existing user state (token/role unchanged)
    setUser(prev => {
      const updated = {
        ...prev,
        fullName:   data.fullName,
        avatarUrl:  data.avatarUrl,
        phone:      data.phone,
        department: data.department,
        position:   data.position,
        birthDate:  data.birthDate,
      };
      localStorage.setItem('smartstock_user', JSON.stringify(updated));
      return updated;
    });
    return data;
  }, []);

  const logout = () => {
    localStorage.removeItem('smartstock_token');
    localStorage.removeItem('smartstock_user');
    setUser(null);
  };

  return (
    <AuthContext.Provider value={{ user, login, register, logout, updateProfile, loading, isAuthenticated: !!user }}>
      {children}
    </AuthContext.Provider>
  );
}

export const useAuth = () => {
  const ctx = useContext(AuthContext);
  if (!ctx) throw new Error('useAuth must be used within AuthProvider');
  return ctx;
};
