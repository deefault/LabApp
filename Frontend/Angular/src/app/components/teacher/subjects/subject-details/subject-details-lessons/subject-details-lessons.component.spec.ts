import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SubjectDetailsLessonsComponent } from './subject-details-lessons.component';

describe('SubjectDetailsLessonsComponent', () => {
  let component: SubjectDetailsLessonsComponent;
  let fixture: ComponentFixture<SubjectDetailsLessonsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SubjectDetailsLessonsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SubjectDetailsLessonsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
