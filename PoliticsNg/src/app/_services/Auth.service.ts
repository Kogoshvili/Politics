import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
@Injectable({
  providedIn: 'root'
})
export class AuthService {
  baseUrl = environment.apiUrl + 'auth/';
  jwtHelper = new JwtHelperService();
  decodedToken: any;

  constructor(private http: HttpClient ) { }

  login(model: any){
    return this.http.post(this.baseUrl + 'login', model)
      .pipe(
        map((res: any) => {
          const user = res;
          if (user) {
            localStorage.setItem('token', user.token);
            this.decodedToken = this.jwtHelper.decodeToken(user.token);
          }
        })
      );
  }

  sendSMS(phone: any): Observable<string> {
    phone.phone = phone.phone.replace(/\s/g,'');
    if(!phone.phone.includes("+995") && phone.phone.leangth == 9)
    {
      phone.phone = "+995"+phone.phone;
    }
    return this.http.post<string>(this.baseUrl + 'phone', phone);
  }

  register(model: any){
    return this.http.post(this.baseUrl + 'register', model)      
    .pipe(
      map((res: any) => {
        const user = res;
        if (user) {
          localStorage.setItem('token', user.token);
          this.decodedToken = this.jwtHelper.decodeToken(user.token);
        }
      })
    );
  }

  loggedIn(){
    const token = localStorage.getItem('token');
    return !this.jwtHelper.isTokenExpired(token);
  }
  
}
