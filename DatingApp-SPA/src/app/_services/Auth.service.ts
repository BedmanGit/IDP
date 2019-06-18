import { Injectable, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import {map} from 'rxjs/operators';
import {JwtHelperService} from '@auth0/angular-jwt';
import { AppUser } from '../_models/AppUser';
import { BehaviorSubject, from } from 'rxjs';
import {UserManager, User, WebStorageStateStore} from 'oidc-client';
import { UserService } from './user.service';
@Injectable({
  providedIn: 'root'
})
export class AuthService {

baseUrl: string = environment.baseUrl + '/auth/';
jwtHelper = new JwtHelperService();
decodedToken: any;
currentUser: AppUser;
userPhotoUrl = new BehaviorSubject<string>('../../assets/user.png');
currentUserPhotoUrl = this.userPhotoUrl.asObservable();

private _userManager: UserManager;
private _user: User;
changeMemberProfilePicture(profilePictureUrl: string) {
  this.userPhotoUrl.next(profilePictureUrl);
}

constructor(private http: HttpClient, private userService: UserService) {
  if (!this._user) {
      const _config = {
        authority: environment.IDPBaseUrl,
        client_id: environment.Client_Id,
        redirect_uri: `${environment.Client_Root}/assets/oidc-login-redirect.html`,
        scope: 'openid DatingApp-API profile',
        response_type: 'id_token token',
        post_logout_redirect_uri:  `${environment.Client_Root}/signout-callback-oidc`,
        userStore: new WebStorageStateStore({store: window.localStorage})
      };
      this._userManager = new UserManager(_config);

      this._userManager.getUser().then(user => {
        if (user && !user.expired) {
          this._user = user;

          localStorage.setItem('token', this._user.access_token);
          this.decodedToken = this.jwtHelper.decodeToken(this._user.access_token);
          this.GetUserFromAPI();

        }
      });
  }
}

  GetUserFromAPI() {
    this.userService.getUser(this._user.profile.sub).subscribe(
      next => {
        console.log(next);
        if (next) {
          this.currentUser = next;
          localStorage.setItem('user', JSON.stringify(next));
          this.changeMemberProfilePicture(this.currentUser.photoUrl);
        }
    },
      error => {
        console.log(error);
      }
    );
  }

  IDP_login() {
    return this._userManager.signinRedirect();
  }
  IDP_logout() {
    return this._userManager.signoutRedirect();
  }
  IDP_loggedIn() {
    return this._user && this._user.access_token && !this._user.expired;
  }

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
