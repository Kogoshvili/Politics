import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { PostService } from '../_services/Post.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Post } from '../_models/Post';

@Injectable()
export class PostListResolver implements Resolve<Post[]> {
  pageNumber = 1;
  pageSize = 10;

  constructor(
    private postService: PostService,
    private router: Router,
    private toastr: ToastrService
  ) {}

  resolve(route: ActivatedRouteSnapshot): Observable<Post[]> {
    return this.postService.getPosts(this.pageNumber, this.pageSize, route.params['provider'])
    .pipe(
      catchError(error => {
        this.toastr.error('Problem retrieving data: Post-List');
        // this.router.navigate(['/']);
        return of(null);
      })
    );
  }
}