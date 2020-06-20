import {Component, EventEmitter, isDevMode, OnInit} from '@angular/core';
import {NbWindowRef} from "@nebular/theme";
import {GroupService} from "../../../../../../clients/teacher";
import {HttpErrorResponse, HttpResponse} from "@angular/common/http";

@Component({
  selector: 'app-group-students-add',
  templateUrl: './group-students-add.component.html',
  styleUrls: ['./group-students-add.component.css']
})
export class GroupStudentsAddComponent implements OnInit {

  groupId: number;
  email: string;

  error: string;

  onAdded: EventEmitter<boolean>;

  constructor(
    protected ref: NbWindowRef,
    private groupService: GroupService
  ) {
    ref.config.title = "Добавить студента"
  }

  ngOnInit() {
  }

  private add() {
    this.groupService.addStudent(this.groupId, this.email).subscribe(
      data => {
        this.onAdded.emit(true); //TODO: check
        this.ref.close();
      },
      (error: HttpErrorResponse) => {
        if (isDevMode()) console.log(error);
        if (error.status == 404) {
          this.error = "Студент не найден";
        }
        if (error.error["email"] != undefined) {
          this.error = "Неверный формат почты"
        }
      });
  }
}
