import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ChnagePass } from 'src/app/models/changepassword.model';
import { User } from 'src/app/models/user.model';
import { AuthService } from 'src/app/services/auth.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-changepassword',
  templateUrl: './changepassword.component.html',
  styleUrls: ['./changepassword.component.css']
})
export class ChangepasswordComponent implements OnInit{
  loginId : string | undefined |null;
  changePass : ChnagePass={
    loginId:'',
    oldPassword:'',
    newPassword:'',
    confirmNewPassword:''
  }
  user : User={
    userId:0,
    firstName:'',
    lastName:'',
    loginId:'',
    email:'',
    contactNumber:'',
  }

  loading: boolean = false;

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
    this.userService.getUserByLoginId(loginId).subscribe({
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

  checkPasswords(form: NgForm):void {
    const password = form.controls['password'];
    const confirmPassword = form.controls['confirmPassword'];
 
    if (password && confirmPassword && password.value !== confirmPassword.value) {
      confirmPassword.setErrors({ passwordMismatch: true });
    } else {
      confirmPassword.setErrors(null);
    }
  }

  onSubmit(changePassForm: NgForm): void {
    if (changePassForm.valid) {
      this.loading = true;
      this.changePass.loginId = this.user.loginId;
      console.log(changePassForm.value);

      this.authService.changePassword(this.changePass).subscribe({
        next:(response) => {
          if(response.success) {
            this.authService.signOut();
            this.router.navigate(['/signin'])
          } else {
            alert(response.message);
          }
          this.loading = false;
        },
        error:(err) => {
          alert(err.error.message);
          this.loading = false;
        }
      })
    }
  }



}
