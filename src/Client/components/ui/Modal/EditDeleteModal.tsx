"use client";

import { Modal, ModalContent, ModalBody, ModalHeader, ModalFooter, Button } from "@heroui/react";

interface CommonModalProps {
  isOpen: boolean;
  onClose: () => void;
  title: string;
  content: string;
  confirmButtonText?: string;
  onConfirm?: () => void;
}

const CommonModal: React.FC<CommonModalProps> = ({ isOpen, onClose, title, content, confirmButtonText = "Confirm", onConfirm }) => {

  return (
    <Modal isOpen={isOpen} onClose={onClose}>
      <ModalContent>
        <>
          <ModalHeader className="flex flex-col gap-1">{title}</ModalHeader>
          <ModalBody>
            <p>{content}</p>
          </ModalBody>
          <ModalFooter>
            <Button color="danger" variant="light" onPress={onClose}>
              Close
            </Button>
            {onConfirm && (
              <Button color="primary" onPress={onConfirm}>
                {confirmButtonText}
              </Button>
            )}
          </ModalFooter>
        </>
      </ModalContent>
    </Modal>
  );
};

export default CommonModal;
