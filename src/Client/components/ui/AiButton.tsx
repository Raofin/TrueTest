import { Button } from "@heroui/react";

export const AiButton = ({ onPress }: { onPress: () => void }) => {
    return (
        <Button onPress={onPress} className="flex items-center py-2 px-3 rounded-full bg-gradient-to-r from-cyan-400 to-purple-500 text-white font-semibold shadow-md hover:opacity-90 transition">
  <svg className="w-5 h-5 text-white" fill="currentColor" viewBox="0 0 20 20">
    <path d="M10 0l1.76 5.26L17 6.18l-4 3.89L14.52 16 10 12.98 5.48 16 7 10.07l-4-3.89 5.24-.92L10 0z" />
  </svg>
            Use AI
        </Button>
    );
};
