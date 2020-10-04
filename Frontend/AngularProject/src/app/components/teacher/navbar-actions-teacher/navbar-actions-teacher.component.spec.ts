import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NavbarActionsTeacherComponent } from './navbar-actions-teacher.component';

describe('NavbarActionsTeacherComponent', () => {
  let component: NavbarActionsTeacherComponent;
  let fixture: ComponentFixture<NavbarActionsTeacherComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NavbarActionsTeacherComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NavbarActionsTeacherComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
