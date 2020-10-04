import {Component, OnInit, ContentChild, TemplateRef, Output, EventEmitter, Input} from '@angular/core';
import {Location} from '@angular/common';
import {Router} from "@angular/router";

@Component({
  selector: 'button-back',
  template: `
    <button (click)="back()" class="btn">
      <nb-icon icon="arrow-back-outline"></nb-icon>
    </button>`,
  styles: []
})
export class ButtonBackComponent implements OnInit {

  @Output() backClick: EventEmitter<any> = new EventEmitter();
  @Input() backUrl: any[] = undefined;

  constructor(
    private location: Location,
    private router: Router
  ) {
  }

  ngOnInit() {
  }

  back() {
    if (this.backClick.observers.length > 0)
      this.backClick.emit();
    else {
      if (!this.backUrl) this.location.back();
      else this.router.navigate(this.backUrl);
    }
  }
}
