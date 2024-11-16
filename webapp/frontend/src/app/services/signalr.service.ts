import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SignalRService {
  public messageReceived = new BehaviorSubject<any>(null);
  public newMessageReceived = new BehaviorSubject<any>(null);
  public customerTyping = new BehaviorSubject<any>(null);
  public newMessage = new BehaviorSubject<any>(null);
  public questionDeleted = new BehaviorSubject<number | null>(null);
  public userRoleChanged = new BehaviorSubject<{userId: number, newRoleId: number} | null>(null);

  private customerSupportHub: signalR.HubConnection;
  private adminHub: signalR.HubConnection;

  constructor() {
    this.customerSupportHub = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:7023/customersupporthub')
      .withAutomaticReconnect()
      .build();

    this.adminHub = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:7023/adminhub')
      .withAutomaticReconnect()
      .build();

    this.startConnections();
    this.addListeners();
  }

  public startConnections() {
    this.startConnection(this.customerSupportHub, 'CustomerSupport');
    this.startConnection(this.adminHub, 'Admin');
  }

  private startConnection(hub: signalR.HubConnection, hubName: string) {
    if (hub.state === signalR.HubConnectionState.Disconnected) {
      hub.start()
        .then(() => console.log(`${hubName} SignalR connection started successfully`))
        .catch(err => console.error(`Error while starting ${hubName} SignalR connection:`, err));
    }

    hub.onreconnected(() => {
      console.log(`${hubName} SignalR connection reconnected`);
    });

    hub.onreconnecting((error) => {
      console.log(`${hubName} SignalR connection reconnecting:`, error);
    });

    hub.onclose((error) => {
      console.log(`${hubName} SignalR connection closed:`, error);
      this.retryConnection(hub, hubName);
    });
  }

  private retryConnection(hub: signalR.HubConnection, hubName: string) {
    setTimeout(() => {
      console.log(`Retrying ${hubName} SignalR connection...`);
      this.startConnection(hub, hubName);
    }, 5000);
  }

  private addListeners() {
    this.customerSupportHub.on('ReceiveNewMessage', (message: any) => {
      console.log('ReceiveNewMessage event received:', message);
      this.newMessageReceived.next(message);
      this.newMessage.next(message);
    });

    this.adminHub.on('UserRoleChanged', (userId: number, newRoleId: number) => {
      console.log('UserRoleChanged event received:', userId, newRoleId);
      this.userRoleChanged.next({userId, newRoleId});
    });
  }

  public isConnected(hub: signalR.HubConnection): boolean {
    return hub.state === signalR.HubConnectionState.Connected;
  }

  public isCustomerSupportConnected(): boolean {
    return this.isConnected(this.customerSupportHub);
  }

  public isAdminConnected(): boolean {
    return this.isConnected(this.adminHub);
  }
}