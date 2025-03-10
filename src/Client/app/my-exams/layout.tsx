'use client'

import React from 'react';
import NavBar from '../navigation-header/NavBar'
interface LayoutProps {
  children: React.ReactNode;
}

export default function RootLayout({ children }: LayoutProps) {

  return (
    <>
      <NavBar/>
        <main>{children}</main>
    </>
  );
}