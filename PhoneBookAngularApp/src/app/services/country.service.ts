import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiResponse } from '../models/ApiResponse{T}';
import { Country } from '../models/country.model';

@Injectable({
  providedIn: 'root'
})
export class CountryService {
  private apiUrl = 'http://localhost:5116/api/Country/';

  constructor(private http:HttpClient) { }

  getAllCountry():Observable<ApiResponse<Country[]>>{
    return this.http.get<ApiResponse<Country[]>>(this.apiUrl+"GetAllCountry");
  }

  getCountryById(countryId:number|undefined):Observable<ApiResponse<Country>>{
    return this.http.get<ApiResponse<Country>>(this.apiUrl+"GetCountryById/"+countryId);
  }
}
