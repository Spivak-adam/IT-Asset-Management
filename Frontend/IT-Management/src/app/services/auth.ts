import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UserRole } from '../models/it-asset.models';

export interface RegisterDto {
  email: string;
  password: string;
  role: UserRole;
}

export interface LoginDto {
  email: string;
  password: string;
}

export interface LoginResponseDto {
  token: string;
  email: string;
  role: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'http://localhost:5058/api/ITAsset';

  constructor(private http: HttpClient) {}

  login(loginDto: LoginDto) {
    return this.http.post<LoginResponseDto>(`${this.apiUrl}/auth/login`, loginDto);
  }

  register(registerDto: RegisterDto) {
    return this.http.post(`${this.apiUrl}/auth/register`, registerDto);
  }

  saveSession(response: LoginResponseDto) {
    localStorage.setItem('token', response.token);
    localStorage.setItem('role', response.role);
    localStorage.setItem('email', response.email);
  }

  logout() {
    localStorage.clear();
  }

  getToken() {
    return localStorage.getItem('token');
  }

  getRole() {
    return localStorage.getItem('role');
  }

  isLoggedIn() {
    return !!this.getToken();
  }
}