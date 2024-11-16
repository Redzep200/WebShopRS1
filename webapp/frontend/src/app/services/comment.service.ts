import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

interface Comment {
    id: number;
    text: string;
    rating: number;
    userId: number;
    productId: number;
    createdAt: string;
  }
@Injectable({
  providedIn: 'root'
})
export class CommentService {
  private apiUrl = 'https://localhost:7023/api/Comment';

  constructor(private http: HttpClient) { }

  createComment(text: string, rating: number, userId: number, productId: number, updateDate?: Date): Observable<any> {
    const body = {
      text: text,
      rating: rating,
      userId: userId,
      productId: productId,
      updateDate: updateDate || new Date()
    };

    return this.http.post(`${this.apiUrl}/CreateNewComment`, body);
  }

  deleteComment(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/DeleteComment?id=${id}`);
  }
  
  getAllComments(): Observable<Comment[]> {
    return this.http.get<Comment[]>(`${this.apiUrl}/GetAllComments`);
  }
}