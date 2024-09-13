import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { User } from 'src/app/models/user.model';
import { AuthService } from 'src/app/services/auth.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-update-user',
  templateUrl: './update-user.component.html',
  styleUrls: ['./update-user.component.css']
})
export class UpdateUserComponent implements OnInit{

  loginId : string | undefined | null;

  loading:boolean=false;
  user : User={
    userId:0,
    firstName:'',
    lastName:'',
    loginId:'',
    email:'',
    contactNumber:'',
  }
  
  constructor(private authService:AuthService,private cdr:ChangeDetectorRef,private userService:UserService,private route:ActivatedRoute,private router:Router){}

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

  onSubmit(updateUserTFForm:NgForm):void{
    if (updateUserTFForm.valid) {
      this.loading = true;
      console.log(updateUserTFForm.value);
      this.userService.UpdateUser(this.user).subscribe({
        next:(response)=>{
          if(response.success){
            this.username = this.user.loginId
            this.authService.UpdateUserDetails(this.username);
            this.router.navigate(['/userdetails',this.username]);
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
      })
      
    }
  }

  signOut(){
    this.authService.signOut();
  }


}
