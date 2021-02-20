import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { Vote } from '../_models/Vote';

@Injectable({
  providedIn: 'root'
})
export class ElectionService {
  baseUrl = environment.apiUrl + 'election/';

  constructor(private http: HttpClient ) { }

  getCandidates(){
    return this.http.get(this.baseUrl+"candidates");
  }

  voterExists(userId: any, userName: string){
    return this.http.post<boolean>(this.baseUrl+"voter", {userId: parseInt(userId), userName: userName});
  }


  registerVote(id: number, votes: Vote){
    return this.http.post(this.baseUrl + id, votes);
  }
}
