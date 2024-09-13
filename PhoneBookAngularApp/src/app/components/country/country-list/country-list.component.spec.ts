import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CountryListComponent } from './country-list.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { FormsModule } from '@angular/forms';
import { RouterTestingModule } from '@angular/router/testing';
import { Country } from 'src/app/models/country.model';
import { of, throwError } from 'rxjs';

describe('CountryListComponent', () => {
  let component: CountryListComponent;
  let fixture: ComponentFixture<CountryListComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports : [HttpClientTestingModule,RouterTestingModule,FormsModule],
      declarations: [CountryListComponent]
    });
    fixture = TestBed.createComponent(CountryListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should call ngOnInit and loadCountries', () => {
    spyOn(component, 'loadCountries');
    component.ngOnInit();
    expect(component.loadCountries).toHaveBeenCalled();
  });
  
  it('should load countries successfully', () => {
    const mockCountries: Country[] = [{ countryId:1,countryName:'Country1' }];
    spyOn(component['countryService'], 'getAllCountry').and.returnValue(of({ success: true, data: mockCountries ,message:''}));
  
    component.loadCountries();
    expect(component.loading).toBe(false); // Loading should be false after response
    expect(component.countries).toEqual(mockCountries);
  });
  
  it('should handle error when loading countries', () => {
    const errorMessage = 'Error fetching countries';
    spyOn(component['countryService'], 'getAllCountry').and.returnValue(throwError({ message: errorMessage }));
    spyOn(console, 'error'); // Mock console.error
  
    component.loadCountries();
  
    expect(component.loading).toBe(false); // Loading should be false after error
    expect(console.error).toHaveBeenCalledWith('Error featching countries', errorMessage); // Check console.error call
   
  });
  
  
  it('should display loading spinner while loading', () => {
    component.loading = true;
    fixture.detectChanges(); // Update the DOM after changing component properties
  
    const loadingElement = fixture.nativeElement.querySelector('.loading-spinner');
    expect(loadingElement); // Check if the loading spinner element exists
  });
  
  
  it('should render countries list when loaded', () => {
    const mockCountries: Country[] = [{ countryId:1,countryName:'country1' }];
    component.countries = mockCountries;
    fixture.detectChanges();
    const countryElements = fixture.nativeElement.querySelectorAll('.country-item');
    expect(countryElements.length).toBe(mockCountries.length=0);
  });
  
});
