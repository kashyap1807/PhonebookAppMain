import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { SigninComponent } from './components/auth/signin/signin.component';
import { SignupComponent } from './components/auth/signup/signup.component';

import { HomeComponent } from './components/home/home.component';
import { PrivacyComponent } from './components/privacy/privacy.component';
import { ContactListComponent } from './components/contact/contact-list/contact-list.component';
import { AddContactComponent } from './components/contact/add-contact/add-contact.component';
import { UpdateContactComponent } from './components/contact/update-contact/update-contact.component';
import { ContactDetailsComponent } from './components/contact/contact-details/contact-details.component';
import { CountryListComponent } from './components/country/country-list/country-list.component';
import { CountryDetailsComponent } from './components/country/country-details/country-details.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AddContactRfComponent } from './components/contact/add-contact-rf/add-contact-rf.component';
import { UpdateContactRfComponent } from './components/contact/update-contact-rf/update-contact-rf.component';
import { AuthService } from './services/auth.service';
import { AuthInterceptor } from './interceptors/auth.interceptor';
import { SignupsuccessComponent } from './components/auth/signupsuccess/signupsuccess.component';
import { FooterComponent } from './components/shared/footer/footer.component';
import { NavbarComponent } from './components/shared/navbar/navbar.component';
import { UserDetailsComponent } from './components/user/user-details/user-details.component';
import { UpdateUserComponent } from './components/user/update-user/update-user.component';
import { ChangepasswordComponent } from './components/auth/changepassword/changepassword.component';
import { ForgotpasswordComponent } from './components/auth/forgotpassword/forgotpassword.component';
import { ContactsByCountryComponent } from './components/report/contacts-by-country/contacts-by-country.component';
import { ContactsByGenderComponent } from './components/report/contacts-by-gender/contacts-by-gender.component';
import { ContactsByStateidComponent } from './components/report/contacts-by-stateid/contacts-by-stateid.component';
import { DatePipe } from '@angular/common';
import { ContactsByMonthComponent } from './components/report/contacts-by-month/contacts-by-month.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    PrivacyComponent,
    ContactListComponent,
    SignupComponent,
    SigninComponent,
    AddContactComponent,
    UpdateContactComponent,
    ContactDetailsComponent,
    CountryListComponent,
    CountryDetailsComponent,
    AddContactRfComponent,
    UpdateContactRfComponent,
    SignupsuccessComponent,
    FooterComponent,
    NavbarComponent,
    UserDetailsComponent,
    UpdateUserComponent,
    ChangepasswordComponent,
    ForgotpasswordComponent,
    ContactsByCountryComponent,
    ContactsByGenderComponent,
    ContactsByStateidComponent,
    ContactsByMonthComponent,
   
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    
  ],
  providers: [DatePipe,AuthService,{provide:HTTP_INTERCEPTORS,useClass:AuthInterceptor, multi: true}],
  bootstrap: [AppComponent]
})
export class AppModule { }
