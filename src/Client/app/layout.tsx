import { Providers } from './providers'


export default function RootLayout({ children }: { children: React.ReactNode }) {
  return (
    <html lang="en" className="">
      <body>
      <Providers>
        <main>{children}</main>
      </Providers>
      </body>
    </html>
  )
}
