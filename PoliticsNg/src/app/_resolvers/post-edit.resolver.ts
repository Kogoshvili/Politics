import { Injectable } from '@angular/core';
import { Post } from '../_models/Post';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { PostService } from '../_services/Post.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()
export class PostEditResolver implements Resolve<Post> {
  constructor(
    private postService: PostService,
    private router: Router,
    private toastr: ToastrService
  ) {}

  resolve(route: ActivatedRouteSnapshot): Observable<Post> {
    return this.postService.getPost(route.params[`id`]).pipe(
      catchError(error => {
        this.toastr.error('Problem retrieving data: Post Edit');
        this.router.navigate(['/home']);
        return of(null);
      })
    );
  }
}