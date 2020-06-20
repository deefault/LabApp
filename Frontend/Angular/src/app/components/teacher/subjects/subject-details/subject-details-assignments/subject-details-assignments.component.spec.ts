import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SubjectDetailsAssignmentsComponent } from './subject-details-assignments.component';

describe('SubjectDetailsAssignmentsComponent', () => {
  let component: SubjectDetailsAssignmentsComponent;
  let fixture: ComponentFixture<SubjectDetailsAssignmentsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SubjectDetailsAssignmentsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SubjectDetailsAssignmentsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
