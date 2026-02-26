export interface Admin {
  adminId: number;
  username: string;
  role: string;
}

export interface CreateAdminRequest {
  username: string;
  password: string;
}
