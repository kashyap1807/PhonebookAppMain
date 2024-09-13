import { TestBed } from '@angular/core/testing';

import { CountryService } from './country.service';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { ApiResponse } from '../models/ApiResponse{T}';
import { Country } from '../models/country.model';

describe('CountryService', () => {
  let service: CountryService;
  let httpMock:HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports:[HttpClientTestingModule],
      providers:[CountryService]
    });
    service = TestBed.inject(CountryService);
    httpMock=TestBed.inject(HttpTestingController)
  });

  afterEach(()=>{
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
  it('should fetch all countries successfully', () => {
    // Arrange
    const mockApiResponse: ApiResponse<Country[]> = {
      success: true,
      message: '',
      data: [
        {
          countryId: 1,
          countryName: "Country 1"
        },
        {
          countryId: 2,
          countryName: "Country 2"
        },
      ]
    };
    const apiUrl = "http://localhost:5116/api/Country/GetAllCountry";

    // Act
    service.getAllCountry().subscribe((response) => {
      // Assert
      expect(response.data.length).toBe(2);
      expect(response.data).toEqual(mockApiResponse.data);
    });

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockApiResponse);
  })

  it('should handle an empty country list', () => {
    // Arrange
    const mockApiResponse: ApiResponse<Country[]> = {
      success: true,
      message: '',
      data: []
    };
    const apiUrl = "http://localhost:5116/api/Country/GetAllCountry";

    // Act
    service.getAllCountry().subscribe((response) => {
      // Assert
      expect(response.data.length).toBe(0);
      expect(response.data).toEqual([]);
    });

    const req = httpMock.expectOne(apiUrl)
    expect(req.request.method).toBe('GET');
    req.flush(mockApiResponse);
  });

  it('should handle http error gracefully', () => {
    // Arrange
    const apiUrl = "http://localhost:5116/api/Country/GetAllCountry";
    const errorMessage = "Failed to load countries";

    // Act
    service.getAllCountry().subscribe(
      () => fail('expected an error, not countries'),
      (error) => {
        // Assert
        expect(error.status).toBe(500);
        expect(error.statusText).toBe('Internal Server Error');
      }
    );

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');

    // Respond with error
    req.flush(errorMessage, {status: 500, statusText: 'Internal Server Error'});
  });
});
