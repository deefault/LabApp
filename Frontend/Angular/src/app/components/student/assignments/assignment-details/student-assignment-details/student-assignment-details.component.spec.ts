import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { StudentAssignmentDetailsComponent } from './student-assignment-details.component';

describe('StudentAssignmentDetailsComponent', () => {
  let component: StudentAssignmentDetailsComponent;
  let fixture: ComponentFixture<StudentAssignmentDetailsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ StudentAssignmentDetailsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(StudentAssignmentDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
