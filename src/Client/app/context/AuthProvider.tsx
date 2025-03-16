'use client'

import React, { createContext, useState, useContext } from 'react';
interface User {
    username: string;
    email: string;
    accountId:string
  }
interface AuthContextType {
    auth: { authToken?: string };
    setAuth: React.Dispatch<React.SetStateAction<{ authToken?: string }>>;
    user: User | null; 
    setUser: React.Dispatch<React.SetStateAction<User | null>>; 
    handleLogout: () => void;
  }

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider = ({ children }: { children: React.ReactNode }) => {
  const [auth, setAuth] = useState<{ authToken?: string }>({});
  const [user, setUser] = useState<User | null>(null);
  const handleLogout = () => {
    setAuth({ authToken: undefined });
    setUser(null); 
  };

  return (
    <AuthContext.Provider value={{ user, setUser,auth,setAuth,handleLogout }}>
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

export default AuthContext;