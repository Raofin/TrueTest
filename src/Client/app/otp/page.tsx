"use client";
import React, { useState } from "react";
import '../../styles/globals.css';
import { Modal, ModalContent, ModalBody, Button, useDisclosure } from "@heroui/react";
import { useRouter,useSearchParams } from 'next/navigation';
import toast, { Toaster } from "react-hot-toast";
import { Controller, SubmitHandler, useForm } from "react-hook-form";
import { InputOtp } from "@heroui/input-otp";
import axios from "axios";

const OtpPage: React.FC = () => {
    const [contactInfo, setContactInfo] = useState("");
    const [isOtpSent, setIsOtpSent] = useState(false);
    const [isOtpVerified, setIsOtpVerified] = useState(false);
    const { isOpen, onOpen, onOpenChange } = useDisclosure();
    const searchParams = useSearchParams();
    const username = searchParams.get("username") || "";
    const email = searchParams.get("email") || "";
    const password = searchParams.get("password") || "";
    const router=useRouter();
    const validateContactInfo = (info: string): boolean => {
        const re = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
        return re.test(info);
    };

    const handleGenerateOtp = async () => {
        if (!contactInfo) {
            toast.error("Email Address is required");
            return;
        }
        if (!validateContactInfo(contactInfo)) {
            toast.error("Invalid email format");
            return;
        }
        try {
            const response = await axios.post(`${process.env.NEXT_PUBLIC_AUTH_URL}/SendOtp`, 
              JSON.stringify({ email: contactInfo}), {
                headers: { 'Content-Type': 'application/json' }
            });
            if (response.status === 200) {
                toast.success("OTP sent to your email. Please check your inbox.");
                setIsOtpSent(true);
                onOpen();
            } else {
                toast.error(response.data?.message || "Failed to send OTP.");
            }
        } catch (error: any) {
            console.error("Error sending OTP:", error);
            if (axios.isAxiosError(error)) {
                toast.error(error.response?.data?.message || "Failed to send OTP.");
            } else {
                toast.error("An unexpected error occurred while sending OTP.");
            }
        }
    };

    interface FormValues {
        otp: string;
        email: string; 
    }

    const { handleSubmit, control, formState: { errors } } = useForm<FormValues>({
        defaultValues: { otp: "" },
    });

    const onSubmit: SubmitHandler<FormValues> = async (data) => {
        try {
            const verifyResponse = await axios.post(`${process.env.NEXT_PUBLIC_AUTH_URL}/IsValidOtp`,
                JSON.stringify({ email: contactInfo, otp: data.otp }),
                { headers: { 'Content-Type': 'application/json' } }
            );
        
            console.log(contactInfo, data.otp,verifyResponse.status); 
        
            if (verifyResponse.status === 200) {
                toast.success("OTP verified successfully!");
                setIsOtpVerified(true);
                console.log("enter 1");
                await axios.post(`${process.env.NEXT_PUBLIC_AUTH_URL}/Register`, {
                    username,
                    email: contactInfo,  
                    password,
                    otp: data.otp
                });
                const userData = {
                    username,
                    email: contactInfo
                };            
                console.log("enter 2");
                toast.success("Signup successful!");
                router.push('/login');

            } else {
                toast.error(verifyResponse.data?.message || "Invalid OTP. Please try again.");
            }
        } catch (error: any) {
            console.error("Error verifying OTP:", error);
            toast.error(error.response?.data?.message || "Failed to verify OTP.");
        }
    };
    
    return (
        <div className="flex items-center justify-center h-[600px] bg-gray-100">
            <div className="bg-white p-8 rounded-lg shadow-lg max-w-md w-full">
                <h1 className="text-2xl font-semibold mb-6 text-center">OTP Authentication</h1>
                {!isOtpSent && (
                    <>
                        <input
                            type="email"
                            placeholder="Enter your email"
                            value={contactInfo}
                            onChange={(e) => setContactInfo(e.target.value)}
                            required
                            className="w-full p-3 border border-gray-300 rounded mb-4"
                        />
                        <button
                            onClick={handleGenerateOtp}
                            className="w-full bg-blue-500 text-white p-3 rounded hover:bg-blue-600"
                        >
                            Generate OTP
                        </button>
                        <Toaster
                            position="top-right"
                            reverseOrder={false}
                            toastOptions={{ duration: 1000 }}
                        />
                    </>
                )}
                <Modal isOpen={isOpen} placement="top-center" onOpenChange={onOpenChange}>
                    <ModalContent>
                        {() => (
                            <>
                                <ModalBody>
                                    <form className="flex flex-col gap-6 ml-10 p-10" onSubmit={handleSubmit(onSubmit)}>
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
                                        <Button
                                            color="primary"
                                            className="max-w-fit ml-20"
                                            type="submit"
                                            variant="solid"
                                        >
                                            Verify OTP
                                        </Button>
                                    </form>
                                </ModalBody>
                            </>
                        )}
                    </ModalContent>
                </Modal>
            </div>
        </div>
    );
};

export default OtpPage;