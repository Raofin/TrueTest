'use client'
import { Providers } from './providers'
import NavBar from './navigation-header/page'
import {usePathname} from "next/navigation";
import '../styles/globals.css'

export default function RootLayout({ children }: { children: React.ReactNode }) {
 const path=usePathname();

  return (
    <html lang="en">
      <body >
      <Providers>
          {!path.includes('login') && !path.includes('registration')
              && !path.includes('settings') && !path.includes('myprofile') && <NavBar />}
          <main>{children}</main>
      </Providers>
      </body>
    </html>
  )
}
