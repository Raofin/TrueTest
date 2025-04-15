import { useEffect, useState } from 'react'

export default function useTheme() {
 const [Mode, setMode] = useState<string | null>(null)
useEffect(() => {
    if (typeof window !== 'undefined') {
      setMode(localStorage.getItem('theme'))
    }
  }, [Mode])
  return Mode;
}
