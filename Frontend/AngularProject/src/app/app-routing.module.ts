import {NgModule} from '@angular/core';
import {Routes, RouterModule} from '@angular/router';
import {LoginComponent} from "./components/login/login.component";
import {RegisterComponent} from "./components/register/register.component";
import {SubjectListComponent} from "./components/teacher/subjects/subject-list/subject-list.component";
import {SubjectDetailsComponent} from "./components/teacher/subjects/subject-details/subject-details.component";
import {SubjectAddComponent} from "./components/teacher/subjects/subject-add/subject-add.component";
import {GroupListComponent} from "./components/teacher/group/group-list/group-list.component";
import {GroupAddComponent} from "./components/teacher/group/group-add/group-add.component";
import {GroupDetailsComponent} from "./components/teacher/group/group-details/group-details.component";
import {GroupStudentsComponent} from "./components/teacher/group/group-details/group-students/group-students.component";
import {SubjectDetailsLessonsComponent} from "./components/teacher/subjects/subject-details/subject-details-lessons/subject-details-lessons.component";
import {LessonAddComponent} from "./components/teacher/subjects/subject-details/subject-details-lessons/lesson-add/lesson-add.component";
import {LessonDetailsComponent} from "./components/teacher/lessons/lesson-details/lesson-details.component";
import {GroupListComponentStudent} from "./components/student/group/group-list/group-list.component";
import {GroupDetailsComponentStudent} from "./components/student/group/group-details/group-details.component";
import {SubjectListComponentStudent} from "./components/student/subject/subject-list/subject-list.component";
import {SubjectDetailsComponentStudent} from "./components/student/subject/subject-details/subject-details.component";
import {LessonsComponentStudent} from "./components/student/subject/subject-details/subject-details-lessons/lessons-component-student.component";
import {AssignmentAddComponent} from "./components/teacher/assignment/assignment-add/assignment-add.component";
import {AssignmentListComponent} from "./components/teacher/assignment/assignment-list/assignment-list.component";
import {SubjectDetailsAssignmentsComponent} from "./components/teacher/subjects/subject-details/subject-details-assignments/subject-details-assignments.component";
import {AssignmentDetailsComponent} from "./components/teacher/assignment/assignment-details/assignment-details.component";
import {StudentAssignmentListComponent} from "./components/teacher/assignment/student-assignment/student-assignment-list/student-assignment-list.component";
import {SubjectAssignmentsStudentComponent} from "./components/student/subject/subject-details/subject-assignments-student/subject-assignments-student.component";
import {AssignmentDetailsStudentComponent} from "./components/student/assignments/assignment-details/assignment-details-student.component";
import {StudentAssignmentListAllComponent} from "./components/teacher/assignment/student-assignment/student-assignment-list-all/student-assignment-list-all.component";
import {StudentAssignmentDetailsComponent} from "./components/student/assignments/assignment-details/student-assignment-details/student-assignment-details.component";
import {StudentAssignmentDetailsTeacherComponent} from "./components/teacher/assignment/student-assignment/student-assignment-details/student-assignment-details.component";
import {GroupTableComponent} from "./components/teacher/group/group-details/group-table/group-table.component";
import {ConversationListComponent} from "./shared/components/conversation-list/conversation-list.component"; // CLI imports router

const routes: Routes = [
  {path: 'login', component: LoginComponent},
  {path: 'register/:userType', component: RegisterComponent},
  {
    path: 'subjects', children:
      [
        {path: '', component: SubjectListComponent},
        {path: 'add', component: SubjectAddComponent},
        {
          path: ':id', component: SubjectDetailsComponent, children:
            [
              {
                path: 'lessons', children: [
                  {path: '', component: SubjectDetailsLessonsComponent},
                  {path: 'add', component: LessonAddComponent},
                  {path: ':lessonId', component: LessonDetailsComponent}
                ]
              },
              {
                path: 'assignments', component: SubjectDetailsAssignmentsComponent
              }
            ]
        }
      ]
  },
  {
    path: 'groups', children:
      [
        {path: '', component: GroupListComponent},
        {path: 'add', component: GroupAddComponent},
        {
          path: ':id', component: GroupDetailsComponent, children:
            [
              {path: 'students', component: GroupStudentsComponent},
              {path: 'table', component: GroupTableComponent}
            ]
        }
      ]
  },
  {
    path: 'assignments', children:
      [
        {path: '', component: AssignmentListComponent},
        {path: 'add', component: AssignmentAddComponent},
        {path: ':id', component: AssignmentDetailsComponent}
      ]
  },
  {
    path: 'student-assignments', children:
      [
        {path: '', component: StudentAssignmentListAllComponent},
        {path: ':id', component: StudentAssignmentDetailsTeacherComponent}
      ]
  },
  {
    path: 'chats', component: ConversationListComponent
  },
  // STUDENT
  {
    path: 'student', children:
      [
        {
          path: 'groups', children:
            [
              {path: '', component: GroupListComponentStudent},
              {path: ':groupId', component: GroupDetailsComponentStudent}
            ]
        },
        {
          path: 'subjects', children:
            [
              {path: '', component: SubjectListComponentStudent},
              {
                path: ':groupId', component: SubjectDetailsComponentStudent, children:
                  [
                    {
                      path: 'lessons', children: [
                        {path: '', component: LessonsComponentStudent},
                        //{path: ':lessonId', component: LessonDetailsComponent}
                      ]
                    },
                    {
                      path: 'assignments', component: SubjectAssignmentsStudentComponent, children: [
                        {path: ":id", component: AssignmentDetailsStudentComponent}

                      ]
                    }
                  ]
              }
            ]
        },
        {
          path: 'assignments', children: [
            {path: ":id", component: AssignmentDetailsStudentComponent}

          ]
        }
      ]
  }
];

// configures NgModule imports and exports
@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {

}

