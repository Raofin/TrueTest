import React from "react";
import { Button } from "@heroui/react";
import { Icon } from "@iconify/react";

interface AIGenerateButtonProps {
  isGenerating: boolean;
  isReveiwing:boolean
  onGenerate: () => void;
  size?: "sm" | "md" | "lg";
  variant?: "solid" | "bordered" | "light" | "flat" | "faded" | "shadow" | "ghost";
}

export const AIGenerateButton: React.FC<AIGenerateButtonProps> = ({
  isGenerating,
  onGenerate,
  isReveiwing,
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
        {isReveiwing ? 
        <>{ isGenerating ? "Generating..." : "Generate With AI"}</>:
        <>{ isGenerating ? "Reviewing..." : "Review With AI"}</>
        }
      </Button>
    
  );
};