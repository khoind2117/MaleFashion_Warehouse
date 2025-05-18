export interface RegisterData {
    firstName: string;
    lastName: string;
    email: string;
    phoneNumber:string; 
    address: string;
    password: string;
}

export interface LoginData {
    userName: string;
    password: string;
}

export interface AuthResponseDto {
    userId: string;
    userName: string;
    email: string;
    roles: string[];
    accessToken: string;
}

export interface UserData {
    userId: string;
    userName: string;
    email: string;
    roles: string[];
}