import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UpdateContactComponent } from './update-contact.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { FormsModule } from '@angular/forms';
import { RouterTestingModule } from '@angular/router/testing';
import { ContactService } from 'src/app/services/contact.service';
import { Router, ActivatedRoute } from '@angular/router';
import { Contact } from 'src/app/models/contact.model';
import { of, throwError } from 'rxjs';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';

describe('UpdateContactComponent', () => {
  let component: UpdateContactComponent;
  let fixture: ComponentFixture<UpdateContactComponent>;
  let contactServiceSpy: jasmine.SpyObj<ContactService>;
  let routerSpy: jasmine.SpyObj<Router>;
  let route: ActivatedRoute;
  const mockContact: Contact = {
    contactId: 0,
    firstName: '',
    lastName: '',
    email: '',
    company: '',
    contactNumber: '',
    fileName: '',
    imageBytes: undefined,
    countryId: 0,
    stateId: 0,
    gender: '',
    isFavourite: true,
    birthDate: undefined,
    country: {
      countryId: 0,
      countryName: '',
    },
    state: {
      stateId: 0,
      stateName: '',
      countryId: 0,
    },
  };
  beforeEach(() => {
    contactServiceSpy = jasmine.createSpyObj('ContactService', ['getContactById']);
    routerSpy = jasmine.createSpyObj('Router', ['navigate']);
    TestBed.configureTestingModule({
      imports : [HttpClientTestingModule,RouterTestingModule,FormsModule],
      declarations: [UpdateContactComponent],
      providers: [
        { provide: ContactService, useValue: contactServiceSpy },
        // { provide: Router, useValue: routerSpy },
        {
          provide: ActivatedRoute,
          useValue: {
            params: of({ contactId: 1 })
          }
        }
      ]
    });
    fixture = TestBed.createComponent(UpdateContactComponent);
    component = fixture.componentInstance;
    route = TestBed.inject(ActivatedRoute);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize contactID from route params and load contact details', () => {
    // Arrange
    const mockResponse: ApiResponse<Contact> = { success: true, data: mockContact, message: '' };
    contactServiceSpy.getContactById.and.returnValue(of(mockResponse));

    // Act
    fixture.detectChanges(); // ngOnInit is called here

    // Assert
    expect(component.contactId).toBe(1);
    expect(contactServiceSpy.getContactById).toHaveBeenCalledWith(1);
    // expect(component.contact).toEqual(mockContact);
  });

  it('should log error message if contact loading fails', () => {
    // Arrange
    const mockResponse: ApiResponse<Contact> = { success: false, data: mockContact, message: 'Failed to fetch contact' };
    contactServiceSpy.getContactById.and.returnValue(of(mockResponse));
    spyOn(console, 'log');

    // Act
    fixture.detectChanges();

    // Assert
    expect(console.log).toHaveBeenCalledWith('Failed to fetch contact', 'Failed to fetch contact');
  });

  it('should alert error message on HTTP error', () => {
    // Arrange
    spyOn(window, 'alert');
    const mockError = { error: { message: 'HTTP error' } };
    contactServiceSpy.getContactById.and.returnValue(throwError(mockError));

    // Act
    fixture.detectChanges();

    // Assert
    expect(window.alert).toHaveBeenCalledWith('HTTP error');
  });

  it('should log "Completed" when contact loading completes', () => {
    // Arrange
    const mockResponse: ApiResponse<Contact> = { success: true, data: mockContact, message: '' };
    contactServiceSpy.getContactById.and.returnValue(of(mockResponse));
    spyOn(console, 'log');

    // Act
    fixture.detectChanges();

    // Assert
    expect(console.log).toHaveBeenCalledWith('Completed');
  });
});
