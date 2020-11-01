import {Component, Input, OnInit, Optional} from '@angular/core';
import {ActivatedRoute, Router} from "@angular/router";
import {AssignmentDetailsDto, AssignmentsService, AttachmentDto, SubjectDto} from "../../../../clients/teacher";
import {BaseComponent} from "../../../../core/base-component";
import {NbDialogRef} from "@nebular/theme";

@Component({
  selector: 'app-assignment-add',
  templateUrl: './assignment-add.component.html',
  styleUrls: ['./assignment-add.component.css']
})
export class AssignmentAddComponent extends BaseComponent implements OnInit {

  @Input() subjectId: number;
  error: string;
  item: AssignmentDetailsDto;
  subjects: SubjectDto;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private assignmentsService: AssignmentsService,
    @Optional() public dialog: NbDialogRef<AssignmentAddComponent>,
  ) {
    super();
    this.item = new class implements AssignmentDetailsDto {
      allowAfterDeadline: boolean = true;
      description: string;
      attachments: Array<AttachmentDto> = [];
      fineAfterDeadline: number = 0;
      id: number;
      maxScore: number;
      name: string;
      subjectId: number;
    }
    if (this.subjectId)
      this.item.subjectId = this.subjectId;
  }

  ngOnInit() {
    this.item.subjectId = this.subjectId;
    console.log();
  }

  add() {
    this.loading = true;
    this.assignmentsService.add(this.item).subscribe(data => {
        this.loading = false;
        if (this.dialog)
          this.dialog.close(data);
        else this.router.navigate(['assignments']);
      },
      err => {
        this.loading = false;
      }
    );
  }

  addAttachment(file: File) {
    this.assignmentsService.uploadAttachmentForm(file).subscribe(data => {
      this.item.attachments.push(data);
    });
  }

  deleteAttachment(dto: AttachmentDto) {
    this.assignmentsService.deleteAttachment(dto.id).subscribe(data => {
      this.item.attachments.splice(this.item.attachments.findIndex(x => x.id == dto.id));
    });
  }
}
