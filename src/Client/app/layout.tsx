import '@/styles/globals.css'
import { Providers } from './providers'
import Navigation from './navigation-header/page'
export default function RootLayout({ children }: { children: React.ReactNode }) {
  return (
    <html lang="en" className="">
      <body>
      <Providers>
      <Navigation/>
        <main>{children}</main>
      </Providers>
      </body>
    </html>
  )
}
