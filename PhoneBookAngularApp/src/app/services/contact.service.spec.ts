import { TestBed } from '@angular/core/testing';

import { ContactService } from './contact.service';

import { HttpClientTestingModule,HttpTestingController } from '@angular/common/http/testing';
import { ApiResponse } from '../models/ApiResponse{T}';
import { Contact } from '../models/contact.model';
import { AddContact } from '../models/add-contact.model';
import { UpdateContact } from '../models/update-contact.model';
import { State } from '../models/state.model';
import { Country } from '../models/country.model';

describe('ContactService', () => {
  let service: ContactService;
  let httpMock :HttpTestingController;
  const mockApiResponse : ApiResponse<Contact[]>={
    success:true,
    data:[
      {contactId: 1,
        firstName: 'Test1',
        lastName: 'TestLast1',
        company: 'Company1',
        email: 'test1@user.com',
        contactNumber: '1234567890',
        fileName: 'SampleImg.png',
        imageBytes:'SampleImg.png',
        gender: 'm',
        stateId: 1,
        countryId: 1,
        isFavourite: true,
        country: {countryId:1,countryName:'country1'},
        state: { stateId:1,stateName:'state1',countryId:1}
      },
      {contactId: 2,
        firstName: 'Test2',
        lastName: 'TestLast2',
        company: 'Company2',
        email: 'test2@user.com',
        contactNumber: '0987654321',
        fileName: 'SampleImg.png',
        imageBytes:'SampleImg.png',
        gender: 'm',
        stateId: 2,
        countryId: 2,
        isFavourite: true,
        country: {countryId:2,countryName:'country2'},
        state: { stateId:2,stateName:'state2',countryId:2}
      },
    ],
    message:''

  }

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports:[HttpClientTestingModule],
      providers:[ContactService]
    });
    service = TestBed.inject(ContactService);
    httpMock=TestBed.inject(HttpTestingController);
  });

  afterEach(()=>{
    httpMock.verify();
  })

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should fetch all contact successfully',()=>{
    //Arrange
    let pageNumber: number = 1;
    let pageSize: number = 10;
    let searchString: string = '';
    let sortName: string = 'default';
    let showFavourite: boolean = false;    
    const apiUrl="http://localhost:5116/api/Contact/GetAllContactsWithPaginationFavSearchSort?page="+pageNumber+"&pageSize="+pageSize+"&search_string="+searchString+"&sort_name="+sortName+"&show_favourites="+showFavourite;

    //Act
    service.getAllContactsWithPaginationFavSearchSort(pageNumber,pageSize,searchString,sortName,showFavourite).subscribe((response)=>{
      //Assert
      expect(response.data.length).toBe(2);
      expect(response.data).toEqual(mockApiResponse.data);

    });

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockApiResponse);

  });

  it('should handle an empty contacts list', () => {

    // Arrange
    let pageNumber: number = 1;
    let pageSize: number = 10;
    let searchString: string = '';
    let sortName: string = 'default';
    let showFavourite: boolean = false;  
    const apiUrl="http://localhost:5116/api/Contact/GetAllContactsWithPaginationFavSearchSort?page="+pageNumber+"&pageSize="+pageSize+"&search_string="+searchString+"&sort_name="+sortName+"&show_favourites="+showFavourite;

    const emptyResponse: ApiResponse<Contact[]> = {
      success: true,
      data: [],
      message:''
    }

    //Act
    service.getAllContactsWithPaginationFavSearchSort(pageNumber,pageSize,searchString,sortName,showFavourite).subscribe((response)=>{
      //Assert
      expect(response.data.length).toBe(0);
      expect(response.data).toEqual([]);

    });

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    req.flush(emptyResponse);
      
  });


  it('should handle HTTP error gracefully', ()=>{
    // Arrange
    let pageNumber: number = 1;
    let pageSize: number = 10;
    let searchString: string = '';
    let sortName: string = 'default';
    let showFavourite: boolean = false;  
    
    const apiUrl="http://localhost:5116/api/Contact/GetAllContactsWithPaginationFavSearchSort?page="+pageNumber+"&pageSize="+pageSize+"&search_string="+searchString+"&sort_name="+sortName+"&show_favourites="+showFavourite;

    const errorMessage ='Failed to load contacts';
    
    // Act
    service.getAllContactsWithPaginationFavSearchSort(pageNumber,pageSize,searchString,sortName,showFavourite).subscribe(
    ()=> fail('expected an error, not contacts'),
    (error) =>{
      // Assert
      expect(error.status).toBe(500);
      expect(error. statusText).toBe('Internal Server Error');
    })

      const req = httpMock.expectOne(apiUrl);
      expect(req.request.method).toBe('GET');
      
      // Respond with error
      req. flush(errorMessage, {status:500, statusText:'Internal Server Error'});
    
  });

  //Add Contact Service Unit Tests
  it('should add contact successfully',()=>{
    //Arrange
    const addContact : AddContact={
      firstName:'NewFirstName',
      lastName:'NewLastName',
      email:'newuser@gmail.com',
      company:'NewCompany',
      contactNumber:'1234567890',
      image:'newfile.png',
      countryId:1,
      stateId:1,
      gender:'male',
      isFavourite:true,
    }

    const formData = new FormData();
    formData.append('firstName','NewFirstName');
    formData.append('lastName','NewLastName');
    formData.append('email','newuser@gmail.com');
    formData.append('company','NewCompany');
    formData.append('contactNumber','1234567890');
    formData.append('image','SampleImg.png');
    formData.append('countryId','1'.toString());
    formData.append('stateId','1'.toString());
    formData.append('gender','');
    formData.append('isFavourite','true');



    const mockSuccessResponse : ApiResponse<string>={
      success:true,
      message:"Contact saved successfully.",
      data:""
    };

    //Act
    service.AddContact(formData).subscribe(response=>{
      //Assert
      expect((response)).toBe(mockSuccessResponse);
    });

    const req = httpMock.expectOne('http://localhost:5116/api/Contact/Create');
    expect(req.request.method).toBe('POST');
    req.flush(mockSuccessResponse);
  });

  it('should handle failed contact addition',()=>{
    //Arrange
    const addContact : AddContact={
      firstName:'New FirstName',
      lastName:'New LastName',
      email:'newuser@gmail.com',
      company:'New Company',
      contactNumber:'1234567890',
      image:'newfile',
      countryId:1,
      stateId:1,
      gender:'male',
      isFavourite:true,
    }

    const formData = new FormData();
    formData.append('firstName','NewFirstName');
    formData.append('lastName','NewLastName');
    formData.append('email','newuser@gmail.com');
    formData.append('company','NewCompany');
    formData.append('contactNumber','1234567890');
    formData.append('image','SampleImg.png');
    formData.append('countryId','1'.toString());
    formData.append('stateId','1'.toString());
    formData.append('gender','');
    formData.append('isFavourite','true');

    const mockErrorResponse : ApiResponse<string>={
      success:false,
      message:"Contact already exists.",
      data:""
    };

    //Act
    service.AddContact(formData).subscribe(response=>{
      //Assert
      expect((response)).toBe(mockErrorResponse);
    });

    const req = httpMock.expectOne('http://localhost:5116/api/Contact/Create');
    expect(req.request.method).toBe('POST');
    req.flush(mockErrorResponse);
  });

  it('should handle http error of add',()=>{
    //Arrange
    const addContact : AddContact={
      firstName:'New FirstName',
      lastName:'New LastName',
      email:'newuser@gmail.com',
      company:'New Company',
      contactNumber:'1234567890',
      image:'SampleImg.png',
      countryId:1,
      stateId:1,
      gender:'male',
      isFavourite:true,
    };
    const formData = new FormData();
    formData.append('firstName','NewFirstName');
    formData.append('lastName','NewLastName');
    formData.append('email','newuser@gmail.com');
    formData.append('company','NewCompany');
    formData.append('contactNumber','1234567890');
    formData.append('image','SampleImg.png');
    formData.append('countryId','1'.toString());
    formData.append('stateId','1'.toString());
    formData.append('gender','');
    formData.append('isFavourite','true');
    const mockHttpError = {
      status:500,
      statusText:'Internal Server error'
    };

    //Act 
    service.AddContact(formData).subscribe({
      next:()=> fail('should have failed with the 500 error'),
      error : (error)=>{
        //Assert
        expect(error.status).toEqual(500);
        expect(error.statusText).toEqual('Internal Server error');
      }
    });
    const req = httpMock.expectOne('http://localhost:5116/api/Contact/Create');
    expect(req.request.method).toBe('POST');
    req.flush({},mockHttpError);

  });

  //Update Contact Unit Tests
  it('should update a contact successfully',()=>{
    //Arrange
    const updateContact: UpdateContact={
      contactId:1,
      firstName:'Update FirstName',
      lastName:'Update LastName',
      email:'newuser@gmail.com',
      company:'Update Company',
      contactNumber:'1234567890',
      image:'UpdateFile',
      countryId:1,
      stateId:1,
      gender:'male',
      isFavourite:true,
    };
    const formData = new FormData();
    formData.append('contactId','1'.toString());
    formData.append('firstName','NewFirstName');
    formData.append('lastName','NewLastName');
    formData.append('email','newuser@gmail.com');
    formData.append('company','NewCompany');
    formData.append('contactNumber','1234567890');
    formData.append('image','SampleImg.png');
    formData.append('countryId','1'.toString());
    formData.append('stateId','1'.toString());
    formData.append('gender','');
    formData.append('isFavourite','true');


    const mockSuccessResponse : ApiResponse<string>={
      success:true,
      data:'',
      message:"Contact updated successfully."
    };

    //Act
    service.UpdateContact(formData).subscribe(response=>{
      //Assert
      expect(response).toEqual(mockSuccessResponse);
    });

    const req = httpMock.expectOne('http://localhost:5116/api/Contact/ModifyContact');
    expect(req.request.method).toBe('PUT');
    req.flush(mockSuccessResponse);
  });

  it('should update a contact failed',()=>{
    //Arrange
    const updateContact: UpdateContact={
      contactId:1,
      firstName:'Update FirstName',
      lastName:'Update LastName',
      email:'newuser@gmail.com',
      company:'Update Company',
      contactNumber:'1234567890',
      image:'UpdateFile',
      countryId:1,
      stateId:1,
      gender:'male',
      isFavourite:true,
    };
    const formData = new FormData();
    formData.append('contactId','1'.toString());
    formData.append('firstName','NewFirstName');
    formData.append('lastName','NewLastName');
    formData.append('email','newuser@gmail.com');
    formData.append('company','NewCompany');
    formData.append('contactNumber','1234567890');
    formData.append('image','SampleImg.png');
    formData.append('countryId','1'.toString());
    formData.append('stateId','1'.toString());
    formData.append('gender','');
    formData.append('isFavourite','true');
    

    const mockErrorResponse : ApiResponse<string>={
      success:false,
      data:'',
      message:"Contact already exists."
    };

    //Act
    service.UpdateContact(formData).subscribe(response=>{
      //Assert
      expect(response).toEqual(mockErrorResponse);
    });

    const req = httpMock.expectOne('http://localhost:5116/api/Contact/ModifyContact');
    expect(req.request.method).toBe('PUT');
    req.flush(mockErrorResponse);
  });

  it('should handle http error of update',()=>{
    //Arrange
    const updateContact: UpdateContact={
      contactId:1,
      firstName:'Update FirstName',
      lastName:'Update LastName',
      email:'newuser@gmail.com',
      company:'Update Company',
      contactNumber:'1234567890',
      image:'UpdateFile',
      countryId:1,
      stateId:1,
      gender:'male',
      isFavourite:true,
    };
    const formData = new FormData();
    formData.append('contactId','1'.toString());
    formData.append('firstName','NewFirstName');
    formData.append('lastName','NewLastName');
    formData.append('email','newuser@gmail.com');
    formData.append('company','NewCompany');
    formData.append('contactNumber','1234567890');
    formData.append('image','SampleImg.png');
    formData.append('countryId','1'.toString());
    formData.append('stateId','1'.toString());
    formData.append('gender','');
    formData.append('isFavourite','true');
    const mockHttpError = {
      status:500,
      statusText:'Internal Server error'
    };

    //Act 
    service.UpdateContact(formData).subscribe({
      next:()=> fail('should have failed with the 500 error'),
      error : (error)=>{
        //Assert
        expect(error.status).toEqual(500);
        expect(error.statusText).toEqual('Internal Server error');
      }
    });
    const req = httpMock.expectOne('http://localhost:5116/api/Contact/ModifyContact');
    expect(req.request.method).toBe('PUT');
    req.flush({},mockHttpError);

  });

  //Get ContactById Unit Tests
  it('should fetch a contact by id successfully',()=>{
    //Arrange
    const contactId = 1;
    const mockSuccessResponse : ApiResponse<Contact>={
      success:true,
      data:{
        contactId:1,
        firstName:'Update FirstName',
        lastName:'Update LastName',
        email:'newuser@gmail.com',
        company:'Update Company',
        contactNumber:'1234567890',
        fileName:'UpdateFile',
        imageBytes:'SampleImg.png',
        countryId:1,
        stateId:1,
        gender:'male',
        isFavourite:true,
        country : {} as Country,
        state: {} as State,
      },
      message : ''
    };
    //Act
    service.getContactById(contactId).subscribe(response=>{
      //Assert
      expect(response.success).toBeTrue();
      expect(response.message).toBe('');
      expect(response.data).toEqual(mockSuccessResponse.data);
      expect(response.data.contactId).toEqual(contactId);
    });
    const req =httpMock.expectOne('http://localhost:5116/api/Contact/GetContactById/'+contactId);
    expect(req.request.method).toBe('GET');
    req.flush(mockSuccessResponse);
  });

  it('should handle failed contact retrival',()=>{
    //Arrange
    const contactId = 1;
    const mockErrorResponse : ApiResponse<Contact>={
      success:false,
      data: {} as Contact,
      message : ''
    };
    //Act
    service.getContactById(contactId).subscribe(response=>{
      //Assert
      expect(response.success).toBeFalse();
      expect(response.message).toEqual('');
      expect(response.data).toEqual(mockErrorResponse.data);
      
    });
    const req =httpMock.expectOne('http://localhost:5116/api/Contact/GetContactById/'+contactId);
    expect(req.request.method).toBe('GET');
    req.flush(mockErrorResponse);
  });

  it('should handle http error of get',()=>{
    //Arrange
    const contactId = 1;
    const mockHttpError ={
      status:500,
      statusText:"Internal Server error"
    }
    //Act
    service.getContactById(contactId).subscribe({
      next:()=> fail('should have failed with the 500 error'),
      error : (error)=>{
        //Assert
        expect(error.status).toEqual(500);
        expect(error.statusText).toEqual('Internal Server error');
      }
    });
    const req =httpMock.expectOne('http://localhost:5116/api/Contact/GetContactById/'+contactId);    
    expect(req.request.method).toBe('GET');
    req.flush({},mockHttpError);
    
  });

  //Delete Contact Unit Tests
  it('should delete contact by id successfully',()=>{
    //Arrange
    const contactId = 1;
    const mockSuccessResponse : ApiResponse<string>={
      success:true,
      data:'',
      message:'Contact deleted successfully.'
    };
    //Act
    service.deleteContactById(contactId).subscribe(response=>{
      //Assert
      expect(response).toEqual(mockSuccessResponse);
    });   
    const req = httpMock.expectOne('http://localhost:5116/api/Contact/Remove/'+contactId);
    expect(req.request.method).toBe('DELETE');
    req.flush(mockSuccessResponse);
  });
  it('should not delete contact by id ',()=>{
    //Arrange
    const contactId = 1;
    const mockErrorResponse : ApiResponse<string>={
      success:true,
      data:'',
      message:'Something went wrong, please try after sometime.'
    };
    //Act
    service.deleteContactById(contactId).subscribe(response=>{
      //Assert
      expect(response).toEqual(mockErrorResponse);
    });   
    const req = httpMock.expectOne('http://localhost:5116/api/Contact/Remove/'+contactId);
    expect(req.request.method).toBe('DELETE');
    req.flush(mockErrorResponse);
  });
  it('should handle http error for delete',()=>{
    //Arrange
    const contactId=1;
    const mockHttpError ={
      status:500,
      statusText:"Internal server error"  
    };
    //Act
    service.deleteContactById(contactId).subscribe({
      next:()=>fail('should have failed with the 500 error'),
      error : (error)=>{
        //Assert
        expect(error.status).toEqual(500);
        expect(error.statusText).toEqual('Internal server error');
      }
    });
    const req = httpMock.expectOne('http://localhost:5116/api/Contact/Remove/'+contactId);
    expect(req.request.method).toBe('DELETE');
    req.flush({},mockHttpError);    
  });  

});
