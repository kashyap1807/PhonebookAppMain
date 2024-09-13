import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiResponse } from '../models/ApiResponse{T}';
import { User } from '../models/user.model';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private apiUrl = 'http://localhost:5116/api/User/';
  constructor(private http:HttpClient) { }

  getUserByLoginId(loginId:string|null|undefined):Observable<ApiResponse<User>>{
    return this.http.get<ApiResponse<User>>(this.apiUrl+"GetUserByLoginId/"+loginId);
  }

  UpdateUser(updateUser:User):Observable<ApiResponse<string>>{
    return this.http.put<ApiResponse<string>>(this.apiUrl+"UpdateUser",updateUser);
  }
}
