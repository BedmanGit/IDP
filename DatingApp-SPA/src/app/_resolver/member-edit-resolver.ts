import { Injectable } from '@angular/core';
import { AppUser } from '../_models/AppUser';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';
import { UserService } from '../_services/user.service';
import { catchError } from 'rxjs/operators';
import { of } from 'rxjs';
import { AuthService } from '../_services/Auth.service';

@Injectable()
export class MemberEditResolver implements Resolve<AppUser> {
    constructor(private userService: UserService, private router: Router, private alertify: AlertifyService, private auth: AuthService) {}

    resolve(route: ActivatedRouteSnapshot) {
        return this.userService.getUser(this.auth.decodedToken.nameid).pipe(
            catchError(error => {
                this.alertify.error('problem retrieving login user data');
                this.router.navigate(['/home/']);
                return of(null);
            })
        );
    }
}
