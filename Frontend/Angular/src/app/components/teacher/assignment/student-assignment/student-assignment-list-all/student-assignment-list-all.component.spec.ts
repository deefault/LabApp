import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { StudentAssignmentListAllComponent } from './student-assignment-list-all.component';

describe('StudentAssignmentListNewComponent', () => {
  let component: StudentAssignmentListAllComponent;
  let fixture: ComponentFixture<StudentAssignmentListAllComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ StudentAssignmentListAllComponent ]
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
