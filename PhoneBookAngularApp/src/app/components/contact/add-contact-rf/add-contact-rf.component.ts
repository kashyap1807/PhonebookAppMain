import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { Country } from 'src/app/models/country.model';
import { State } from 'src/app/models/state.model';
import { ContactService } from 'src/app/services/contact.service';
import { CountryService } from 'src/app/services/country.service';
import { StateService } from 'src/app/services/state.service';

@Component({
  selector: 'app-add-contact-rf',
  templateUrl: './add-contact-rf.component.html',
  styleUrls: ['./add-contact-rf.component.css']
})
export class AddContactRfComponent implements OnInit{

  loading : boolean = false;
  
  countries : Country[] | undefined;
  states : State[] | undefined;

  contactForm! : FormGroup;

  constructor(private contactService:ContactService,private countryService:CountryService,private stateService:StateService,private route:ActivatedRoute,private router:Router,private fb: FormBuilder){}
  

  ngOnInit(): void {
    this.contactForm = this.fb.group({
      firstName: ['',[Validators.required,Validators.minLength(3)]],
      lastName: ['',[Validators.required,Validators.minLength(3)]],
      email:['',[Validators.required,Validators.email]],
      company:['',[Validators.required,Validators.minLength(3)]],
      contactNumber:['',[Validators.required, Validators.minLength(10)]],
      // fileName:[''],
      Image:[''],
      countryId:[0,[Validators.required,this.countryValidator]],
      stateId:[0,[Validators.required,this.stateValidator]],
      gender:['',Validators.required],
      isFavourite:[true],

    });

    this.loadCountries();
    this.contactForm.get('countryId')?.valueChanges.subscribe(countryId =>{
      this.stateService.getStateByCountryId(countryId).subscribe(response =>{
        this.states = response.data;
        this.contactForm.get('stateId');
      });
    });
  }

  countryValidator(control : any){
    return control.value == ''? {invalidCountry:true}:null;
  }

  stateValidator(control : any){
    return control.value == ''? {invalidState:true}:null;
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
      this.contactService.AddContact(formData).subscribe({
        next:(response)=>{
          if (response.success) {
            this.router.navigate(['/contact']);
          }else{
            alert(response.message);
          }
        },
        error(err) {
          console.error('Failed to add contact',err.error.message);
        },
        complete: ()=>{
          console.log('Completed'); 
        }
      });
      
    }
  }

}
