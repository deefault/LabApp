import { Component, OnInit } from '@angular/core';
import {StudentAssignmentDto} from "../../../../../clients/teacher";
import {Router} from "@angular/router";

@Component({
  selector: 'app-student-assignment-list-new',
  templateUrl: './student-assignment-list-all.component.html',
  styleUrls: ['./student-assignment-list-all.component.scss']
})
export class StudentAssignmentListAllComponent implements OnInit {
  onlyNew: boolean = true;

  constructor(
    private router: Router
  ) { }

  ngOnInit() {
  }

  selected(assignment: StudentAssignmentDto) {
    this.router.navigate(['student-assignments', assignment.id]);
  }
}
