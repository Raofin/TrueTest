"use client";
import React, { useState } from "react";
import '../../styles/globals.css'
import {Modal, ModalContent, ModalHeader, ModalBody, ModalFooter, Button, useDisclosure} from "@heroui/react";
import { useRouter } from 'next/navigation'
import toast, {Toaster} from "react-hot-toast";
import {Controller, SubmitHandler, useForm} from "react-hook-form";
import {InputOtp} from "@heroui/input-otp";
const OtpPage: React.FC = () => {
    const [contactInfo, setContactInfo] = useState("");
    const [deliveryMethod, setDeliveryMethod] = useState("email");
    const [otp, setOtp] = useState("");
    const [message, setMessage] = useState("");
    const [isOtpSent, setIsOtpSent] = useState(false);
    const [isOtpVerified, setIsOtpVerified] = useState(false);
    const [isSuccess, setIsSuccess] = useState(true);
    const {isOpen, onOpen, onOpenChange} = useDisclosure();
    const router = useRouter();
    const validateContactInfo = (info: string): boolean => {
            const re = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
            return re.test(info);
    };
    interface FormValues {
        otp: string;
    }
    const handleGenerateOtp = async () => {
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
        if(isSuccess){
            onOpen();
            toast.success('Check you email')
        }
        // const res = await fetch("", {
        //     method: "POST",
        //     headers: { "Content-Type": "application/json" },
        //     body: JSON.stringify({
        //         email: contactInfo , deliveryMethod,
        //     }),
        // });
        // const result = await res.json();
        // setMessage(result.message);
        // if (res.status === 200) {
        //     setIsOtpSent(true);
        //     setIsSuccess(true);
        // } else {
        //     setIsSuccess(false);
        // }
    };

    // const handleVerifyOtp = async () => {
    //     const res = await fetch("/api/auth/otp/verify", {
    //         method: "POST",
    //         headers: { "Content-Type": "application/json" },
    //         body: JSON.stringify({
    //             email: contactInfo, otp,
    //         }),
    //     });
    //     const result = await res.json();
    //     setMessage(result.message);
    //     if (res.status === 200) {
    //         setIsOtpVerified(true);
    //         setIsSuccess(true);
    //     } else {
    //         setIsSuccess(false);
    //     }
    // };
    const {handleSubmit, control, formState: {errors},} = useForm<FormValues>({
        defaultValues: {otp: ""}});

    const onSubmit: SubmitHandler<FormValues> = (data) => {
        alert(JSON.stringify(data));
    };
    return (
        <div className="flex items-center justify-center  h-[600px] bg-gray-100">
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
                         <button onClick={handleGenerateOtp} className="w-full bg-blue-500 text-white p-3 rounded hover:bg-blue-600">
                            Generate OTP
                        </button>
                        {isSuccess?<Toaster
                            position="top-right"
                            reverseOrder={false} toastOptions={{ duration: 1000,}}
                        />: <Toaster
                            position="top-center" toastOptions={{ duration: 1000,}}
                            reverseOrder={false}
                        />}
                    </>
                )
                }
                <Modal isOpen={isOpen} placement="top-center" onOpenChange={onOpenChange}>
                    <ModalContent>
                        {(onClose) => (
                            <>
                                <ModalBody className=''>
                                    <form className="flex flex-col gap-6 ml-10 p-10" onSubmit={handleSubmit(onSubmit)}>
                                        <Controller control={control} name="otp" render={({field}) => (
                                                <InputOtp     classNames={{
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
                                        <Button color="primary" className="max-w-fit ml-20 " type="submit" variant="solid">
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