import {Component, isDevMode, OnInit} from '@angular/core';
import {BaseComponent} from "../../../../core/base-component";
import {ActivatedRoute} from "@angular/router";
import {AttachmentDto, LessonDto, LessonsService} from "../../../../clients/teacher";
import {NotificationService} from "../../../../services/notification/notification.service";

@Component({
  selector: 'app-lesson-details',
  templateUrl: './lesson-details.component.html',
  styleUrls: ['./lesson-details.component.css']
})
export class LessonDetailsComponent extends BaseComponent implements OnInit {

  id: number;
  item: LessonDto;

  constructor(
    private route: ActivatedRoute,
    private lessonsService: LessonsService,
    private notificationService: NotificationService,
  ) {
    super();
  }


  ngOnInit() {
    this.loading = true;
    this.id = +this.route.snapshot.paramMap.get('lessonId');
    this.lessonsService.getById(this.id).subscribe(
      data => {
        this.item = data;
        this.loading = false;
      },
      error => {
        if (isDevMode()) console.log(error);
        this.loading = false;
      }
    )
  }

  addAttachment(event: File) {
    this.lessonsService.addAttachmentForm(this.id, event).subscribe(data => {
        this.item.attachments.push(data);
      },
      error => {
        this.notificationService.showError("Ошибка")
      }
    );
  }


  deleteAttachment(event: AttachmentDto) {
    this.lessonsService.deleteAttachment(event.id).subscribe(_ => {
        let i = this.item.attachments.indexOf(event);
        if (i != -1) this.item.attachments.splice(i, 1);
      },
      error => {
        this.notificationService.showError("Ошибка")
      }
    );
  }
}
