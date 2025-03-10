'use client'
import '../../styles/globals.css'
import React, { useState, useEffect } from 'react';
import NavBar from '../navigation-header/NavBar'

const applyTheme = (theme: string) => {
    if (theme === 'dark') {
        document.documentElement.classList.add('dark');
        document.documentElement.classList.remove('light');
    } else {
        document.documentElement.classList.add('light');
        document.documentElement.classList.remove('dark');
    }
    localStorage.setItem('theme', theme);
};

export default function RootLayout({ children }: { children: React.ReactNode }) {
    const [theme, setTheme] = useState('dark');
    useEffect(() => {
        const storedTheme = localStorage.getItem('theme');
        const initialTheme = storedTheme || 'dark'; 
        setTheme(initialTheme);
        applyTheme(initialTheme);
    },);

    const handleThemeToggle = (newTheme: string) => {
        setTheme(newTheme);
        applyTheme(newTheme);
    };
    return (
        <div >
            <NavBar onThemeToggle={handleThemeToggle} />
            <main >{children}</main>
        </div>
    );
}
