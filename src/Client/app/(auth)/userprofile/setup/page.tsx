"use client";

import React, { useState } from "react";
import { Button, Form } from "@heroui/react";
import ProfileEdit from "@/components/profile/ProfileEdit";
import { useRouter } from "next/navigation";
import api from "@/lib/api";
import { ProfileFormData } from "@/components/types/profile";
import ROUTES from "@/constants/route";
import LoadingModal from "@/components/ui/Modal/LoadingModal";

export default function ProfileSetUp() {
    const router = useRouter();
    const [loading, setLoading] = useState(false);
    const [formData, setFormData] = useState<ProfileFormData>({
        firstName: "",
        lastName: "",
        bio: "",
        instituteName: "",
        phoneNumber: "",
        imageFileId: "",
        profileLinks: [{ name: "", link: "" }],
    });

    const handleProfileEdit = async (e: React.FormEvent) => {
        e.preventDefault();
        setLoading(true);
        try {
            const response = await api.put(ROUTES.PROFILE_SAVE, formData);
            if (response.status === 200) {
                const response = await api.get(ROUTES.USER_INFO);
                if (response.status === 200) {
                    const isAdmin = response.data.roles.some(
                        (role: string) => role.toLowerCase() === "admin"
                    );
                    router.push(isAdmin ? "/overview" : "/home");
                }
            }
        } catch {
            alert("Profile update failed. Please try again.");
        } finally {
            setLoading(false);
        }
    };

    const handleSkipButton = async () => {
        try {
            const response = await api.get(ROUTES.USER_INFO);
            const isAdmin = response.data.roles.some(
                (role: string) => role.toLowerCase() === "admin"
            );
            router.push(isAdmin ? "/overview" : "/home");
        } catch  {
            alert("Failed to fetch user info. Please try again.");
        }
    };

    return (
        <div className="flex justify-center items-center">
            <LoadingModal isOpen={loading} message="Loading..." />
            <Form
                className="py-5 px-8 rounded-lg shadow-none bg-white dark:bg-[#18181b]"
                onSubmit={handleProfileEdit}
            >
                <h2 className="w-full text-xl font-bold text-center">
                    Add Details
                </h2>
                <ProfileEdit formData={formData} setFormData={setFormData} />

                <div className="w-full flex justify-between mt-6">
                    <Button onPress={handleSkipButton}>Skip for now</Button>
                    <Button color="primary" radius="lg" type="submit">
                        Save & Continue
                    </Button>
                </div>
            </Form>
        </div>
    );
}
