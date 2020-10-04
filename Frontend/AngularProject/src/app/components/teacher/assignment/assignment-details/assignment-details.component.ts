import {Component, OnInit} from '@angular/core';
import {AssignmentDetailsDto, AssignmentsService, AttachmentDto} from "../../../../clients/teacher";
import {ActivatedRoute, Router} from "@angular/router";
import {BaseComponent} from "../../../../core/base-component";

@Component({
  selector: 'app-assignment-details',
  templateUrl: './assignment-details.component.html',
  styleUrls: ['./assignment-details.component.scss']
})
export class AssignmentDetailsComponent extends BaseComponent implements OnInit {

  item: AssignmentDetailsDto;
  id: number;
  edit: boolean = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private assignmentsService: AssignmentsService
  ) {
    super();
    this.id = +this.route.snapshot.paramMap.get("id");
  }

  ngOnInit() {
    this.reload();
  }

  addAttachment($event: File) {
    this.assignmentsService.addAttachment(this.id, $event).subscribe(data => this.item.attachments.push(data));
  }

  deleteAttachment($event: AttachmentDto) {
    this.assignmentsService.deleteAttachment($event.id).subscribe(_ =>
      this.item.attachments.splice(this.item.attachments.findIndex(x => x.id == $event.id))
    );
  }

  editChange() {
    this.edit = !this.edit;
    this.reload()
  }

  private reload() {
    this.loading = true;
    this.assignmentsService.getById(this.id).subscribe(data => {
      this.item = data;
      this.loading = false;
    })
  }

  submit() {
    this.loading = true;
    this.assignmentsService.update(this.id, this.item).subscribe(data => {
        this.reload();
        this.loading = false;
      },
      error => {
        this.loading = false;
      });
  }
}

