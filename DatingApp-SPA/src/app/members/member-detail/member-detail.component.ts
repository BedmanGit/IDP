import { Component, OnInit } from '@angular/core';
import { UserService } from 'src/app/_services/user.service';
import { AppUser } from 'src/app/_models/AppUser';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { ActivatedRoute } from '@angular/router';
import { NgxGalleryOptions, NgxGalleryImage, NgxGalleryAnimation } from 'ngx-gallery';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit {
  user: AppUser;
  galleryOpts: NgxGalleryOptions[];
  galleryImgs: NgxGalleryImage[];
  constructor(private userservice: UserService, private alertify: AlertifyService
      , private activeroute: ActivatedRoute) {}

  ngOnInit() {
    this.activeroute.data.subscribe(data => {
      this.user = data['user'];
    });

    this.galleryOpts = [{
      width: '800px',
      height: '800px',
      imagePercent: 100,
      thumbnailsColumns: 4,
      imageAnimation: NgxGalleryAnimation.Slide,
      preview: false
    }];

    this.galleryImgs = this.getImages();
  }

  getImages() {
    const imageUrls = [];
    for (let i = 0; i < this.user.photos.length; i++) {
      imageUrls.push({
        small: this.user.photos[i].url,
        medium: this.user.photos[i].url,
        big: this.user.photos[i].url,
        description: this.user.photos[i].description
      });
    }
    return imageUrls;
  }
}
