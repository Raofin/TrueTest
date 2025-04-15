import Cookies from 'js-cookie'
import CryptoJS from 'crypto-js';

const SECRET_KEY = process.env.NEXT_PUBLIC_SECRET_KEY; 
if (!SECRET_KEY) {
  throw new Error('An error occured with jwt token');
}

const encryptToken = (token: string): string => {
  return CryptoJS.AES.encrypt(token, SECRET_KEY).toString();
};

const decryptToken = (encryptedToken: string): string => {
  const bytes = CryptoJS.AES.decrypt(encryptedToken, SECRET_KEY);
  const decryptedData = bytes.toString(CryptoJS.enc.Utf8);
  return decryptedData;
};

export function setAuthToken(token: string,rememberMe:boolean) {
  const encryptedToken = encryptToken(token);
if(rememberMe){
  Cookies.set('token', encryptedToken, {
    secure: true, 
    sameSite: 'Strict', 
    expires: 30,
    path: '/',
  })
}
else{
  Cookies.set('token', encryptedToken, {
    secure: true, 
    sameSite: 'Strict', 
    path: '/',
  })
}
}
export function getAuthToken(): string | undefined {
  const encryptedToken = Cookies.get('token');
  return encryptedToken ? decryptToken(encryptedToken) : undefined;
}

export function removeAuthToken() {
  Cookies.remove('token');
}
