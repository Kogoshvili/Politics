import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from '../_services/Auth.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(
    private authService: AuthService, 
    private toastr: ToastrService, 
    private router: Router
  ){}

  canActivate(): boolean {
    if (this.authService.loggedIn()){
      return true;
    }

    this.toastr.error('NONONONO!');
    this.router.navigate(['/home']);
    return false;
  }
  
}
