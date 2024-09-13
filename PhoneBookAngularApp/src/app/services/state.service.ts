import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiResponse } from '../models/ApiResponse{T}';
import { State } from '../models/state.model';

@Injectable({
  providedIn: 'root'
})
export class StateService {

  private apiUrl = 'http://localhost:5116/api/State/';

  constructor(private http:HttpClient) { }

  getStateByCountryId(countryId:number|undefined):Observable<ApiResponse<State[]>>{
    return this.http.get<ApiResponse<State[]>>(this.apiUrl+"GetStateByCountryId/"+countryId);
  }
}
