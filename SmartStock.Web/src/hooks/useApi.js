import { useState, useCallback, useRef, useEffect } from 'react';

/**
 * Generic hook for API calls with loading, error, and data state management.
 *
 * Usage:
 *   const { data, loading, error, execute, reset } = useApi(productsAPI.getAll);
 *   // auto-fetch on mount:
 *   useEffect(() => { execute({ page: 1 }); }, [execute]);
 *
 * @param {Function} apiFn - The axios-wrapped API function to call
 * @param {Object} options
 * @param {boolean} options.immediate - If true, call on mount with initialParams
 * @param {*} options.initialParams - Params to pass on immediate call
 * @param {*} options.initialData - Default data value (default null)
 */
export default function useApi(apiFn, { immediate = false, initialParams, initialData = null } = {}) {
  const [data, setData] = useState(initialData);
  const [loading, setLoading] = useState(immediate);
  const [error, setError] = useState(null);
  const mountedRef = useRef(true);

  useEffect(() => {
    mountedRef.current = true;
    return () => { mountedRef.current = false; };
  }, []);

  const execute = useCallback(async (...args) => {
    setLoading(true);
    setError(null);
    try {
      const res = await apiFn(...args);
      if (mountedRef.current) {
        setData(res.data);
      }
      return res.data;
    } catch (err) {
      if (mountedRef.current) {
        const msg = err.response?.data?.message
          || err.message
          || 'Error inesperado';
        setError(msg);
      }
      throw err;
    } finally {
      if (mountedRef.current) setLoading(false);
    }
  }, [apiFn]);

  const reset = useCallback(() => {
    setData(initialData);
    setError(null);
    setLoading(false);
  }, [initialData]);

  // Auto-fetch on mount if requested
  useEffect(() => {
    if (immediate) {
      execute(initialParams);
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  return { data, loading, error, execute, reset, setData };
}
