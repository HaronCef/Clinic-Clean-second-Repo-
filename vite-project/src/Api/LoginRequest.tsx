import axios from "axios";
import type { LoginRequestProps, LoginResponse } from "../TypeModel/Types";


export const LoginRequest = async (user: LoginRequestProps): Promise<LoginResponse> => {
try{
    const response = await axios.post<LoginResponse>('https://localhost:7120/Auth/Login', user);
    const data = response.data;
    return data;
}
catch (error) {
console.error("Failed to login:", error);
throw new Error("Login failed. Please check your credentials.");
}
};