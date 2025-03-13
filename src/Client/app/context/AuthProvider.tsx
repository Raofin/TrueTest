'use client'
import axios from 'axios';
import React, { createContext, useState, useContext, useEffect } from 'react';
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
  useEffect(() => {
    const fetchUserData = async () => {
      if (auth.authToken) {
        try {
          const response = await axios.get(`${process.env.NEXT_PUBLIC_URL}/User/UserInfo`, {
            headers: {
              Authorization: `Bearer ${auth.authToken}`,
            },
          });
          if (response.status === 200) {
            console.log(response.data)
            setUser(response.data);
          } else {
            console.error("Failed to fetch user data");
          }
        } catch (error) {
          console.error("Error fetching user data:", error);
        }
      }
    };

    fetchUserData();
  }, [auth.authToken]); 

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