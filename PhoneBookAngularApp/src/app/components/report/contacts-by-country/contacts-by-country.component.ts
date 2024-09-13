import { Component, OnInit } from '@angular/core';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { ContactByCountry } from 'src/app/models/contacts-by-country.model';
import { ContactService } from 'src/app/services/contact.service';

@Component({
  selector: 'app-contacts-by-country',
  templateUrl: './contacts-by-country.component.html',
  styleUrls: ['./contacts-by-country.component.css']
})
export class ContactsByCountryComponent implements OnInit{

  contactsbycountry: ContactByCountry [] | undefined;
  loading:boolean=false;
  constructor(private contactService:ContactService){}
  ngOnInit(): void {
    this.loadContactsByCountry();
  }

  loadContactsByCountry():void{
    this.loading=true;
    this.contactService.getAllContactByCountry().subscribe({
      next:(response:ApiResponse<ContactByCountry[]>)=>{
        if (response.success) {
          this.contactsbycountry = response.data;
        }
        else{
        console.error('Failed to fetch contacts',response.message);
          
        }
        this.loading=false;
      },
      error:(error)=>{
        console.error('Error fetching contacts',error);
        this.loading=false;
      }
    });
  }
}
