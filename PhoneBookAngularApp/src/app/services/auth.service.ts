import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { LocalstorageService } from './helpers/localstorage.service';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { ApiResponse } from '../models/ApiResponse{T}';
import { LocalStorageKeys } from './helpers/localstoragekeys';
import { ChnagePass } from '../models/changepassword.model';
import { ForgotPass } from '../models/forgotpassword.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private apiUrl = 'http://localhost:5116/api/Auth/';

  private authState = new BehaviorSubject<boolean>(this.localStorageHelper.hasItem(LocalStorageKeys.TokenName));

  private usernameSubject = new BehaviorSubject<string|null|undefined>(this.localStorageHelper.getItem(LocalStorageKeys.UserId));

  constructor(private http:HttpClient,private localStorageHelper:LocalstorageService) { }

  signUp(user:any):Observable<any>{
    const body = user;
    return this.http.post<any>(this.apiUrl+'Register',body);
  }

  signIn(username:string,password:string):Observable<ApiResponse<string>>{
    const body={username,password};
    return this.http.post<ApiResponse<string>>(this.apiUrl+"Login",body).pipe(
      tap(response=>{
        if (response.success) {
          this.localStorageHelper.setIitem(LocalStorageKeys.TokenName,response.data);
          this.localStorageHelper.setIitem(LocalStorageKeys.UserId,username);
          this.authState.next(this.localStorageHelper.hasItem(LocalStorageKeys.TokenName));
          this.usernameSubject.next(username);
        }
      })
    );
  }

  signOut() {
    this.localStorageHelper.removeItem(LocalStorageKeys.TokenName);
    this.localStorageHelper.removeItem(LocalStorageKeys.UserId);
    this.authState.next(false);
    this.usernameSubject.next(null);

  }

  isAuthenticated(){
    return this.authState.asObservable();
  }

  getUserName():Observable<string|null|undefined>{
    return this.usernameSubject.asObservable();
  }

  UpdateUserDetails(username:string):void{
    this.localStorageHelper.setIitem(LocalStorageKeys.UserId,username);
    this.usernameSubject.next(username);

  }

  changePassword(user:any):Observable<ApiResponse<ChnagePass>>{
    const body = user;
    return this.http.put<ApiResponse<ChnagePass>>(this.apiUrl+"ChangePassword",body);
  }

  forgotPassword(user:any):Observable<ApiResponse<ForgotPass>>{
    const body = user;
    return this.http.put<ApiResponse<ForgotPass>>(this.apiUrl+"ForgotPassword",body);
  }

}
