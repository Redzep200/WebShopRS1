import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

interface Order {
    id: string;
    state: string;
    orderPrice: number;
    orderDate: string;
}

@Injectable({
    providedIn: 'root'
  })
  export class OrdersService {
    private orderUrl = 'https://localhost:7023/api/Order';
    private orderItemUrl = 'https://localhost:7023/api/OrderItem';
    
    constructor(private http: HttpClient) { }

    getOrdersByUserId(userId: number): Observable<Order[]> {
      return this.http.get<Order[]>(`${this.orderUrl}/GetOrdersByUserId?userId=${userId}`);
    }

    updateOrderState(orderId: number, newState: number): Observable<any> {
      return this.http.put(`${this.orderUrl}/UpdateOrderState/${orderId}/state`, { newState: newState });
    }  
      
    getAllOrders(): Observable<any[]> {
      return this.http.get<any[]>(`${this.orderUrl}/GetAllOrders`);
    }

    getOrderItemsByOrderId(orderId:number):Observable<any>{
      return this.http.get<any>(`${this.orderItemUrl}/GetOrderItemsByOrderId?id=${orderId}`);
    }

    processRefund(orderId: number): Observable<any> {
      return this.http.post(`https://localhost:7023/api/Stripe/refund?id=${orderId}`, {});
    }
}