'use client'

import { Modal, ModalContent, ModalHeader, ModalBody, ModalFooter, Button } from '@heroui/react'

interface CommonModalProps {
  isOpen: boolean
  onClose: () => void
  title: string
  content: React.ReactNode
  confirmButtonText: string
  onConfirm: () => void
}
const CommonModal = ({ isOpen, onClose, title, content, confirmButtonText, onConfirm }: CommonModalProps) => {
  return (
    <Modal isOpen={isOpen}>
      <ModalContent>
        {(close) => (
          <>
            <ModalHeader>{title}</ModalHeader>
            <ModalBody>
              <p>{content}</p>
            </ModalBody>
            <ModalFooter>
              <Button
                color="default"
                onPress={() => {
                  onClose()
                  close()
                }}
              >
                Close
              </Button>
              <Button
                color="primary"
                onPress={async () => {
                  await onConfirm()
                  onClose()
                  close()
                }}
              >
                {confirmButtonText}
              </Button>
            </ModalFooter>
          </>
        )}
      </ModalContent>
    </Modal>
  )
}

export default CommonModal
