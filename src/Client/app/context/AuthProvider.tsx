"use client";
import React, { ReactNode, useState, createContext, useContext, useEffect } from "react";
import Cookies from "js-cookie";

interface AuthContextType {
  user: string | null;
  setUser: React.Dispatch<React.SetStateAction<string | null>>;
  logout: () => void;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

interface AuthProviderProps {
  children: ReactNode;
  initialUser: string | null; 
}

export const AuthProvider = ({ children, initialUser }: AuthProviderProps) => {
  const [user, setUser] = useState<string | null>(initialUser || null); 

  useEffect(() => {
    const authToken = Cookies.get("authToken");

    if (authToken && !user) {
      const storedUser = Cookies.get("userEmail");
      if (storedUser) {
        setUser(storedUser);
      }
    }
  }, [user]);

  const logout = () => {
    Cookies.remove("authToken", { path: "/" });
    Cookies.remove("userEmail", { path: "/" }); 
    setUser(null);
  };

  return (
    <AuthContext.Provider value={{ user, setUser, logout }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error("useAuth must be used within an AuthProvider");
  }
  return context;
};