import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { RatingService } from '../_services/Rating.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { RatingTopicLite } from '../_models/RatingTopic';

@Injectable()
export class RatingListResolver implements Resolve<RatingTopicLite[]> {
  pageNumber = 1;
  pageSize = 10;

  constructor(
    private ratingService: RatingService,
    private router: Router,
    private toastr: ToastrService
  ) {}

  resolve(route: ActivatedRouteSnapshot): Observable<RatingTopicLite[]> {
    return this.ratingService.getTopics(this.pageNumber, this.pageSize)
    .pipe(
      catchError(error => {
        this.toastr.error('Problem retrieving data: Rating');
        this.router.navigate(['home']);
        return of(null);
      })
    );
  }
}