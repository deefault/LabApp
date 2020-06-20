import {Component} from '@angular/core';
import {AuthService} from "./services/auth/auth.service";
import {ActivatedRoute, NavigationEnd, Router} from "@angular/router";
import {SettingsService} from "./services/settings.service";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent {
  title = 'app';
  hideSideBar = false;

  constructor(private authService: AuthService, private router: Router, private settings: SettingsService) {
    router.events.subscribe(event => {
      if (event instanceof NavigationEnd){
        if (event.url == '/login' || event.url.startsWith('/register')){
          this.hideSideBar = true
        }
        else if (this.hideSideBar) this.hideSideBar = false;
      }
    })
  }

  getThemeName(t: string): string {
    switch (t) {
      case 'default': return 'Светлая';
      case 'dark': return 'Темная';
      case 'cosmic': return 'Космическая';
    }
  }
}
