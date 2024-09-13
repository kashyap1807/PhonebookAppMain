import { TestBed } from '@angular/core/testing';

import { StateService } from './state.service';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { ApiResponse } from '../models/ApiResponse{T}';
import { State } from '../models/state.model';

describe('StateService', () => {
  let service: StateService;
  let httpMock:HttpTestingController;
  beforeEach(() => {
    TestBed.configureTestingModule({
      imports:[HttpClientTestingModule],
      providers:[StateService]
    });
    service = TestBed.inject(StateService);
    httpMock = TestBed.inject(HttpTestingController)
  });

  afterEach(()=>{
    httpMock.verify();
  })

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should fetch all states by country id successfully', () => {
    // Arrange
    const countryId = 1;
    const mockApiResponse: ApiResponse<State[]> = {
      success: true,
      message: '',
      data: [
        {
          stateId: 1,
          stateName: 'State 1',
          countryId: countryId,
        },
        {
          stateId: 2,
          stateName: 'State 2',
          countryId: countryId,
        },
      ]
    };

    const apiUrl = "http://localhost:5116/api/State/GetStateByCountryId/" + countryId;

    // Act
    service.getStateByCountryId(countryId).subscribe((response) => {
      // Assert
      expect(response.data.length).toBe(2);
      expect(response.data).toEqual(mockApiResponse.data);
    });

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockApiResponse);
  })

  it('should handle an empty state list', () => {
    // Arrange
    const countryId = 1;
    const mockApiResponse: ApiResponse<State[]> = {
      success: true,
      message: '',
      data: []
    };

    const apiUrl = "http://localhost:5116/api/State/GetStateByCountryId/" + countryId;

    // Act
    service.getStateByCountryId(countryId).subscribe((response) => {
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
    const countryId = 1;
    const apiUrl = "http://localhost:5116/api/State/GetStateByCountryId/" + countryId;
    const errorMessage = "Failed to load states";

    // Act
    service.getStateByCountryId(countryId).subscribe(
      () => fail('expected an error, not states'),
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
