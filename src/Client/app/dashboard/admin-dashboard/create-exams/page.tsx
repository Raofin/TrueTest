'use client';

import {
    Button,
    DatePicker,
    Input,
    Textarea,
    TimeInput,
    useDisclosure,
    Modal,
    ModalContent,
    ModalBody,
    ModalFooter,
} from '@heroui/react';
import axios from 'axios';
import Link from 'next/link';
import React, { FormEvent, useState } from 'react';
import toast, { Toaster } from 'react-hot-toast';
import { CalendarDate, parseDate, Time } from "@internationalized/date";

interface FormData {
    title: string;
    description: string;
    durationMinutes: string;
    // date: string;
    opensAt: string;
    closesAt: string;
    // totalpoints: string;
}

function parseTime(time: string): Time | null {
    if (!time) return null;
    const [hour, minute] = time.split(':').map(Number);
    return new Time(hour, minute);
}

export default function Component() {
    const [loading, setLoading] = useState<boolean>(false);
    const { isOpen, onOpen, onOpenChange } = useDisclosure();
    const [formData, setFormData] = useState<FormData>({
        title: "",
        description: "",
        durationMinutes: "",
        opensAt: "",
        closesAt: "",
    });

    const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
        const { name, value } = e.target;
        setFormData({ ...formData, [name]: value });
    };

    // const handleDateChange = (date: CalendarDate | null) => {
    //     setFormData({ ...formData, date: date ? date.toString() : "" });
    // };

    const handleTimeChange = (name: "opensAt" | "closesAt", value: Time | null) => {
        setFormData(prevState => {
            const updatedFormData = { ...prevState, [name]: value ? `${value.hour}:${value.minute}` : "" };

            if (updatedFormData.opensAt && updatedFormData.closesAt) {
                const opensAtTime = parseTime(updatedFormData.opensAt);
                const closesAtTime = parseTime(updatedFormData.closesAt);

                if (opensAtTime && closesAtTime) {
                    const startMinutes = opensAtTime.hour * 60 + opensAtTime.minute;
                    const endMinutes = closesAtTime.hour * 60 + closesAtTime.minute;
                    const duration = endMinutes - startMinutes;

                    updatedFormData.durationMinutes = duration > 0 ? duration.toString() : "0";
                }
            }
            return updatedFormData;
        });
    };

    const handleSubmit = async (e: FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        setLoading(true);
        try {
            const response = await axios.post(`${process.env.NEXT_PUBLIC_URL}/Exam`,
                formData
            );
            if (response.status === 200) {
                onOpen();
                setFormData({
                    title: '',
                    description: '',
                    durationMinutes: '',
                    // date: '',
                    opensAt: '',
                    closesAt: '',
                    // totalpoints: '',
                });
            } else {
                toast.error(response.data?.message || 'Failed to create exam.');
            }
        } catch (error) {
            console.error('Error creating exam:', error);
            if (axios.isAxiosError(error)) {
                toast.error(error.response?.data?.message || 'Failed to create exam.');
            } else {
                toast.error('An unexpected error occurred.');
            }
        } finally {
            setLoading(false);
        }
    };

    return (
        <>
            <form className="flex gap-4 flex-wrap flex-col" onSubmit={handleSubmit}>
                <Input
                    isRequired
                    label="Title"
                    name="title"
                    type="text"
                    variant="bordered"
                    value={formData.title}
                    onChange={handleChange}
                />
                <Textarea
                    isRequired
                    label="Description"
                    name="description"
                    type="text"
                    variant="bordered"
                    value={formData.description}
                    onChange={handleChange}
                />
                <div className="flex gap-5">
                    <DatePicker
                        className="flex-1"
                        isRequired
                        label="Date"
                        name="date"
                     //   value={formData.date ? parseDate(formData.date) : undefined}
                     //   onChange={handleDateChange}
                    />
                    <Input
                        className="flex-1"
                        isRequired
                        label="Total Points"
                        name="totalpoints"
                        type={'text'}
                        variant="bordered"
                        //value={formData.totalpoints}
                      //  onChange={handleChange}
                    />
                </div>
                <div className="flex gap-5">
                    <TimeInput
                        label="Start Time"
                        name="opensAt"
                        value={parseTime(formData.opensAt)}
                        onChange={(value) => handleTimeChange("opensAt", value as Time)}
                        isRequired
                    />
                    <TimeInput
                        label="End Time"
                        name="closesAt"
                        value={parseTime(formData.closesAt)}
                        onChange={(value) => handleTimeChange("closesAt", value as Time)}
                        isRequired
                    />
                </div>
                <div className="flex-1 mt-2 ml-96 pl-96">
                    <Button color="success">Publish</Button>
                    <Button color="danger" className="mx-3">
                        Delete
                    </Button>
                    <Button color="primary" type="submit"> Save
                    </Button>
                </div>
            </form>
            <div className="flex gap-3 my-7 ml-44">
                <Link href="">
                    <Button color="primary">Add Problem Solving Question</Button>
                </Link>
                <Button color="primary">Add Written Question</Button>
                <Button color="primary">Add MCQ Question</Button>
            </div>
            <Modal isOpen={isOpen} onOpenChange={onOpenChange}>
                <ModalContent>
                    {(onClose) => (
                        <>
                            <ModalBody>
                                <p>Exam Created Successfully</p>
                            </ModalBody>
                            <ModalFooter>
                                <Button color="danger" variant="light" onPress={onClose}>
                                    Close
                                </Button>
                            </ModalFooter>
                        </>
                    )}
                </ModalContent>
            </Modal>
            <Toaster />
        </>
    );
}