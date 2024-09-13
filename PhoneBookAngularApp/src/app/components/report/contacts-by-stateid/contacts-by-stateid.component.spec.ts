import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ContactsByStateidComponent } from './contacts-by-stateid.component';

describe('ContactsByStateidComponent', () => {
  let component: ContactsByStateidComponent;
  let fixture: ComponentFixture<ContactsByStateidComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ContactsByStateidComponent]
    });
    fixture = TestBed.createComponent(ContactsByStateidComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
