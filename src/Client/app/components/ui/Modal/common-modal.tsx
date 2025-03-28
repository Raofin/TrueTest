'use client'

import {Modal, ModalContent, ModalBody ,Button, ModalFooter} from "@heroui/react";
import handleDelete from '@/app/components/ui/handleDelete'
interface PageProps{
    isOpen:boolean,
    onOpenChange:(isOpen:boolean)=>void,
    title:string
}
const CommonModal = ({ isOpen, onOpenChange, title}:PageProps) => {
    return (
        <Modal isOpen={isOpen} onOpenChange={onOpenChange} onSubmit={handleDelete}>
        <ModalContent>
            {(onClose) => (
                <>
                    <ModalBody>
                        <p>{title}</p>
                    </ModalBody>
                    <ModalFooter>
                        <Button color="primary" onPress={onClose}>
                            Close
                        </Button>
                    </ModalFooter>
                </>
            )}
        </ModalContent>
    </Modal>
    );
};

export default CommonModal;
