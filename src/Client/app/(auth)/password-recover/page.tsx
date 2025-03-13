"use client";

import React, { FormEvent, useState } from "react";
import '../../../styles/globals.css';
import { Button, useDisclosure } from "@heroui/react";
import Link from "next/link";
import toast, { Toaster } from "react-hot-toast";
import axios from "axios";
import { useRouter } from 'next/navigation';
import { useForm, SubmitHandler } from "react-hook-form";
import OTPModal from "@/app/components/ui/Modal/otp-verification";

interface FormValues {
    otp: string;
}

const PasswordRecoverPage: React.FC = () => {
    const [contactInfo, setContactInfo] = useState("");
    const [loading, setLoading] = useState<boolean>(false);
    const { isOpen, onOpen } = useDisclosure();
    const router = useRouter();
    const isDarkMode = localStorage.getItem("theme");
    const { handleSubmit, control, formState: { errors } } = useForm<FormValues>();

    const validateContactInfo = (info: string): boolean => {
        const re = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        return re.test(info);
    };

    const handlePasswordOTP = async (e: FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        setLoading(true);
        if (!validateContactInfo(contactInfo)) {
            toast.error("please enter a valid email")
            return;
        }
        try {
            const response = await axios.post(`${process.env.NEXT_PUBLIC_AUTH_URL}/SendOtp`,
               { email: contactInfo }, {
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
            <form className={`p-8 rounded-lg shadow-lg max-w-md w-full ${isDarkMode === "dark" ? "bg-[#18181b]" : "bg-white"}`} onSubmit={handlePasswordOTP} >
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
            <OTPModal
                isOpen={isOpen ? true : false}
                onOpenChange={() => { }}
                control={control}
                handleFormSubmit={handleSubmit(onSubmit)}
                errors={errors}
            />
        </div>
    );
};

export default PasswordRecoverPage;
