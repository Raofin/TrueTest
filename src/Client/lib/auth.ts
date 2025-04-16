import Cookies from "js-cookie";
import CryptoJS from "crypto-js";

const SECRET_KEY = process.env.NEXT_PUBLIC_SECRET_KEY;
if (!SECRET_KEY) {
  throw new Error("NEXT_PUBLIC_SECRET_KEY is not defined");
}

const encryptToken = (token: string): string => {
  return CryptoJS.AES.encrypt(token, SECRET_KEY).toString();
};

const decryptToken = (encryptedToken: string): string | null => {
  try {
    const bytes = CryptoJS.AES.decrypt(encryptedToken, SECRET_KEY);
    const decryptedData = bytes.toString(CryptoJS.enc.Utf8);
    return decryptedData || null;
  } catch (error) {
    console.error("Failed to decrypt token:", error);
    return null;
  }
};
export function setAuthToken(token: string, rememberMe: boolean) {
  const encryptedToken = encryptToken(token);
  if (rememberMe) {
    Cookies.set("authToken", encryptedToken, {
      secure: true,
      sameSite: "Strict",
      expires: 30,
      path: "/",
    });
    Cookies.set("authTokenExpiry", String(Date.now() + 30 * 24 * 60 * 60 * 1000), {
      secure: true,
      sameSite: "Strict",
      expires: 30,
      path: "/",
    });
  } else {
    Cookies.set("authToken", encryptedToken, {
      secure: true,
      sameSite: "Strict",
      path: "/",
    });
  }
}

export function getAuthToken(): string | null {
  const encryptedToken = Cookies.get("authToken");
  const expiry = Cookies.get("authTokenExpiry");
  if (encryptedToken && expiry) {
    if (Date.now() < Number(expiry)) {
      const decrypted = decryptToken(encryptedToken);
      if (decrypted) return decrypted;
    }
    removeAuthToken();
    return null;
  }
  const sessionEncryptedToken = Cookies.get("authToken");
  if (sessionEncryptedToken) {
    return decryptToken(sessionEncryptedToken);
  }
  return null;
}

export function removeAuthToken() {
  Cookies.remove("authToken");
  Cookies.remove("authTokenExpiry");
  Cookies.remove("token");
  Cookies.remove("Month-token");
}
