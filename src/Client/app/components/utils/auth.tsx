import Cookies from 'js-cookie'

export function setAuthToken(token: string) {
  Cookies.set('token', token, {
    secure: true,
    httpOnly:true,
    sameSite: 'Strict',
    path: '/',
  })
}

export function getAuthToken(): string | undefined {
  return Cookies.get('token')
}

export function removeAuthToken() {
  Cookies.remove('token')
}
