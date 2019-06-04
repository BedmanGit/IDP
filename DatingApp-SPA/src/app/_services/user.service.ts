import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { User } from '../_models/user';


@Injectable({
  providedIn: 'root'
})
export class UserService {

baseUrl = environment.baseUrl;
constructor(private http: HttpClient) { }

getUser(id: number): Observable<User> {
  return this.http.get<User>(this.baseUrl + '/users/' + id);
}
getUsers(): Observable<User[]> {
  return this.http.get<User[]>(this.baseUrl + '/users');
}

updateUser(id: number, user: User) {
  return this.http.put(this.baseUrl + '/users/' + id, user);
}

setMainPhoto(userId: number, id: number) {
  return this.http.post(this.baseUrl + '/users/' + userId + '/photos/' + id + '/setMain/', {});
}

deletePhoto(userId: number, id: number) {
  return this.http.delete(this.baseUrl + '/users/' + userId + '/photos/' + id);
}
}
