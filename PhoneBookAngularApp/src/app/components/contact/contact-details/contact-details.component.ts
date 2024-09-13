import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Contact } from 'src/app/models/contact.model';
import { ContactService } from 'src/app/services/contact.service';

@Component({
  selector: 'app-contact-details',
  templateUrl: './contact-details.component.html',
  styleUrls: ['./contact-details.component.css']
})
export class ContactDetailsComponent implements OnInit{

  contactId:number|undefined;
  formData!: FormData;
  contact : Contact={
    contactId: 0,
    firstName: '',
    lastName: '',
    email: '',
    company: '',
    contactNumber: '',
    fileName: '',
    imageBytes: undefined,
    countryId: 0,
    stateId: 0,
    gender: '',
    isFavourite: true,
    birthDate:undefined,
    country: {
      countryId: 0,
      countryName: ''
    },
    state: {
      stateId: 0,
      stateName: '',
      countryId: 0
    },
  }

  constructor(private contactService:ContactService,private route:ActivatedRoute,private router:Router,private cdr:ChangeDetectorRef){}


  ngOnInit(): void {
    this.route.params.subscribe((params)=>{
      this.contactId = params['contactId'];
      this.loadContactDetails(this.contactId);
   }); 
   this.formData = new FormData;
  }

  loadContactDetails(contactId:number|undefined):void{
    this.contactService.getContactById(contactId).subscribe({
      next : (response)=>{
        if (response.success) {
          this.contact = response.data;
        }else{
          console.log('Failed to fetch contact',response.message);
          
        }
      },
      error:(err)=>{
        alert(err.error.message);
      },
      complete:()=>{
        console.log('Completed');
        
      }
    });
  }

  onStarClick():void{
    this.contact.isFavourite = !this.contact.isFavourite;
    this.formData.append('contactId',this.contact.contactId.toString());
    this.formData.append('FirstName',this.contact.firstName);
    this.formData.append('LastName', this.contact.lastName);
    this.formData.append('Email', this.contact.email);
    this.formData.append('Company', this.contact.company);
    this.formData.append('ContactNumber', this.contact.contactNumber);
    this.formData.append('Gender', this.contact.gender);
    this.formData.append('IsFavourite', this.contact.isFavourite.toString());
    this.formData.append('CountryId', this.contact.countryId.toString());
    this.formData.append('StateId', this.contact.stateId.toString());
    this.contactService.UpdateContact(this.formData).subscribe({
      next:(response) =>{
        if(response.success){
          
        }
        else{
          alert(response.message);
        }
        
      },
      error:(err) => {
        console.log(err.error.message);
        alert(err.error.message);
      },
      complete:() =>{
        console.log("completed");
      }
    });
  }

}
