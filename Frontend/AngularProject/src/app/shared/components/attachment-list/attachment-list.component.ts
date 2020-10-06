import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {AttachmentDto} from "../../../clients/teacher";
import {AppService} from "../../../services/app.service";

@Component({
  selector: 'app-attachment-list',
  templateUrl: './attachment-list.component.html',
  styleUrls: ['./attachment-list.component.css']
})
export class AttachmentListComponent implements OnInit {


  @Input() editable: boolean = true;
  @Input() items: AttachmentDto[] = [];
  @Output() onDelete: EventEmitter<AttachmentDto> = new EventEmitter<AttachmentDto>();
  @Output() onAdd: EventEmitter<File> = new EventEmitter<File>();

  constructor(
    public app: AppService
  ) {
    this.items = [];
  }

  ngOnInit() {
  }

  getSize(size: number) : string {
    return (size / 1024).toFixed(2) + " Kb" ;
  }

  deleteClick(attachment: AttachmentDto) {
    this.onDelete.emit(attachment);
  }

  private onSave(attachment: AttachmentDto) {
  }

  change(event: Event) {
    let target = event.target as HTMLInputElement;
    if (target == null) throw new Error();
    let file: File = target.files[0];
    this.onAdd.emit(file);
    target.value = '';
  }

  showDialog($event: MouseEvent) {
    $event.preventDefault();
    document.getElementById("file-upload").click();
  }
}
