import {Component, OnInit} from "@angular/core";
import {SignalRService} from "../../services/signalr/signalr.service";
import {EventBusService} from "../../services/event-bus.service";


export class NavBarBase implements OnInit {

  public messages: number;

  constructor(
    private signalRService: SignalRService,
    private eventBus: EventBusService
  ) {
  }

  ngOnInit(): void {
    this.eventBus.changeNewMessageNumber.subscribe(n => {
      if (n < 0 && Math.abs(n) > this.messages) {
        this.messages = 0;
      } else {
        this.messages += n;
      }
    })
  }
}