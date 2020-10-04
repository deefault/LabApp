import {Component, isDevMode, OnInit} from '@angular/core';
import {AttachmentDto, LessonDto, LessonsService} from "../../../../../../clients/teacher";
import {ActivatedRoute, Router} from "@angular/router";
import {BaseComponent} from "../../../../../../core/base-component";
import {Location} from '@angular/common';
import {NotificationService} from "../../../../../../services/notification/notification.service";

@Component({
  selector: 'app-lesson-add',
  templateUrl: './lesson-add.component.html',
  styleUrls: ['./lesson-add.component.css']
})
export class LessonAddComponent extends BaseComponent implements OnInit {

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private lessonService: LessonsService,
    private location: Location,
    private notificationService: NotificationService
  ) {
    super();
    this.item = new class implements LessonDto {
      assignmentId?: number | null;
      attachments: AttachmentDto[];
      description: string;
      id: number;
      isPractical: boolean;
      name: string;
      order: number;
      subjectId: number;
      subjectName?: string;
    }
    this.item.attachments = [];
    this.subjectId = +this.route.snapshot.parent.parent.paramMap.get("id");
    this.item.subjectId = this.subjectId;
    this.item.isPractical = false;
  }

  item: LessonDto;
  subjectId: number;

  ngOnInit() {

  }

  add() {
    this.lessonService.add(this.item).subscribe(
      data => {
        console.log(data);
        this.location.back();
      },
      error => {
        if (isDevMode()) console.log(error);
      }
    )
  }

  deleteAttachment(event: AttachmentDto) {
    this.lessonService.deleteAttachment(event.id).subscribe(_ => {
        let i = this.item.attachments.indexOf(event);
        if (i != -1) this.item.attachments.splice(i, 1);
      },
      _ => {
        this.notificationService.showError("Ошибка")
      }
    );
  }

  addAttachment(event: File) {
    this.loading = true;
    this.lessonService.uploadAttachment(event).subscribe(data => {
        this.item.attachments.push(data);
        this.loading = false;
      },
      _ => {
        this.notificationService.showError("Ошибка");
        this.loading = false;
      })
  }
}
