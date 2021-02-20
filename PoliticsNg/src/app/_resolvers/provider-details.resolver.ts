import { Injectable } from '@angular/core';
import { Post } from '../_models/Post';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { ProviderService } from '../_services/Provider.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Provider } from '../_models/Provider';

@Injectable()
export class ProviderDetailResolver implements Resolve<Provider> {
  
  constructor(
    private providerService: ProviderService,
    private router: Router,
    private toastr: ToastrService
  ) {}

  resolve(route: ActivatedRouteSnapshot): Observable<Provider> {
    return this.providerService.getProvider(route.params[`provider`]).pipe(
      catchError(error => {
        this.toastr.error('Problem retrieving data: Provider');
        this.router.navigate(['/home']);
        return of(null);
      })
    );
  }
}