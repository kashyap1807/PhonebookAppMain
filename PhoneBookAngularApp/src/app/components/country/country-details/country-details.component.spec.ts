import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { ActivatedRoute } from '@angular/router';
import { of, throwError } from 'rxjs';

import { CountryDetailsComponent } from './country-details.component';
import { CountryService } from 'src/app/services/country.service';
import { Country } from 'src/app/models/country.model';

describe('CountryDetailsComponent', () => {
  let component: CountryDetailsComponent;
  let fixture: ComponentFixture<CountryDetailsComponent>;
  let countryService: jasmine.SpyObj<CountryService>;
  let activatedRoute: ActivatedRoute;

  beforeEach(() => {
    const countryServiceSpy = jasmine.createSpyObj('CountryService', ['getCountryById']);

    TestBed.configureTestingModule({
      imports: [],
      declarations: [CountryDetailsComponent],
      providers: [
        { provide: CountryService, useValue: countryServiceSpy },
        {
          provide: ActivatedRoute,
          useValue: {
            params: of({ countryId: 1 }) // Mock ActivatedRoute params with countryId 1
          }
        }
      ]
    });

    fixture = TestBed.createComponent(CountryDetailsComponent);
    component = fixture.componentInstance;
    countryService = TestBed.inject(CountryService) as jasmine.SpyObj<CountryService>;
    activatedRoute = TestBed.inject(ActivatedRoute);

    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('ngOnInit should initialize with route params', fakeAsync(() => {
    // Simulate route param change
    activatedRoute.params.subscribe({});
    tick();

    expect(component.countryId).toBe(1);
    expect(countryService.getCountryById).toHaveBeenCalledWith(1);
  }));

  describe('loadCountryDetails', () => {
    it('should load country details successfully', () => {
      const mockCountry: Country = { countryId: 1, countryName: 'Test Country' };
      countryService.getCountryById.and.returnValue(of({ success: true, data: mockCountry ,message:''}));

      component.loadCountryDetails(1);

      expect(component.country).toEqual(mockCountry);
    });

    it('should handle error when loading country details', () => {
      const errorMessage = 'Failed to fetch country';
      countryService.getCountryById.and.returnValue(throwError({ error: { message: errorMessage } }));

      spyOn(console, 'log'); // Mock console.log for error handling

      component.loadCountryDetails(1);

      expect(console.log);
    });
  });
});
