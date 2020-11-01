import {Component, OnInit} from '@angular/core';
import {AuthService} from "../../services/auth/auth.service";
import {NbMenuItem, NbThemeService} from "@nebular/theme";
import {SettingsService} from "../../services/settings.service";
import {ApplicationService} from "../../clients/teacher";

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.css']
})
export class MenuComponent implements OnInit {

  teacherItems: NbMenuItem[] = [
    {
      title: 'Предметы',
      icon: 'book-open-outline',
      link: `/subjects`,
    },
    {
      title: 'Группы',
      icon: 'person-outline',
      link: '/groups',
    },
    {
      title: 'Лабораторные',
      icon: 'briefcase-outline',
      link: '/assignments',
    },
    {
      title: 'На проверку',
      icon: 'bulb-outline',
      link: '/student-assignments',
    },
    {
      title: 'Сообщения',
      icon: 'message-circle-outline\n',
      link: '/messages',
    }
  ]

  studentItems: NbMenuItem[] = [
    {
      title: 'Предметы',
      icon: 'book-open-outline',
      link: `/student/subjects`,
    },
    {
      title: 'Группы',
      icon: 'person-outline',
      link: '/student/groups',
    },
    {
      title: 'Сообщения',
      icon: 'message-circle-outline\n',
      link: '/student/messages',
    }
  ]
  items: any = [];

  constructor(
    private authService: AuthService,
    private themeService: NbThemeService,
    private settings: SettingsService,
    private appService: ApplicationService,
  ) {
  }

  ngOnInit() {
    this.items = this.authService.isTeacher ? this.teacherItems : this.studentItems;
  }

}
