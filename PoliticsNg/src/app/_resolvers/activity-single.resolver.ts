import { Injectable } from '@angular/core';
import { Post } from '../_models/Post';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { ActivityService } from '../_services/Activity.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Activity } from '../_models/Activity';

@Injectable()
export class ActivitySingleResolver implements Resolve<Activity> {
  constructor(
    private activityService: ActivityService,
    private router: Router,
    private toastr: ToastrService
  ) {}

  resolve(route: ActivatedRouteSnapshot): Observable<Activity> {
    return this.activityService.getActivity().pipe(
      catchError(error => {
        this.toastr.error('Problem retrieving data: Activicy-Single');
        this.router.navigate(['/home']);
        return of(null);
      })
    );
  }
}