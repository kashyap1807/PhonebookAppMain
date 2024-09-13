import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { Contact } from 'src/app/models/contact.model';
import { Country } from 'src/app/models/country.model';
import { State } from 'src/app/models/state.model';
import { ContactService } from 'src/app/services/contact.service';
import { CountryService } from 'src/app/services/country.service';
import { StateService } from 'src/app/services/state.service';

@Component({
  selector: 'app-update-contact-rf',
  templateUrl: './update-contact-rf.component.html',
  styleUrls: ['./update-contact-rf.component.css']
})
export class UpdateContactRfComponent implements OnInit {

  countries : Country[] | undefined;

  states : State[] | undefined;

  contactId:number|undefined;

  loading : boolean = false;

  contactForm! : FormGroup;

  constructor(private contactService:ContactService,private countryService:CountryService,private stateService:StateService,private route:ActivatedRoute,private router:Router,private fb: FormBuilder){}

  ngOnInit(): void {
    this.route.params.subscribe((params)=>{
      this.loadCountries();
      this.contactId = +params['contactId'];
      this.loadContactsDetails(this.contactId);
    });
    this.contactForm = this.fb.group({
      contactId:[''],
      firstName: ['',[Validators.required,Validators.minLength(3)]],
      lastName: ['',[Validators.required,Validators.minLength(3)]],
      email:['',[Validators.required,Validators.email]],
      company:['',[Validators.required,Validators.minLength(3)]],
      contactNumber:['',[Validators.required, Validators.maxLength(10)]],
      // fileName:[''],
      Image:[''],
      fileToUpload:[''],
      countryId:[0,[Validators.required]],
      stateId:[0,[Validators.required]],
      gender:['',Validators.required],
      isFavourite:[false],

    });
    this.loadCountries();
    this.contactForm.get('countryId')?.valueChanges.subscribe(countryId =>{
      this.stateService.getStateByCountryId(countryId).subscribe(response =>{
        this.states = response.data;
        this.contactForm.get('stateId');
      });
    });
    
  };



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
          // this.states[0].stateId;
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

  loadContactsDetails(contactId:number|undefined):void{
    this.contactService.getContactById(contactId).subscribe({
      next:(response:ApiResponse<Contact>)=>{
        if (response.success) {
          this.contactForm.setValue({
            contactId: response.data.contactId,
            firstName: response.data.firstName,
            lastName: response.data.lastName,
            company: response.data.company,
            email: response.data.email,
            contactNumber: response.data.contactNumber,
            countryId: response.data.countryId,
            stateId: response.data.stateId,
            gender: response.data.gender,
            isFavourite: response.data.isFavourite,
            // fileName: response.data.fileName,
            Image:response.data.fileName,
            fileToUpload:''
          });
        }else{
          console.log('Failed to fetch contact',response.message);
          
        }
      },
      error(err) {
        alert(err.error.message);
      },
      complete:()=> {
        console.log('Completed');
        
      }
    });
  }

  get formControls() {
    return this.contactForm.controls;
  }

 //For File upload
 handleFileInput(event: any) {
  this.contactForm.patchValue({
    Image :  event.target.files[0]
  })
  console.log(this.contactForm.value);
}


  onSubmit(){
    const formData = new FormData();
    formData.append('contactId', this.contactForm.get('contactId')?.value);
    formData.append('FirstName', this.contactForm.get('firstName')?.value);
    formData.append('LastName', this.contactForm.get('lastName')?.value);
    formData.append('Email', this.contactForm.get('email')?.value);
    formData.append('Company', this.contactForm.get('company')?.value);
    formData.append('ContactNumber', this.contactForm.get('contactNumber')?.value);
    formData.append('countryId', this.contactForm.get('countryId')?.value);
    formData.append('stateId', this.contactForm.get('stateId')?.value);
    formData.append('gender', this.contactForm.get('gender')?.value);
    formData.append('isFavourite', this.contactForm.get('isFavourite')?.value);
    formData.append('Image', this.contactForm.get('Image')?.value);
    if (this.contactForm.valid) {
      console.log(this.contactForm.value);
      this.contactService.UpdateContact(formData).subscribe({
        next:(response)=>{
          if (response.success) {
            this.router.navigate(['/contact']);
          }else{
            alert(response.message);
          }
        },
        error:(err)=>{
          console.error('Failed to add contact: ',err.error.message);
          
        },
        complete:()=>{
          console.log('Completed');
          
        }
      });
      
    }
  }

}
