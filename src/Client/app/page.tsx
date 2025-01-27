import { Button } from '@heroui/button'
import Link from 'next/link'

export default function Home() {
  return (
    <main>
      <Link href="/login">
        <Button>Login</Button>
      </Link>
    </main>
  )
}
