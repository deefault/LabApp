import {Component, EventEmitter, OnInit} from '@angular/core';
import {BaseComponent} from "../../../../../core/base-component";
import {ActivatedRoute, Router} from "@angular/router";
//import {LessonDto, LessonsService} from "../../../../../clients/teacher";
import {LessonDto, LessonsService} from "../../../../../clients/student";

@Component({
  selector: 'app-subject-details-lessons',
  templateUrl: './lessons-component-student.component.html',
  styleUrls: ['./lessons-component-student.component.css']
})
export class LessonsComponentStudent extends BaseComponent implements OnInit {

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
    this.subjectId = +this.route.snapshot.parent.parent.paramMap.get('groupId');
    this.onAdded.subscribe(_ => this.reload());
  }

  ngOnInit() {
    this.reload();
  }


  private reload() {
    this.lessonService.get(this.subjectId).subscribe(data => this.items = data);
  }

  lessonClicked(item: LessonDto) {
    this.router.navigate([`student/subjects/${this.subjectId}/lessons/${item.id}`]);
  }

  goToAdd() {
    this.router.navigate([`student/subjects/${this.subjectId}/lessons/add`]);
  }
}
