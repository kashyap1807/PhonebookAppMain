import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ContactsByCountryComponent } from './contacts-by-country.component';

describe('ContactsByCountryComponent', () => {
  let component: ContactsByCountryComponent;
  let fixture: ComponentFixture<ContactsByCountryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ContactsByCountryComponent]
    });
    fixture = TestBed.createComponent(ContactsByCountryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
