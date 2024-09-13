import { Component, OnInit } from '@angular/core';
import { ContactsByMonth } from 'src/app/models/contacts-by-month.model';
import { ContactService } from 'src/app/services/contact.service';

@Component({
  selector: 'app-contacts-by-month',
  templateUrl: './contacts-by-month.component.html',
  styleUrls: ['./contacts-by-month.component.css']
})
export class ContactsByMonthComponent {

  months = [
    { name: 'January', value: 1 },
    { name: 'February', value: 2 },
    { name: 'March', value: 3 },
    { name: 'April', value: 4 },
    { name: 'May', value: 5 },
    { name: 'June', value: 6 },
    { name: 'July', value: 7 },
    { name: 'August', value: 8 },
    { name: 'September', value: 9 },
    { name: 'October', value: 10 },
    { name: 'November', value: 11 },
    { name: 'December', value: 12 }
  ];
  selectedMonth: number = 0;
  monthNames:string[] = this.months.map(m => m.name);
  contactbymonth : ContactsByMonth [] | null = [];


  constructor(private contactService:ContactService){}

  loadContacts():void{
    if (this.selectedMonth) {
      this.contactService.getContactByMonth(this.selectedMonth).subscribe({
        next:(response)=>{
          if (response.success && response.data.length > 0) {
            this.contactbymonth = response.data;
          }
          else{
            console.error("Failed to fetch contacts");
            this.contactbymonth = null;
          }
        },
        error:(err)=>{
          alert(err.error.message);
        },
        complete:()=>{
          console.log('Completed');
          
        }
      })
    }
  }

  loadMonths():void{
    this.contactbymonth = null;
    this.loadContacts();
  }
}
