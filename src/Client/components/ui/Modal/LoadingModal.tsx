'use Client'

import React from 'react';
import styles from '../LoadingModal.module.css'; 

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
      <div className={styles.modalContent}>
        <div className={styles.spinner}></div>
        <p className={styles.message}>{message}</p>
      </div>
    </div>
  );
};

export default LoadingModal;