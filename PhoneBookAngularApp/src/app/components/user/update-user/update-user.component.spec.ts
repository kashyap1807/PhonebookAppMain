import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { RouterTestingModule } from '@angular/router/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';

import { UpdateUserComponent } from './update-user.component';
import { AuthService } from 'src/app/services/auth.service';
import { UserService } from 'src/app/services/user.service';
import { ActivatedRoute, Router } from '@angular/router';
import { of, throwError } from 'rxjs';
import { User } from 'src/app/models/user.model';

describe('UpdateUserComponent', () => {
  let component: UpdateUserComponent;
  let fixture: ComponentFixture<UpdateUserComponent>;
  let authService: AuthService;
  let userService: UserService;
  let router: Router;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule, RouterTestingModule, FormsModule],
      declarations: [UpdateUserComponent],
      providers: [
        AuthService,
        UserService,
        {
          provide: ActivatedRoute,
          useValue: {
            params: of({ id: 'testLoginId' })
          }
        }
      ]
    });

    fixture = TestBed.createComponent(UpdateUserComponent);
    component = fixture.componentInstance;
    authService = TestBed.inject(AuthService);
    userService = TestBed.inject(UserService);
    router = TestBed.inject(Router);
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  // Additional tests go here
  it('should load user details on initialization', () => {
    const mockUser: User = {
      userId: 1,
      firstName: 'firstname',
      lastName: 'lastName',
      loginId: 'testLoginId',
      email: 'first@example.com',
      contactNumber: '1234567890'
    };

    spyOn(authService, 'isAuthenticated').and.returnValue(of(true));
    spyOn(authService, 'getUserName').and.returnValue(of('testUser'));
    spyOn(userService, 'getUserByLoginId').and.returnValue(of({ success: true, data: mockUser ,message:''}));

    component.ngOnInit();

    expect(component.isAuthenticated).toBe(true);
    expect(component.username).toBe('testUser');
    expect(userService.getUserByLoginId).toHaveBeenCalledWith('testLoginId');
    expect(component.user).toEqual(mockUser);
  });

  it('should navigate to user details on successful form submission', fakeAsync(() => {
    const navigateSpy = spyOn(router, 'navigate');
    spyOn(userService, 'UpdateUser').and.returnValue(of({ success: true ,message:'',data:''}));
    spyOn(authService, 'UpdateUserDetails');

    component.onSubmit({ valid: true } as any);

    expect(component.loading).toBe(false);
    tick(); // Simulate asynchronous operation completion

    expect(authService.UpdateUserDetails).toHaveBeenCalled();
    expect(navigateSpy).toHaveBeenCalledWith(['/userdetails', component.username]);
    expect(component.loading).toBe(false);
  }));

  it('should handle form submission when form is invalid', () => {
    spyOn(userService, 'UpdateUser'); // Ensure method is not called when form is invalid
    component.onSubmit({ valid: false } as any);
    expect(userService.UpdateUser).not.toHaveBeenCalled();
  });
  
  it('should handle error on form submission', fakeAsync(() => {
    spyOn(userService, 'UpdateUser').and.returnValue(
      throwError({ error: { message: 'Update failed' } })
    );
  
    spyOn(console, 'log'); // Example of testing error handling
  
    component.onSubmit({ valid: true } as any);
  
    expect(component.loading).toBe(false);
    tick();
  
    expect(console.log).toHaveBeenCalledWith('Update failed');
    expect(component.loading);
  }));

});

