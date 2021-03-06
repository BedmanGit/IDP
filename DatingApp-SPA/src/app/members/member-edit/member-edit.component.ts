import { Component, OnInit, ViewChild, HostListener } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AppUser } from 'src/app/_models/AppUser';
import { UserService } from 'src/app/_services/user.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { NgForm } from '@angular/forms';
import { AuthService } from 'src/app/_services/Auth.service';
import { Photo } from 'src/app/_models/photo';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {
user: AppUser;
currentUserPhotoUrl: string;
@ViewChild('editForm') editForm: NgForm;
@HostListener('window:beforeunload', ['$event'])
unloadNotification($event: any) {
  if (this.editForm.dirty) {
    $event.returnValue = true;
  }
}
  constructor(private route: ActivatedRoute, private userService: UserService
    , private alertify: AlertifyService, private auth: AuthService) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.user = data['user'];
    });
    this.auth.currentUserPhotoUrl.subscribe(p => this.currentUserPhotoUrl = p);
  }

  setMain(photoUrl: string) {
    this.user.photoUrl = photoUrl;
  }

  updateUser(): void {
    this.userService.updateUser(this.auth.decodedToken.sub, this.user).subscribe(next => {
      this.alertify.success('Profile updated successfully');
      this.editForm.reset(this.user);
    }, error => {
      this.alertify.error(error);
    });

  }
}
