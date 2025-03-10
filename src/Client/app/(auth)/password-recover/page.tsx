"use client";
import React, { FormEvent, useState } from "react";
import '../../../styles/globals.css';
import { InputOtp } from "@heroui/input-otp";
import { Button, Input, Modal, ModalBody, ModalContent, useDisclosure } from "@heroui/react";
import Link from "next/link";
import toast, { Toaster } from "react-hot-toast";
import axios from "axios";
import { useRouter } from 'next/navigation'; 
import { useForm, Controller, SubmitHandler } from "react-hook-form";

interface FormValues {
    otp: string;
}

const PasswordRecoverPage: React.FC = () => {
    const [contactInfo, setContactInfo] = useState("");
    const [loading, setLoading] = useState<boolean>(false);
    const { isOpen, onOpen, onOpenChange } = useDisclosure();
    const router = useRouter(); 
    const isDarkMode=localStorage.getItem("theme");
    const { handleSubmit, control, formState: { errors } } = useForm<FormValues>();

    const validateContactInfo = (info: string): boolean => {
        const re = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        return re.test(info);
    };

    const handlePasswordOTP = async (e: FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        setLoading(true);
        if(!validateContactInfo(contactInfo)){
            toast.error("please enter a valid email")
            return;
        }
        try {
            const response = await axios.post(`${process.env.NEXT_PUBLIC_AUTH_URL}/SendOtp`, 
                JSON.stringify({ email: contactInfo }), {
                    headers: { 'Content-Type': 'application/json' }
                });
            if (response.status === 200) {
                toast.success("OTP sent to your email. Please check your inbox.");
                onOpen(); 
            } else {
                toast.error(response.data?.message || "Failed to send OTP.");
            }
        } catch (error) {
            console.error("Error sending OTP:", error);
            if (axios.isAxiosError(error)) {
                toast.error(error.response?.data?.message || "Failed to send OTP.");
            } else {
                toast.error("An unexpected error occurred while sending OTP.");
            }
        } finally {
            setLoading(false);
        }
    };

    const onSubmit: SubmitHandler<FormValues> = async (data) => {
        try {
            if (!data.otp) {
                toast.error("OTP is required.");
                return;
            }
            const verifyResponse = await axios.post(
                `${process.env.NEXT_PUBLIC_AUTH_URL}/IsValidOtp`,
                { email: contactInfo, otp: data.otp }
            );
            if (verifyResponse.data.isValidOtp) {
                toast.success("OTP verified successfully!");
                router.push(`/password-recover/reset-password?otp=${data.otp}&email=${contactInfo}`);
            } else {
                toast.error(verifyResponse.data?.message || "Invalid OTP. Please try again.");
            }
        } catch (error) {
            if (axios.isAxiosError(error)) {
                toast.error(error.response?.data?.message || "Failed to verify OTP.");
            } else {
                toast.error("An unexpected error occurred.");
            }
        }
    };

    return (
        <div className="flex items-center justify-center h-[600px]">
            <form className={`p-8 rounded-lg shadow-lg max-w-md w-full ${isDarkMode==="dark"? "bg-[#18181b]" : "bg-white"}`} onSubmit={handlePasswordOTP} >
                <h1 className="text-2xl font-semibold mb-6 text-center">
                    Password Recovery
                </h1>
                <input
                    type="email"
                    placeholder="Enter your email"
                    value={contactInfo}
                    onChange={(e) => setContactInfo(e.target.value)}
                    required
                    className="w-full p-3 border border-gray-300 rounded mb-4"
                />
                <Button className="my-4 w-full" color="primary" type="submit" isLoading={loading}>
                    Verify Email
                </Button>
                <p>Want to create a new account? <Link className="text-blue-500 ml-2" href="/signup">Sign Up</Link></p>
            </form>
            <Toaster />
          
            <Modal isOpen={isOpen} placement="top-center" onOpenChange={onOpenChange}>
                <ModalContent>
                    <ModalBody>
                        <form className="flex flex-col gap-6 p-10" onSubmit={handleSubmit(onSubmit)}>
                            <Controller
                                control={control}
                                name="otp"
                                render={({ field }) => (
                                    <InputOtp
                                        classNames={{
                                            segmentWrapper: "gap-x-5",
                                            segment: [
                                                "relative",
                                                "h-12",
                                                "w-12",
                                                "border-y",
                                                "border-r",
                                                "first:rounded-l-md",
                                                "first:border-l",
                                                "last:rounded-r-md",
                                                "border-default-200",
                                                "data-[active=true]:border",
                                                "data-[active=true]:z-20",
                                                "data-[active=true]:ring-2",
                                                "data-[active=true]:ring-offset-2",
                                                "data-[active=true]:ring-offset-background",
                                                "data-[active=true]:ring-foreground",
                                            ],
                                        }}
                                        {...field}
                                        errorMessage={errors.otp?.message}
                                        isInvalid={!!errors.otp}
                                        length={4}
                                    />
                                )}
                                rules={{
                                    required: "OTP is required",
                                    minLength: {
                                        value: 4,
                                        message: "Please enter a valid OTP",
                                    },
                                }}
                            />
                            <Button color="primary" className="max-w-fit ml-20" type="submit">
                                Verify OTP
                            </Button>
                        </form>
                    </ModalBody>
                </ModalContent>
            </Modal>
        </div>
    );
};

export default PasswordRecoverPage;
