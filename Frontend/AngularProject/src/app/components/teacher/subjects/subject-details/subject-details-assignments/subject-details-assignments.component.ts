import {Component, EventEmitter, OnInit} from '@angular/core';
import {BaseComponent} from "../../../../../core/base-component";
import {ActivatedRoute, Router} from "@angular/router";
import {AssignmentDetailsDto, AssignmentDto, AssignmentsService, AttachmentDto} from "../../../../../clients/teacher";
import {NbDialogRef, NbDialogService} from "@nebular/theme";
import {AssignmentAddComponent} from "../../../assignment/assignment-add/assignment-add.component";


@Component({
  selector: 'app-subject-details-assignments',
  templateUrl: './subject-details-assignments.component.html',
  styleUrls: ['./subject-details-assignments.component.scss']
})
export class SubjectDetailsAssignmentsComponent extends BaseComponent implements OnInit {

  items: AssignmentDto[] = [];
  subjectId: number;
  onAdded: EventEmitter<boolean> = new EventEmitter<boolean>()
  addedItem: AssignmentDetailsDto = new class implements AssignmentDetailsDto {
    allowAfterDeadline: boolean;
    attachments: Array<AttachmentDto> = [];
    description: string;
    fineAfterDeadline: number;
    id: number;
    maxScore: number;
    name: string;
    subjectId: number;
    subjectName: string;
  }

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private assignmentService: AssignmentsService,
    private dialogService: NbDialogService,
  ) {
    super();
    this.subjectId = +this.route.snapshot.parent.paramMap.get('id');
    this.onAdded.subscribe(_ => this.reload());
  }

  ngOnInit() {
    this.reload();
  }


  private reload() {
    this.loading = true;
    this.assignmentService.get(this.subjectId).subscribe(data => {
      this.items = data;
      this.loading = false;
    });
  }

  onClick(item: AssignmentDto) {
    this.router.navigate([`/assignments/${item.id}`]);
  }

  goToAdd() {
    //this.router.navigate([`/subjects/${this.subjectId}/lessons/add`]);
    this.dialogService.open(AssignmentAddComponent, {
      hasScroll: true,
      hasBackdrop: true,
      context: {
        subjectId: this.subjectId,
      }
    }).onClose.subscribe(data => {
      if (data) {
        this.reload();
      }
    });
  }
}
