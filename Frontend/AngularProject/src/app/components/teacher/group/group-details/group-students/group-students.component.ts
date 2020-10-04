import {Component, EventEmitter, OnInit} from '@angular/core';
import {GroupDto, GroupService, SubjectDto, SubjectService, UserListDto} from "../../../../../clients/teacher";
import {ActivatedRoute, Router} from "@angular/router";
import {NbWindowService} from "@nebular/theme";
import {GroupStudentsAddComponent} from "./group-students-add/group-students-add.component";

@Component({
  selector: 'app-group-students',
  templateUrl: './group-students.component.html',
  styleUrls: ['./group-students.component.css']
})
export class GroupStudentsComponent implements OnInit {

  students: UserListDto[] = [];
  id: number;
  onAdded: EventEmitter<boolean> = new EventEmitter<boolean>()

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private groupService: GroupService,
    private subjectService: SubjectService,
    private windowService: NbWindowService,
  ) {
    this.id = +this.route.parent.snapshot.paramMap.get('id');
    this.onAdded.subscribe(data=>this.reload());
  }

  ngOnInit() {
    this.groupService.students(this.id).subscribe(data => this.students = data);
  }

  open() {
    this.windowService.open(GroupStudentsAddComponent, {
      context: {
        groupId: this.id,
        onAdded: this.onAdded,
      },
    });
  }

  private reload(){
    this.groupService.students(this.id).subscribe(data => this.students = data);
  }
}
