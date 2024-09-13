import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';
import { LocalstorageService } from 'src/app/services/helpers/localstorage.service';
import { LocalStorageKeys } from 'src/app/services/helpers/localstoragekeys';

@Component({
  selector: 'app-signin',
  templateUrl: './signin.component.html',
  styleUrls: ['./signin.component.css']
})
export class SigninComponent {
  loading:boolean = false;
  username: string='';
  password: string='';

  constructor(private authService:AuthService,private localStorageHelper:LocalstorageService,private router:Router,){}

  login(){
    this.loading=true;

    this.authService.signIn(this.username,this.password).subscribe({
      next:(response)=>{
        if (response.success) {
          this.localStorageHelper.setIitem(LocalStorageKeys.TokenName,response.data);
          this.localStorageHelper.setIitem(LocalStorageKeys.UserId,this.username);
          this.router.navigate(['/']);
        }else{
          alert(response.message);
        }
        this.loading=false;
      },
      error:(err)=>{
        alert(err.error.message);
        this.loading=false;
      },
    });
  }
}
