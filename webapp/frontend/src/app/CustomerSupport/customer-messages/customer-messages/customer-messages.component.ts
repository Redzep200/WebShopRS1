import { Component, OnInit } from '@angular/core';
import { CustomerSupportService } from 'src/app/services/customer-support.service';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-customer-messages',
  templateUrl: './customer-messages.component.html',
  styleUrls: ['./customer-messages.component.css']
})
export class CustomerMessagesComponent implements OnInit {
  openQuestions: any[] = [];
  closedQuestions: any[] = [];
  newQuestionText: string = '';
  currentUserId: number | null = null;  
  startDate: string = '';
  endDate: string = '';
  allQuestions: any[] = [];


  constructor(
    private customerSupportService: CustomerSupportService,
    private authService: AuthService
  ) { }

  ngOnInit(): void {
    this.getCurrentUserId();
    this.loadCustomerQuestions();
  }


  getCurrentUserId(): void {
    const userId = this.authService.getCurrentUserId();
    if (userId) {
      this.currentUserId = parseInt(userId, 10);
    } else {
      console.error('No user ID found');
    }
  }

  
  loadCustomerQuestions() {
    const userId = this.authService.getCurrentUserId();
    if (userId) {
      this.customerSupportService.getCustomerQuestionByUserId(parseInt(userId)).subscribe(
        questions => {
          this.allQuestions = questions;
          this.filterAndSortQuestions();
        }
      );
    }
  }

  filterAndSortQuestions() {
    let filteredQuestions = this.allQuestions;

    if (this.startDate && this.endDate) {
      const start = new Date(this.startDate);
      const end = new Date(this.endDate);
      end.setHours(23, 59, 59); 

      filteredQuestions = filteredQuestions.filter(q => {
        const questionDate = new Date(q.questionDate);
        return questionDate >= start && questionDate <= end;
      });
    }


    filteredQuestions.sort((a, b) => new Date(b.questionDate).getTime() - new Date(a.questionDate).getTime());

    this.openQuestions = filteredQuestions.filter(q => !q.closed);
    this.closedQuestions = filteredQuestions.filter(q => q.closed);
  }

  onDateSearch() {
    this.filterAndSortQuestions();
  }

  createNewQuestion(): void {
    if (this.newQuestionText.trim() && this.currentUserId) {
      this.customerSupportService.createCustomerQuestion(this.newQuestionText, this.currentUserId).subscribe(
        (response) => {
          console.log('Pitanje uspješno poslano:', response);
          this.newQuestionText = '';
          this.loadCustomerQuestions(); 
        },
        (error) => {
          console.error('Greška u kreaciji pitanja:', error);
        }
      );
    }
  }

  deleteQuestion(questionId: number): void {
    if (confirm('Jeste li sigurni da želite obrisati pitanje?')) {
      this.customerSupportService.deleteCustomerQuestion(questionId).subscribe(
        (response) => {
          console.log('Pitanje uspješno obrisano:', response);
          this.loadCustomerQuestions(); 
        },
        (error) => {
          console.error('Greška prilikom brisanja pitanja:', error);
        }
      );
    }
  }

  

}
