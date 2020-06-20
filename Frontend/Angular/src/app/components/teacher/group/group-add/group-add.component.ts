import {Component, isDevMode, OnInit} from '@angular/core';
import {GroupDto, GroupService, SubjectDto, SubjectService} from "../../../../clients/teacher";
import {Router} from "@angular/router";
import {BaseComponent} from "../../../../core/base-component";

@Component({
  selector: 'app-group-add',
  templateUrl: './group-add.component.html',
  styleUrls: ['./group-add.component.css']
})
export class GroupAddComponent extends BaseComponent implements OnInit {

  item: GroupDto;
  //subjects: SubjectDto[];

  constructor(
    private groupService: GroupService,
    private subjectService: SubjectService,
    private router: Router,
  ) {
    super();
  }

  ngOnInit() {
    this.item = new class implements GroupDto {
      groupName: string;
      id: number;
      studyYear: number;
      subjectId: number;
      teacherId: number;
    }
  }

  add() {
    //this.item = Object.assign(this.subject, this.form.value);
    this.groupService.add(this.item).subscribe(
      data => {
        this.router.navigate(['groups', data.id])
      }, error => {
        if (isDevMode()) console.log(error);
      }
    )
  }
}
