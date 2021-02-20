import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ProviderService {
  baseUrl = environment.apiUrl + 'provider/';


  constructor(private http: HttpClient) { }

  getProviders(){
    return this.http.get(this.baseUrl);
  }

  getProvider(id: any){
    return this.http.get(this.baseUrl + id);
  }
}
