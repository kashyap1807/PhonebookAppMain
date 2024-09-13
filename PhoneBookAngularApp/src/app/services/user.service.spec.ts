import { TestBed } from '@angular/core/testing';

import { UserService } from './user.service';
import { HttpClientModule } from '@angular/common/http';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { FormsModule } from '@angular/forms';
import { RouterTestingModule } from '@angular/router/testing';
import { User } from '../models/user.model';
import { ApiResponse } from '../models/ApiResponse{T}';

describe('UserService', () => {
  let service: UserService;
  let httpMock: HttpTestingController;
  beforeEach(() => {
    TestBed.configureTestingModule({
      imports : [HttpClientTestingModule,RouterTestingModule,FormsModule],
      
      providers: [UserService]
    });
    service = TestBed.inject(UserService);
    httpMock = TestBed.inject(HttpTestingController);
  });
  afterEach(() => {
    // After each test, verify there are no outstanding HTTP requests
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should retrieve user by login ID', () => {
    const loginId = 'testLoginId';
    const mockUser: User = { userId: 1, firstName: 'firstName',lastName:'lastName' ,loginId:'1',contactNumber:'1234567890',email: 'test@example.com' };

    service.getUserByLoginId(loginId).subscribe((response: ApiResponse<User>) => {
      expect(response.data).toEqual(mockUser); // Check if the response matches the mockUser
    });

    const req = httpMock.expectOne(`http://localhost:5116/api/User/GetUserByLoginId/${loginId}`);
    expect(req.request.method).toBe('GET');

    // Respond with mockUser
    req.flush({ data: mockUser });

  });
  it('should update user', () => {
    const updateUser: User = { userId: 1, firstName: 'firstName',lastName:'lastName' ,loginId:'1',contactNumber:'1234567890',email: 'test@example.com' };
    const mockResponse: ApiResponse<string> = {success:true,message:'' ,data: 'User updated successfully' };

    service.UpdateUser(updateUser).subscribe((response: ApiResponse<string>) => {
      expect(response.data).toEqual('User updated successfully'); // Check if the response message is correct
    });

    const req = httpMock.expectOne(`http://localhost:5116/api/User/UpdateUser`);
    expect(req.request.method).toBe('PUT');
    expect(req.request.body).toEqual(updateUser); // Check if the request body matches updateUser

    req.flush(mockResponse);
  });
});
