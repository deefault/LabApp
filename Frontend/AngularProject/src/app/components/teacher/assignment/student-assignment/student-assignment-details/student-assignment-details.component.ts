import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from "@angular/router";
import {BaseComponent} from "../../../../../core/base-component";
import {
  AssignmentDetailsDto,
  AssignmentDto, AssignmentsService,
  AssignmentStatus,
  StudentAssignmentDetailDto,
  StudentAssignmentsService
} from "../../../../../clients/teacher";
import {NbComponentStatus} from "@nebular/theme";
import {ConversationDto} from "../../../../../clients/common";

@Component({
  selector: 'app-student-assignment-details-teacher',
  templateUrl: './student-assignment-details.component.html',
  styleUrls: ['./student-assignment-details.component.scss']
})
export class StudentAssignmentDetailsTeacherComponent extends BaseComponent implements OnInit {

  id: number;
  back: string;
  item: StudentAssignmentDetailDto;
  assignment: AssignmentDetailsDto;
  conversation: ConversationDto;
  editShow: boolean = false;
  score: number;
  fineScore: number;
  showFineScore: boolean = false;
  ApprovedStatus: AssignmentStatus = AssignmentStatus.NUMBER_3;
  ChangesRequestedStatus: AssignmentStatus = AssignmentStatus.NUMBER_2;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private studentAssignmentsService: StudentAssignmentsService,
    private assignmentsService: AssignmentsService
  ) {
    super();
    this.id = +route.snapshot.paramMap.get("id");
    this.back = '/student-assignments/'
  }

  ngOnInit() {
    this.loading = true;
    this.reload(true);
  }

  isEditable() {
    return this.item.status == AssignmentStatus.NUMBER_0 ||
      this.item.status == AssignmentStatus.NUMBER_1 ||
      this.item.status == AssignmentStatus.NUMBER_2
  }

  private loadConversation(id: number) {
    this.studentAssignmentsService.getConversation(id).subscribe(
      data => {
        this.conversation = data;
      }
    );
  }

  private reload(loadAssignment?: boolean) {
    this.loading = true;
    this.studentAssignmentsService.getById(this.id).subscribe(data => {
        this.item = data;
        this.loadConversation(data.id);
        if (loadAssignment)
          this.assignmentsService.getById(this.item.assignmentId, this.item.groupId).subscribe(a => {
            this.assignment = a;
            this.loading = false;
          });
        if (!loadAssignment) {
          this.loading = false;

        }
      }
    );
  }

  private approve() {
    this.loading = true;
    this.studentAssignmentsService.approve(this.id, this.score, this.fineScore).subscribe(data => {
      this.item = data;
      this.loading = false;
    });
  }

  private requestChanges() {
    this.loading = true;
    this.studentAssignmentsService.requestChanges(this.id).subscribe(data => {
      this.item = data;
      this.loading = false;
    });
  }

  private updateScore() {
    this.loading = true;
    this.studentAssignmentsService.updateScore(this.id, this.score, this.fineScore).subscribe(data => {
      this.item = data;
      this.editShow = false;
      this.loading = false;
    });
  }

  onSave() {
    this.item.status == this.ApprovedStatus
      ? this.updateScore()
      : this.approve();
  }

  getStatus(): NbComponentStatus {
    if (this.editShow) return 'basic';
    return this.item.status != this.ApprovedStatus
      ? 'success'
      : 'primary';
  }

  getStatusForStatusAlert(status: AssignmentStatus): NbComponentStatus {
    switch (status) {
      case 0:
      case 2:
        return 'info';
      case 1:
        return 'warning';
      case 3:
        return 'success'
    }
  }

  // TODO: move to service
  getTextForStatusAlert(status: AssignmentStatus): string {
    switch (status) {
      case 0:
        return 'Новая работа';
      case 1:
        return 'Вы запросили изменения у студента';
      case 2:
        return 'Студент внес требуемые изменения';
      case 3:
        return 'Вы приняли работу';

    }
  }

  editClick() {
    this.editShow = !this.editShow;
    if (this.item.status == this.ApprovedStatus) {
      this.score = this.item.score;
      this.fineScore = this.item.fineScore;
    }
  }
}
