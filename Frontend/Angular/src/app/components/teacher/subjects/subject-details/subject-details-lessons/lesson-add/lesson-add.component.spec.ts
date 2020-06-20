import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LessonAddComponent } from './lesson-add.component';

describe('LessonAddComponent', () => {
  let component: LessonAddComponent;
  let fixture: ComponentFixture<LessonAddComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LessonAddComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LessonAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
