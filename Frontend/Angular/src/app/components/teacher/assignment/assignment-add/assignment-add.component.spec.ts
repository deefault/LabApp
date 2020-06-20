import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AssignmentAddComponent } from './assignment-add.component';

describe('AssignmentAddComponent', () => {
  let component: AssignmentAddComponent;
  let fixture: ComponentFixture<AssignmentAddComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AssignmentAddComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AssignmentAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
