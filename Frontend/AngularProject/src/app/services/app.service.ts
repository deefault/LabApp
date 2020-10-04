import {Injectable} from '@angular/core';
import {ApplicationService, InitDto} from "../clients/teacher";
import {ApplicationService as CommonApplicationService} from "../clients/common";
import {BehaviorSubject} from "rxjs";
import {AuthService} from "./auth/auth.service";
import {SignalRService} from "./signalr/signalr.service";

@Injectable({
  providedIn: 'root'
})
export class AppService {

  public initTeacher: BehaviorSubject<InitDto> = new BehaviorSubject<InitDto>(null);
  public attachmentUrl: string;

  constructor(
    private teacherAppService: ApplicationService,
    private authService: AuthService,
    private common: CommonApplicationService,
    private signalrService: SignalRService,
  ) {
    this.reload();
    this.authService.logged.subscribe(next => this.reload());
  }

  reload() {
    if (this.authService.isLoggedIn && this.authService.isTeacher)
      this.teacherAppService.init().subscribe(x => {
        this.initTeacher.next(x);
      });
    this.common.getImageDownloadPath().subscribe(data => {
      this.attachmentUrl = data
    });
    if (this.authService.isLoggedIn) {
      this.signalrService.startConnection();
    }
  }

  getAttachmentUrl(name: string, downloadName?: string) {
    return downloadName ? `${this.attachmentUrl}/${name}?downloadName=${encodeURIComponent(downloadName)}` : `${this.attachmentUrl}/${name}`;
  }

}
