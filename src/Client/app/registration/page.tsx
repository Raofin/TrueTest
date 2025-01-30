"use client";

import React from "react";
import {Button, Input, Checkbox, Link} from "@heroui/react";
import {Icon} from "@iconify/react";

export default function Component() {
    const [isVisible, setIsVisible] = React.useState(false);
    const [isConfirmVisible, setIsConfirmVisible] = React.useState(false);

    const toggleVisibility = () => setIsVisible(!isVisible);
    const toggleConfirmVisibility = () => setIsConfirmVisible(!isConfirmVisible);

    return (
        <div className="flex h-full w-full items-center justify-center">
            <div className="flex w-full max-w-sm flex-col gap-4 rounded-large px-8 pb-10 pt-6 shadow-small">
                <p className="pb-4 text-center text-3xl font-semibold">Sign Up</p>
                <form className="flex flex-col gap-4" onSubmit={(e) => e.preventDefault()}>
                    <Input isRequired label="Username" name="username" type="text" variant="bordered"/>
                    <Input isRequired label="Email" name="email" type="email" variant="bordered"/>
                    <Input isRequired endContent={
                            <button type="button" onClick={toggleVisibility}>
                                {isVisible ? (
                                    <Icon
                                        className="pointer-events-none text-2xl text-default-400"
                                        icon="solar:eye-closed-linear"
                                    />
                                ) : (
                                    <Icon
                                        className="pointer-events-none text-2xl text-default-400"
                                        icon="solar:eye-bold"
                                    />
                                )}
                            </button>
                        }
                        label="Password" name="password" type={isVisible ? "text" : "password"} variant="bordered"/>
                    <Input isRequired endContent={
                            <button type="button" onClick={toggleConfirmVisibility}>
                                {isConfirmVisible ? (
                                    <Icon
                                        className="pointer-events-none text-2xl text-default-400"
                                        icon="solar:eye-closed-linear"
                                    />
                                ) : (
                                    <Icon
                                        className="pointer-events-none text-2xl text-default-400"
                                        icon="solar:eye-bold"
                                    />
                                )}
                            </button>
                        }
                        label="Confirm Password" name="confirmPassword" type={isConfirmVisible ? "text" : "password"} variant="bordered"/>
                    <Checkbox isRequired className="py-4" size="sm">
                        I agree with the&nbsp;
                        <Link href="#" size="sm">
                            Terms
                        </Link>
                        &nbsp; and&nbsp;
                        <Link href="#" size="sm">
                            Privacy Policy
                        </Link>
                    </Checkbox>
                    <Button color="primary" type="submit">
                        Sign Up
                    </Button>
                </form>
                <p className="text-center text-small">Already have an account?
                    <Link className='ml-2' href="/login" size="sm">Log In</Link>
                </p>
            </div>
        </div>
    );
}
