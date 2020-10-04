import {isDevMode} from '@angular/core';
import {ModelState} from "../models/modelState";
import {NgModel} from "@angular/forms";

export class BaseComponent {

  protected loading: boolean = true;

  protected getErrorText(error: ModelState, customText?: string) {
    return isDevMode()
      ? JSON.stringify(error)
      : customText == undefined ? "Ошибка от сервера" : customText;
  }

  protected isInvalid(ngModel: NgModel) {
    return ngModel.invalid && (ngModel.dirty || ngModel.touched)
  }
}
