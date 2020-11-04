import {EventEmitter, Injectable} from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class EventBusService {

  public changeNewMessageNumber: EventEmitter<{conversationId: number, num: number}> = new EventEmitter<{conversationId: number, num: number}>();
  
  constructor() { }
}
