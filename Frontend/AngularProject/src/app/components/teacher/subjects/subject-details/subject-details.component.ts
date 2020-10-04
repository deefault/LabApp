import { Component, OnInit } from '@angular/core';
import {SubjectDto, SubjectService} from "../../../../clients/teacher";
import {ActivatedRoute, Router} from "@angular/router";
import {BaseComponent} from "../../../../core/base-component";
import {Location} from "@angular/common";

@Component({
  selector: 'app-subject-details',
  templateUrl: './subject-details.component.html',
  styleUrls: ['./subject-details.component.css']
})
export class SubjectDetailsComponent extends BaseComponent implements OnInit {

  subject: SubjectDto;
  loading: boolean = true;
  tabs: any[];
  id: number;
  back: string;

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private subjectService: SubjectService,
    private location: Location
  ) {
    super();
    this.id = +this.route.snapshot.paramMap.get('id');
    this.tabs = [
      {
        title: 'Занятия',
        route: `/subjects/${this.id}/lessons/`,
        enabled: true,
        active: true,
      },
      {
        title: 'Лабораторные',
        route: `/subjects/${this.id}/assignments/`,
        enabled: true,
        active: false,
      }
    ];
    //this.back = this.location.
  }

  ngOnInit() {

    this.subjectService.getById(this.id).subscribe(
      next => {
        this.subject = next;
        this.loading = false;
      },
      error => {
        console.log(error);
      }
    )
  }
}
