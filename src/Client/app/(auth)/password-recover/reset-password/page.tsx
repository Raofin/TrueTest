'use client'

import { useAuth } from "@/app/context/AuthProvider";
import { Button, Input, Link } from "@heroui/react"
import axios from "axios";
import { useSearchParams,useRouter } from "next/navigation";
import { FormEvent, useState } from "react";
import toast from "react-hot-toast";

export default function Component() {
     const [loading, setLoading] = useState<boolean>(false);
     const searchParams = useSearchParams();
     const otp = searchParams.get("otp");
     const email = searchParams.get("email");
     const {user}=useAuth();
     const router=useRouter();
     const isDarkMode=localStorage.getItem("theme");
    const handleResetPassword = async (e: FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        // const email = (e.target as any).email.value;
        const newPassword = (e.target as any).newpassword.value;
        const confirmPassword = (e.target as any).newconfirmpassword.value;
        if (newPassword !== confirmPassword) {
            toast.error("Passwords do not match.");
            return;
        }
        try {
            setLoading(true);
            const response = await axios.post(
                `${process.env.NEXT_PUBLIC_AUTH_URL}/PasswordRecovery`,
                {
                    email,
                    password: newPassword,
                    otp
                }
            );

            if (response.status === 200) {
                toast.success("Password reset successfully.");
                router.push("/login"); 
            } else {
                toast.error(response.data?.message || "Failed to reset password.");
            }
        } catch (error) {
            if (axios.isAxiosError(error)) {
                toast.error(error.response?.data?.message);
            } else {
                toast.error("An unexpected error occurred.");
            }
        } finally {
            setLoading(false);
        }
    };
    return (

        <div className="flex h-full w-full items-center justify-center mb-16">
            <div className="flex w-full max-w-sm flex-col mt-16 gap-4 rounded-large bg-content1 px-8 pb-10 pt-6 shadow-small">
                <div className="flex flex-col gap-3">
                    <h1 className="text-xl font-semibold text-center mb-6">Reset Password</h1>
                </div>
                <form className={`flex w-full flex-wrap md:flex-nowrap gap-4 flex-col ${isDarkMode==="dark"? "bg-[#18181b]" : "bg-white"}`} onSubmit={handleResetPassword}>
                    <Input isRequired label="Email Address" name="email" type="email" variant="bordered" defaultValue={email || ""} />
                    <Input isRequired label="New Password" name="newpassword" type="password" variant="bordered" />
                    <Input isRequired label="Confirm Password" name="newconfirmpassword" type="password" variant="bordered" />
                    <Button className="w-full mt-4 text-medium" color="primary" type="submit">
                        Change Password
                    </Button>
                    <p>Want to create a new account? <Link className="text-blue-500 ml-2" href="/registration">Sign Up</Link></p>
                </form>
            </div>
        </div>

    )
}