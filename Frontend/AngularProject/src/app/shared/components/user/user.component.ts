import {Component, Input, OnInit} from '@angular/core';
import {UserListDto} from "../../../clients/teacher";

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserComponent implements OnInit {

  @Input() size: 'small' | 'tiny' | 'medium' | 'giant';
  @Input() user: UserListDto

  constructor() {
  }

  ngOnInit() {
  }
}
