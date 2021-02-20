import { Component, OnInit } from '@angular/core';
import { AuthService } from './_services/Auth.service';
import { JwtHelperService } from '@auth0/angular-jwt';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'PoliticsNg';
  jwtHelper = new JwtHelperService();

  constructor(private authService: AuthService, private spinner: NgxSpinnerService){}

  ngOnInit(){
    // this.spinner.show();
    const token = localStorage.getItem('token');
    if (token){
      this.authService.decodedToken = this.jwtHelper.decodeToken(token);
    }
  }
}
