import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AssignmentDetailsStudentComponent } from './assignment-details-student.component';
import {NbCardModule, NbInputModule, NbListModule} from "@nebular/theme";

describe('AssignmentDetailsComponent', () => {
  let component: AssignmentDetailsStudentComponent;
  let fixture: ComponentFixture<AssignmentDetailsStudentComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AssignmentDetailsStudentComponent ],
      imports: [NbCardModule, NbListModule, NbInputModule]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AssignmentDetailsStudentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
