'use client';

import { IoMoon, IoSunnySharp } from 'react-icons/io5';
import { useTheme } from 'next-themes';
import React from 'react';

interface ThemeSwitchProps {
  readonly withText?: boolean;
}

export default function ThemeSwitch({ withText }: ThemeSwitchProps) {
  const { setTheme, resolvedTheme } = useTheme();

  const iconStyle = {
    cursor: 'pointer',
    fontSize: '1.5rem', 
  };

  if (resolvedTheme === 'dark') {
    return (
      <button
        onClick={() => setTheme('light')}
        style={{ display: 'flex', alignItems: 'center', gap: '0.5rem', background: 'none', border: 'none', padding: 0, cursor: 'pointer' }}
      >
        <IoSunnySharp style={iconStyle} />
        {withText && <span className='ml-1'>Light</span>}
      </button>
    );
  }

  return (
    <button
      onClick={() => setTheme('dark')}
      style={{ display: 'flex', alignItems: 'center', gap: '0.5rem', background: 'none', border: 'none', padding: 0, cursor: 'pointer' }}
    >
      <IoMoon style={iconStyle} />
      {withText && <span className='ml-1'>Dark</span>}
    </button>
  );
}