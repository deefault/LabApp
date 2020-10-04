import {Component, Input, OnInit} from '@angular/core';
import {ActivatedRoute, Route, Router} from "@angular/router";
import {AssignmentDto, AssignmentsService, SubjectDto, SubjectService} from "../../../../clients/teacher";
import {BaseComponent} from "../../../../core/base-component";
import {NotificationService} from "../../../../services/notification/notification.service";
import {HttpErrorResponse, HttpResponse} from "@angular/common/http";


@Component({
  selector: 'app-assignment-list',
  templateUrl: './assignment-list.component.html',
  styleUrls: ['./assignment-list.component.css']
})
export class AssignmentListComponent extends BaseComponent implements OnInit {

  @Input() subjectId: number = null;
  items: AssignmentDto[] = [];

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private subjectService: SubjectService,
    private assignmentsService: AssignmentsService,
    private notificationService: NotificationService
  ) {
    super();
    if (route.snapshot.parent != null && route.snapshot.parent.paramMap["id"]){
      this.subjectId = +route.snapshot.parent.paramMap["id"];
    }
  }

  ngOnInit() {
    this.loading = true;
    this.assignmentsService.get(this.subjectId).subscribe(data => {
        this.loading = false;
        this.items = data;
      },
      (err: HttpErrorResponse) => {
        this.notificationService.showError(err.message);
        this.loading = false;
      }
    );
  }

  onClick(item: AssignmentDto) {
    if (this.subjectId) {
      this.router.navigate(['subjects', this.subjectId, 'assignments']);
    } else {
      this.router.navigate(['assignments', item.id]);
    }
  }
}
