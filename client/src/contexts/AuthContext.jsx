import { createContext, useContext, useState, useEffect } from "react";
import { login as apiLogin, register as apiRegister } from "../services/api/authService";
import { getCurrentUser } from "../services/api/userService";

const AuthContext = createContext();

export function AuthProvider({ children }) {
  const [user, setUser] = useState(null);

  useEffect(() => {
    const token = localStorage.getItem("token");

    if (token && !user) {
      getCurrentUser()
        .then(setUser)
        .catch(() => {
          localStorage.removeItem("token");
          setUser(null);
        });
    }
  }, [user]);

  const login = async (credentials) => {
    const data = await apiLogin(credentials);
    const { jwtToken } = data;

    localStorage.setItem("token", jwtToken);

    const me = await getCurrentUser();
    setUser(me);
  };

  const register = async (payload) => {
    await apiRegister(payload);
  };

  const logout = () => {
    localStorage.removeItem("token");
    setUser(null);
  };

  const isAuthenticated = !!user;

  return (
    <AuthContext.Provider value={{ user, login, logout, register, isAuthenticated }}>
      {children}
    </AuthContext.Provider>
  );
}

export const useAuth = () => useContext(AuthContext);
