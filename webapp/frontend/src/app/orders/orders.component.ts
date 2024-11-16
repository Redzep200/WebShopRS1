import { Component, OnInit } from '@angular/core';
import { OrdersService } from '../services/orders.service';
import { AuthService } from '../services/auth.service';
import { TranslateService } from '@ngx-translate/core';

interface OrderItem {
  id: number;
  orderId: number;
  product: {
    id: number;
    name: string;
    price: number;
    category: {
      name: string;
    };
    manufacturer: {
      name: string;
    };
  };
  quantity: number;
  price: number;
  totalPrice: number;
}

@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.css']
})
export class OrdersComponent implements OnInit {
  orders: any[] = [];
  orderItems: { [orderId: number]: OrderItem[] } = {};
  expandedOrders: { [orderId: number]: boolean } = {};
  loadingItems: { [orderId: number]: boolean } = {};
  isEmployee: boolean = false;

  stateMapping: { [key: number]: string } = {
    0: 'ORDER_STATES.PENDING',
    1: 'ORDER_STATES.PROCESSING',
    2: 'ORDER_STATES.SHIPPED',
    3: 'ORDER_STATES.DELIVERED',
    4: 'ORDER_STATES.CANCELLED',
    5: 'ORDER_STATES.REFUNDED'
  };
  
  constructor(
    private ordersService: OrdersService,
    private authService: AuthService,
    private translateService: TranslateService
  ) { }

  ngOnInit(): void {
    const userIdString: string | null = this.authService.getCurrentUserId();
    
    if (userIdString !== null) {
      const userId = parseInt(userIdString, 10);
      this.authService.isEmployee().subscribe({next:(isEmployee) => this.isEmployee = isEmployee})
    if(this.isEmployee){
      this.ordersService.getAllOrders().subscribe((data) => {
        this.orders = data;
        
      });
      console.log(this.orders)
    }
    else{
      this.ordersService.getOrdersByUserId(userId).subscribe({
        next: (data) => this.orders = data,
        error: (error) => console.error('Failed to load orders', error)
      });     
    }
  }
  console.log(this.orders);
  console.log(this.isEmployee)
}
getStateString(stateNumber: number): string {
  const translationKey = this.stateMapping[stateNumber] || 'ORDER_STATES.UNKNOWN';
  return this.translateService.instant(translationKey);
}

updateOrderState(orderId: number, event: Event): void {
  const select = event.target as HTMLSelectElement;
  const newState = parseInt(select.value, 10);

  this.ordersService.updateOrderState(orderId, newState).subscribe({
    next: () => {
      const order = this.orders.find(o => o.id === orderId);
      console.log(order)
      if (order) {
        order.state = newState;
      }
    },
    error: (error) => console.error('Failed to update order state', error)
  });
}

toggleOrderItems(orderId: number) {
  if (this.expandedOrders[orderId]) {
    this.expandedOrders[orderId] = false;
  } else {
    this.expandedOrders[orderId] = true;
    if (!this.orderItems[orderId]) {
      this.loadOrderItems(orderId);
    }
  }
}

loadOrderItems(orderId: number) {
  this.loadingItems[orderId] = true;
  this.ordersService.getOrderItemsByOrderId(orderId)
    .subscribe(
      (items: OrderItem[]) => {
        this.orderItems[orderId] = items;
        this.loadingItems[orderId] = false;
      },
      (error) => {
        console.error('Error loading order items', error);
        this.orderItems[orderId] = [];
        this.loadingItems[orderId] = false;
      }
    );
}

onRefund(orderId: number) {
  this.ordersService.processRefund(orderId).subscribe(
    (response) => {
      console.log('Refund successful:', response);
      // Update the order state in your frontend (e.g., by refreshing the order data)
      this.updateOrderStateLocally(orderId, 5); // Assuming 4 is the state for 'Refunded'
    },
    (error) => {
      console.error('Refund failed:', error);
    }
  );
}

updateOrderStateLocally(orderId: number, newState: number) {
  const order = this.orders.find(o => o.id === orderId);
  if (order) {
    order.state = newState;
  }
}
}
