import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from "@angular/router";
import {
  AssignmentStatus,
  GroupService,
  GroupTableDto,
  StudentAssignmentDto,
  SubjectService,
  UserListDto
} from "../../../../../clients/teacher";
import {NbWindowService} from "@nebular/theme";
import {BaseComponent} from "../../../../../core/base-component";

export class Data {
  student: UserListDto;
  assignments: StudentAssignmentDto[] = [];
}


@Component({
  selector: 'app-group-table',
  templateUrl: './group-table.component.html',
  styleUrls: ['./group-table.component.scss']
})
export class GroupTableComponent extends BaseComponent implements OnInit {

  id: number;
  items: GroupTableDto;

  table: Data[] = [];

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private groupService: GroupService,
    private subjectService: SubjectService,
  ) {
    super();
    this.id = +this.route.parent.snapshot.paramMap.get('id');
  }

  ngOnInit() {
    this.loading = true;
    this.groupService.studentScoreTable(this.id).subscribe(data => {
        this.items = data;
        for (let e of this.items.entries) {
          let obj = new Data();
          obj.student = e.student;
          for (let a of this.items.assignments) {
            let stAssignment = e.studentAssignments.find(y => y.assignmentId == a.id);
            obj.assignments.push(stAssignment);
          }
          this.table.push(obj);
        }
        this.loading = false;
      }
    );
  }

  findAssignment(id: number, studentAssignments: Array<StudentAssignmentDto>): StudentAssignmentDto {
    return studentAssignments.find(y => y.assignmentId == id);
  }

  getStatusIcon(status: AssignmentStatus): string {
    switch (status) {
      case 0:
        return 'bulb-outline'
      case 1:
        return 'clock-outline'
      case 2:
        return 'bulb-outline'
      case 3:
        return 'checkmark-circle-2-outline'

    }
  }

  getTooltipText(status: AssignmentStatus) {
    switch (status) {
      case AssignmentStatus.NUMBER_0:
        return 'Новая'
      case AssignmentStatus.NUMBER_1:
        return 'Вы запросили изменения'
      case AssignmentStatus.NUMBER_2:
        return 'На проверку: студент внес изменения'
      case AssignmentStatus.NUMBER_3:
        return 'Проверено'

    }
  }
}
