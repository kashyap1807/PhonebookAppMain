import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';

import { AddContactRfComponent } from './add-contact-rf.component';
import { ContactService } from 'src/app/services/contact.service';
import { CountryService } from 'src/app/services/country.service';
import { StateService } from 'src/app/services/state.service';

describe('AddContactRfComponent', () => {
  let component: AddContactRfComponent;
  let fixture: ComponentFixture<AddContactRfComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule, RouterTestingModule, ReactiveFormsModule],
      declarations: [AddContactRfComponent],
      providers: [ContactService, CountryService, StateService]
    });

    fixture = TestBed.createComponent(AddContactRfComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

 
  it('should initialize contactForm with default values', () => {
    expect(component.contactForm).toBeDefined();
    expect(component.contactForm.get('firstName')).toBeDefined();
    expect(component.contactForm.get('lastName')).toBeDefined();
    
  });
  it('should mark form controls as invalid if empty', () => {
    let firstName = component.contactForm.get('firstName');
    firstName?.setValue('');
    expect(firstName?.valid).toBeFalsy();
    expect(firstName?.errors?.['required']).toBeTruthy();
  });
});
