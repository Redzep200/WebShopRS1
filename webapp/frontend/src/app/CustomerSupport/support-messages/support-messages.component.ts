import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { CustomerSupportService } from 'src/app/services/customer-support.service';
import { UserService } from 'src/app/services/user.service';
import { forkJoin } from 'rxjs';
import { SignalRService } from 'src/app/services/signalr.service';
import { NotificationService } from 'src/app/services/notification.service';

@Component({
  selector: 'app-support-messages',
  templateUrl: './support-messages.component.html',
  styleUrls: ['./support-messages.component.css']
})
export class SupportMessagesComponent implements OnInit {
  allMessages: any[] = [];
  openMessages: any[] = [];
  closedMessages: any[] = [];
  answerText: string = '';
  searchTerm: string = '';
  users: any[] = [];
  answerTexts: { [key: number]: string } = {};
  startDate: string = '';
  endDate: string = '';
  @Output() questionAnswered: EventEmitter<void> = new EventEmitter<void>();
  

  constructor(private customerSupportService: CustomerSupportService,
     private userService: UserService,
    private signalRService: SignalRService,
    private notificationService: NotificationService) {}

  ngOnInit() {
    this.setupSignalR();
    this.loadAllMessages();
  }

  setupSignalR() {
    this.signalRService.newMessage.subscribe(message => {
      if (message) {
        this.addNewMessage(message);
        this.showDesktopNotification(message);
      }
    });
  }

  addNewMessage(message: any) {
    const existingMessage = this.allMessages.find(m => m.id === message.id);
    if (existingMessage) {
      Object.assign(existingMessage, message);
    } else {
      this.allMessages.unshift(message);
    }
    this.filterMessages();
  }
  
  showDesktopNotification(message: any) {
    if (Notification.permission === "granted") {
      new Notification("New Support Message", { body: message.text });
    }
  }
  

  loadAllMessages() {
    forkJoin({
      messages: this.customerSupportService.getAllCustomerQuestions(),
      users: this.userService.getAllUsers()
    }).subscribe(
      ({ messages, users }) => {
        this.users = users;
        this.allMessages = messages.map(message => ({
          ...message,
          user: this.users.find(user => user.id === message.userId)
        }));
        this.filterMessages();
      },
      (error) => console.error('Error loading data:', error)
    );
  }


  filterMessages() {
    let filteredMessages = this.allMessages;
  
    if (this.searchTerm) {
      filteredMessages = filteredMessages.filter(m => 
        (m.user && m.user.username.toLowerCase().startsWith(this.searchTerm.toLowerCase())) ||
        m.text.toLowerCase().includes(this.searchTerm.toLowerCase())
      );
    }
  
    if (this.startDate && this.endDate) {
      const start = new Date(this.startDate);
      const end = new Date(this.endDate);
      end.setHours(23, 59, 59); 
  
      filteredMessages = filteredMessages.filter(m => {
        const messageDate = new Date(m.questionDate);
        return messageDate >= start && messageDate <= end;
      });
    }
  
    filteredMessages.sort((a, b) => new Date(b.questionDate).getTime() - new Date(a.questionDate).getTime());
  
    this.openMessages = filteredMessages.filter(m => !m.closed);
    this.closedMessages = filteredMessages.filter(m => m.closed);
  }

  onSearch() {
    this.filterMessages();
  }

  onDateSearch() {
    this.filterMessages();
  }

  answerQuestion(questionId: number) {
    const answerText = this.answerTexts[questionId];
    if (answerText && answerText.trim()) {
      this.customerSupportService.createAnswer(questionId, answerText).subscribe(
        () => {
          delete this.answerTexts[questionId];
          
          const question = this.allMessages.find(m => m.id === questionId);
          if (question) {
            question.closed = true;
            this.notificationService.notifyQuestionAnswered(); 
            this.filterMessages();
          }
        },
        (error) => console.error('Greška pri slanju odgovora:', error)
      );
    }
  }

  deleteQuestion(questionId: number) {
    this.customerSupportService.deleteCustomerQuestion(questionId).subscribe(
      () => {
        this.allMessages = this.allMessages.filter(m => m.id !== questionId);
        this.filterMessages();
        this.notificationService.notifyQuestionAnswered(); 
      },
      (error) => console.error('Greška pri brisanju pitanja:', error)
    );
  }
}
