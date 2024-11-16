import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ProductService } from 'src/app/services/product.service';
import { Product } from 'src/app/shared/models/product.model';
import { CartService } from 'src/app/services/cart.service';
import { AuthService } from 'src/app/services/auth.service';
import { CommentService } from 'src/app/services/comment.service';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.css'],
})
export class ProductDetailsComponent implements OnInit {
  product!: Product;
  cartItems: any[] = [];
  quantity: number = 1;
  
  userId: string | null = null;
  commentText: string = '';
  rating: number = 5;
  comments: any[] = []; 
  productId!: number;
  isAdmin!:boolean;
  currentUserRole: string = 'Unknown';

  constructor(
    private route: ActivatedRoute,
    private productService: ProductService,
    private cartService: CartService,
    private commentService: CommentService,
    private authService: AuthService
  ) {}

  ngOnInit() {
    this.route.params.subscribe((params) => {
      this.productId = +params['id'];
      const productId = params['id'];
      this.productService.getProductById(productId).subscribe(
        (product) => {
          this.product = product;
          this.loadComments();
        },
        (error) => console.error('Error loading product', error)
      );
    });

    this.loadCartItems();
    this.userId = this.authService.getCurrentUserId();
    this.authService.getCurrentRole().subscribe(role => {
      this.currentUserRole = role;
    }); 
  }

  loadCartItems() {
    this.cartService.cartItems$.subscribe((items) => {
      this.cartItems = items;
    });
  }

  addToCart(productId: number, quantity: number) {
    this.cartService.addToCart(productId, this.quantity).subscribe((response) => {
      this.loadCartItems();
    });
  }

  submitComment() {
    if (this.commentText.trim() && this.product && this.userId) {
      const userIdNumber = parseInt(this.userId, 10);
      if (isNaN(userIdNumber)) {
        console.error('Invalid user ID');
        return;
      }
      
      this.commentService.createComment(this.commentText, this.rating, userIdNumber, this.product.productId)
        .subscribe(
          response => {
            console.log('Comment submitted successfully', response);
            this.commentText = '';
            this.rating = 5;
            this.loadComments();           
          },
          error => {
            console.error('Error submitting comment', error);           
          }
        );
    } 
  }

  loadComments() {
    this.commentService.getAllComments().subscribe(
      (allComments) => {
        this.comments = allComments.filter(comment => comment.productId === this.productId);
      },
      (error) => console.error('Error loading comments', error)
    );
  }

  canDeleteComment(comment: any): boolean {
    if (!comment || !comment.userId) {
      return false;
    }
    
    const commentUserId = comment.userId.toString();
    const currentUserId = this.userId?.toString();
  
    return currentUserId === commentUserId || 
           ['Web shop admin', 'Radnik'].includes(this.currentUserRole);
  }

  deleteComment(commentId: number) {
    this.commentService.deleteComment(commentId).subscribe(
      () => {
        this.comments = this.comments.filter(c => c.id !== commentId);
      }
    );
  }
}