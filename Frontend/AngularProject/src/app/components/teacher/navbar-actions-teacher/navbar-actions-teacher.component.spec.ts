import {async, ComponentFixture, inject, TestBed} from '@angular/core/testing';

import {NavbarActionsTeacherComponent} from './navbar-actions-teacher.component';
import {EventBusService} from "../../../services/event-bus.service";
import {NbActionsModule} from "@nebular/theme";
import {RouterModule} from "@angular/router";
import {NO_ERRORS_SCHEMA} from "@angular/core";

describe('NavbarActionsTeacherComponent', () => {
  let component: NavbarActionsTeacherComponent;
  let fixture: ComponentFixture<NavbarActionsTeacherComponent>;

  const bus = new EventBusService()

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [NavbarActionsTeacherComponent],
      providers: [
        {provide: EventBusService, useValue: bus},
      ],
      imports: [NbActionsModule, RouterModule],
      schemas: [NO_ERRORS_SCHEMA]
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

  it('should add new messages', () => {
    component.messages = 5;
    component.ngOnInit();
    bus.changeNewMessageNumber.emit(10);

    expect(component.messages).toBe(15);

  });
});
