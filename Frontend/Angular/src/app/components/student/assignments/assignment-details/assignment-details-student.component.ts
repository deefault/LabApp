import {Component, OnInit} from '@angular/core';
import {BaseComponent} from "../../../../core/base-component";
import {ActivatedRoute, Router} from "@angular/router";
import {AssignmentDetailsDtoStudent, AssignmentsService} from "../../../../clients/student";
import {Observable} from "rxjs";
import {map} from "rxjs/operators";

@Component({
  selector: 'app-assignment-details',
  templateUrl: './assignment-details-student.component.html',
  styleUrls: ['./assignment-details-student.component.scss']
})
export class AssignmentDetailsStudentComponent extends BaseComponent implements OnInit {

  id: number;
  item: AssignmentDetailsDtoStudent;
  groupId: number;
  private state$: Observable<any>;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private assignmentsService: AssignmentsService,
  ) {
    super();
    this.id = +this.route.snapshot.paramMap.get("id");
    this.state$ = this.route.paramMap
      .pipe(map(() => window.history.state));
  }

  ngOnInit() {
    this.loading = true;
    this.state$.subscribe(state=>{
      this.groupId = state.groupId;
    })
    this.assignmentsService.getById(this.id).subscribe(data => {
      this.item = data;
      this.loading = false;
    });
  }

  addAssignmentOpen() {

  }
}

