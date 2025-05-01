import React from "react";
import { Button } from "@heroui/react";
import { Icon } from "@iconify/react";

interface AIGenerateButtonProps {
  isGenerating: boolean;
  onGenerate: () => void;
  size?: "sm" | "md" | "lg";
  variant?: "solid" | "bordered" | "light" | "flat" | "faded" | "shadow" | "ghost";
}
export const AIGenerateButton: React.FC<AIGenerateButtonProps> = ({
  isGenerating,
  onGenerate,
  size = "md",
  variant = "solid"
}) => {
  return (
      <Button
        color="primary"
        variant={variant}
        size={size}
        onPress={onGenerate}
        isLoading={isGenerating}
        startContent={
          !isGenerating && (
            <Icon
              icon="lucide:sparkles"
              className="text-lg"
            />
          )
        }
        className="min-w-[160px] font-medium"
      >
        { isGenerating ? "Generating..." : "Generate With AI"}
      </Button>
    
  );
};