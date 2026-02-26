export interface LoginRequest {
  username: string;
  password: string;
}

export interface LoginResponse {
  token: string;
  username: string;
  role: string;
  expiresAt: Date;
}

export interface VoterRegisterRequest {
  aadhar: string;
  password: string;
  name: string;
  birthDate: Date;
}

export interface ChangePasswordRequest {
  newPassword: string;
}
