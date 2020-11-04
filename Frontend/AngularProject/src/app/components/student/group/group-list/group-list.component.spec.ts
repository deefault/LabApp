import {async, ComponentFixture, TestBed} from '@angular/core/testing';
import {GroupListComponent} from "../../../teacher/group/group-list/group-list.component";
import {NbListModule} from "@nebular/theme";

describe('GroupListComponent', () => {
  let component: GroupListComponent;
  let fixture: ComponentFixture<GroupListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [GroupListComponent],
      imports: [NbListModule]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GroupListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
