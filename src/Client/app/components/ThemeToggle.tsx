import React from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faSun, faMoon } from "@fortawesome/free-solid-svg-icons";
import { Button } from "@nextui-org/react";
import { useTheme } from "../ThemeProvider";

const ThemeToggle: React.FC = () => {
  const { isDarkMode, toggleTheme } = useTheme();

  return (
    <Button isIconOnly radius="full" variant="light" onPress={toggleTheme} className="">
      <FontAwesomeIcon icon={isDarkMode ? faSun : faMoon} width={24} />
    </Button>
  );
};

export default ThemeToggle;
