import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { Country } from 'src/app/models/country.model';
import { State } from 'src/app/models/state.model';
import { UpdateContact } from 'src/app/models/update-contact.model';
import { ContactService } from 'src/app/services/contact.service';
import { CountryService } from 'src/app/services/country.service';
import { StateService } from 'src/app/services/state.service';

@Component({
  selector: 'app-update-contact',
  templateUrl: './update-contact.component.html',
  styleUrls: ['./update-contact.component.css']
})
export class UpdateContactComponent implements OnInit{

  contactId : number | undefined;

  contact : UpdateContact={
    contactId:0,
    firstName:'',
    lastName:'',
    email:'',
    company:'',
    contactNumber:'',
    image:null,
    countryId:0,
    stateId:0,
    gender:'',
    isFavourite:true,
    birthDate:undefined,
  }

  formData! : FormData;

  loading:boolean=false;

  countries: Country[] | undefined;

  states : State[] | undefined;

  initialStateId : number = 0;

  constructor(
    private contsctService: ContactService,
    private countryService: CountryService,
    private stateService:StateService,
    private route:ActivatedRoute,
    private router:Router
  ){}

  ngOnInit(): void {
    this.formData = new FormData;
    this.route.params.subscribe((params)=>{
    this.loadCountries();
    this.contactId = +params['contactId'];
    this.loadContactDetails(this.contactId);

    });
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
          if (this.initialStateId == -1) {
            this.contact.stateId = this.states[0].stateId;
          }
          this.initialStateId = -1;
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

  loadContactDetails(contactId:number|undefined):void{
    this.contsctService.getContactById(contactId).subscribe({
      next : (response)=>{
        if (response.success) {
          this.contact.contactId = response.data.contactId;
          this.contact.firstName = response.data.firstName;
          this.contact.lastName = response.data.lastName;
          this.contact.contactNumber = response.data.contactNumber;
          this.contact.gender = response.data.gender;
          this.contact.countryId = response.data.countryId;
          this.contact.stateId = response.data.stateId;
          this.contact.isFavourite = response.data.isFavourite;
          this.contact.email = response.data.email;
          this.contact.company = response.data.company;
          this.contact.image = response.data.fileName;
          this.contact.birthDate = response.data.birthDate;

          //Load states only after contact details loaded
          this.initialStateId = response.data.stateId;
          this.loadStateDetails(this.contact.countryId);
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

  handleFileInput(event: any) {
    this.contact.image = event.target.files[0];
  }

  onSubmit(updateContactTfForm: NgForm){
    this.formData=new FormData;
    this.formData.append('contactId',this.contact.contactId.toString());
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


    if (updateContactTfForm.valid) {
      this.loading=true;
      console.log(updateContactTfForm.value);
      
      this.contsctService.UpdateContact(this.formData).subscribe({
        next:(response)=>{
          if (response.success) {
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
