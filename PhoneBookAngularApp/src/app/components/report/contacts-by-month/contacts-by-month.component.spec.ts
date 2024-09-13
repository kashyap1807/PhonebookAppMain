import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ContactsByMonthComponent } from './contacts-by-month.component';

describe('ContactsByMonthComponent', () => {
  let component: ContactsByMonthComponent;
  let fixture: ComponentFixture<ContactsByMonthComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ContactsByMonthComponent]
    });
    fixture = TestBed.createComponent(ContactsByMonthComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
