import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MyConfig } from 'src/app/my-config';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  constructor(private fb: FormBuilder, private http: HttpClient, private userService:UserService) {}
  registerForm!: FormGroup;
  name: string = '';
  surname: string = '';
  email: string = '';
  username: string = '';
  password: string = '';
  registrationMessage: string = '';
  registrationError: string | null = null;

  ngOnInit(): void {
    this.registerForm = this.fb.group({
      name: ['', Validators.required],
      surname: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  register() {
    if (this.registerForm.invalid) {
      this.registrationError = "Nepravilno uneseni podaci!";
      this.registrationMessage = "";
      return;
    }
  
    this.userService.hasEmailRegistered(this.email).subscribe(
      (emailRegistered) => {
        if (emailRegistered) {
          this.registrationError = "Email je već registrovan. Molimo koristite drugi email.";
          this.registrationMessage = "";
        } else {
          this.userService.hasUsernameRegistered(this.username).subscribe(
            (usernameRegistered) => {
              if (usernameRegistered) {
                this.registrationError = "Korisničko ime je već registrovano. Molimo izaberite drugo korisničko ime.";
                this.registrationMessage = "";
              } else {
                this.http.post(MyConfig.APIurl + '/api/User/RegisterUser', {
                  name: this.name,
                  surname: this.surname,
                  email: this.email,
                  username: this.username,
                  password: this.password
                }).subscribe(
                  (response: any) => {
                    console.log('Registration successful:', response);
                    this.registrationError = null;
                    this.registrationMessage = "Registracija uspješna! Molimo provjerite svoj email za verifikaciju računa.";
                  },
                  (error) => {
                    console.error('Registration error:', error);
                    this.registrationError = "Došlo je do greške prilikom registracije. Molimo pokušajte ponovno.";
                    this.registrationMessage = "";
                  }
                );
              }
            },
            (error) => {
              console.error('Username check error:', error);
              this.registrationError = "Došlo je do greške prilikom provjere korisničkog imena. Molimo pokušajte ponovno.";
              this.registrationMessage = "";
            }
          );
        }
      },
      (error) => {
        console.error('Email check error:', error);
        this.registrationError = "Došlo je do greške prilikom provjere e-maila. Molimo pokušajte ponovno.";
        this.registrationMessage = "";
      }
    );
  }
}