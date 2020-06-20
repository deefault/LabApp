import {Component, isDevMode, OnInit} from '@angular/core';
import {SubjectDto, SubjectService} from "../../../../clients/teacher";
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {Router} from "@angular/router";
import {ReactiveFormsModule} from "@angular/forms";

@Component({
  selector: 'app-subject-add',
  templateUrl: './subject-add.component.html',
  styleUrls: ['./subject-add.component.css']
})
export class SubjectAddComponent implements OnInit {

  subject: SubjectDto;
  form: FormGroup;

  constructor(
    private subjectService: SubjectService,
    private router: Router,
  ) {
  }

  ngOnInit() {
    this.subject = new class implements SubjectDto {
      description: string;
      id: number;
      name: string;
      teacherId: number;
    }
    this.form = new FormGroup({
      'name': new FormControl('', Validators.required),
      'description': new FormControl(''),
    });
  }

  get f() {
    return this.form.controls;
  }

  get name() {
    return this.form.get('name');
  }

  get description() {
    return this.description.get('power');
  }

  onSubmit() {
    this.subject = Object.assign(this.subject, this.form.value);
    this.subjectService.add(this.subject).subscribe(
      data => {
        this.router.navigate(['/', 'subjects', data.id])
      }, error => {
        if (isDevMode()) console.log(error);
      }
    )
  }
}
