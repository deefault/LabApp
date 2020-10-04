import {Component, isDevMode, OnInit} from '@angular/core';
import {GroupSubjectDto, SubjectsService} from "../../../../clients/student";
import {ActivatedRoute, Router} from "@angular/router";

@Component({
  selector: 'app-subject-list',
  templateUrl: './subject-list.component.html',
  styleUrls: ['./subject-list.component.css']
})
export class SubjectListComponentStudent implements OnInit {

  items: GroupSubjectDto[] = [];

  constructor(
    private subjectService: SubjectsService,
    private router: Router,
    private route: ActivatedRoute
  ) {
  }

  ngOnInit() {
    this.subjectService.get().subscribe(
      next => {
         this.items = next;
      },
      error => {
        if (isDevMode()) console.log(error);
      }
    )
  }

  onSelect(item: GroupSubjectDto) {
    this.router.navigate(['student', 'subjects', item.id]);
  }
}
