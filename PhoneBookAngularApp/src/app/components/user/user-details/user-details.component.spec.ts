import { ComponentFixture, TestBed } from '@angular/core/testing';
import { UserDetailsComponent } from './user-details.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { FormsModule } from '@angular/forms';
import { RouterTestingModule } from '@angular/router/testing';
import { AuthService } from 'src/app/services/auth.service';
import { UserService } from 'src/app/services/user.service';
import { ActivatedRoute } from '@angular/router';
import { of } from 'rxjs';

describe('UserDetailsComponent', () => {
  let component: UserDetailsComponent;
  let fixture: ComponentFixture<UserDetailsComponent>;
  let authService: AuthService;
  let userService: UserService;
  let activatedRoute: ActivatedRoute;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HttpClientTestingModule, RouterTestingModule, FormsModule],
      declarations: [UserDetailsComponent],
      providers: [AuthService, UserService, { provide: ActivatedRoute, useValue: { params: of({ id: 'testId' }) } }]
    }).compileComponents();

    fixture = TestBed.createComponent(UserDetailsComponent);
    component = fixture.componentInstance;
    authService = TestBed.inject(AuthService);
    userService = TestBed.inject(UserService);
    activatedRoute = TestBed.inject(ActivatedRoute);

    spyOn(authService, 'isAuthenticated').and.returnValue(of(true)); // Mocking isAuthenticated method
    spyOn(authService, 'getUserName').and.returnValue(of('testUser')); // Mocking getUserName method
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize component properties', () => {
    fixture.detectChanges();
    expect(component.isAuthenticated).toBe(true);
    expect(component.username).toBe('testUser');
    expect(component.loginId).toBe('testId');
  });

  it('should load user details on initialization', () => {
    const mockUser = {
      userId: 1,
      firstName: 'first',
      lastName: 'last',
      loginId: 'testId',
      email: 'first@example.com',
      contactNumber: '1234567890'
    };

    spyOn(userService, 'getUserByLoginId').and.returnValue(of({ success: true, data: mockUser ,message:''}));

    fixture.detectChanges();

    expect(component.user).toEqual(mockUser);
  });
});
