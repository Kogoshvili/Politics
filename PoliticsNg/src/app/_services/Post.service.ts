import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Post } from '../_models/Post';
import { PaginatedResult } from '../_models/Pagination';
import { map } from 'rxjs/operators';
import { Category } from '../_models/Category';
import { Provider } from '../_models/Provider';
import { PostFull } from '../_models/PostFull';

@Injectable({
  providedIn: 'root'
})
export class PostService {
  baseUrl = environment.apiUrl + 'posts/';

  constructor(private http: HttpClient) { }

  getPosts(page?, itemsPerPage?, provider?, category?): Observable<PaginatedResult<Post[]>> {
    const paginatedResult: PaginatedResult<Post[]> = new PaginatedResult<Post[]>();

    let params = new HttpParams();

    if (page != null && itemsPerPage != null) {
      params = params.append('pageNumber', page);
      params = params.append('pageSize', itemsPerPage);
    }

    if (provider != null) {
      params = params.append('provider', provider);
    }
    if (category != null) {
      params = params.append('category', category);
    }

    return this.http
      .get<Post[]>(this.baseUrl, { observe: 'response', params })
      .pipe(
        map(response => {
          paginatedResult.result = response.body;
          if (response.headers.get('Pagination') != null) {
            paginatedResult.pagination = JSON.parse(
              response.headers.get('Pagination')
            );
          }
          return paginatedResult;
        })
      );
  }

  getPost(id): Observable<PostFull> {
    return this.http.get<PostFull>(this.baseUrl + id);
  }

  createPost(userId: number, post: Post) {
    return this.http.post(this.baseUrl + userId, post);
  }

  updatePost(userId: number, post: Post) {
    return this.http.put(this.baseUrl + userId, post);
  }

  deletePost(userId: number, postId: number) {
    return this.http.delete(this.baseUrl + userId + '/delete/' + postId);
  }

  getCategories(): Observable<Category[]> {
    return this.http.get<Category[]>(this.baseUrl + 'categories');
  }

  getProviderss(): Observable<Provider[]> {
    return this.http.get<Provider[]>(this.baseUrl + 'providers');
  }

  likePost(userId: any, postId: any){
    return this.http.get(this.baseUrl + userId + '/like/' + postId);
  }
}
