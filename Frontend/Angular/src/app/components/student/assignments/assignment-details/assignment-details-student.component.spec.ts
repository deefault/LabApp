import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AssignmentDetailsStudentComponent } from './assignment-details-student.component';

describe('AssignmentDetailsComponent', () => {
  let component: AssignmentDetailsStudentComponent;
  let fixture: ComponentFixture<AssignmentDetailsStudentComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AssignmentDetailsStudentComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AssignmentDetailsStudentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
