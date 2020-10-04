import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from "@angular/router";
import {UserType} from "../../enums/user-type.enum";
import {TeacherRegisterDto, TeacherService} from "../../clients/teacher";
import {StudentRegisterDto, StudentService} from "../../clients/student";
import {NgModel} from "@angular/forms";
import {isDevMode} from '@angular/core';
import {BaseComponent} from '../../core/base-component'
import {HttpErrorResponse} from "@angular/common/http";
import {Observable} from "rxjs";

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent extends BaseComponent implements OnInit {

  private submitted: boolean;
  private error: string;
  private type: UserType;
  private model: TeacherRegisterDto;
  private studentProfile: StudentRegisterDto;
  private teacherProfile: TeacherRegisterDto;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private teacherService: TeacherService,
    private studentService: StudentService
  ) {
    super();
  }

  ngOnInit() {
    this.type = this.route.snapshot.params["userType"] == 'teacher' ? UserType.Teacher : UserType.Student;
    this.model = new class implements TeacherRegisterDto {
      academicRankId: number;
      contactEmail: string;
      dateBirth: Date;
      email: string;
      middlename: string;
      name: string;
      password: string;
      phone: number;
      phoneCode: number;
      surname: string;
    }
  }

  onSubmit() {
    this.submitted = true;
    if (this.type === UserType.Teacher) {
      this.teacherService.register(this.model).subscribe(
        data => {
          this.router.navigate(["/", "login"]);
        },
        (error: HttpErrorResponse) => {
          if (error.status == 400) {
            this.error = error.error;
          } else this.error = this.getErrorText(error.error)
        }
      );
    } else {
      this.studentService.register(this.model).subscribe(
        data => {
          this.router.navigate(["/", 'login']);
        },
        (error: HttpErrorResponse) => {
          if (error.status == 400) {
            this.error = error.error;
          } else this.error = this.getErrorText(error.error)
        }
      );
    }
  }

  isInvalid(ngModel: NgModel) {
    return ngModel.invalid && (ngModel.dirty || ngModel.touched)
  }
}
