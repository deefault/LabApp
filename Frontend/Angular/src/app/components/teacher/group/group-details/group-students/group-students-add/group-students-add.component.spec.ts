import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GroupStudentsAddComponent } from './group-students-add.component';

describe('GroupStudentsAddComponent', () => {
  let component: GroupStudentsAddComponent;
  let fixture: ComponentFixture<GroupStudentsAddComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GroupStudentsAddComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GroupStudentsAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
