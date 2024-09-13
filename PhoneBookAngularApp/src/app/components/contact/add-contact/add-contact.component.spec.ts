import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddContactComponent } from './add-contact.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { FormsModule, NgForm } from '@angular/forms';
import { RouterTestingModule } from '@angular/router/testing';
import { ContactService } from 'src/app/services/contact.service';
import { Router } from '@angular/router';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { of, throwError } from 'rxjs';

describe('AddContactComponent', () => {
  let component: AddContactComponent;
  let fixture: ComponentFixture<AddContactComponent>;
  let contactServiceSpy: jasmine.SpyObj<ContactService>;
  let routerSpy: Router

  beforeEach(() => {
    contactServiceSpy = jasmine.createSpyObj('ContactService', ['AddContact']);
    routerSpy = jasmine.createSpyObj('Router', ['navigate']);
    TestBed.configureTestingModule({
      imports : [HttpClientTestingModule,RouterTestingModule,FormsModule],
      declarations: [AddContactComponent],
      providers: [
        { provide: ContactService, useValue: contactServiceSpy },
        // { provide: Router, useValue: routerSpy }
      ]
    });
    fixture = TestBed.createComponent(AddContactComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
    routerSpy = TestBed.inject(Router);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should navigate to /contact on successful contact addition', () => {
    spyOn(routerSpy, 'navigate');
    const mockResponse: ApiResponse<string> = { success: true, data: '', message: '' };
    contactServiceSpy.AddContact.and.returnValue(of(mockResponse));

    const form = <NgForm><unknown>{
      valid: true,
      value: {
        contactId: 1,
        FirstName: 'Test Description'
      },
      controls: {
        contactId: { value: 1 },
        FirstName: { value: 'Test Description' }
      }
    };

    // Act
    component.onSubmit(form);

    // Assert
    expect(routerSpy.navigate).toHaveBeenCalledWith(['/contact']);
    expect(component.loading).toBe(false);
  });
  it('should alert error message on unsuccessful contact addition', () => {
    // Arrange
    const mockResponse: ApiResponse<string> = { success: false, data: '', message: 'Error adding category' };
    contactServiceSpy.AddContact.and.returnValue(of(mockResponse));
    spyOn(window, 'alert');
    const form = <NgForm><unknown>{
      valid: true,
      value: {
        contactId: 1,
        FirstName: 'Test Description'
      },
      controls: {
        contactId: { value: 1 },
        FirstName: { value: 'Test Description' }
      }
    };

    // Act
    component.onSubmit(form);

    // Assert
    expect(window.alert).toHaveBeenCalledWith('Error adding category');
    expect(component.loading).toBe(false);
  });

  it('should alert error message on HTTP error', () => {
    spyOn(window, 'alert');
    const mockError = { error: { message: 'HTTP error' } };
    contactServiceSpy.AddContact.and.returnValue(throwError(mockError));

    const form = <NgForm><unknown>{
      valid: true,
      value: {
        contactId: 1,
        FirstName: 'Test Description'
      },
      controls: {
        contactId: { value: 1 },
        FirstName: { value: 'Test Description' }
      }
    };

    component.onSubmit(form);

    expect(window.alert).toHaveBeenCalledWith('HTTP error');
    expect(component.loading).toBe(false);
  });

  it('should not call categoryService.AddCategory on invalid form submission', () => {
    const form = <NgForm>{ valid: false };

    component.onSubmit(form);

    expect(contactServiceSpy.AddContact).not.toHaveBeenCalled();
    expect(component.loading).toBe(true);
  });
});
