import Cookies from 'js-cookie'

export default function setAuthToken(token: string) {
  Cookies.set('token', token, {
    httpOnly: true,
    sameSite: 'strict',
    path: '/',
  })
}

export function getAuthToken(): string | undefined {
  return Cookies.get('token')
}

export function removeAuthToken() {
  Cookies.remove('token')
}
