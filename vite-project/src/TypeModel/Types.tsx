export type LoginRequestProps = {
    email: string;
    password: string;
}

export type LoginResponse={
    id: string;
    name: string;
    role: "user" | "admin" | "manager";
}

export type AuthContextType = {
    user: LoginResponse | null;
    login: (data: LoginRequestProps) => Promise<void>;
    logout: () => void;
    registration: (data: LoginRequestProps) => Promise<void>;
}

export type Doctor = {
    id: string;
    name: string;
    description: string;
    specialization: string;
}

export type PersonForm = {
  name: string;
  specialization?: string;
};