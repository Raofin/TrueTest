import { createContext } from "react";


const AuthContext = createContext(null);
export default AuthContext;

// const AuthProvider=({children})=>{
//     const createUser=()=>{
//         return (
//           <AuthContext.Provider value={createUser}>
//             {children}
//           </AuthContext.Provider>
//         )
//     }
// }