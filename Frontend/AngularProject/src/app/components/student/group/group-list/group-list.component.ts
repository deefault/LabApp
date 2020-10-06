import {Component, isDevMode, OnInit} from '@angular/core';
import {Router} from "@angular/router";
import {GroupDto, GroupsService} from "../../../../clients/student";
import {NbDialogRef, NbDialogService} from "@nebular/theme";
import {BaseComponent} from "../../../../core/base-component";
import {HttpErrorResponse} from "@angular/common/http";

@Component({
  selector: 'app-group-list',
  templateUrl: './group-list.component.html',
  styleUrls: ['./group-list.component.css']
})
export class GroupListComponentStudent extends BaseComponent implements OnInit {
  items: GroupDto[] = [];
  code: string = "";

  constructor(
    private router: Router,
    private groupService: GroupsService,
    private dialogService: NbDialogService,
  ) {
    super();
  }

  ngOnInit() {
    this.refresh();
  }

  onSelect(item: GroupDto) {
    this.router.navigate(['student', 'groups', item.id]);
  }

  join() {

  }

  open() {
    this.dialogService.open(JoinGroupDialog).onClose.subscribe((data: boolean) => {
      if (data) this.refresh();
    })
  }

  private refresh() {
    this.loading = true;
    this.groupService.list().subscribe(next => {
      this.items = next;
      this.loading = false;
    }, error => {
      if (isDevMode()) console.log(error);
      this.loading = false;
    })
  }
}

@Component({
  selector: 'app-join-group-dialog',
  template: `
    <nb-card>
      <nb-card-header>
        <h6>Введите код</h6>
      </nb-card-header>
      <nb-card-body>
        <form #form="ngForm">
          <div class="form-group">
            <div class="alert text-danger" *ngIf="error">{{error}}</div>
            <input nbInput minlength="{{codeLength}}" maxlength="{{codeLength}}" required type="text" [(ngModel)]="code"
                   #input placeholder="{{'#'.repeat(codeLength)}}" class="text-center" name="code"
                   style="text-transform: uppercase;">
          </div>
        </form>
      </nb-card-body>
      <nb-card-footer>
        <button class="float-left" nbButton status="basic" (click)="dialogRef.close(false)">Отмена</button>
        <button class="ml-3 float-right" nbButton (click)="join()" [disabled]="form.invalid">Вступить в группу
        </button>
      </nb-card-footer>
    </nb-card>
  `
})
export class JoinGroupDialog {

  codeLength = 8;
  error: string;
  code: string;

  constructor(private dialogService: NbDialogService,
              public dialogRef: NbDialogRef<JoinGroupDialog>,
              private groupService: GroupsService,
  ) {
  }

  join() {
    this.groupService.joinByCode(this.code.toUpperCase()).subscribe(data => {
        this.dialogRef.close(true);
      },
      (err: HttpErrorResponse) => {
        if (err.status == 404) {
          this.error = "Группа не найдена";
          if (isDevMode()) console.log(err);
        }
      }
    );
  }
}
