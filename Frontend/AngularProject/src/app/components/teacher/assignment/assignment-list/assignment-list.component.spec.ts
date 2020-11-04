import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AssignmentListComponent } from './assignment-list.component';
import {NbAlertModule, NbCardModule, NbListModule} from "@nebular/theme";

describe('AssignmentListComponent', () => {
  let component: AssignmentListComponent;
  let fixture: ComponentFixture<AssignmentListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AssignmentListComponent ],
      imports: [NbCardModule, NbListModule]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AssignmentListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
