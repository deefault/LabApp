import { NgModule, ModuleWithProviders, SkipSelf, Optional } from '@angular/core';
import { Configuration } from './configuration';
import { HttpClient } from '@angular/common/http';


import { ApplicationService } from './api/application.service';
import { AssignmentsService } from './api/assignments.service';
import { GroupService } from './api/group.service';
import { LessonsService } from './api/lessons.service';
import { RedirectService } from './api/redirect.service';
import { StudentAssignmentsService } from './api/studentAssignments.service';
import { SubjectService } from './api/subject.service';
import { TeacherService } from './api/teacher.service';
import { UserService } from './api/user.service';

@NgModule({
  imports:      [],
  declarations: [],
  exports:      [],
  providers: [
    ApplicationService,
    AssignmentsService,
    GroupService,
    LessonsService,
    RedirectService,
    StudentAssignmentsService,
    SubjectService,
    TeacherService,
    UserService ]
})
export class ApiModule {
    public static forRoot(configurationFactory: () => Configuration): ModuleWithProviders {
        return {
            ngModule: ApiModule,
            providers: [ { provide: Configuration, useFactory: configurationFactory } ]
        };
    }

    constructor( @Optional() @SkipSelf() parentModule: ApiModule,
                 @Optional() http: HttpClient) {
        if (parentModule) {
            throw new Error('ApiModule is already loaded. Import in your base AppModule only.');
        }
        if (!http) {
            throw new Error('You need to import the HttpClientModule in your AppModule! \n' +
            'See also https://github.com/angular/angular/issues/20575');
        }
    }
}
