import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiResponse } from '../models/ApiResponse{T}';
import { Contact } from '../models/contact.model';
import { AddContact } from '../models/add-contact.model';
import { UpdateContact } from '../models/update-contact.model';
import { ContactByCountry } from '../models/contacts-by-country.model';
import { ContactByGender } from '../models/contacts-by-gender.model';
import { ContactByStateId } from '../models/contacts-by-stateid.model';
import { ContactsByMonth } from '../models/contacts-by-month.model';

@Injectable({
  providedIn: 'root'
})
export class ContactService {

  private apiUrl = 'http://localhost:5116/api/Contact/';

  constructor(private http:HttpClient) { }

  // getAllContacts():Observable<ApiResponse<Contact[]>>{
  //   return this.http.get<ApiResponse<Contact[]>>(this.apiUrl+"GetAllContact");
  // }

  getContactById(contactId:number|undefined):Observable<ApiResponse<Contact>>{
    return this.http.get<ApiResponse<Contact>>(this.apiUrl+"GetContactById/"+contactId)
  }

  deleteContactById(contactId: number | undefined):Observable<ApiResponse<string>>{
    return this.http.delete<ApiResponse<string>>(this.apiUrl+"Remove/"+contactId)
  }

  AddContact(formData:FormData):Observable<ApiResponse<string>>{
    let headers = new HttpHeaders();
    headers.append('Content-Type', 'multipart/form-data');
    headers.append('Accept', 'application/json');
    return this.http.post<ApiResponse<string>>(this.apiUrl+"Create",formData,{headers:headers});
  }

  UpdateContact(formData:FormData):Observable<ApiResponse<string>>{
    let headers = new HttpHeaders();
    headers.append('Content-Type', 'multipart/form-data');
    headers.append('Accept', 'application/json');
    return this.http.put<ApiResponse<string>>(this.apiUrl+"ModifyContact",formData,{headers:headers});
  }

  //Pagination
  // getTotalContactsCount() : Observable<ApiResponse<number>>{
  //   return this.http.get<ApiResponse<number>>(this.apiUrl+"GetTotalContacts")
  // }

  getAllContactsWithPaginationFavSearchSort(pageNumber:number,pageSize:number,searchString:string,sortName:string,showFavourite:boolean) : Observable<ApiResponse<Contact[]>>{
    return this.http.get<ApiResponse<Contact[]>>(this.apiUrl+"GetAllContactsWithPaginationFavSearchSort?page="+pageNumber+"&pageSize="+pageSize+"&search_string="+searchString+"&sort_name="+sortName+"&show_favourites="+showFavourite);
  }

  getTotalContactsWithSearchFav(searchString:string,showFavourite:boolean):Observable<ApiResponse<string>>{
    return this.http.get<ApiResponse<string>>(this.apiUrl+"GetTotalContactsWithSearchFav?search_string="+searchString+"&show_favourites="+showFavourite);
  }
  
  //---StoreProcedure reports---
  getAllContactByCountry():Observable<ApiResponse<ContactByCountry[]>>{
    return this.http.get<ApiResponse<ContactByCountry[]>>(this.apiUrl+"GetAllContactByCountry");
  }

  getAllContactByGender():Observable<ApiResponse<ContactByGender[]>>{
    return this.http.get<ApiResponse<ContactByGender[]>>(this.apiUrl+"GetAllContactByGenderSP");
  }

  getContactByStateId(stateId:number|undefined):Observable<ApiResponse<ContactByStateId[]>>{
    return this.http.get<ApiResponse<ContactByStateId[]>>(this.apiUrl+"GetAllContactByStateIdSP/"+stateId)
  }

  getContactByMonth(month:number|undefined):Observable<ApiResponse<ContactsByMonth[]>>{
    return this.http.get<ApiResponse<ContactsByMonth[]>>(this.apiUrl+"GetAllContactByMonth/"+month)
  }

}
