import { Component, Input, Output, EventEmitter } from '@angular/core';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-edit-user-dialog',
  templateUrl: './edit.user-dialog.component.html',
  styleUrls: ['./edit.user-dialog.component.css']
})
export class EditUserDialogComponent {
  @Input() user: any;
  @Input() roles: any[] = [];
  @Output() userUpdated = new EventEmitter<any>();
  @Output() userDeleted = new EventEmitter<number>();
  @Output() dialogClosed = new EventEmitter<void>();

  constructor(private userService: UserService) {}

  saveUser() {
    this.userService.updateUser(this.user.id, this.user).subscribe(
      (response) => {
        console.log('User updated successfully:', response);
        this.userUpdated.emit(this.user);
      },
      (error) => {
        console.error('Error updating user:', error);
      }
    );
  }

  deleteUser() {
    if (confirm('Are you sure you want to delete this user?')) {
      this.userDeleted.emit(this.user.id);
    }
  }

  closeDialog() {
    this.dialogClosed.emit();
  }
}