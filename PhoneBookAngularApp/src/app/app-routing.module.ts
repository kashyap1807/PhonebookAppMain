import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SigninComponent } from './components/auth/signin/signin.component';
import { SignupComponent } from './components/auth/signup/signup.component';
import { ContactListComponent } from './components/contact/contact-list/contact-list.component';
import { HomeComponent } from './components/home/home.component';
import { PrivacyComponent } from './components/privacy/privacy.component';
import { AddContactComponent } from './components/contact/add-contact/add-contact.component';
import { ContactDetailsComponent } from './components/contact/contact-details/contact-details.component';
import { UpdateContactComponent } from './components/contact/update-contact/update-contact.component';
import { AddContactRfComponent } from './components/contact/add-contact-rf/add-contact-rf.component';
import { UpdateContactRfComponent } from './components/contact/update-contact-rf/update-contact-rf.component';
import { SignupsuccessComponent } from './components/auth/signupsuccess/signupsuccess.component';
import { authGuard } from './guards/auth.guard';
import { UserDetailsComponent } from './components/user/user-details/user-details.component';
import { UpdateUserComponent } from './components/user/update-user/update-user.component';
import { ChangepasswordComponent } from './components/auth/changepassword/changepassword.component';
import { ForgotpasswordComponent } from './components/auth/forgotpassword/forgotpassword.component';
import { ContactsByCountryComponent } from './components/report/contacts-by-country/contacts-by-country.component';
import { ContactsByGenderComponent } from './components/report/contacts-by-gender/contacts-by-gender.component';
import { ContactsByStateidComponent } from './components/report/contacts-by-stateid/contacts-by-stateid.component';
import { ContactsByMonthComponent } from './components/report/contacts-by-month/contacts-by-month.component';



const routes: Routes = [
  {path:'',redirectTo:'home',pathMatch:'full'},
  {path:'home',component:HomeComponent},
  {path:'privacy',component:PrivacyComponent},

  {path:'contact',component:ContactListComponent,canActivate:[authGuard]},
  {path:'contact/addcontact',component:AddContactComponent,canActivate:[authGuard]},
  {path:'contact/contactdetails/:contactId',component:ContactDetailsComponent},
  {path:'contact/updatecontact/:contactId',component:UpdateContactComponent,canActivate:[authGuard]},

  {path:'contact/addcontactrf',component:AddContactRfComponent,canActivate:[authGuard]},
  {path:'contact/updatecontactrf/:contactId',component:UpdateContactRfComponent,canActivate:[authGuard]},



  
  {path:'signup',component:SignupComponent},
  {path:'signin',component:SigninComponent},
  {path:'signupsuccess',component:SignupsuccessComponent},
  {path:'changepassword/:id',component:ChangepasswordComponent},


  {path:'userdetails/:id',component:UserDetailsComponent},
  {path:'updateuser/:id',component:UpdateUserComponent},

  {path:'forgetpassword',component:ForgotpasswordComponent},

  {path:'conatctsbycountry',component:ContactsByCountryComponent},
  {path:'conatctsbygender',component:ContactsByGenderComponent},
  {path:'conatctsbystate',component:ContactsByStateidComponent},
  {path:'conatctsbymonth',component:ContactsByMonthComponent},




  
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
