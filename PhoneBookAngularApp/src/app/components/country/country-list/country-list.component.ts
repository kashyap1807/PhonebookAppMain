import { Component, OnInit } from '@angular/core';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { Country } from 'src/app/models/country.model';
import { CountryService } from 'src/app/services/country.service';

@Component({
  selector: 'app-country-list',
  templateUrl: './country-list.component.html',
  styleUrls: ['./country-list.component.css']
})
export class CountryListComponent implements OnInit{

  countries : Country[] | undefined;
  loading:boolean=false;

  constructor(private countryService:CountryService){}

  ngOnInit(): void {
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
}
