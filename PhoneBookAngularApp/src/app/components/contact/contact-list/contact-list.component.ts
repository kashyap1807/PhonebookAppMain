import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { Contact } from 'src/app/models/contact.model';
import { ContactService } from 'src/app/services/contact.service';


@Component({
  selector: 'app-contact-list',
  templateUrl: './contact-list.component.html',
  styleUrls: ['./contact-list.component.css']
})
export class ContactListComponent implements OnInit{
  contactId : number | undefined;
  contacts: Contact[] | undefined | null;
  loading:boolean=false;

  //pagination
  pageNumber: number = 1;
  pageSize: number = 4;
  totalItems:number = 0;
  totalPages: number =0;

  //Filter
  searchArray: string[] = ['ALL', 'A', 'B', 'C', 'D', 'E','F','G', 'H', 'I', 'J', 'K','L','M', 'N', 'O', 'P', 'Q','R','S', 'T', 'U', 'V', 'W','X','Y','Z'];
  searchString: string = '';
  sortName: string = 'default';
  showFavourite : boolean =false;


  
  
  //Search
  searchQuery: string = '';

  constructor(private contactService:ContactService,private router:Router){}
  
  ngOnInit(): void {
    // this.loadContactsCount();
    this.loadContacts(this.searchString,this.pageNumber,this.sortName,this.showFavourite);
  }

  loadContactsCount():void{
    this.contactService.getTotalContactsWithSearchFav(this.searchString,this.showFavourite).subscribe({
      next:(response: ApiResponse<string>)=>{
        if (response.success) {
          console.log(response.data);
          this.totalItems = Number(response.data);
          this.totalPages = Math.ceil(this.totalItems/this.pageSize);
         
        }else{
          console.error('Failed to fetch contacts count', response.message);
          
        }
        this.loading=false;
      },
      error:(err)=> {
        this.contacts=[];
        console.error('Error featching contacts count',err.error);
        this.loading =false;
      },
    });
  }
  
  loadContacts(searchCharachter:string,pageNumber:number,sortName:string ,showFavourite:boolean):void{
    this.loading=true;
    this.showFavourite = showFavourite;
    this.searchString = searchCharachter;
    this.sortName = sortName;
    if (searchCharachter=="ALL") {
      this.showFavourite=false;
      this.searchString="";
    }
    this.pageNumber = pageNumber;
    this.loadContactsCount();
    this.contactService.getAllContactsWithPaginationFavSearchSort(this.pageNumber,this.pageSize,this.searchString,this.sortName,this.showFavourite).subscribe({
      next:(response:ApiResponse<Contact[]>)=>{
        if(response.success){
          this.contacts = response.data;
        }
        else{
          console.error('Failed to fetch contacts',response.message);
          
        }
        this.loading=false;
      },error:(error)=>{
        this.contacts=null;
        console.error('Error fetching contacts',error);
        this.loading=false;
      }
      
    });
  }

  confirmDelete(id : number): void{
    if(confirm('Are you sure?')){
      this.contactId = id;
      this.deleteContact();
    }
  }

  deleteContact(): void{
    this.contactService.deleteContactById(this.contactId).subscribe({
      next:(response) => {
        if(response.success){
          this.loadContacts(this.searchString,1,this.sortName,this.showFavourite);
        }
        else{
          alert(response.message);
        }
      },
      error: (err) =>{
        alert(err.error.message);
      },
      complete:() =>{
        console.log('Completed')
      }
    });
  }

  changePage(pageNumber: number): void {
    this.pageNumber = pageNumber;
    this.loadContacts(this.searchString,pageNumber,this.sortName,this.showFavourite);
  }

  changePageSize(pageSize: number): void {
   
    this.pageSize = pageSize;
    this.pageNumber = 1; // Reset to first page
    this.totalPages = Math.ceil(this.totalItems / this.pageSize); // Recalculate total pages
    this.loadContacts(this.searchString,1,this.sortName,this.showFavourite);
  };

  
    nextPage() {
        if (this.pageNumber < this.totalPages) {
            this.pageNumber++;
            this.loadContacts(this.searchString,this.pageNumber,this.sortName,this.showFavourite);
        }
    }

    previousPage() {
        if (this.pageNumber > 1) {
            this.pageNumber--;
            this.loadContacts(this.searchString,this.pageNumber,this.sortName,this.showFavourite);

        }
    }


  //row to details navigation
  goToDetails(contactId: number): void {
    this.router.navigate(['/contact/contactdetails', contactId]);
  }

  //sort A to Z
  toggleSort() {
    if (this.sortName == 'default') {
      this.sortName = 'asc';
    } 
    else if (this.sortName == 'asc') {
      this.sortName = 'desc'
    }
    else{
      
      this.sortName = 'default';
    }

    this.loadContacts(this.searchString,this.pageNumber,this.sortName,this.showFavourite)
  }

  clearSearch(): void {
    this.searchString = '';
    this.loadContacts(this.searchString, this.pageNumber, this.sortName, this.showFavourite);
  }
}
