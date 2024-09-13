export interface UpdateContact{
    contactId:number;
    firstName:string;
    lastName:string;
    email:string;
    company:string;
    contactNumber:string;
    image:any;
    countryId:number;
    stateId:number;
    gender:string;
    isFavourite:boolean;
    birthDate:Date|undefined;
}