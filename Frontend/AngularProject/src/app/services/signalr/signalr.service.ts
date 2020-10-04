import {Injectable, isDevMode} from '@angular/core';
import {HubConnection, HubConnectionBuilder, HubConnectionState} from "@microsoft/signalr";
import {AuthService} from "../auth/auth.service";

@Injectable({
  providedIn: 'root'
})
export class SignalRService {

  private hubConnection: HubConnection;
  
  public startConnection = () => {
    if (this.hubConnection && this.hubConnection.state == HubConnectionState.Connected) {
      this.hubConnection.stop()
    }
    this.hubConnection = new HubConnectionBuilder()
      .withUrl('http://localhost:5003/hubs/common', {
        accessTokenFactory: () => localStorage.getItem(AuthService.TokenName),
        logMessageContent: isDevMode(),
      })
      .withAutomaticReconnect()
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
