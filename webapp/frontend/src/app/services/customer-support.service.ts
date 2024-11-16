import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { SignalRService } from './signalr.service';

@Injectable({
  providedIn: 'root',
})
export class CustomerSupportService {
  private apiUrl = 'https://localhost:7023/api/CustomerQuestion';
  private hasNewAnswers = new BehaviorSubject<boolean>(false);

  constructor(private http: HttpClient, private signalRService: SignalRService) {
    this.signalRService.newMessageReceived.subscribe((message) => {
      if (message) {
        console.log('New message notification in CustomerSupportService:', message);
        this.hasNewAnswers.next(true);
      }
    });
  }

  getAllCustomerQuestions(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/GetAllCustomerQuestions`);
  }

  getCustomerQuestionById(id: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/GetCustomerQuestionById?id=${id}`);
  }

  getCustomerQuestionByUserId(userId: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/GetCustomerQuestionByUserId?id=${userId}`);
  }

  createCustomerQuestion(text: string, userId: number): Observable<any> {
    const body = { text, userId };
    return this.http.post<any>(`${this.apiUrl}/CreateCustomerQuestion`, body);
  }

  deleteCustomerQuestion(id: number): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/DeleteCustomerQuestion?id=${id}`);
  }

  createAnswer(id: number, answer: string): Observable<any> {
    const body = { questionId: id, answer };
    return this.http.put<any>(`${this.apiUrl}/CreateAnswer`, body);
  }

  getUnansweredQuestionCount(): Observable<number> {
    return this.http.get<number>(`${this.apiUrl}/GetUnansweredQuestionCount`);
  }
}