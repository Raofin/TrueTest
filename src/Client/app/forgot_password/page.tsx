"use client";
import React, { useState } from "react";
import '../../styles/globals.css'
import {Link} from "@nextui-org/react";
import toast, {Toaster} from "react-hot-toast";
const ForgotPasswordPage: React.FC = () => {
    const [contactInfo, setContactInfo] = useState("");
    const [mail, setEmail] = useState("");
    const [message, setMessage] = useState("");
    const [isEmailSent, setIsEmailSent] = useState(false);
    const [isEmailVerified, setIsEmailVerified] = useState(false);
    const [isSuccess, setIsSuccess] = useState(true);

    const validateContactInfo = (info: string): boolean => {
            const re = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
            return re.test(info);
    };
    const handleGenerateEmail = async () => {
        if (!contactInfo) {
            toast.error("Email Address is required");
            setIsSuccess(false);
            return;
        }
        if (!validateContactInfo(contactInfo)) {
            toast.error("Invalid email format");
            setIsSuccess(false);
            return;
        }
        if(isSuccess) toast.success('Check your email');
        const res = await fetch("", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({email: contactInfo}),
        });
        const result = await res.json();
        setMessage(result.message);
        if (res.status === 200) {
            setIsEmailSent(true);
            setIsSuccess(true);
        } else {
            setIsSuccess(false);
        }
    };

    const handleVerifyEmail = async () => {
        const res = await fetch("/api/auth/forgotpassword/verify", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({
                email: contactInfo , mail,
            }),
        });
        const result = await res.json();
        setMessage(result.message);
        if (res.status === 200) {
            setIsEmailVerified(true);
            setIsSuccess(true);
        } else {
            setIsSuccess(false);
        }
    };

    return (
        <div className="flex items-center justify-center h-[600px] bg-gray-100">
            <div className="bg-white p-8 rounded-lg shadow-lg max-w-md w-full">
                <h1 className="text-2xl font-semibold mb-6 text-center">
                    Forgot Password
                </h1>
                {!isEmailSent ? (
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
                            onClick={handleGenerateEmail}
                            className="w-full bg-blue-500 text-white p-3 rounded hover:bg-blue-600">
                            Send
                        </button>
                        {isSuccess?<Toaster
                            position="top-right" toastOptions={{ duration: 3000}}
                            reverseOrder={false}
                        />: <Toaster
                            position="top-center" toastOptions={{ duration: 2000}}
                            reverseOrder={false}
                        />}
                    </>
                ) : (
                    <div>
                        {!isEmailVerified ? (
                            <div>
                                <input
                                    type="text"
                                    placeholder="Enter Email"
                                    value={mail}
                                    onChange={(e) => setEmail(e.target.value)}
                                    required
                                    className="w-full p-3 border border-gray-300 rounded mb-4"
                                />
                                <button onClick={handleVerifyEmail}
                                    className="w-full bg-blue-500 text-white p-3 rounded">
                                   Send
                                </button>

                            </div>
                        ) : (
                            <div className="text-center">
                                <Link href="/forgot_password/reset_password" className="text-xl font-semibold">Reset Password</Link>
                            </div>
                        )}
                    </div>
                )}
            </div>
        </div>
    );
};
export default ForgotPasswordPage;