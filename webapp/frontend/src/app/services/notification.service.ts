import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class NotificationService {
  private questionAnsweredSource = new Subject<void>();

  questionAnswered$ = this.questionAnsweredSource.asObservable();

  notifyQuestionAnswered() {
    this.questionAnsweredSource.next();
  }
}