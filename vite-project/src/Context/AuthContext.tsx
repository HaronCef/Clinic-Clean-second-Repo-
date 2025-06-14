import { createContext, useContext, useEffect, useState } from 'react';
import { LoginRequest } from '../Api/LoginRequest';
import type{ AuthContextType, LoginRequestProps, LoginResponse } from '../TypeModel/Types';
import { RegisterRequest } from '../Api/RegisterRequest';

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
    const [user, setUser] = useState<LoginResponse | null>(null);

    const login = async (data : LoginRequestProps) => {
            const response: LoginResponse = await LoginRequest(data);
            setUser(response);
        }
 
const registration = async (data: LoginRequestProps) => {
    const response: string = await RegisterRequest(data);
}

    const logout = () => {
        setUser(null);
    };

    return (
        <AuthContext.Provider value={{ user, login, logout, registration }}>
            {children}
        </AuthContext.Provider>
    );
};

export const useAuth = () => useContext(AuthContext)!;