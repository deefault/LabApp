import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BackHeaderComponent } from './back-header.component';

describe('FormHeaderComponent', () => {
  let component: BackHeaderComponent;
  let fixture: ComponentFixture<BackHeaderComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BackHeaderComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BackHeaderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
