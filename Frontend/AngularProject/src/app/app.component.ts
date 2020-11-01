import {Component} from '@angular/core';
import {AuthService} from "./services/auth/auth.service";
import {NavigationEnd, Router} from "@angular/router";
import {SettingsService} from "./services/settings.service";
import {SignalRService} from "./services/signalr/signalr.service";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent {
  title = 'app';
  hideSideBar = false;
  newMessages: number = 0;

  constructor(public authService: AuthService, private router: Router, public settings: SettingsService, private signalrService: SignalRService,) {
    router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        if (event.url == '/login' || event.url.startsWith('/register')) {
          this.hideSideBar = true
        } else if (this.hideSideBar) this.hideSideBar = false;
      }
    });
    this.reloadSignalR();
    this.authService.logged.subscribe(_ => this.reloadSignalR());
    
    this.signalrService.newMessage.subscribe(_ => this.newMessages++);
  }
  
  reloadSignalR() {
      if (this.authService.isLoggedIn) {
          this.signalrService.startConnection();
      }
  }

  getThemeName(t: string): string {
    switch (t) {
      case 'default':
        return 'Светлая';
      case 'dark':
        return 'Темная';
      case 'cosmic':
        return 'Космическая';
    }
  }
}
