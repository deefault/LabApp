import {Component, OnInit} from '@angular/core';
import {
  GroupDetailsTeacherDto,
  GroupDto,
  GroupService,
  SubjectDto,
  SubjectService,
  UserListDto
} from "../../../../clients/teacher";
import {ActivatedRoute, Router} from "@angular/router";
import {NotificationService} from "../../../../services/notification/notification.service";

@Component({
  selector: 'app-group-details',
  templateUrl: './group-details.component.html',
  styleUrls: ['./group-details.component.css']
})
export class GroupDetailsComponent implements OnInit {

  item: GroupDetailsTeacherDto;
  subject: SubjectDto;
  //students: UserListDto[];
  id: number;
  tabs = [];

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private groupService: GroupService,
    private subjectService: SubjectService,
    private notificationService: NotificationService,
  ) {
    this.id = +this.route.snapshot.paramMap.get('id');
    this.tabs = [
      {
        title: 'Студенты',
        route: `/groups/${this.id}/students`,
        enabled: true,
        active: true,

      },
      {
        title: 'Успеваемость',
        route: `/groups/${this.id}/table`,
        enabled: true,
        active: true,
      }
    ];
    this.item = new class implements GroupDetailsTeacherDto {
      groupName: string;
      id: number;
      studyYear: number;
      subjectId: number;
      teacherId: number;
    }
    this.subject = new class implements SubjectDto {
      description: string;
      id: number;
      name: string;
      teacherId: number;
    }
  }

  ngOnInit() {
    this.groupService.get(this.id).subscribe(data => {
      this.item = data;
      this.subjectService.getById(this.item.subjectId).subscribe(data => this.subject = data);
    });
  }

  getUrl(): string {
    return encodeURI(`${window.location.origin}/JoinGroup/?code=${this.item.inviteCode}`)
  }

  copy(input: HTMLInputElement) {
    input.select()
    document.execCommand('copy');
    input.setSelectionRange(0, 0);
    this.notificationService.showCopied();

  }
}
