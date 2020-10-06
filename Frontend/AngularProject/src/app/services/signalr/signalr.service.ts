import {Injectable, isDevMode} from '@angular/core';
import {HubConnection, HubConnectionBuilder, HubConnectionState, LogLevel} from "@microsoft/signalr";
import {AuthService} from "../auth/auth.service";

@Injectable({
  providedIn: 'root'
})
export class SignalRService {

  private hubConnection: HubConnection;

  public startConnection = () => {
    console.log("Starting signalr connection...")
    if (this.hubConnection && this.hubConnection.state == HubConnectionState.Connected) {
      this.hubConnection.stop()
    }
    this.hubConnection = new HubConnectionBuilder()
      .withUrl('http://localhost:5004/hubs/common', {
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
    this.hubConnection.on('transferchartdata', (data) => {
      console.log(data);
    });
    this.hubConnection.on('transferchartdata', (data) => {
      console.log(data);
    });

    this.hubConnection.on('NewMessage', (data) => {
      console.log(data);
    });

    this.hubConnection.on('newMessage', (data) => {
      console.log(data);
    });
  }
}
