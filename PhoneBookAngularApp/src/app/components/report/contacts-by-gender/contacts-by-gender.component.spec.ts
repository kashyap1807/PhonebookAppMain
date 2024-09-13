import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ContactsByGenderComponent } from './contacts-by-gender.component';

describe('ContactsByGenderComponent', () => {
  let component: ContactsByGenderComponent;
  let fixture: ComponentFixture<ContactsByGenderComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ContactsByGenderComponent]
    });
    fixture = TestBed.createComponent(ContactsByGenderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
