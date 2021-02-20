import { Injectable } from '@angular/core';
import { User } from '../_models/User';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from '../_services/Auth.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { UserService } from '../_services/User.service';

@Injectable()
export class UserEditResolver implements Resolve<User> {
  constructor(
    private authService: AuthService,
    private userService: UserService,
    private router: Router,
    private toastr: ToastrService
  ) {}

  resolve(route: ActivatedRouteSnapshot): Observable<User> {
    let id = this.authService.decodedToken.nameid;
    return this.userService.getUser(id).pipe(
      catchError(error => {
        this.toastr.error('Problem retrieving data: User');
        this.router.navigate(['/home']);
        return of(null);
      })
    );
  }
}