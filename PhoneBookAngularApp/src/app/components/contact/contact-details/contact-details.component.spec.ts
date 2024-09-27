import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { FormsModule } from '@angular/forms';
import { RouterTestingModule } from '@angular/router/testing';
import { ActivatedRoute } from '@angular/router';
import { of } from 'rxjs';
import { ContactDetailsComponent } from './contact-details.component';
import { ContactService } from 'src/app/services/contact.service';
import { Contact } from 'src/app/models/contact.model';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';

describe('ContactDetailsComponent', () => {
  let component: ContactDetailsComponent;
  let fixture: ComponentFixture<ContactDetailsComponent>;
  let contactService: jasmine.SpyObj<ContactService>;
  let route: ActivatedRoute;
  const mockContact: Contact = {
    contactId: 1,
    firstName: 'firstname',
    lastName: 'lastname',
    email: 'first@example.com',
    company: 'Example Inc.',
    contactNumber: '1234567890',
    fileName: '',
    imageBytes: undefined,
    countryId: 1,
    stateId: 1,
    gender: 'Male',
    isFavourite: true,
    birthDate: undefined,
    country: {
      countryId: 1,
      countryName: 'Country',
    },
    state: {
      stateId: 1,
      stateName: 'State',
      countryId: 1,
    },
  };

  beforeEach(() => {
    const contactServiceSpy = jasmine.createSpyObj('ContactService', ['getContactById', 'UpdateContact']);
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule, RouterTestingModule.withRoutes([]), FormsModule],
      declarations: [ContactDetailsComponent],
      providers: [
        { provide: ContactService, useValue: contactServiceSpy },
        {
          provide: ActivatedRoute,
          useValue: {
            params: of({ contactId: 1 })
          }
        }
      ]
    });
    fixture = TestBed.createComponent(ContactDetailsComponent);
    component = fixture.componentInstance;
    contactService = TestBed.inject(ContactService) as jasmine.SpyObj<ContactService>;
    route = TestBed.inject(ActivatedRoute);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize contactId from route params and load contact details', () => {
    const mockResponse: ApiResponse<Contact> = { success: true, data: mockContact, message: '' };
    contactService.getContactById.and.returnValue(of(mockResponse));

    fixture.detectChanges();

    expect(component.contactId).toBe(1);
    expect(contactService.getContactById).toHaveBeenCalledWith(1);
    expect(component.contact).toEqual(mockContact);
  });

  it('should toggle favorite status and call UpdateContact', () => {
    const mockResponse: ApiResponse<any> = { success: true,data:'', message: '' };
    contactService.UpdateContact.and.returnValue(of(mockResponse));

    fixture.detectChanges();
    component.onStarClick();

    expect(component.contact.isFavourite).toBe(false); // toggled value
    expect(contactService.UpdateContact).toHaveBeenCalledOnceWith(component.formData);
  });

  it('should log "Completed" when contact loading completes', () => {
    const mockResponse: ApiResponse<Contact> = { success: true, data: mockContact, message: '' };
    contactService.getContactById.and.returnValue(of(mockResponse));

    spyOn(console, 'log');

    fixture.detectChanges();

    expect(console.log).toHaveBeenCalledWith('Completed');
  });
});
