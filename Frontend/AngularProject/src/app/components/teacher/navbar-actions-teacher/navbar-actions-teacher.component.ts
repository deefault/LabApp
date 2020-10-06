import {Component, OnInit} from '@angular/core';
import {AppService} from "../../../services/app.service";
import {AuthService} from "../../../services/auth/auth.service";
import {SettingsService} from "../../../services/settings.service";

@Component({
  selector: 'app-navbar-actions-teacher',
  templateUrl: './navbar-actions-teacher.component.html',
  styleUrls: ['./navbar-actions-teacher.component.scss']
})
export class NavbarActionsTeacherComponent implements OnInit {
  countNewAssignments: number = 0;

  constructor(
    private appService: AppService,
    public authService: AuthService,
    private settings: SettingsService
  ) {
    this.appService.initTeacher.subscribe(next => {
      if (next) {
        this.countNewAssignments = next.newAssignmentsCount;
      }
    });
  }

  ngOnInit() {

  }

}
