import { TestBed } from '@angular/core/testing';

import { AuthService } from './auth.service';
import { HttpClientModule } from '@angular/common/http';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { FormsModule } from '@angular/forms';
import { RouterTestingModule } from '@angular/router/testing';
import { User } from '../models/user.model';
import { ApiResponse } from '../models/ApiResponse{T}';
import { ForgotPass } from '../models/forgotpassword.model';

describe('AuthService', () => {
  let service: AuthService;
  let httpMock:HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports : [HttpClientTestingModule,RouterTestingModule,FormsModule],
      
      providers: [HttpClientModule]
    });
    service = TestBed.inject(AuthService);
    httpMock=TestBed.inject(HttpTestingController);
  });
  afterEach(()=>{
    httpMock.verify();
    
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  // Register User
  it('should register user successfully',()=>{
    //arrange
    const registerUser:User={
      "userId": 1,
      "firstName": "string",
      "lastName": "string",
      "loginId": "string",
      "email": "user@example.com",
      "contactNumber": "330407 1959",
      
    };
    const mockSuccessResponse:ApiResponse<string>={
      success:true,
      message:"User register successfully.",
      data:""
    }
    //act
    service.signUp(registerUser).subscribe(response=>{
      //assert
      expect(response).toBe(mockSuccessResponse);
    });
    const req=httpMock.expectOne('http://localhost:5116/api/Auth/Register');
    expect(req.request.method).toBe('POST');
    req.flush(mockSuccessResponse);

  });
  it('should handle failed user register',()=>{
    //arrange
    const registerUser:User={
      "userId": 0,
      "firstName": "string",
      "lastName": "string",
      "loginId": "string",
      "email": "user@example.com",
      "contactNumber": "330407 1959",
      
    };
    const mockErrorResponse:ApiResponse<string>={
      success:true,
      message:"User already exists.",
      data:""
    }
    //act
    service.signUp(registerUser).subscribe(response=>{
      //assert
      expect(response).toBe(mockErrorResponse);
    });
    const req=httpMock.expectOne('http://localhost:5116/api/Auth/Register');
    expect(req.request.method).toBe('POST');
    req.flush(mockErrorResponse);

  });
  it('should handle Http error while register user',()=>{
    //arrange
    const registerUser:User={
      "userId": 0,
      "firstName": "string",
      "lastName": "string",
      "loginId": "string",
      "email": "user@example.com",
      "contactNumber": "330407 1959",
      
    };
    const mockHttpError={
      statusText:"Internal Server Error",
      status:500
    }
    //act
    service.signUp(registerUser).subscribe({
      next:()=>fail('should have failed with the 500 error'),
      error:(error)=>{
        //assert
      expect(error.status).toEqual(500);
      expect(error.statusText).toEqual('Internal Server Error');
      }
    });
    const req=httpMock.expectOne('http://localhost:5116/api/Auth/Register');
    expect(req.request.method).toBe('POST');
    req.flush({},mockHttpError);

  });
  

});
