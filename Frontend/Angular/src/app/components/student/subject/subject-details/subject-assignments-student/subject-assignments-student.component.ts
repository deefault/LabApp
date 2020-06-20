import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from "@angular/router";
import {AssignmentDtoStudent, AssignmentsService, AssignmentStatus} from "../../../../../clients/student";
import {BaseComponent} from "../../../../../core/base-component";
import {AssignmentDto} from "../../../../../clients/student/model/assignmentDto";

@Component({
  selector: 'app-subject-assignments-student',
  templateUrl: './subject-assignments-student.component.html',
  styleUrls: ['./subject-assignments-student.component.scss']
})
export class SubjectAssignmentsStudentComponent extends BaseComponent implements OnInit {

  groupId: number;
  items: AssignmentDtoStudent[] = []

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private assignmentsService: AssignmentsService,
  ) {
    super();
    this.groupId = +this.route.snapshot.parent.paramMap.get("groupId");
  }

  ngOnInit() {
    this.loading = true;
    console.log(this.groupId);
    this.assignmentsService.get(this.groupId).subscribe(data => {
      this.items = data;
      this.loading = false;
    });
  }

  getIcon(status: AssignmentStatus): string {
    /*
        Submitted,
        ChangesRequested,
        NeedReview,
        Approved,
    */
    switch (status) {
      case 0:
      case 2:
        return 'clock-outline';
      case 1:
        return 'close-circle-outline'
      case 3:
        return 'checkmark-circle-2-outline';

    }
  }

  onClick(item: AssignmentDtoStudent) {
    this.router.navigate(['student', 'assignments', item.id], {state: {groupId: this.groupId}});
  }
}
