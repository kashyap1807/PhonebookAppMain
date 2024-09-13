import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { AddContact } from 'src/app/models/add-contact.model';
import { Country } from 'src/app/models/country.model';
import { State } from 'src/app/models/state.model';
import { ContactService } from 'src/app/services/contact.service';
import { CountryService } from 'src/app/services/country.service';
import { StateService } from 'src/app/services/state.service';

@Component({
  selector: 'app-add-contact',
  templateUrl: './add-contact.component.html',
  styleUrls: ['./add-contact.component.css']
})
export class AddContactComponent implements OnInit{


  contact: AddContact={
    firstName:'',
    lastName:'',
    email:'',
    company:'',
    contactNumber:'',
    image:null,
    countryId:0,
    stateId:0,
    gender:'',
    isFavourite:false,
    birthDate:undefined,
  }

  formData!:FormData;

  countryId : number | undefined;

  countries : Country[] | undefined;

  states: State[] | undefined;

  loading : boolean = false;

  constructor(private contactService:ContactService,private countryService:CountryService,private stateService:StateService,private route:ActivatedRoute,private router:Router){}

  ngOnInit(): void {
    this.formData = new FormData;
    this.loadCountries();
    
  }

  loadCountries():void{
    this.loading=true;
    this.countryService.getAllCountry().subscribe({
      next:(response:ApiResponse<Country[]>)=>{
        if (response.success) {
          this.countries = response.data;
        }
        else{
        console.error('Failed to fetch countries',response.message);
          
        }
        this.loading=false;
      },
      error:(error)=>{
        console.error('Error featching countries',error.message);
        this.loading=false;
      }
    });
  }

  loadStateDetails(countryId:number|undefined){
    this.stateService.getStateByCountryId(countryId).subscribe({
      next : (response)=>{
        if (response.success) {
          this.states = response.data;
          this.contact.stateId = this.states[0].stateId;
        }else{
          console.log('Failed to fetch state',response.message);
          
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

  handleFileInput(event: any) {
    this.contact.image = event.target.files[0];
  }

  onSubmit(addContactTfForm:NgForm){
    this.formData=new FormData;
    this.formData.append('FirstName', this.contact.firstName);
    this.formData.append('LastName', this.contact.lastName);
    this.formData.append('ContactNumber', this.contact.contactNumber);
    this.formData.append('Gender', this.contact.gender);
    this.formData.append('CountryId', this.contact.countryId.toString());
    this.formData.append('StateId', this.contact.stateId.toString());
    this.formData.append('IsFavourite', this.contact.isFavourite.toString());
    this.formData.append('Email', this.contact.email);
    this.formData.append('Company', this.contact.company);
    this.formData.append('Image', this.contact.image);
    this.formData.append('BirthDate',this.contact.birthDate!.toString());

    if (addContactTfForm.valid) {
      this.loading=true;
      console.log(addContactTfForm.value);
      

      this.contactService.AddContact(this.formData).subscribe({
        next:(response)=>{
          if(response.success){
            this.router.navigate(['/contact']);
          }else{
            alert(response.message);
          }
          this.loading=false;
        },
        error:(err)=>{
          console.log(err.error.message);
          alert(err.error.message);
          this.loading=false;
          
        },
        complete:()=>{
          console.log("Completed");
          
        } 
      });
    }
  }

  maxDate(): string {
    // Get current date in YYYY-MM-DD format
    const today = new Date();
    const dd = String(today.getDate()).padStart(2, '0');
    const mm = String(today.getMonth() + 1).padStart(2, '0'); // January is 0!
    const yyyy = today.getFullYear();

    return `${yyyy}-${mm}-${dd}`;
  }


}
