import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs/internal/Observable';
import { PaginatedResult } from '../_models/Pagination';
import { RatingTopicLite, TopicComment } from '../_models/RatingTopic';
import { map } from 'rxjs/operators';
import { identifierModuleUrl } from '@angular/compiler';

@Injectable({
  providedIn: 'root'
})
export class RatingService {
  baseUrl = environment.apiUrl + 'ratings/';

  constructor(private http: HttpClient) { }

  getTopics(page?, itemsPerPage?) : Observable<PaginatedResult<RatingTopicLite[]>>
  {
    const paginatedResult: PaginatedResult<RatingTopicLite[]> = new PaginatedResult<RatingTopicLite[]>();
    let params = new HttpParams();

    if (page != null && itemsPerPage != null) {
      params = params.append('pageNumber', page);
      params = params.append('pageSize', itemsPerPage);
    }
    
    return this.http.get<RatingTopicLite[]>(this.baseUrl, { observe: 'response', params })
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
  
  likeTopic(userId: any, topicId: any){
    return this.http.get(this.baseUrl + userId + '/like/' + topicId);
  }

  topicComments(id: any){
    return this.http.get<TopicComment[]>(this.baseUrl + id + '/comments', );
  }

  commentTopic(userId: any, topicId: any, comment: any){
    return this.http.post(this.baseUrl + userId + "/comment/" + topicId, comment);
  }

  offerTopic(topic: any){
    return this.http.post(this.baseUrl + 'offertopic', topic);
  }

  likeComment(userId: any, commentId: any){
    return this.http.get(this.baseUrl + userId + '/likecomment/' + commentId);
  }

}


