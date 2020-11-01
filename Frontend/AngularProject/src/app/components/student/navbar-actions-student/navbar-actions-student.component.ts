import { Component, OnInit } from '@angular/core';
import {AuthService} from "../../../services/auth/auth.service";
import {NavBarBase} from "../../../shared/components/NavBarBase";
import {SignalRService} from "../../../services/signalr/signalr.service";
import {AppService} from "../../../services/app.service";
import {EventBusService} from "../../../services/event-bus.service";

@Component({
  selector: 'app-navbar-actions-student',
  templateUrl: './navbar-actions-student.component.html',
  styleUrls: ['./navbar-actions-student.component.scss']
})
export class NavbarActionsStudentComponent extends NavBarBase {

  constructor(
      public authService: AuthService,
      signalRService: SignalRService,
      eventBus: EventBusService,
      public appService: AppService
  ) { 
    super(signalRService, eventBus);
  }

  ngOnInit() {
    super.ngOnInit();
    this.appService.initStudent.subscribe(next => {
      if (next) {
        this.messages = next.newMessages;
      }
    });
  }
}
