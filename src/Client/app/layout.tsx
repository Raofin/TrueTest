'use client'
import { Providers } from './providers'
import NavBar from './navigation-header/NavBar'
import { usePathname } from "next/navigation";
import '../styles/globals.css'
import React, { useState,useEffect } from 'react'; 

interface RootLayoutProps {
    children: React.ReactNode;
  }
  
  export default function RootLayout({ children }: RootLayoutProps) {
    const path = usePathname();
    const [theme, setTheme] = useState('light'); 
    useEffect(() => {
      const storedTheme = localStorage.getItem('theme');
      if (storedTheme) {
          setTheme(storedTheme);
          document.body.classList.toggle('dark', storedTheme === 'dark'); 
      }
  }, []); 
    const handleThemeToggle = (newTheme: string) => {
        setTheme(newTheme);
        document.body.classList.toggle('dark', newTheme === 'dark');
    };

    return (
        <html lang="en">
            <body className={theme === 'dark' ? 'dark' : ''}> 
                <Providers>
                    {!path.includes('login') && !path.includes('registration')
                        && !path.includes('settings') && !path.includes('myprofile') && !path.includes('exam-review') && <NavBar onThemeToggle={handleThemeToggle} />}
                    <main className="h-screen">{children}</main> 
                </Providers>
            </body>
        </html>
    )
}