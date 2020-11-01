import {Injectable} from '@angular/core';
import {ApplicationService, InitDtoTeacher} from "../clients/teacher";
import {ApplicationService as StudentApplicationService, InitDtoStudent} from "../clients/student";
import {ApplicationService as CommonApplicationService} from "../clients/common";
import {BehaviorSubject} from "rxjs";
import {AuthService} from "./auth/auth.service";
import {SignalRService} from "./signalr/signalr.service";

@Injectable({
  providedIn: 'root'
})
export class AppService {

  public initTeacher: BehaviorSubject<InitDtoTeacher> = new BehaviorSubject<InitDtoTeacher>(null);
  public initStudent: BehaviorSubject<InitDtoStudent> = new BehaviorSubject<InitDtoStudent>(null);
  public attachmentUrl: string;

  constructor(
    private teacherAppService: ApplicationService,
    private authService: AuthService,
    private common: CommonApplicationService,
    private signalrService: SignalRService,
    private studentAppService: StudentApplicationService
  ) {
    this.reload();
    this.authService.logged.subscribe(next => this.reload());
  }

  reload() {
    if (this.authService.isLoggedIn)
      if (this.authService.isTeacher) {
        this.teacherAppService.init().subscribe(x => {
          this.initTeacher.next(x);
        });
      } else {
        this.studentAppService.init().subscribe(x => {
          this.initStudent.next(x);
        });
      }

    this.common.getImageDownloadPath().subscribe(data => {
      this.attachmentUrl = data
    });
  }

  getAttachmentUrl(name: string, downloadName?: string) {
    return downloadName ? `${this.attachmentUrl}/${name}?downloadName=${encodeURIComponent(downloadName)}` : `${this.attachmentUrl}/${name}`;
  }

}
