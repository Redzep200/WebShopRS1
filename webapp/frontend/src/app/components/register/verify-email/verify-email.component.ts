import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { MyConfig } from 'src/app/my-config';
import { MatDialog } from '@angular/material/dialog';
import { LoginModalComponent } from 'src/app/login-modal/login-modal.component';
import { RegisterComponent } from '../register.component';


@Component({
  selector: 'app-verify-email',
  templateUrl: './verify-email.component.html',
  styleUrls: ['./verify-email.component.css']
})
export class VerifyEmailComponent implements OnInit {
  token!: string;
  verificationStatus!: string;

  constructor(
    private route: ActivatedRoute,
    private http: HttpClient,
    public dialog: MatDialog,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      this.token = params['token'];
      this.verificationStatus = "verifikacija u toku"
      this.verifyEmail();
    });
  }

  verifyEmail(): void {
    this.http.get(`${MyConfig.APIurl}/api/User/VerifyEmail`, { params: { token: this.token } })
      .subscribe(
        (response: any) => {
          this.verificationStatus = response.message || 'Email uspješno verifikovan. Možete se logirati.';
          setTimeout(() => {
            this.router.navigate(['/shop']).then(() => {
              setTimeout(() => this.dialog.open(LoginModalComponent), 100); 
            });
          }, 2000); 
        },
        (error) => {
          this.verificationStatus = error.error || 'Nevaljan ili isteknut verifikacijski token';
          setTimeout(() => this.dialog.open(RegisterComponent), 3000); 
        }
      );
  }
}
