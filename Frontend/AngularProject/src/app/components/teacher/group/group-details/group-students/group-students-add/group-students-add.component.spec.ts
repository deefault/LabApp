import {async, ComponentFixture, TestBed} from '@angular/core/testing';

import {GroupStudentsAddComponent} from './group-students-add.component';
import {NbInputModule} from "@nebular/theme";

describe('GroupStudentsAddComponent', () => {
  let component: GroupStudentsAddComponent;
  let fixture: ComponentFixture<GroupStudentsAddComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [GroupStudentsAddComponent],
      imports: [NbInputModule]
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
