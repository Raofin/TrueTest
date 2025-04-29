'use client'

import React from 'react';
import styles from '@/styles/LoadingModal.module.css'; 

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
        <div className={styles.spinner}></div>
        <p className={styles.message}>{message}</p>
      </div>
    </div>
  );
};

export default LoadingModal;