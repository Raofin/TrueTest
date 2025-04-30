import { Spinner,Button } from "@heroui/react";

export const AiButton = ({onPress, loading = false,}: { onPress: () => void; loading?: boolean;}) => {
  return (
    <Button
      onPress={onPress}
      className="flex items-center gap-2 py-2 px-3 rounded-full bg-gradient-to-r from-cyan-400 to-purple-500 text-white font-semibold shadow-md hover:opacity-90 transition"
      isDisabled={loading}
    >
      {loading ? (
        <Spinner
          classNames={{ label: "text-white text-sm" }}
          variant="wave"
        />
      ) : (
        <>
          <svg className="w-5 h-5 text-white" fill="currentColor" viewBox="0 0 20 20">
            <path d="M10 0l1.76 5.26L17 6.18l-4 3.89L14.52 16 10 12.98 5.48 16 7 10.07l-4-3.89 5.24-.92L10 0z" />
          </svg>
          Generate With AI
        </>
      )}
    </Button>
  );
};
