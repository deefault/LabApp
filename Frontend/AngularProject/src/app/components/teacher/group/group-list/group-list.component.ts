import {Component, isDevMode, OnInit} from '@angular/core';
import {GroupDto, GroupService} from "../../../../clients/teacher";
import {Router} from "@angular/router";

@Component({
  selector: 'app-group-list',
  templateUrl: './group-list.component.html',
  styleUrls: ['./group-list.component.css']
})
export class GroupListComponent implements OnInit {
  items: GroupDto[] = [];

  constructor(
    public router: Router,
    private groupService: GroupService,
  ) {
  }

  ngOnInit() {
    this.groupService.list().subscribe(next => {
      this.items = next;
    }, error => {
      if (isDevMode()) console.log(error);
    })
  }

  onSelect(item: GroupDto) {
    this.router.navigate(['groups', item.id]);
  }
}
