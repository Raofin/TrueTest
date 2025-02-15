import { useRouter } from "next/navigation";
import { Button } from "@heroui/react";

interface Props {
    dashboardType: string;
}

export default function GoTo({ dashboardType }: Props) {
    const router = useRouter();

    return (
        <Button className={"bg-primary text-white"} onPress={() => router.push(`/${dashboardType}_dashboard`)}>
            Go to {dashboardType} page
        </Button>
    );
}
