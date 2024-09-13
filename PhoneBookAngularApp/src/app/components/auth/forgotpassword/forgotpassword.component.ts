import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ForgotPass } from 'src/app/models/forgotpassword.model';
import { User } from 'src/app/models/user.model';
import { AuthService } from 'src/app/services/auth.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-forgotpassword',
  templateUrl: './forgotpassword.component.html',
  styleUrls: ['./forgotpassword.component.css']
})
export class ForgotpasswordComponent implements OnInit{

  loginId : string | undefined|null;
  forgotPass : ForgotPass={
    loginId:'',
    email:'',
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

  onSubmit(forgotPassForm:NgForm):void{
    if (forgotPassForm.valid) {
      this.loading=true;
      this.forgotPass.loginId = this.user.loginId;
      console.log(forgotPassForm.value);
      this.authService.forgotPassword(this.forgotPass).subscribe({
        next:(response) => {
          if(response.success) {
            
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
