import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { User } from 'src/app/models/user.model';
import { AuthService } from 'src/app/services/auth.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-user-details',
  templateUrl: './user-details.component.html',
  styleUrls: ['./user-details.component.css']
})
export class UserDetailsComponent implements OnInit{

  loginId:string|undefined|null;

  user : User={
    userId:0,
    firstName:'',
    lastName:'',
    loginId:'',
    email:'',
    contactNumber:'',
  }

  constructor(private authService:AuthService,private cdr:ChangeDetectorRef,private userService:UserService,private route:ActivatedRoute){}

  isAuthenticated: boolean = false;
  username : string|null|undefined;

  ngOnInit(): void {
    this.authService.isAuthenticated().subscribe((authState:boolean)=>{
      this.isAuthenticated = authState;
      this.cdr.detectChanges(); // Manually trigger change detection
    });
    this.authService.getUserName().subscribe((username : string|null|undefined)=>{
      this.username = username;
      this.cdr.detectChanges()
    });
    this.route.params.subscribe((params)=>{
      this.loginId = params['id'];
      this.loadUserDetails(this.loginId);
    });
  }

  loadUserDetails(loginId:string|undefined|null):void{
    this.userService.getUserByLoginId(this.loginId).subscribe({
      next:(response)=>{
        if (response.success) {
          this.user = response.data;
        }
        else{
          console.log('Failed to fetch user',response.message);
          
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
