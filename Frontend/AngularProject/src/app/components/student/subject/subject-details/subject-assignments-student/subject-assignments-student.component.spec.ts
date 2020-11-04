import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SubjectAssignmentsStudentComponent } from './subject-assignments-student.component';
import {NbAlertModule, NbCardModule, NbListModule, NbToggleModule} from "@nebular/theme";

describe('SubjectAssignmentsStudentComponent', () => {
  let component: SubjectAssignmentsStudentComponent;
  let fixture: ComponentFixture<SubjectAssignmentsStudentComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SubjectAssignmentsStudentComponent ],
      imports: [NbAlertModule, NbListModule]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SubjectAssignmentsStudentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
