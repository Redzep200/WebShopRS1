import {
  Component,
  OnInit,
  OnDestroy,
  ElementRef,
  ViewChild,
  ViewContainerRef,
  Output,
  EventEmitter,
} from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { LoginModalComponent } from 'src/app/login-modal/login-modal.component';
import { AuthService } from 'src/app/services/auth.service';
import { Subscription } from 'rxjs';
import { Router } from '@angular/router';
import { CartService } from 'src/app/services/cart.service';
import { CartComponent } from 'src/app/shop/cart/cart.component';
import { ChangeDetectorRef } from '@angular/core';
import { CustomerSupportService } from 'src/app/services/customer-support.service';
import { NotificationService } from 'src/app/services/notification.service';
import { SignalRService } from 'src/app/services/signalr.service';
import { trigger, transition, style, animate } from '@angular/animations';
import { TranslateService } from '@ngx-translate/core';
import { RegisterComponent } from 'src/app/components/register/register.component';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.css'],
  animations: [
    trigger('badgeAnimation', [
      transition(':increment', [
        style({ transform: 'scale(1.5)' }),
        animate('300ms ease-out', style({ transform: 'scale(1)' }))
      ])
    ])
  ],
})
export class NavBarComponent implements OnInit, OnDestroy {
  searchQuery: string = '';
  isLoggedIn: boolean = false;
  userName: string = '';
  isAdmin: boolean = false;
  isSistemAdmin: boolean = false;
  isSupport: boolean = false;
  isCustomer: boolean = false;
  isCartOpen = false;
  cartItemCount = 0;
  activeQuestionCount: number = 0;
  activeLink: string = ''; 
  
  private intervalSubscription?: Subscription;
  private signalRSubscription?: Subscription;

  private authSubscription?: Subscription;
  private cartSubscription?: Subscription;

  @ViewChild('cartIconContainer', { read: ViewContainerRef })
  cartIconContainer!: ViewContainerRef;
  @ViewChild('cartIcon') cartIcon!: ElementRef;

  @Output() searchQueryChanged: EventEmitter<string> =
    new EventEmitter<string>();

  constructor(
    private router: Router,
    public dialog: MatDialog,
    private authService: AuthService,
    public cartService: CartService,
    private cd: ChangeDetectorRef,
    private customerSupportService: CustomerSupportService,
    private notificationService: NotificationService,
    private signalRService: SignalRService,
    private translate: TranslateService
  ) {
    translate.setDefaultLang('en');
    translate.use('en');
  }

  useLanguage(language: string): void {
    this.translate.use(language);
  }

  

  ngOnInit(): void {
    this.authSubscription = this.authService
      .getCurrentUser()
      .subscribe((name) => {
        this.userName = name ?? '';
        this.isLoggedIn = !!name;
        if (name) {
          this.authService.isAdmin().subscribe((isAdmin) => {
            this.isAdmin = isAdmin; 
            this.cd.detectChanges();        
          });
          this.authService.isSistemAdmin().subscribe((isSistemAdmin) => {
            this.isSistemAdmin = isSistemAdmin; 
            this.cd.detectChanges();
          });
          this.authService.isSupport().subscribe((isSupport) => {
            this.isSupport = isSupport; 
            this.cd.detectChanges();
            if (isSupport) {
              this.initializeActiveQuestionCount(); 
            }
          });
          this.authService.isCustomer().subscribe((isCustomer) => {
            this.isCustomer = isCustomer;
            this.cd.detectChanges();
          });
          
          this.loadCartItems();
        } else {
          this.cartItemCount = 0;
          this.cartService.clearCart();
          this.isAdmin = false;
          this.isSistemAdmin = false;
          this.isSupport = false;
          this.isCustomer = false;
          this.activeQuestionCount = 0;
        }
      });

    this.cartSubscription = this.cartService.cartItems$.subscribe((items) => {
      this.cartItemCount = this.isLoggedIn ? items.length : 0;
    });

    this.notificationService.questionAnswered$.subscribe(() => {
      this.updateActiveQuestionCountOnAnswer();
    });

    this.setupSupportNotifications();
  }

  setupSupportNotifications(): void {
    console.log('Setting up support notifications');
  this.signalRService.newMessageReceived.subscribe(message => {
    console.log('SignalR message received in NavBarComponent:', message);
    if (message) {
      console.log('Message is not null, processing...');
      if (this.isSupport) {
        console.log('User is support, updating count');
        if (!message.closed) { // Only increment if the message is not closed
          this.activeQuestionCount++;
          this.cd.detectChanges();
          this.playNotificationSound();
          this.showDesktopNotification(message);
        }
      } else {
        console.log('User is not support, isSupport:', this.isSupport);
      }
    } else {
      console.log('Received null message');
    }
  });
  }

  private updateActiveQuestionCount(): void {
    this.customerSupportService.getAllCustomerQuestions().subscribe(
      (questions) => {
        this.activeQuestionCount = questions.filter(q => !q.closed).length;
        this.cd.detectChanges();
      },
      (error) => console.error('Error fetching active questions:', error)
    );
  }
  
  private playNotificationSound(): void {
    const audio = new Audio('assets/sounds/notification.mp3');
  audio.volume = 0.5; // Set volume to 50%
  audio.play().catch(error => console.error('Error playing notification sound:', error));
  }
  
  private showDesktopNotification(message: any): void {
    if (Notification.permission === "granted") {
      new Notification("New Support Message", { 
        body: message.text,
        icon: 'assets/images/notification-icon.png' // Add an icon for the notification
      });
    } else if (Notification.permission !== "denied") {
      Notification.requestPermission().then(permission => {
        if (permission === "granted") {
          this.showDesktopNotification(message);
        }
      });
    }
  }

  openAdminPanel(): void {
    console.log('Navigating to admin-panel');
    this.router.navigate(['/admin-panel']);
  }
  openSistemAdminPanel(): void {
    console.log('Navigating to sistem.admin-panel');
    this.router.navigate(['/sistem.admin-panel']);
  }

  searchProducts() {
    this.searchQueryChanged.emit(this.searchQuery);
    console.log(this.searchQuery);
  }

  openLoginDialog(): void {
    const dialogRef = this.dialog.open(LoginModalComponent);
    dialogRef.afterClosed().subscribe(() => {});
  }

  openRegisterDialog(): void {
    const dialogRef = this.dialog.open(RegisterComponent);
    dialogRef.afterClosed().subscribe(() => {});
  }


  navigateToShop(): void {
    this.router.navigate(['/shop']);
    this.activeLink = 'shop';
  }

  navigateToOrders(): void {
    this.router.navigate(['/orders']);
    this.activeLink = 'orders';
  }

  navigateToDashboard(): void {
    this.router.navigate(['/dashboard']);
    this.activeLink = 'dashboard';
  }

  navigateToCoupons(): void {
    this.router.navigate(['/coupons']);
    this.activeLink = 'coupons';
  }

  logout(): void {
    this.authService.logout();
    this.isLoggedIn = false;
    this.isAdmin = false;
    this.isSistemAdmin=false;
    this.isSupport = false;
    this.isCustomer = false;
    this.cartItemCount = 0;
    this.cartService.clearCart();
    this.router.navigate(['/shop']);
  }

  loadCartItems() {
    if (this.isLoggedIn) {
      this.cartService.loadCartItems();
    }
  }

  toggleCart() {
    if (!this.isLoggedIn) {
      return;
    }

    if (this.isCartOpen) {
      this.cartIconContainer.clear();
      this.isCartOpen = false;
    } else {
      this.loadCartItems();
      try {
        const cartComponentRef =
          this.cartIconContainer.createComponent(CartComponent);
        if (cartComponentRef && cartComponentRef.instance) {
          cartComponentRef.instance.position =
            this.cartIcon.nativeElement.getBoundingClientRect();
          this.isCartOpen = true;
        } else {
          console.error('Failed to create CartComponent');
        }
      } catch (error) {
        console.error('Error creating CartComponent:', error);
      }
    }
  }

  updateNotifications(): void {
    if (this.isSupport) {
      this.customerSupportService.getAllCustomerQuestions().subscribe(
        (questions) => {
          this.activeQuestionCount = questions.filter(q => !q.closed).length;
          this.cd.detectChanges();
        },
        (error) => console.error('Error fetching active questions:', error)
      );
    }
  }

  openMessages(): void {
    if (this.isSupport) {
      this.router.navigate(['/support-messages']);
    } else if (this.isCustomer) {
      this.router.navigate(['/customer-messages']);
    }
  }

  updateActiveQuestionCountOnAnswer(): void {
    if (this.activeQuestionCount > 0) {
      this.activeQuestionCount--;
      this.cd.detectChanges();
    }
  }

  private initializeActiveQuestionCount(): void {
    if (this.isSupport) {
      this.customerSupportService.getAllCustomerQuestions().subscribe(
        (questions) => {
          this.activeQuestionCount = questions.filter(q => !q.closed).length;
          this.cd.detectChanges();
        },
        (error) => console.error('Error fetching initial active questions:', error)
      );
    }
  }

  setupSignalRListeners(): void {
    this.signalRSubscription = this.signalRService.questionDeleted.subscribe(
      (questionId) => {
        if (questionId !== null && this.isSupport) {
          this.decrementActiveQuestionCount();
        }
      }
    );
  }

  private decrementActiveQuestionCount(): void {
    if (this.activeQuestionCount > 0) {
      this.activeQuestionCount--;
      this.cd.detectChanges();
    }
  }


  ngOnDestroy(): void {
    if (this.authSubscription) {
      this.authSubscription.unsubscribe();
    }
    if (this.cartSubscription) {
      this.cartSubscription.unsubscribe();
    }
    if (this.intervalSubscription) {
      this.intervalSubscription.unsubscribe();
    }
    if (this.signalRSubscription) {
      this.signalRSubscription.unsubscribe();
    }
    if (this.signalRSubscription) {
      this.signalRSubscription.unsubscribe();
    }
  }

  navigateToVisitUs(): void {
    this.router.navigate(['/visit-us']);
    this.activeLink = 'visit-us';
  }
}