'use client'

import React, { useCallback, useState } from 'react';
import { Button, Input, Checkbox, Link, Form, Divider } from '@heroui/react';
import { Icon } from '@iconify/react';
import axios from 'axios';
import '../../styles/globals.css';
import { useRouter } from 'next/navigation';

export default function LoginComponent() {
  const [isVisible, setIsVisible] = useState(false);
  const [user, setUser] = useState({ email: '', password: '' });
  const [error, setError] = useState('');
  const router = useRouter();

  const toggleVisibility = useCallback(() => setIsVisible(prev => !prev), []);

  const handleSubmit = useCallback(async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    try {
      const response = await axios.post('https://localhost:9998/api/Auth/Login', {
        Email: user.email,
        Password: user.password,
      });
      if (response.data && response.data.token) {
    
        localStorage.setItem('authToken', response.data.token);
        router.push('/admin_dashboard');
      }
      else {
        setError('Invalid credentials');
      }
    } catch (error) {
      setError('An error occurred during login');
      console.error(error);
    }
  },
  [user, router]
  );
  
  return (
      <div className="flex h-full w-full items-center justify-center">
        <div className="flex w-full max-w-sm flex-col gap-4 rounded-large bg-content1 px-8 pb-10 pt-6 shadow-small">
          <div className="flex flex-col gap-1">
            <h1 className="text-large font-medium">Sign in to your account</h1>
          </div>
          <Form className="flex w-full flex-wrap md:flex-nowrap gap-4 flex-col" validationBehavior="native" onSubmit={handleSubmit}>
            {error && <p className="text-red-500 text-center">{error}</p>}

            <Input
                isRequired
                label="Email"
                name="email"
                type="email"
                variant="bordered"
                value={user.email}
                onChange={(e) => setUser(prev => ({ ...prev, email: e.target.value }))}
            />
            <Input
                isRequired
                label="Password"
                name="password"
                type={isVisible ? 'text' : 'password'}
                variant="bordered"
                value={user.password}
                onChange={(e) => setUser(prev => ({ ...prev, password: e.target.value }))}
                endContent={
                  <button type="button" onClick={toggleVisibility}>
                    {isVisible ? (
                        <Icon className="pointer-events-none text-2xl text-default-400" icon="solar:eye-closed-linear" />
                    ) : (
                        <Icon className="pointer-events-none text-2xl text-default-400" icon="solar:eye-bold" />
                    )}
                  </button>
                }
            />
            <div className="flex w-full items-center justify-between px-1 py-2">
              <Checkbox name="remember" size="sm">
                Remember me
              </Checkbox>
              <Link className="text-default-500" href="#" size="sm">
                Forgot password?
              </Link>
            </div>
            <Button className="w-full" color="primary" type="submit">
              Sign In
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
            <Button startContent={<Icon className="text-default-500" icon="fe:github" width={24} />} variant="bordered">
              Continue with Github
            </Button>
          </div>
          <p className="text-center text-small">
            Need to create an account?&nbsp;
            <Link href="/registration" size="sm">
              Sign Up
            </Link>
          </p>
        </div>
      </div>
  );
}
