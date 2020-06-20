import {Component, EventEmitter, OnInit} from '@angular/core';
import {BaseComponent} from "../../../../../core/base-component";
import {ActivatedRoute, Router} from "@angular/router";
import {NbDialogService, NbWindowService} from "@nebular/theme";
import {GroupStudentsAddComponent} from "../../../group/group-details/group-students/group-students-add/group-students-add.component";
//import {LessonDto, LessonsService} from "../../../../../clients/teacher";
import {LessonAddComponent} from "./lesson-add/lesson-add.component";
import {LessonDto, LessonsService} from "../../../../../clients/teacher";

@Component({
  selector: 'app-subject-details-lessons',
  templateUrl: './subject-details-lessons.component.html',
  styleUrls: ['./subject-details-lessons.component.css']
})
export class SubjectDetailsLessonsComponent extends BaseComponent implements OnInit {

  items: LessonDto[] = [];
  subjectId: number;
  onAdded: EventEmitter<boolean> = new EventEmitter<boolean>()
  addedItem: LessonDto = new class implements LessonDto {
    assignmentId: number;
    id: number;
    isPractical: boolean;
    name: string;
    order: number;
    subjectId: number;
    subjectName: string;
  }

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private lessonService: LessonsService,
  ) {
    super();
    this.subjectId = +this.route.snapshot.parent.parent.paramMap.get('id');
    this.onAdded.subscribe(_ => this.reload());
  }

  ngOnInit() {
    this.reload();
  }


  private reload() {
    this.lessonService.get(this.subjectId).subscribe(data => this.items = data);
  }

  lessonClicked(item: LessonDto) {
    this.router.navigate([`/subjects/${this.subjectId}/lessons/${item.id}`]);
  }

  goToAdd() {
    this.router.navigate([`/subjects/${this.subjectId}/lessons/add`]);
  }
}
