'use client'

import React from 'react';
import styles from '@/styles/LoadingModal.module.css'; 
import { Spinner } from '@heroui/spinner'

interface LoadingModalProps {
  isOpen: boolean;
  message?: string;
}

const LoadingModal: React.FC<LoadingModalProps> = ({ isOpen, message = 'Loading...' }) => {
  if (!isOpen) {
    return null;
  }
  return (
    <div className={styles.modalOverlay}>
      <div className={`${styles.modalContent} dark:bg-[#18181b] dark:text-white`}>
        <Spinner/>
        <p className={styles.message}>{message}</p>
      </div>
    </div>
  );
};

export default LoadingModal;