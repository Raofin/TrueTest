"use client";

import React, { useState } from "react";
import { Button, Form } from "@heroui/react";
import ProfileEdit from "@/components/profile/ProfileEdit";
import RootNavBar from "@/app/(root)/root-navbar";
import api from "@/lib/api";
import { usePathname, useRouter } from "next/navigation";
import { ProfileFormData } from "@/components/types/profile";
import ROUTES from "@/constants/route";

export default function MyProfileEdit() {
    const router = useRouter();
    const pathname = usePathname();
    const [formData, setFormData] = useState<ProfileFormData>({
        firstName: "",
        lastName: "",
        bio: "",
        instituteName: "",
        phoneNumber: "",
        imageFileId: '',
        profileLinks: [{ name: "", link: "" }],
    });
    const handleProfileUpdate = async (e: React.FormEvent) => {
        e.preventDefault();
        try {
            const response = await api.put(ROUTES.PROFILE_SAVE, formData);
            console.log(response.data)
            if (response.status === 200) {
                const response = await api.get(ROUTES.USER_INFO);
                const isAdmin = response.data.roles.some(
                    (role: string) => role.toLowerCase() === "admin"
                );
                router.push(isAdmin ? "/profile" : "/myprofile");
            }
        } catch {
            alert("Profile update failed. Please try again.");
        }
    };
    return (
        <>
            {pathname.includes("/myprofile") && <RootNavBar />}
            <div className="flex justify-center items-center min-h-screen ">
                <Form
                    className=" p-6 rounded-lg shadow-none bg-white dark:bg-[#18181b]"
                    onSubmit={handleProfileUpdate}
                >
                    <h2 className="w-full text-lg font-semibold text-center">
                        Update Profile
                    </h2>
                    <ProfileEdit
                        formData={formData}
                        setFormData={setFormData}
                    />
                    <div className="w-full mt-5 flex justify-center">
                        <Button
                            className=" text-center "
                            color="primary"
                            radius="lg"
                            type="submit"
                        >
                            Save Changes
                        </Button>
                    </div>
                </Form>
            </div>
        </>
    );
}
