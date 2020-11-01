import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NavbarActionsStudentComponent } from './navbar-actions-student.component';

describe('NavbarActionsStudentComponent', () => {
  let component: NavbarActionsStudentComponent;
  let fixture: ComponentFixture<NavbarActionsStudentComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NavbarActionsStudentComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NavbarActionsStudentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
