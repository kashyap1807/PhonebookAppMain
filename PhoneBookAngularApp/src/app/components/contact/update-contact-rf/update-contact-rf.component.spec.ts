import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UpdateContactRfComponent } from './update-contact-rf.component';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';

describe('UpdateContactRfComponent', () => {
  let component: UpdateContactRfComponent;
  let fixture: ComponentFixture<UpdateContactRfComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports : [HttpClientTestingModule,RouterTestingModule,ReactiveFormsModule],
      declarations: [UpdateContactRfComponent]
    });
    fixture = TestBed.createComponent(UpdateContactRfComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
