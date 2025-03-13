'use client';
import { Providers } from './providers';
import '../styles/globals.css';
import React, { useState, useEffect } from 'react';
import { ThemeProvider } from './ThemeProvider';

export default function RootLayout({ children }: { children: React.ReactNode }) {

    return (
        <html lang="en">
            <body >
                <Providers>
                    <ThemeProvider><main>{children}</main></ThemeProvider>
                </Providers>
            </body>
        </html>
    );
}