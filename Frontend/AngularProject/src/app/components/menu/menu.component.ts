import {Component, OnInit} from '@angular/core';
import {AuthService} from "../../services/auth/auth.service";
import {NbMenuItem} from "@nebular/theme";


const chatItem: NbMenuItem = {
  title: 'Сообщения',
  icon: 'message-circle-outline',
  link: '/chats',
};

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
    chatItem
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
    chatItem
  ]
  items: any = [];

  constructor(
    private authService: AuthService,
  ) {
  }

  ngOnInit() {
    this.items = this.authService.isTeacher ? this.teacherItems : this.studentItems;
  }

}
