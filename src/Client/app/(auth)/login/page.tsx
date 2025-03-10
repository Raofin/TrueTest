'use client';

import React, { useState } from 'react';
import { Button, Input, Checkbox, Link, Form, Divider } from '@heroui/react';
import { Icon } from '@iconify/react';
import '../../../styles/globals.css';
import { useRouter } from 'next/navigation';
import { useAuth } from '../../context/AuthProvider';
import axios from '../../axios/page';

export default function LoginComponent() {
  const [isVisible, setIsVisible] = useState(false);
  const [user, setUser] = useState({ email: '', password: '' });
  const [error, setError] = useState('');
  const router = useRouter();
  const { setAuth } = useAuth();
   const isDarkMode=localStorage.getItem("theme");
  const toggleVisibility = () => setIsVisible((prev) => !prev);
  const LoginURL = '/login';
  const [isLoading, setIsLoading] = useState(false);
  const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
    setIsLoading(true);
    event.preventDefault();
    setError('');
    try {
      setIsLoading(false);
      const response = await axios.post(LoginURL, {
        usernameOrEmail: user.email,
        password: user.password,
      }, {
        headers: { 'Content-Type': 'application/json' },
       
      });

      if (response.data?.token) {
        setAuth({ authToken: response.data.token });
        router.push('/dashboard/admin');
      } else {
        setError('Invalid email or password');
      }
    } catch (error:any) {
      setIsLoading(false);
      if (error.response) {
      console.error('Login error:', error.response?.data || error.message);
      setError(error.response.data.message || 'Login failed');
      }else if (error.request) {
        console.error('Login error: Network error');
        setError('Network error. Please check your connection.');
    }else {
      console.error('Login error:', error.message);
      setError('An unexpected error occurred.');
    }
   }
  };

  return (
    <div className="flex h-full w-full items-center justify-center">
      <div className={`mt-12 flex w-full max-w-sm flex-col gap-4 rounded-large px-8 pb-7 mb-12 shadow-2xl  ${isDarkMode==="dark"? "bg-[#18181b]" : "bg-white"}`}>
        <div className="flex flex-col gap-1">
          <h1 className="text-large font-medium text-center mt-2">Log In</h1>
        </div>
        <Form
          className="flex w-full flex-wrap md:flex-nowrap gap-4 flex-col"
          validationBehavior="native"
          onSubmit={handleSubmit}
        >
          {error && <p className="text-red-500 text-center">{error}</p>}
          <Input 
            isRequired
            label="Email"
            name="email"
            type="email"
            variant="bordered"
            value={user.email}
            onChange={(e) => setUser((prev) => ({ ...prev, email: e.target.value }))}
              />
          <Input
            isRequired
            label="Password"
            name="password"
            type={isVisible ? 'text' : 'password'}
            variant="bordered"
            value={user.password}
            onChange={(e) => setUser((prev) => ({ ...prev, password: e.target.value }))}
            endContent={
              <button type="button" onClick={toggleVisibility}>
                <Icon
                  className="pointer-events-none text-2xl text-default-400"
                  icon={isVisible ? "solar:eye-closed-linear" : "solar:eye-bold"}
                />
              </button>
            }
          />
          <div className="flex w-full items-center justify-between px-1 py-2">
            <Checkbox name="remember" size="sm">
              <p>Remember me</p>
            </Checkbox>
            <Link className="text-default-500" href="/password-recover" size="sm">
              Forgot password?
            </Link>
          </div>
          <Button className="w-full" color="primary" type="submit">
            Log In
          </Button>
        </Form>
        <div className="flex items-center gap-4 py-2">
          <Divider className="flex-1" />
          <p className="shrink-0 text-tiny text-default-500">OR</p>
          <Divider className="flex-1" />
        </div>
        <div className="flex flex-col gap-2">
          <Link href="#">
            <Button className="w-full" startContent={<Icon icon="flat-color-icons:google" />} variant="bordered">
              Continue with Google
            </Button>
          </Link>
          <Button className="w-full" startContent={<Icon className="text-default-500" icon="fe:github" width={24} />} variant="bordered">
            Continue with Github
          </Button>
        </div>
        <p className="text-center text-small">
          Need to create an account?&nbsp;
          <Link href="/signup" size="sm">
            Sign Up
          </Link>
        </p>
      </div>
    </div>
  );
}
