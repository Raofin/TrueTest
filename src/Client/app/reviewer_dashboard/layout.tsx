import NavigationHeader from "./navigation-header/page";
import '../../styles/globals.css'
export default function Layout({ children }: { children: React.ReactNode }) {
    return (
        <div>
            <NavigationHeader/>
            <main>{children}</main>
        </div>
    );
}
