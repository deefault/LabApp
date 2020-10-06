import {Component, isDevMode, OnInit} from '@angular/core';
import {SubjectDto, SubjectService} from "../../../../clients/teacher";
import {ActivatedRoute, Router} from "@angular/router";

@Component({
  selector: 'app-subject-list',
  templateUrl: './subject-list.component.html',
  styleUrls: ['./subject-list.component.css']
})
export class SubjectListComponent implements OnInit {

  items: SubjectDto[] = [];

  constructor(
    private subjectService: SubjectService,
    public router: Router,
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

  onSelect(item: SubjectDto) {
    this.router.navigate(['/', 'subjects', item.id]);
  }
}
