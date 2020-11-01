import {EventEmitter, Injectable} from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class EventBusService {

  public changeNewMessageNumber: EventEmitter<number> = new EventEmitter<number>();
  
  constructor() { }
}
