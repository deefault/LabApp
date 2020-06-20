import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LessonsComponentStudent } from './lessons-component-student.component';

describe('SubjectDetailsLessonsComponent', () => {
  let component: LessonsComponentStudent;
  let fixture: ComponentFixture<LessonsComponentStudent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LessonsComponentStudent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LessonsComponentStudent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
