// "use client";
// import React, { ReactNode, useState, createContext, useContext, useEffect } from "react";
// import Cookies from "js-cookie";
// import axios from "axios";

// interface AuthContextType {
//   user: string | null;
//   setUser: React.Dispatch<React.SetStateAction<string | null>>;
//   logout: () => void;
// }

// const AuthContext = createContext<AuthContextType | undefined>(undefined);

// export const AuthProvider = ({ children }: { children: ReactNode }) => {
//   const [user, setUser] = useState<string | null>(Cookies.get("userEmail") || null);

//   useEffect(() => {
//     axios
//       .get(`${process.env.NEXT_PUBLIC_URL}/User/UserInfo`, { withCredentials: true })
//       .then((response) => {
//         setUser(response.data.email);
//       })
//       .catch(() => {
//         setUser(null);
//       });
//   }, []);

//   const logout = () => {
//     setUser(null);
//   };

//   return (
//     <AuthContext.Provider value={{ user, setUser, logout }}>
//       {children}
//     </AuthContext.Provider>
//   );
// };


// export const useAuth = () => {
//   const context = useContext(AuthContext);
//   if (!context) {
//     throw new Error("useAuth must be used within an AuthProvider");
//   }
//   return context;
// };
