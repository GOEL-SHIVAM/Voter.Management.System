export interface ApiResponse<T = any> {
  data?: T;
  message?: string;
  errors?: ValidationError[];
}

export interface ValidationError {
  property: string;
  error: string;
}

export interface ApiError {
  status: number;
  message: string;
  timestamp: Date;
}
