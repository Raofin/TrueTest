'use client'

import { useEffect } from 'react'
import Swal from 'sweetalert2'

type SweetAlertIcon = 'success' | 'error' | 'warning' | 'info' | 'question';
interface PageProps {
  icon: SweetAlertIcon
  text: string
  showConfirmButton: boolean
  timer: number
}


const SweetAlert = ({ icon, text, showConfirmButton, timer }: PageProps) => {
  useEffect(() => {
    Swal.fire({
      position: 'center',
      icon: icon,
      text: text,
      showConfirmButton: showConfirmButton,
      timer: timer,
    })
  }, [icon, showConfirmButton, text, timer])
  return null
}
export default SweetAlert
