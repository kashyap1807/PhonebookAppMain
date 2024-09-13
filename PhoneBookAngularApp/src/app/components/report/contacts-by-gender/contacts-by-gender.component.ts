import { Component, OnInit } from '@angular/core';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { ContactByGender } from 'src/app/models/contacts-by-gender.model';
import { ContactService } from 'src/app/services/contact.service';

@Component({
  selector: 'app-contacts-by-gender',
  templateUrl: './contacts-by-gender.component.html',
  styleUrls: ['./contacts-by-gender.component.css']
})
export class ContactsByGenderComponent implements OnInit{

  contactsbygender:ContactByGender [] | undefined;
  loading:boolean=false;
  constructor(private contactService:ContactService){}

  ngOnInit(): void {
    this.loadContactByGender();
  }

  loadContactByGender():void{
    this.loading=true;
    this.contactService.getAllContactByGender().subscribe({
      next:(response:ApiResponse<ContactByGender[]>)=>{
        if (response.success) {
          this.contactsbygender = response.data;
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
