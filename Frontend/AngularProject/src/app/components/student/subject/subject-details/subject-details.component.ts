import { Component, OnInit } from '@angular/core';
import {GroupSubjectDto, SubjectsService} from "../../../../clients/student";
import {ActivatedRoute, Router} from "@angular/router";
import {BaseComponent} from "../../../../core/base-component";
import {Location} from "@angular/common";
import {PreviousLocationService} from "../../../../services/previous-location.service";

@Component({
  selector: 'app-subject-details',
  templateUrl: './subject-details.component.html',
  styleUrls: ['./subject-details.component.css']
})
export class SubjectDetailsComponentStudent extends BaseComponent implements OnInit {

  group: GroupSubjectDto;
  loading: boolean = true;
  tabs: any[];
  groupId: number;
  back: string;

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private subjectService: SubjectsService,
    private location: Location,
    private previousLocation: PreviousLocationService,
  ) {
    super();
    this.groupId = +this.route.snapshot.paramMap.get('groupId');
    this.back = '/student/subjects/';
    this.tabs = [
      {
        title: 'Занятия',
        route: `/student/subjects/${this.groupId}/lessons/`,
        enabled: true,
        active: true
      },
      {
        title: 'Лабораторные',
        route: `/student/subjects/${this.groupId}/assignments/`,
        enabled: true,
        active: false
      }
    ];
    //this.back = this.location.
  }

  ngOnInit() {

    this.subjectService.getByGroupId(this.groupId).subscribe(
      next => {
        this.group = next;
        this.loading = false;
      },
      error => {
        console.log(error);
      }
    )
  }
}
