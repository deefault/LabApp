import {EventEmitter, Injectable, isDevMode} from '@angular/core';
import {HubConnection, HubConnectionBuilder, HubConnectionState, LogLevel} from "@microsoft/signalr";
import {AuthService} from "../auth/auth.service";
import {environment} from "../../../environments/environment";
import {NewMessage} from "../../models/events/new-message";
import {EventBusService} from "../event-bus.service";

@Injectable({
  providedIn: 'root'
})
export class SignalRService {

  private hubConnection: HubConnection;
  public newMessage: EventEmitter<NewMessage> = new EventEmitter<NewMessage>();


  constructor(private eventBus: EventBusService) {
  }

  public startConnection = () => {
    console.log("Starting signalr connection...")
    if (this.hubConnection && this.hubConnection.state == HubConnectionState.Connected) {
      this.hubConnection.stop()
    }

    this.hubConnection = new HubConnectionBuilder()
      .withUrl(`${environment.signalRUrl}/hubs/common`, {
        accessTokenFactory: () => localStorage.getItem(AuthService.TokenName),
        logMessageContent: isDevMode(),
      })
      .withAutomaticReconnect()
      .configureLogging(isDevMode() ? LogLevel.Trace : LogLevel.Information)
      .build();
    this.hubConnection
      .start()
      .then(() => {
        console.log('Connection started');
        this.subscribeAll();
      })
      .catch(err => {
        console.log('Error while starting connection: ' + err)
        this.hubConnection.stop();
      })
  }

  public subscribeAll = () => {
    console.log("Subscribing to signalR events")

    this.hubConnection.on('newMessage', (data: NewMessage) => {
      console.log(data);
      this.newMessage.emit(data);
      this.eventBus.changeNewMessageNumber.emit({conversationId: data.ConversationId, num: 1});
    });
  }
}
