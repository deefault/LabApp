import {Injectable} from "@angular/core";
import {NbGlobalLogicalPosition, NbToastrService} from "@nebular/theme";


@Injectable({
  providedIn: 'root'
})
export class NotificationService {

  constructor(
    private toastrService: NbToastrService
  ) {
  }

  public showError(message: string) {
    this.toastrService.show(message, null, {position: NbGlobalLogicalPosition.TOP_END, status: "danger"});
  }

  public showCopied(message: string = undefined) {
    this.toastrService.show(null, message ? message : "Скопировано в буфер", {position: NbGlobalLogicalPosition.TOP_END, status: "info"});
  }
}
