"use client";

import {useTheme} from "@heroui/use-theme";

export const ThemeSwitcher = () => {
    const { theme, setTheme } = useTheme()

    return (
        <div>
            {theme}
            <button onClick={() => setTheme('light')}>Light Mode</button>
            <button onClick={() => setTheme('dark')}>Dark Mode</button>
        </div>
    )
};