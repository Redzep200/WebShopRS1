import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MyConfig } from 'src/app/my-config';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit{
constructor(private fb:FormBuilder, private http: HttpClient){}
loginForm!:FormGroup
emailModel!:string
passwordModel!:string

ngOnInit(): void {
  this.loginForm = this.fb.group({
    email: ['',Validators.required],
    password: ['',Validators.required]
  });
}

login(){
 if(this.loginForm.invalid){
  alert("Nepravilno uneseni podaci!");
  return;
 }
 this.http.post(MyConfig.APIurl+'/api/User/Authenticate',{
  "email": this.emailModel,
  "password": this.passwordModel
 }).subscribe((response)=>
 console.log(response),
 (error)=>
 console.log(error));
console.log(this.emailModel,this.passwordModel);
}
}
