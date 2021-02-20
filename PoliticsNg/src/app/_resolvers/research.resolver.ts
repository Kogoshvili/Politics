import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { PostService } from '../_services/Post.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Candidate } from '../_models/Candidate';
import { ElectionService } from '../_services/Election.service';

@Injectable()
export class ResearchResolver implements Resolve<Candidate[]> {

  constructor(
    private electionService: ElectionService,
    private router: Router,
    private toastr: ToastrService
  ) {}

  resolve(route: ActivatedRouteSnapshot): Observable<Candidate[]> {
    return this.electionService.getCandidates()
    .pipe(
      catchError(error => {
        this.toastr.error('Problem retrieving data: Research');
        this.router.navigate(['home']);
        return of(null);
      })
    );
  }
}