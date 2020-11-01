import {Component, Input, OnInit} from '@angular/core';
import {ActivatedRoute} from "@angular/router";
import {BaseComponent} from "../../../../../core/base-component";
import {
  AssignmentsService,
  AssignmentStatus,
  AttachmentDto,
  StudentAssignmentDto
} from "../../../../../clients/student";
import {NotificationService} from "../../../../../services/notification/notification.service";
import {NbComponentStatus} from "@nebular/theme";
import {ConversationDto} from "../../../../../clients/common";

@Component({
  selector: 'app-student-assignment-details',
  templateUrl: './student-assignment-details.component.html',
  styleUrls: ['./student-assignment-details.component.scss']
})
export class StudentAssignmentDetailsComponent extends BaseComponent implements OnInit {

  edit: boolean = false;

  @Input() groupId: number;
  @Input() studentAssignmentId: number | null = null;
  @Input() assignmentId: number;
  @Input() item: StudentAssignmentDto;

  conversation: ConversationDto;

  constructor(
    private route: ActivatedRoute,
    private assignmentsService: AssignmentsService,
    private notification: NotificationService
  ) {
    super();
  }

  ngOnInit() {
    this.loading = true;
    if (!this.item) {
      this.item = new class implements StudentAssignmentDto {
        assignmentId: number;
        attachments: Array<AttachmentDto> = [];
        body: string;
        completed: Date;
        fineScore: number;
        groupId: number;
        id: number;
        lastStatusChange: Date;
        score: number;
        status: AssignmentStatus;
        studentId: number;
        submitted: Date;
      }
    }
    if (this.studentAssignmentId != null)
      this.reload();
    else if (this.item && this.item.id){
      this.assignmentsService.getConversation(this.item.id).subscribe(
        conv => {
          this.conversation = conv;
        }
      );
    }
    this.loading = false;
  }

  isEditable(): boolean {
    return this.item && (this.item.status == undefined || this.edit && this.statusEditEnabled(this.item.status));
  }

  isNew(): boolean {
    return !this.item || !this.item.id || this.item.status == undefined;
  }

  statusEditEnabled(status: AssignmentStatus) {
    return status == AssignmentStatus.NUMBER_1 || status == AssignmentStatus.NUMBER_0 || status == AssignmentStatus.NUMBER_2
  }

  private reload(id?: number) {
    this.loading = true;
    this.assignmentsService.getStudentAssignmentById(id ? id : this.studentAssignmentId ? this.studentAssignmentId : this.item.id).subscribe(data => {
      this.item = data;
      this.assignmentsService.getConversation(data.id).subscribe(
       conv => {
         this.conversation = conv;
         this.loading = false;
       }
      )
    })
  }

  private submit() {
    if (this.item.status == undefined && !this.item.id && this.assignmentId) {
      this.assignmentsService.submit(this.assignmentId, this.item).subscribe(data => {
          //this.item = data;
          this.reload(this.item.id);
        },
        err => this.notification.showError(err)
      );
    } else console.log("Wrong status!");
  }

  private update() {
    if (this.statusEditEnabled(this.item.status)) {
      this.assignmentsService.update(this.item.id, this.item).subscribe(data => {
          this.reload(this.item.id);
        },
        err => this.notification.showError(err)
      );
    }
  }

  needReview() {
    this.assignmentsService.needReview(this.item.id).subscribe(data =>
      this.reload()
    );
  }

  editChange() {
    this.edit = !this.edit;
    if (!this.edit) this.reload()
  }

  formSubmitClicked() {
    this.item.groupId = this.groupId;
    if (this.item && this.item.id) {
      this.update();
    } else this.submit();
  }

  addAttachment(event: File) {
    this.assignmentsService.uploadAttachmentForm(event, this.item.id).subscribe(data => {
      this.item.attachments.push(data);
    })
  }

  deleteAttachment(event: AttachmentDto) {
    this.assignmentsService.deleteAttachment(event.id).subscribe(data => {
      this.item.attachments.splice(this.item.attachments.findIndex(x => x.id == event.id))
    })
  }

  getAlertStatus(): NbComponentStatus {
    switch (this.item.status) {
      case 0:
      case 2:
        return "info"
      case 1:
        return "warning"
      case 3:
        return "success"
    }
  }

  getAlertText(): string {
    switch (this.item.status) {
      case 0:
        return "Вы отправили работу. Дождитесь проверки"
      case 1:
        return "Преподаватель не принял работу и запросил изменения"
      case 2:
        return "Вы запросили повторную проверку. Дождитесь проверки"
      case 3:
        let text = `Работа принята: вы поллучили баллов: ${this.item.score}`;
        if (this.item.fineScore) text = text.concat(`(штраф: ${this.item.fineScore})`);
        return text;
    }
  }
}
