/**
 * Displays active/archived counts when archive view is enabled.
 *
 * @param {Object} props
 * @param {number} props.activeCount
 * @param {number} props.archivedCount
 * @param {string} [props.activeLabel]   — defaults to "activos"
 * @param {string} [props.archivedLabel] — defaults to "archivados"
 */
export default function ArchiveStatsBar({ activeCount, archivedCount, activeLabel = 'activos', archivedLabel = 'archivados' }) {
  return (
    <div style={{
      display: 'flex', gap: '12px', marginBottom: '16px',
      padding: '10px 16px', borderRadius: '10px',
      background: 'rgba(245,158,11,0.06)',
      border: '1px solid rgba(245,158,11,0.2)',
      fontSize: '0.85rem', color: 'var(--color-text-secondary)',
    }}>
      <span>✅ <strong style={{ color: 'var(--color-success)' }}>{activeCount}</strong> {activeLabel}</span>
      <span style={{ color: 'var(--glass-border)' }}>│</span>
      <span>📦 <strong style={{ color: 'var(--color-warning)' }}>{archivedCount}</strong> {archivedLabel}</span>
    </div>
  );
}
