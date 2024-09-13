import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Country } from 'src/app/models/country.model';
import { CountryService } from 'src/app/services/country.service';

@Component({
  selector: 'app-country-details',
  templateUrl: './country-details.component.html',
  styleUrls: ['./country-details.component.css']
})
export class CountryDetailsComponent implements OnInit{

  countryId : number |undefined;
  country: Country={
    countryId:0,
    countryName:'',
  }

  constructor(private countryService:CountryService,private route:ActivatedRoute){}

  ngOnInit(): void {
    this.route.params.subscribe((params)=>{
      this.countryId = params['countryId'];
      this.loadCountryDetails(this.countryId);
    });
  }

  loadCountryDetails(countryId:number|undefined):void{
    this.countryService.getCountryById(countryId).subscribe({
      next : (response)=>{
        if (response.success) {
          this.country = response.data;
        }else{
          console.log('Failed to fetch country',response.message);
          
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
}
