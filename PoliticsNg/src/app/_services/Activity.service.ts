import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ActivityService {
  baseUrl = environment.apiUrl + 'activities/';

  constructor(private http: HttpClient) { }

  getActivity(){
    return this.http.get(this.baseUrl);
  }

  likeActivity(userId: any, activityId: any){
    return this.http.get(this.baseUrl + userId + '/like/' + activityId);
  }

  dislikeActivity(userId: any, activityId: any){
    return this.http.get(this.baseUrl + userId + '/dislike/' + activityId);
  }

  commentActivity(userId: any, activityId: any, comment: any){
    return this.http.post(this.baseUrl + userId + "/comment/" + activityId, comment);
  }

  likeComment(userId: any, commentId: any){
    return this.http.get(this.baseUrl + userId + '/likecomment/' + commentId);
  }
}
