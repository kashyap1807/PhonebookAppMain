import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ContactListComponent } from './contact-list.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { FormsModule } from '@angular/forms';
import { RouterTestingModule } from '@angular/router/testing';
import { ContactService } from 'src/app/services/contact.service';
import { Contact } from 'src/app/models/contact.model';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { of, throwError } from 'rxjs';

describe('ContactListComponent', () => {
  let component: ContactListComponent;
  let fixture: ComponentFixture<ContactListComponent>;
  let mockContactService: jasmine.SpyObj<ContactService>;

  beforeEach(() => {
    mockContactService = jasmine.createSpyObj('ContactService', [
      'getAllContactsWithPaginationFavSearchSort',
      'getTotalContactsWithSearchFav',
      'deleteContactById'
    ]);
    
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule, RouterTestingModule, FormsModule],
      declarations: [ContactListComponent],
      providers: [
        { provide: ContactService, useValue: mockContactService }
      ]
    });
    
    fixture = TestBed.createComponent(ContactListComponent);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load contacts successfully', () => {
    const mockContacts: Contact[] = [
      {
        contactId: 0,
        firstName: 'John',
        lastName: 'Doe',
        email: 'john.doe@example.com',
        company: 'Company',
        contactNumber: '1234567890',
        fileName: 'image.jpg',
        imageBytes: undefined,
        countryId: 0,
        stateId: 0,
        gender: 'Male',
        isFavourite: true,
        country: {
          countryId: 0,
          countryName: 'CountryName'
        },
        state: {
          stateId: 0,
          stateName: 'StateName',
          countryId: 0
        }
      },
    ];
    const mockApiResponse: ApiResponse<Contact[]> = {
      success: true,
      data: mockContacts,
      message: ''
    };

    mockContactService.getAllContactsWithPaginationFavSearchSort.and.returnValue(of(mockApiResponse));
    mockContactService.getTotalContactsWithSearchFav.and.returnValue(of({ success: true, data: '1', message: '' }));

    fixture.detectChanges(); // Trigger ngOnInit

    expect(component.loading).toBeFalse();
    expect(component.contacts).toEqual(mockContacts);
    expect(component.totalItems).toBe(1);
  });

  it('should handle error while loading contacts', () => {
    mockContactService.getAllContactsWithPaginationFavSearchSort.and.returnValue(throwError({ error: { message: 'Error' } }));
    mockContactService.getTotalContactsWithSearchFav.and.returnValue(of({ success: true, data: '0', message: '' }));

    fixture.detectChanges(); // Trigger ngOnInit

    expect(component.loading).toBeFalse();
    expect(component.contacts).toBeNull();
  });

  it('should delete contact successfully', () => {
    const mockDeleteResponse: ApiResponse<string> = {
      success: true,
      data: '',
      message: ''
    };
  
    mockContactService.deleteContactById.and.returnValue(of(mockDeleteResponse));
    spyOn(window, 'confirm').and.returnValue(true); // Ensure confirm returns true
    mockContactService.getAllContactsWithPaginationFavSearchSort.and.returnValue(of({ success: true, data: [], message: '' }));
    mockContactService.getTotalContactsWithSearchFav.and.returnValue(of({ success: true, data: '0', message: '' }));
  
    component.confirmDelete(1);
  
    expect(mockContactService.deleteContactById).toHaveBeenCalledWith(1);
  });

  it('should handle error while deleting contact', () => {
    mockContactService.deleteContactById.and.returnValue(throwError({ error: { message: 'Delete Error' } }));
    spyOn(window, 'alert');
    spyOn(window, 'confirm').and.returnValue(true); // Ensure confirm returns true
  
    component.confirmDelete(1);
  
    expect(window.alert).toHaveBeenCalledWith('Delete Error');
  });

  it('should change page', () => {
    const mockContacts: Contact[] = [
      {
        contactId: 0,
        firstName: 'firstname',
        lastName: 'lastname',
        email: 'first@example.com',
        company: 'Company',
        contactNumber: '1234567890',
        fileName: 'image.jpg',
        imageBytes: undefined,
        countryId: 0,
        stateId: 0,
        gender: 'Male',
        isFavourite: true,
        country: {
          countryId: 0,
          countryName: 'CountryName'
        },
        state: {
          stateId: 0,
          stateName: 'StateName',
          countryId: 0
        }
      },
    ];
    const mockApiResponse: ApiResponse<Contact[]> = {
      success: true,
      data: mockContacts,
      message: ''
    };

    mockContactService.getAllContactsWithPaginationFavSearchSort.and.returnValue(of(mockApiResponse));
    mockContactService.getTotalContactsWithSearchFav.and.returnValue(of({ success: true, data: '1', message: '' }));

    component.changePage(2);

    expect(component.pageNumber).toBe(2);
  });

  it('should change page size', () => {
    const mockContacts: Contact[] = [
      {
        contactId: 0,
        firstName: 'firstname',
        lastName: 'lastname',
        email: 'first@example.com',
        company: 'Company',
        contactNumber: '1234567890',
        fileName: 'image.jpg',
        imageBytes: undefined,
        countryId: 0,
        stateId: 0,
        gender: 'Male',
        isFavourite: true,
        country: {
          countryId: 0,
          countryName: 'CountryName'
        },
        state: {
          stateId: 0,
          stateName: 'StateName',
          countryId: 0
        }
      },
    ];
    const mockApiResponse: ApiResponse<Contact[]> = {
      success: true,
      data: mockContacts,
      message: ''
    };

    mockContactService.getAllContactsWithPaginationFavSearchSort.and.returnValue(of(mockApiResponse));
    mockContactService.getTotalContactsWithSearchFav.and.returnValue(of({ success: true, data: '1', message: '' }));

    component.changePageSize(10);

    expect(component.pageSize).toBe(10);
    expect(component.pageNumber).toBe(1);
  });

  it('should toggle sort order', () => {
    const mockContacts: Contact[] = [
      {
        contactId: 0,
        firstName: 'firstname',
        lastName: 'lastname',
        email: 'first@example.com',
        company: 'Company',
        contactNumber: '1234567890',
        fileName: 'image.jpg',
        imageBytes: undefined,
        countryId: 0,
        stateId: 0,
        gender: 'Male',
        isFavourite: true,
        country: {
          countryId: 0,
          countryName: 'CountryName'
        },
        state: {
          stateId: 0,
          stateName: 'StateName',
          countryId: 0
        }
      },
    ];
    const mockApiResponse: ApiResponse<Contact[]> = {
      success: true,
      data: mockContacts,
      message: ''
    };

    mockContactService.getAllContactsWithPaginationFavSearchSort.and.returnValue(of(mockApiResponse));
    mockContactService.getTotalContactsWithSearchFav.and.returnValue(of({ success: true, data: '1', message: '' }));

    fixture.detectChanges(); // Trigger ngOnInit

    component.toggleSort();
    expect(component.sortName).toBe('asc');
    
    component.toggleSort();
    expect(component.sortName).toBe('desc');
    
    component.toggleSort();
    expect(component.sortName).toBe('default');
  });
});
