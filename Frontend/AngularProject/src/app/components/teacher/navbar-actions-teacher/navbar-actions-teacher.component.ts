import {Component, OnInit} from '@angular/core';
import {AppService} from "../../../services/app.service";
import {AuthService} from "../../../services/auth/auth.service";
import {SettingsService} from "../../../services/settings.service";
import {NavBarBase} from "../../../shared/components/NavBarBase";
import {SignalRService} from "../../../services/signalr/signalr.service";
import {EventBusService} from "../../../services/event-bus.service";

@Component({
  selector: 'app-navbar-actions-teacher',
  templateUrl: './navbar-actions-teacher.component.html',
  styleUrls: ['./navbar-actions-teacher.component.scss']
})
export class NavbarActionsTeacherComponent extends NavBarBase {
  countNewAssignments: number = 0;

  constructor(
    private appService: AppService,
    public authService: AuthService,
    private settings: SettingsService,
    signalRService: SignalRService,
    eventBus: EventBusService,
  ) {
    super(signalRService, eventBus);
  }

  ngOnInit() {
    super.ngOnInit();
    this.appService.initTeacher.subscribe(next => {
      if (next) {
        this.countNewAssignments = next.newAssignmentsCount;
        this.messages = next.newMessages;
      }
    });
  }

}
