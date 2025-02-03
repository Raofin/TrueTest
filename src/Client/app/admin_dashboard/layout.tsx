import '../../styles/globals.css'
import Sidebar from './sidebar/page'
export default function Layout({ children }: { children: React.ReactNode }) {
    return (
        <div className="flex">
            <Sidebar/>
            <main className="ml-8">{children}</main>
        </div>
    );
}
