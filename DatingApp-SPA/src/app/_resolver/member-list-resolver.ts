import { Injectable } from '@angular/core';
import { AppUser } from '../_models/AppUser';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';
import { UserService } from '../_services/user.service';
import { catchError } from 'rxjs/operators';
import { of } from 'rxjs';

@Injectable()
export class MemberListResolver implements Resolve<AppUser[]> {
    constructor(private userService: UserService, private router: Router, private alertify: AlertifyService) {}
    resolve(route: ActivatedRouteSnapshot) {
        return this.userService.getUsers().pipe(
            catchError(error => {
                this.alertify.error('problem retrieving data');
                this.router.navigate(['/home']);
                return of(null);
            })
        );
    }
}
