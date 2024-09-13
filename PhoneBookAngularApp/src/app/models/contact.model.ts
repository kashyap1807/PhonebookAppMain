import { Country } from "./country.model";
import { State } from "./state.model";

export interface Contact{
    contactId:number;
    firstName:string;
    lastName:string;
    email:string;
    company:string;
    contactNumber:string;
    fileName:string;
    imageBytes:any;
    countryId:number;
    stateId:number;
    gender:string;
    isFavourite:boolean;
    country:Country;
    state:State;
    birthDate:Date|undefined;
}