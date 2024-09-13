import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SignupsuccessComponent } from './signupsuccess.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { FormsModule } from '@angular/forms';
import { RouterTestingModule } from '@angular/router/testing';

describe('SignupsuccessComponent', () => {
  let component: SignupsuccessComponent;
  let fixture: ComponentFixture<SignupsuccessComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports : [HttpClientTestingModule,RouterTestingModule,FormsModule],
      declarations: [SignupsuccessComponent]
    });
    fixture = TestBed.createComponent(SignupsuccessComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
