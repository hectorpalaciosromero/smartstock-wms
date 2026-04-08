import { useState, useRef } from 'react';
import { useAuth } from '../context/AuthContext';
import { authAPI } from '../services/api';
import { useToast } from '../components/ui/Toast';
import {
  User, Mail, Shield, CheckCircle2, XCircle, Clock, Edit2, Save, X,
  Phone, Building2, Briefcase, Calendar, UploadCloud, Loader2, Camera, Key
} from 'lucide-react';

const CLOUDINARY_CLOUD = import.meta.env.VITE_CLOUDINARY_CLOUD_NAME || 'dwfdo3rhu';
const CLOUDINARY_PRESET = import.meta.env.VITE_CLOUDINARY_UPLOAD_PRESET || 'smartstock';

const ROLE_META = {
  Admin:      { color: '#EF4444', label: 'Administrador', gradient: 'linear-gradient(135deg,#EF444420,#EF444408)' },
  Supervisor: { color: '#F59E0B', label: 'Supervisor',    gradient: 'linear-gradient(135deg,#F59E0B20,#F59E0B08)' },
  Almacenero: { color: '#10B981', label: 'Almacenero',    gradient: 'linear-gradient(135deg,#10B98120,#10B98108)' },
  Viewer:     { color: '#3B82F6', label: 'Solo Lectura',  gradient: 'linear-gradient(135deg,#3B82F620,#3B82F608)' },
};

const ROLE_PERMISSIONS = {
  Admin: [
    { action: 'Acceder al Dashboard general', allowed: true },
    { action: 'Visualizar inventario, almacenes y categorías', allowed: true },
    { action: 'Registrar entradas y salidas de stock', allowed: true },
    { action: 'Exportar reportes del sistema a CSV/Excel', allowed: true },
    { action: 'Crear, editar y archivar módulos maestros', allowed: true },
    { action: 'Restaurar elementos archivados', allowed: true },
    { action: 'Cambiar el rol de los empleados', allowed: true },
    { action: 'Desactivar o banear cuentas del sistema', allowed: true },
  ],
  Supervisor: [
    { action: 'Acceder al Dashboard general', allowed: true },
    { action: 'Visualizar inventario, almacenes y categorías', allowed: true },
    { action: 'Registrar entradas y salidas de stock', allowed: true },
    { action: 'Exportar reportes del sistema a CSV/Excel', allowed: true },
    { action: 'Crear, editar y archivar módulos maestros', allowed: true },
    { action: 'Restaurar elementos archivados', allowed: true },
    { action: 'Cambiar el rol de los empleados', allowed: false },
    { action: 'Desactivar o banear cuentas del sistema', allowed: false },
  ],
  Almacenero: [
    { action: 'Acceder al Dashboard general', allowed: true },
    { action: 'Visualizar inventario, almacenes y categorías', allowed: true },
    { action: 'Registrar entradas y salidas mediante escáner', allowed: true },
    { action: 'Crear, editar y archivar módulos maestros', allowed: false },
    { action: 'Restaurar elementos archivados', allowed: false },
    { action: 'Exportar reportes del sistema', allowed: false },
    { action: 'Auditar accesos de usuarios', allowed: false },
  ],
  Viewer: [
    { action: 'Acceso de navegación en Solo Lectura', allowed: true },
    { action: 'Visualizar inventario, almacenes y categorías', allowed: true },
    { action: 'Registrar movimientos de inventario', allowed: false },
    { action: 'Crear, editar y archivar módulos maestros', allowed: false },
    { action: 'Auditar accesos de usuarios', allowed: false },
  ],
};

function InfoRow({ icon: Icon, label, value }) {
  return (
    <div style={{ display: 'flex', alignItems: 'center', gap: '12px', padding: '12px 0', borderBottom: '1px solid var(--glass-border)' }}>
      <div style={{ width: '36px', height: '36px', borderRadius: '8px', background: 'var(--color-bg-secondary)', display: 'flex', alignItems: 'center', justifyContent: 'center', flexShrink: 0 }}>
        <Icon size={16} color="var(--color-accent)" />
      </div>
      <div>
        <div style={{ fontSize: '0.72rem', color: 'var(--color-text-muted)', textTransform: 'uppercase', letterSpacing: '0.06em', fontWeight: 600 }}>{label}</div>
        <div style={{ fontSize: '0.92rem', color: value ? 'var(--color-text-primary)' : 'var(--color-text-muted)', marginTop: '2px' }}>
          {value || 'No especificado'}
        </div>
      </div>
    </div>
  );
}

export default function Profile() {
  const { user, updateProfile } = useAuth();
  const toast = useToast();

  const roleMeta = ROLE_META[user?.role] || ROLE_META.Viewer;
  const myPermissions = ROLE_PERMISSIONS[user?.role] || ROLE_PERMISSIONS.Viewer;

  const [isEditing, setIsEditing]       = useState(false);
  const [saving, setSaving]             = useState(false);
  const [uploadingAvatar, setUploadingAvatar] = useState(false);
  
  // Password change state
  const [isChangingPassword, setIsChangingPassword] = useState(false);
  const [savingPassword, setSavingPassword] = useState(false);
  const [pwdForm, setPwdForm] = useState({ currentPassword: '', newPassword: '', confirmPassword: '' });

  const [form, setForm] = useState({
    fullName:   user?.fullName   || '',
    phone:      user?.phone      || '',
    department: user?.department || '',
    position:   user?.position   || '',
    birthDate:  user?.birthDate  ? user.birthDate.split('T')[0] : '',
    avatarUrl:  user?.avatarUrl  || '',
  });

  const fileInputRef = useRef(null);

  const startEdit = () => {
    setForm({
      fullName:   user?.fullName   || '',
      phone:      user?.phone      || '',
      department: user?.department || '',
      position:   user?.position   || '',
      birthDate:  user?.birthDate  ? user.birthDate.split('T')[0] : '',
      avatarUrl:  user?.avatarUrl  || '',
    });
    setIsEditing(true);
  };

  const cancelEdit = () => setIsEditing(false);

  const handleAvatarUpload = async (e) => {
    const file = e.target.files[0];
    if (!file) return;
    setUploadingAvatar(true);
    try {
      const fd = new FormData();
      fd.append('file', file);
      fd.append('upload_preset', CLOUDINARY_PRESET);
      fd.append('cloud_name', CLOUDINARY_CLOUD);
      const res = await fetch(`https://api.cloudinary.com/v1_1/${CLOUDINARY_CLOUD}/image/upload`, { method: 'POST', body: fd });
      const data = await res.json();
      if (data.secure_url) {
        setForm(f => ({ ...f, avatarUrl: data.secure_url }));
        toast.success("Foto subida temporalmente. Guarda los cambios.");
      }
    } catch {
      toast.error('Error al subir la imagen');
    } finally {
      setUploadingAvatar(false);
    }
  };

  const handleSave = async (e) => {
    e.preventDefault();
    setSaving(true);
    try {
      await updateProfile({
        fullName:   form.fullName,
        phone:      form.phone     || null,
        department: form.department || null,
        position:   form.position  || null,
        birthDate:  form.birthDate  ? new Date(form.birthDate).toISOString() : null,
        avatarUrl:  form.avatarUrl  || null,
      });
      toast.success("Perfil actualizado exitosamente");
      setIsEditing(false);
    } catch (err) {
      toast.error(err.response?.data?.message || 'Error al guardar cambios');
    } finally {
      setSaving(false);
    }
  };

  const handleChangePassword = async (e) => {
    e.preventDefault();
    if (pwdForm.newPassword !== pwdForm.confirmPassword) {
      toast.error('Las contraseñas no coinciden');
      return;
    }
    setSavingPassword(true);
    try {
      await authAPI.changePassword({
        currentPassword: pwdForm.currentPassword,
        newPassword: pwdForm.newPassword
      });
      toast.success("Contraseña actualizada exitosamente");
      setIsChangingPassword(false);
      setPwdForm({ currentPassword: '', newPassword: '', confirmPassword: '' });
    } catch (err) {
      toast.error(err.response?.data?.message || 'Error al cambiar la contraseña');
    } finally {
      setSavingPassword(false);
    }
  };

  const displayName = user?.fullName || user?.email || 'Usuario';
  const avatarSrc   = user?.avatarUrl;
  const avatarInitial = displayName.charAt(0).toUpperCase();

  return (
    <div style={{ maxWidth: '860px', margin: '0 auto', paddingBottom: '48px' }}>
      <div className="page-header" style={{ marginBottom: '32px' }}>
        <h1>Mi Perfil</h1>
        <p>Gestiona tu información personal y configuración de cuenta</p>
      </div>

      <div className="card animate-in" style={{ padding: '32px', marginBottom: '24px', background: roleMeta.gradient, borderColor: `${roleMeta.color}30`, position: 'relative' }}>
        {!isEditing ? (
          <button className="btn btn-ghost btn-sm" style={{ position: 'absolute', top: '20px', right: '20px', gap: '6px' }} onClick={startEdit}>
            <Edit2 size={15} /> Editar Perfil
          </button>
        ) : (
          <button className="btn btn-ghost btn-sm" style={{ position: 'absolute', top: '20px', right: '20px', color: 'var(--color-danger)' }} onClick={cancelEdit}>
            <X size={15} /> Cancelar
          </button>
        )}

        <div style={{ display: 'flex', alignItems: 'center', gap: '24px', flexWrap: 'wrap' }}>
          <div style={{ position: 'relative', flexShrink: 0 }}>
            <div style={{ width: '96px', height: '96px', borderRadius: '50%', overflow: 'hidden', border: `3px solid ${roleMeta.color}`, boxShadow: `0 0 24px ${roleMeta.color}40`, background: avatarSrc ? 'transparent' : 'var(--color-accent)', display: 'flex', alignItems: 'center', justifyContent: 'center', fontSize: '2.4rem', fontWeight: 700, color: '#fff' }}>
              {isEditing && form.avatarUrl ? (
                <img src={form.avatarUrl} alt="avatar" style={{ width: '100%', height: '100%', objectFit: 'cover' }} />
              ) : avatarSrc ? (
                <img src={avatarSrc} alt="avatar" style={{ width: '100%', height: '100%', objectFit: 'cover' }} />
              ) : avatarInitial}
            </div>
            {isEditing && (
              <button type="button" onClick={() => fileInputRef.current?.click()} disabled={uploadingAvatar} style={{ position: 'absolute', bottom: 0, right: 0, width: '28px', height: '28px', borderRadius: '50%', background: 'var(--color-accent)', border: '2px solid var(--color-bg-primary)', display: 'flex', alignItems: 'center', justifyContent: 'center', cursor: 'pointer', color: '#fff' }} title="Cambiar foto">
                {uploadingAvatar ? <Loader2 size={12} className="spinner" /> : <Camera size={12} />}
              </button>
            )}
            <input ref={fileInputRef} type="file" accept="image/*" style={{ display: 'none' }} onChange={handleAvatarUpload} />
          </div>

          <div style={{ flex: 1 }}>
            {!isEditing ? (
              <>
                <h2 style={{ fontSize: '1.7rem', fontWeight: 700, margin: '0 0 6px 0' }}>{displayName}</h2>
                <div style={{ display: 'flex', alignItems: 'center', gap: '8px', color: 'var(--color-text-secondary)', marginBottom: '12px', fontSize: '0.9rem' }}>
                  <Mail size={15} /><span>{user?.email}</span>
                </div>
              </>
            ) : (
              <div className="form-group" style={{ marginBottom: '8px' }}>
                <label className="form-label">Nombre Completo *</label>
                <input className="form-input" value={form.fullName} onChange={e => setForm(f => ({ ...f, fullName: e.target.value }))} required disabled={saving} style={{ maxWidth: '340px' }} />
              </div>
            )}
            <div style={{ display: 'inline-flex', alignItems: 'center', gap: '8px', background: 'var(--color-bg-primary)', padding: '8px 18px', borderRadius: '99px', border: `1px solid ${roleMeta.color}40` }}>
              <Shield size={16} color={roleMeta.color} />
              <span style={{ fontWeight: 600, color: roleMeta.color, fontSize: '0.9rem' }}>{roleMeta.label}</span>
            </div>
          </div>
        </div>
      </div>

      {isEditing ? (
        <form onSubmit={handleSave}>
          <div className="card animate-in" style={{ padding: '28px', marginBottom: '20px' }}>
            <h3 style={{ fontSize: '1rem', fontWeight: 600, marginBottom: '20px', display: 'flex', alignItems: 'center', gap: '8px' }}>
              <Edit2 size={16} color="var(--color-accent)" /> Datos Personales
            </h3>
            <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '16px' }}>
              <div className="form-group"><label className="form-label"><Phone size={13} style={{ display: 'inline', verticalAlign: 'middle', marginRight: '4px' }} />Teléfono</label><input className="form-input" value={form.phone} onChange={e => setForm(f => ({ ...f, phone: e.target.value }))} placeholder="+51 999 999 999" disabled={saving} /></div>
              <div className="form-group"><label className="form-label"><Calendar size={13} style={{ display: 'inline', verticalAlign: 'middle', marginRight: '4px' }} />Fecha de Nacimiento</label><input className="form-input" type="date" value={form.birthDate} onChange={e => setForm(f => ({ ...f, birthDate: e.target.value }))} disabled={saving} /></div>
              <div className="form-group"><label className="form-label"><Building2 size={13} style={{ display: 'inline', verticalAlign: 'middle', marginRight: '4px' }} />Departamento / Área</label><input className="form-input" value={form.department} onChange={e => setForm(f => ({ ...f, department: e.target.value }))} placeholder="Logística, Tecnología..." disabled={saving} /></div>
              <div className="form-group"><label className="form-label"><Briefcase size={13} style={{ display: 'inline', verticalAlign: 'middle', marginRight: '4px' }} />Cargo / Posición</label><input className="form-input" value={form.position} onChange={e => setForm(f => ({ ...f, position: e.target.value }))} placeholder="Analista, Coordinador..." disabled={saving} /></div>
            </div>
            <div style={{ display: 'flex', gap: '12px', justifyContent: 'flex-end', marginTop: '20px' }}>
              <button type="button" className="btn btn-ghost" onClick={cancelEdit} disabled={saving}>Cancelar</button>
              <button type="submit" className="btn btn-accent" disabled={saving}>{saving ? <><Loader2 size={16} className="spinner" /> Guardando…</> : <><Save size={16} /> Guardar Cambios</>}</button>
            </div>
          </div>
        </form>
      ) : (
        <div className="card animate-in" style={{ padding: '28px', marginBottom: '24px' }}>
          <h3 style={{ fontSize: '1rem', fontWeight: 600, marginBottom: '8px', display: 'flex', alignItems: 'center', gap: '8px' }}>
            <User size={16} color="var(--color-accent)" /> Datos Personales
          </h3>
          <InfoRow icon={Mail}      label="Correo Electrónico" value={user?.email} />
          <InfoRow icon={Phone}     label="Teléfono"           value={user?.phone} />
          <InfoRow icon={Building2} label="Departamento / Área" value={user?.department} />
          <InfoRow icon={Briefcase} label="Cargo / Posición"   value={user?.position} />
          <InfoRow icon={Calendar}  label="Fecha de Nacimiento" value={user?.birthDate ? new Date(user.birthDate).toLocaleDateString('es-PE', { day: '2-digit', month: 'long', year: 'numeric' }) : null} />
          
          <div style={{ marginTop: '24px', paddingTop: '24px', borderTop: '1px solid var(--glass-border)' }}>
            <h3 style={{ fontSize: '1rem', fontWeight: 600, marginBottom: '16px', display: 'flex', alignItems: 'center', gap: '8px' }}>
              <Key size={16} color="var(--color-accent)" /> Seguridad de la Cuenta
            </h3>
            {isChangingPassword ? (
              <form onSubmit={handleChangePassword} style={{ background: 'var(--color-bg-secondary)', padding: '20px', borderRadius: '12px', border: '1px solid var(--glass-border)' }}>
                <div className="form-group"><label className="form-label">Contraseña Actual *</label><input type="password" required minLength={6} className="form-input" value={pwdForm.currentPassword} onChange={e => setPwdForm(f => ({ ...f, currentPassword: e.target.value }))} disabled={savingPassword} /></div>
                <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '16px' }}>
                  <div className="form-group"><label className="form-label">Nueva Contraseña *</label><input type="password" required minLength={6} className="form-input" value={pwdForm.newPassword} onChange={e => setPwdForm(f => ({ ...f, newPassword: e.target.value }))} disabled={savingPassword} /></div>
                  <div className="form-group"><label className="form-label">Confirmar Contraseña *</label><input type="password" required minLength={6} className="form-input" value={pwdForm.confirmPassword} onChange={e => setPwdForm(f => ({ ...f, confirmPassword: e.target.value }))} disabled={savingPassword} /></div>
                </div>
                <div style={{ display: 'flex', gap: '12px', justifyContent: 'flex-end', marginTop: '16px' }}>
                  <button type="button" className="btn btn-ghost" onClick={() => setIsChangingPassword(false)} disabled={savingPassword}>Cancelar</button>
                  <button type="submit" className="btn btn-accent" disabled={savingPassword}>{savingPassword ? 'Cambiando...' : 'Cambiar Contraseña'}</button>
                </div>
              </form>
            ) : (
              <button className="btn btn-ghost" onClick={() => setIsChangingPassword(true)} style={{ color: 'var(--color-text-primary)', border: '1px solid var(--glass-border)' }}>
                <Key size={16} /> Cambiar mi contraseña
              </button>
            )}
          </div>
        </div>
      )}

      <h3 style={{ fontSize: '1rem', fontWeight: 600, marginBottom: '16px', display: 'flex', alignItems: 'center', gap: '8px' }}>
        <CheckCircle2 size={18} color="var(--color-accent)" /> Capacidades del Rol
      </h3>
      <div className="table-container animate-in" style={{ borderRadius: '16px', overflow: 'hidden', marginBottom: '24px' }}>
        <table style={{ margin: 0 }}>
          <thead><tr><th>Acción del Sistema</th><th style={{ width: '130px', textAlign: 'center' }}>Permiso</th></tr></thead>
          <tbody>
            {myPermissions.map((perm, idx) => (
              <tr key={idx} style={{ background: perm.allowed ? 'transparent' : 'rgba(239,68,68,0.03)' }}>
                <td style={{ color: perm.allowed ? 'var(--color-text-primary)' : 'var(--color-text-muted)', fontWeight: perm.allowed ? 500 : 400 }}>{perm.action}</td>
                <td style={{ textAlign: 'center' }}>
                  {perm.allowed ? <span className="badge-status badge-success" style={{ display: 'inline-flex', alignItems: 'center', gap: '4px' }}><CheckCircle2 size={13} /> Autorizado</span> : <span className="badge-status badge-danger" style={{ display: 'inline-flex', alignItems: 'center', gap: '4px' }}><XCircle size={13} /> Denegado</span>}
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>

      <div style={{ padding: '16px', borderRadius: '12px', background: 'var(--color-bg-secondary)', display: 'flex', gap: '12px', alignItems: 'flex-start' }}>
        <Clock size={18} color="var(--color-text-muted)" style={{ flexShrink: 0, marginTop: '2px' }} />
        <p style={{ fontSize: '0.84rem', color: 'var(--color-text-secondary)', margin: 0, lineHeight: 1.6 }}>La sesión actual está autenticada mediante un <strong>Token JWT seguro</strong>. Si tus permisos son incorrectos, contacta al <strong>Administrador del Sistema</strong> para solicitar un ajuste de rol.</p>
      </div>
    </div>
  );
}
