import {BrowserModule} from '@angular/platform-browser';
import {NgModule} from '@angular/core';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {HTTP_INTERCEPTORS, HttpClientModule} from '@angular/common/http';
import {RouterModule} from '@angular/router';
import {AppComponent} from './app.component';
import {NavMenuComponent} from './components/nav-menu/nav-menu.component';
import {HomeComponent} from './components/home/home.component';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {AppRoutingModule} from './app-routing.module';
import {NebularModule} from "./angmaterial";
import {
    NbAccordionModule,
    NbActionsModule,
    NbAlertModule,
    NbCardModule, NbChatMessageFileComponent, NbChatModule, NbDialogModule,
    NbIconModule,
    NbListModule,
    NbMenuModule, NbRouteTabsetModule, NbSelectModule, NbSpinnerModule, NbTabsetModule,
    NbThemeModule, NbToastrModule, NbToggleModule, NbTooltipModule, NbUserModule, NbWindowModule,
} from '@nebular/theme';
import {HttpService} from "./services/http/http.service";
import {AuthService} from "./services/auth/auth.service";
import {JwtModule} from "@auth0/angular-jwt";
import {JwtHelperService} from '@auth0/angular-jwt';
import {
  NbSidebarModule,
  NbLayoutModule,
  NbButtonModule,
  NbInputModule,
} from '@nebular/theme';
import {LoginComponent} from "./components/login/login.component";
import {NbEvaIconsModule} from '@nebular/eva-icons';
import {RegisterComponent} from "./components/register/register.component";
import {AuthenticationService, BASE_PATH} from "./clients/common";
import {
  ApiModule as ApiModuleStudent,
  BASE_PATH as STUDENT_BASE_PATH,
  StudentService
}
from "./clients/student";
import {
  ApiModule as ApiModuleTeacher,
  BASE_PATH as TEACHER_BASE_PATH,
  GroupService, LessonsService,
  SubjectService,
  TeacherService
} from "./clients/teacher";

import {
  ApiModule as ApiModuleCommon,
  BASE_PATH as COMMON_BASE_PATH,
} from "./clients/common";

import {environment} from "../environments/environment";
import {MenuComponent} from "./components/menu/menu.component";
import {TokenInterceptor} from "./interceptors/token.interceptor";
import {SubjectListComponent} from "./components/teacher/subjects/subject-list/subject-list.component";
import {ButtonBackComponent} from "./shared/components/button-back/button-back.component";
import {SubjectDetailsComponent} from "./components/teacher/subjects/subject-details/subject-details.component";
import {SubjectAddComponent} from "./components/teacher/subjects/subject-add/subject-add.component";
import {GroupListComponent} from "./components/teacher/group/group-list/group-list.component";
import {GroupAddComponent} from "./components/teacher/group/group-add/group-add.component";
import {GroupDetailsComponent} from "./components/teacher/group/group-details/group-details.component";
import {GroupStudentsComponent} from "./components/teacher/group/group-details/group-students/group-students.component";
import {GroupStudentsAddComponent} from "./components/teacher/group/group-details/group-students/group-students-add/group-students-add.component";
import {SpinnerComponent} from "./shared/components/spinner/spinner.component";
import {SubjectDetailsLessonsComponent} from "./components/teacher/subjects/subject-details/subject-details-lessons/subject-details-lessons.component";
import {LessonAddComponent} from "./components/teacher/subjects/subject-details/subject-details-lessons/lesson-add/lesson-add.component";
import {LessonDetailsComponent} from "./components/teacher/lessons/lesson-details/lesson-details.component";
import {AttachmentListComponent} from "./shared/components/attachment-list/attachment-list.component";
import {NotificationService} from "./services/notification/notification.service";
import {JoinGroupDialog, GroupListComponentStudent} from "./components/student/group/group-list/group-list.component";
import {GroupDetailsComponentStudent} from "./components/student/group/group-details/group-details.component";
import {SubjectListComponentStudent} from "./components/student/subject/subject-list/subject-list.component";
import {SubjectDetailsComponentStudent} from "./components/student/subject/subject-details/subject-details.component";
import {LessonsComponentStudent} from "./components/student/subject/subject-details/subject-details-lessons/lessons-component-student.component";
import {UserComponent} from "./shared/components/user/user.component";
import {AssignmentAddComponent} from "./components/teacher/assignment/assignment-add/assignment-add.component";
import {SubjectSelectorComponent} from "./shared/components/teacher/subject-selector/subject-selector.component";
import { AssignmentListComponent } from './components/teacher/assignment/assignment-list/assignment-list.component';
import {SettingsService} from "./services/settings.service";
import { LabelComponent } from './shared/components/forms/label/label.component';
import { InputErrorComponent } from './shared/components/forms/input-error/input-error.component';
import { BackHeaderComponent } from './shared/components/forms/form-header/back-header.component';
import { AssignmentDetailsComponent } from './components/teacher/assignment/assignment-details/assignment-details.component';
import { StudentAssignmentListComponent } from './components/teacher/assignment/student-assignment/student-assignment-list/student-assignment-list.component';
import { SubjectDetailsAssignmentsComponent } from './components/teacher/subjects/subject-details/subject-details-assignments/subject-details-assignments.component';
import { SubjectAssignmentsStudentComponent } from './components/student/subject/subject-details/subject-assignments-student/subject-assignments-student.component';
import {AssignmentDetailsStudentComponent} from "./components/student/assignments/assignment-details/assignment-details-student.component";
import { StudentAssignmentDetailsComponent } from './components/student/assignments/assignment-details/student-assignment-details/student-assignment-details.component';
import {PreviousLocationService} from "./services/previous-location.service";
import { NavbarActionsTeacherComponent } from './components/teacher/navbar-actions-teacher/navbar-actions-teacher.component';
import {AppService} from "./services/app.service";
import { StudentAssignmentListAllComponent } from './components/teacher/assignment/student-assignment/student-assignment-list-all/student-assignment-list-all.component';
import {StudentAssignmentDetailsTeacherComponent} from "./components/teacher/assignment/student-assignment/student-assignment-details/student-assignment-details.component";
import { ConversationComponent } from './shared/components/conversation/conversation.component';
import { GroupTableComponent } from './components/teacher/group/group-details/group-table/group-table.component';
import { NavbarActionsStudentComponent } from './components/student/navbar-actions-student/navbar-actions-student.component';
import { ConversationListComponent } from './shared/components/conversation-list/conversation-list.component';


export function tokenGetter() {
  return localStorage.getItem(AuthService.TokenName);
}


@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    LoginComponent,
    RegisterComponent,
    MenuComponent,
    SubjectListComponent,
    SubjectDetailsComponent,
    SubjectAddComponent,
    SubjectDetailsLessonsComponent,

    // Teacher
    GroupListComponent,
    GroupAddComponent,
    GroupDetailsComponent,
    GroupStudentsComponent,
    GroupStudentsAddComponent,
    LessonAddComponent,
    LessonDetailsComponent,
    AssignmentAddComponent,

    //Student
    GroupListComponentStudent,
    GroupDetailsComponentStudent,
    SubjectListComponentStudent,
    SubjectDetailsComponentStudent,
    LessonsComponentStudent,
    JoinGroupDialog,

    // Shared
    AttachmentListComponent,
    ButtonBackComponent,
    SpinnerComponent,
    UserComponent,
    SubjectSelectorComponent,
    AssignmentListComponent,
    LabelComponent,
    InputErrorComponent,
    BackHeaderComponent,
    AssignmentDetailsComponent,
    StudentAssignmentListComponent,
    SubjectDetailsAssignmentsComponent,
    SubjectAssignmentsStudentComponent,
    AssignmentDetailsStudentComponent,
    StudentAssignmentDetailsComponent,
    NavbarActionsTeacherComponent,
    StudentAssignmentListAllComponent,
    StudentAssignmentDetailsTeacherComponent,
    ConversationComponent,
    GroupTableComponent,
    NavbarActionsStudentComponent,
    ConversationListComponent
  ],
  entryComponents: [
    GroupStudentsAddComponent,
    JoinGroupDialog,
  ],
    imports: [
        ReactiveFormsModule,
        AppRoutingModule,
        BrowserModule.withServerTransition({appId: 'ng-cli-universal'}),
        HttpClientModule,
        FormsModule,
        RouterModule.forRoot([
            {path: '', component: HomeComponent, pathMatch: 'full'},
        ]),
        JwtModule.forRoot({
            config: {
                tokenGetter: tokenGetter,
                blacklistedRoutes: ["/api/authentication/token"]
            }
        }),
        BrowserAnimationsModule,
        NbThemeModule.forRoot({name: 'default'}),
        NbLayoutModule,
        NbSidebarModule.forRoot(),
        NbButtonModule,
        NbInputModule,
        NbCardModule,
        NbEvaIconsModule,
        NbActionsModule,
        NbAlertModule,
        NbIconModule,
        NbMenuModule.forRoot(),
        NbListModule,
        NbSelectModule,
        NbUserModule,
        NbRouteTabsetModule,
        NbTabsetModule,
        NbDialogModule.forRoot(),
        NbToastrModule.forRoot(),
        NbWindowModule.forRoot(),
        NbSpinnerModule,
        NbToastrModule.forRoot(),
        NbToggleModule,
        NbChatModule.forRoot(),
        NbTooltipModule,
        NbAccordionModule,
        ApiModuleTeacher,
        ApiModuleStudent,
        ApiModuleCommon
    ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: TokenInterceptor,
      multi: true
    },
    {
      provide: BASE_PATH,
      useFactory: () => {
        return environment.baseUrl;
      }
    },
    {
      provide: TEACHER_BASE_PATH,
      useFactory: () => {
          return environment.baseUrl;
      }
    },
    {
      provide: STUDENT_BASE_PATH,
      useFactory: () => {
          return environment.baseUrl;
      }
    },
    {
      provide: COMMON_BASE_PATH,
      useFactory: () => {
          return environment.baseUrl;
      }
    },
    SettingsService,
    HttpService,
    AuthService,
    AuthenticationService,
    TeacherService,
    StudentService,
    SubjectService,
    GroupService,
    LessonsService,
    NotificationService,
    PreviousLocationService,
    AppService
  ],
  bootstrap: [AppComponent]
})
export class AppModule {

}
