import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { StudentAssignmentListAllComponent } from './student-assignment-list-all.component';
import {NbCardModule, NbToggleModule} from "@nebular/theme";

describe('StudentAssignmentListNewComponent', () => {
  let component: StudentAssignmentListAllComponent;
  let fixture: ComponentFixture<StudentAssignmentListAllComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ StudentAssignmentListAllComponent ],
      imports: [NbToggleModule, NbCardModule]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(StudentAssignmentListAllComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
