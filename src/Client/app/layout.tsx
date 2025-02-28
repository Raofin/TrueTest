'use client'
import { Providers } from './providers'
import NavBar from './navigation-header/page'
import { usePathname } from "next/navigation";
import '../styles/globals.css'
import React, { useState,useEffect } from 'react'; 

export default function RootLayout({ children }: { children: React.ReactNode }) {
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
                        && !path.includes('settings') && !path.includes('myprofile') && !path.includes('otp') && !path.includes('exam-review') && <NavBar onThemeToggle={handleThemeToggle} />} {/* Pass callback to NavBar */}
                    <main className="h-screen">{children}</main> 
                </Providers>
            </body>
        </html>
    )
}