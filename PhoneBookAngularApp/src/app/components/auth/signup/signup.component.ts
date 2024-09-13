import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css']
})
export class SignupComponent {
  user = {
    userId: 0,
    firstName: "",
    lastName: "",
    loginId: "",
    email: "",
    contactNumber:"",
    password: "",
    confirmPassword:""
  };

  loading: boolean = false;
    constructor(private authService: AuthService, private router: Router){}

    checkPasswords(form: NgForm):void {
      const password = form.controls['password'];
      const confirmPassword = form.controls['confirmPassword'];
   
      if (password && confirmPassword && password.value !== confirmPassword.value) {
        confirmPassword.setErrors({ passwordMismatch: true });
      } else {
        confirmPassword.setErrors(null);
      }
    }

    onSubmit(signUpForm: NgForm): void {
      if (signUpForm.valid) {
        this.loading = true;
        console.log(signUpForm.value);
        this.authService.signUp(this.user).subscribe({
          next:(response) => {
            if(response.success) {
              this.router.navigate(['/signupsuccess'])
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
