import {Component, OnInit} from '@angular/core';
import {

  GroupDto,
  GroupsService,
  UserListDto
} from "../../../../clients/student";
import {ActivatedRoute, Router} from "@angular/router";
import {NotificationService} from "../../../../services/notification/notification.service";

@Component({
  selector: 'app-group-details',
  templateUrl: './group-details.component.html',
  styleUrls: ['./group-details.component.css']
})
export class GroupDetailsComponentStudent implements OnInit {

  item: GroupDto;
  //students: UserListDto[];
  id: number;
  tabs = [];

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private groupService: GroupsService,
    private notificationService: NotificationService,
  ) {
    this.id = +this.route.snapshot.paramMap.get('id');
    this.tabs = [
      /*{
        title: 'Студенты',
        route: `/groups/${this.groupId}/students`,
        enabled: true,
        active: true
      }*/
    ];
    this.item = new class implements GroupDto {
      groupName: string;
      id: number;
      studyYear: number;
      subjectId: number;
      teacherId: number;
    }
    /*this.subject = new class implements SubjectDto {
      description: string;
      groupId: number;
      name: string;
      teacherId: number;
    }*/
  }

  ngOnInit() {
    this.groupService.get(this.id).subscribe(data => {
      this.item = data;
    });
  }

}
