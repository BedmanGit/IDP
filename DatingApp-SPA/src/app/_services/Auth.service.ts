import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import {map} from 'rxjs/operators';
import {JwtHelperService} from '@auth0/angular-jwt';
import { User } from '../_models/user';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
baseUrl: string = environment.baseUrl + '/auth/';
jwtHelper = new JwtHelperService();
decodedToken: any;
currentUser: User;
userPhotoUrl = new BehaviorSubject<string>('../../assets/user.png');
currentUserPhotoUrl = this.userPhotoUrl.asObservable();

changeMemberProfilePicture(profilePictureUrl: string) {
  this.userPhotoUrl.next(profilePictureUrl);
}

constructor(private http: HttpClient) { }
  login(model: any) {
    return this.http.post(this.baseUrl + 'login', model)
      .pipe(
        map((response: any) => {
          const resp = response;
          if (resp) {
            localStorage.setItem('token', resp.token);
            localStorage.setItem('user', JSON.stringify(resp.user));
            this.decodedToken = this.jwtHelper.decodeToken(resp.token);
            this.currentUser = resp.user;
            this.changeMemberProfilePicture(this.currentUser.photoUrl);
          }
        })
      );
  }


  register(model: any) {
    return this.http.post(this.baseUrl + 'Register', model);
  }

  loggedIn() {
    const token = localStorage.getItem('token');
    return !this.jwtHelper.isTokenExpired(token);
  }
}
