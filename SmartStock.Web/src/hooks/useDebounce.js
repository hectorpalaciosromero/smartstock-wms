import { useState, useEffect } from 'react';

/**
 * Debounces a value by the specified delay.
 * @param {*} value - The value to debounce
 * @param {number} delay - Delay in milliseconds (default 400ms)
 * @returns The debounced value
 */
export default function useDebounce(value, delay = 400) {
  const [debounced, setDebounced] = useState(value);

  useEffect(() => {
    const timer = setTimeout(() => setDebounced(value), delay);
    return () => clearTimeout(timer);
  }, [value, delay]);

  return debounced;
}
