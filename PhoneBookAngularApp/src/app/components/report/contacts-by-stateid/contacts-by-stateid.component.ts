import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { ContactByStateId } from 'src/app/models/contacts-by-stateid.model';
import { Country } from 'src/app/models/country.model';
import { State } from 'src/app/models/state.model';
import { ContactService } from 'src/app/services/contact.service';
import { CountryService } from 'src/app/services/country.service';
import { StateService } from 'src/app/services/state.service';

@Component({
  selector: 'app-contacts-by-stateid',
  templateUrl: './contacts-by-stateid.component.html',
  styleUrls: ['./contacts-by-stateid.component.css']
})
export class ContactsByStateidComponent implements OnInit{
  
  stateId: number | undefined = 0;
  contactbystate : ContactByStateId [] | undefined;
  countryId : number | undefined = 0;

  countries : Country[] | undefined;

  states: State[] | undefined;

  loading : boolean = false;

  constructor(private contactService:ContactService,private route:ActivatedRoute,private countryService:CountryService,private stateService:StateService,private router:Router){}
  
  ngOnInit(): void {
    
      // this.loadContactByStateId(this.stateId);
      this.loadCountries();
    
  }

  loadContactByStateId(stateId:number|undefined){
    this.contactService.getContactByStateId(stateId).subscribe({
      next : (response)=>{
        if (response.success) {
          this.contactbystate = response.data;
        }else{
          console.log('Failed to fetch category',response.message);
          
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
          this.stateId = 0
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

    //row to details navigation
    goToDetails(contactId: number): void {
      this.router.navigate(['/contact/contactdetails', contactId]);
    }
}
