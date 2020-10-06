import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {SubjectDto, SubjectService} from "../../../../clients/teacher";
import {NgModel} from "@angular/forms";

@Component({
  selector: 'app-subject-selector',
  templateUrl: './subject-selector.component.html',
  styleUrls: ['./subject-selector.component.css']
})
export class SubjectSelectorComponent implements OnInit {

  subjects: SubjectDto[] = [];

  @Input() readonly: boolean = false;
  @Input() required: boolean = true;
  @Input() placeHolderText: string = "Предмет";
  @Input() subjectId: number;
  selected: SubjectDto = null;
  @Output() selectedChange: EventEmitter<SubjectDto> = new EventEmitter<SubjectDto>();

  constructor(
    private subjectService: SubjectService,
  ) {
  }

  ngOnInit() {
    this.subjectService.get().subscribe(data => {
      this.subjects = data;
      if (this.subjectId) {
        let s = this.subjects.find(x => x.id == this.subjectId);
        if (s) {
          this.selected = s;
        }
      }
    });
  }

  isInvalid(ngModel: NgModel) {
    return this.required && ngModel.invalid && (ngModel.dirty || ngModel.touched)
  }
}
