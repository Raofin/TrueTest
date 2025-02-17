import '../../styles/globals.css'
import Navbar from './navigation-header/page'
export default function Layout({ children }: { children: React.ReactNode }) {
    return (
        <div className="">
            <Navbar/>
            <main className="ml-8">{children}</main>
        </div>
    );
}
