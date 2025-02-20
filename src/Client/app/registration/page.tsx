"use client";

import React, { KeyboardEvent , useState, ChangeEvent, FormEvent, useEffect } from "react";
import axios from "axios";
import { Button, Input, Checkbox, Link } from "@heroui/react";
import { Icon } from "@iconify/react";
import '../../styles/globals.css';

import { useRouter } from "next/navigation";

interface FormData {
    username: string;
    email: string;
    password: string;
    confirmPassword: string;
    agreeTerms: boolean;
}

export default function Signup() {
    const [formData, setFormData] = useState<FormData>({
        username: "",
        email: "",
        password: "",
        confirmPassword: "",
        agreeTerms: false,
    });
    const router=useRouter();
    const [isVisible, setIsVisible] = useState<boolean>(false);
    const [isConfirmVisible, setIsConfirmVisible] = useState<boolean>(false);
    const [loading, setLoading] = useState<boolean>(false);
    const [message, setMessage] = useState<string>("");
    const [emailError, setEmailError] = useState<string>("");
    const [checkingEmail, setCheckingEmail] = useState<boolean>(false);

    const toggleVisibility = () => setIsVisible(!isVisible);
    const toggleConfirmVisibility = () => setIsConfirmVisible(!isConfirmVisible);

    const handleChange = (e: ChangeEvent<HTMLInputElement>) => {
        const { name, value, type, checked } = e.target;
        setFormData((prev) => ({
            ...prev,
            [name]: type === "checkbox" ? checked : value,
        }));

        if (name === "email") {
            setEmailError("");
        }
    };
    useEffect(() => {
        const checkEmailUnique = async () => {
            if (!formData.email) return;
            setCheckingEmail(true);
            try {
                const response = await axios.post(`${process.env.NEXT_PUBLIC_AUTH_URL}/IsUserUnique`,
                    JSON.stringify({ username:formData.username, email: formData.email }),
                    {
                        headers: { 'Content-Type': 'application/json' }
                    }
                );
                if (!response.data.isUnique) {
                    setEmailError("This email is already taken.");
                } else {
                    setEmailError("");
                }
            } 
            catch (error: unknown) {
                if (axios.isAxiosError(error)) {
                    setEmailError(error.response?.data?.message);
                } else {
                    setEmailError("An unexpected error occurred.");
                }
                console.log(error);
            }
            finally {
                setCheckingEmail(false);
            }
        };
    }, [formData.email]);

    const handleSubmit = async (e: FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        setMessage("");
        if (!formData.agreeTerms) {
            setMessage("You must agree to the terms.");
            return;
        }
        if (formData.password !== formData.confirmPassword) {
            setMessage("Passwords do not match.");
            return;
        }
        if (emailError) {
            setMessage("Please use a unique email.");
            return;
        }
        setLoading(true);
      
        router.push(`/otp?username=${encodeURIComponent(formData.username)}&email=${encodeURIComponent(formData.email)}&password=${encodeURIComponent(formData.password)}`);

          
    };

    return (
        <div className="flex h-full w-full items-center justify-center">
            <div className="flex w-full max-w-sm flex-col gap-4 rounded-large px-8 pb-10 pt-6 shadow-small">
                <p className="pb-4 text-center text-3xl font-semibold">Sign Up</p>

                {message && <p className="text-center text-sm text-red-500">{message}</p>}

                <form className="flex flex-col gap-4" onSubmit={handleSubmit}>
                    <Input
                        isRequired
                        label="Username"
                        name="username"
                        type="text"
                        variant="bordered"
                        value={formData.username}
                        onChange={handleChange}
                    />
                    <div className="relative">
                        <Input
                            isRequired
                            label="Email"
                            name="email"
                            type="email"
                            variant="bordered"
                            value={formData.email}
                            onChange={handleChange}
                        />
                        {checkingEmail && (
                            <p className="text-sm text-gray-500 mt-1">Checking email...</p>
                        )}
                        {emailError && (
                            <p className="text-sm text-red-500 mt-1">{emailError}</p>
                        )}
                    </div>
                    <Input
                        isRequired
                        endContent={
                            <button type="button" onClick={toggleVisibility}>
                                <Icon className="text-2xl text-default-400" icon={isVisible ? "solar:eye-closed-linear" : "solar:eye-bold"} />
                            </button>
                        }
                        label="Password"
                        name="password"
                        type={isVisible ? "text" : "password"}
                        variant="bordered"
                        value={formData.password}
                        onChange={handleChange}
                    />
                    <Input
                        isRequired
                        endContent={
                            <button type="button" onClick={toggleConfirmVisibility}>
                                <Icon className="text-2xl text-default-400" icon={isConfirmVisible ? "solar:eye-closed-linear" : "solar:eye-bold"} />
                            </button>
                        }
                        label="Confirm Password"
                        name="confirmPassword"
                        type={isConfirmVisible ? "text" : "password"}
                        variant="bordered"
                        value={formData.confirmPassword}
                        onChange={handleChange}
                    />
                    <Checkbox
                        isRequired
                        className="py-4"
                        size="sm"
                        name="agreeTerms"
                        checked={formData.agreeTerms}
                        onChange={handleChange}
                    >
                        I agree with the&nbsp;
                        <Link href="#" size="sm">Terms</Link>&nbsp; and&nbsp;
                        <Link href="#" size="sm">Privacy Policy</Link>
                    </Checkbox>
                    <Button color="primary" type="submit" isDisabled={loading || !!emailError}>
                        {loading ? "Signing Up..." : "Sign Up"}
                    </Button>
                </form>

                <p className="text-center text-small">
                    Already have an account?
                    <Link className='ml-2' href="/login" size="sm">Log In</Link>
                </p>
            </div>
        </div>
    );
}
