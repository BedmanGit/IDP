import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { AppUser } from '../_models/AppUser';


@Injectable({
  providedIn: 'root'
})
export class UserService {

baseUrl = environment.baseUrl;
constructor(private http: HttpClient) { }

getUser(id: number): Observable<AppUser> {
  return this.http.get<AppUser>(this.baseUrl + '/users/' + id);
}
getUsers(): Observable<AppUser[]> {
  return this.http.get<AppUser[]>(this.baseUrl + '/users');
}

updateUser(id: number, user: AppUser) {
  return this.http.put(this.baseUrl + '/users/' + id, user);
}

setMainPhoto(userId: number, id: number) {
  return this.http.post(this.baseUrl + '/users/' + userId + '/photos/' + id + '/setMain/', {});
}

deletePhoto(userId: number, id: number) {
  return this.http.delete(this.baseUrl + '/users/' + userId + '/photos/' + id);
}
}
