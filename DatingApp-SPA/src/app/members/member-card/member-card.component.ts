import { Component, OnInit, Input } from '@angular/core';
import { AppUser } from 'src/app/_models/AppUser';

@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.css']
})
export class MemberCardComponent implements OnInit {
@Input() user: AppUser;

  constructor() { }

  ngOnInit() {
  }

}
