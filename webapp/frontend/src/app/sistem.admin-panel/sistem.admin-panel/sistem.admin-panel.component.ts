import { Component, OnInit } from '@angular/core';
import { UserService } from 'src/app/services/user.service';
import { RoleService } from 'src/app/services/role.service';

@Component({
  selector: 'app-sistem-admin-panel',
  templateUrl: './sistem.admin-panel.component.html',
  styleUrls: ['./sistem.admin-panel.component.css']
})
export class SistemAdminPanelComponent implements OnInit {
  users: any[] = [];
  groupedUsers: { [key: string]: any[] } = {};
  roles: any[] = [];
  searchTerm: string = '';
  editedUser: any | null = null;
  showEditDialog: boolean = false;

  constructor(private userService: UserService, private roleService: RoleService) {}

  ngOnInit() {
    this.loadUsers();
    this.loadRoles();
  }

  loadUsers() {
    this.userService.getAllUsers().subscribe((users) => {
      this.users = users;
      this.groupUsersByRole();
    });
  }

  loadRoles() {
    this.roleService.getAllRoles().subscribe((roles) => {
      this.roles = roles;
    });
  }

  groupUsersByRole() {
    this.groupedUsers = {};
    this.users.forEach(user => {
      if (!this.groupedUsers[user.role.name]) {
        this.groupedUsers[user.role.name] = [];
      }
      this.groupedUsers[user.role.name].push(user);
    });
  }

  searchUsers() {
    const searchLower = this.searchTerm.toLowerCase();
    this.groupedUsers = {};
    this.users.filter(user => 
      user.name.toLowerCase().includes(searchLower) || 
      user.surname.toLowerCase().includes(searchLower)
    ).forEach(user => {
      if (!this.groupedUsers[user.role.name]) {
        this.groupedUsers[user.role.name] = [];
      }
      this.groupedUsers[user.role.name].push(user);
    });
  }

  editUser(user: any) {
    this.editedUser = { ...user };
    this.showEditDialog = true;
  }

  onUserUpdated(updatedUser: any) {
    this.loadUsers(); 
    this.closeEditDialog();
  }

  closeEditDialog() {
    this.showEditDialog = false;
    this.editedUser = null;
  }

  onUserDeleted(userId: number) {
    this.userService.deleteUser(userId).subscribe(
      (response) => {
        console.log('User deleted:', response);
        this.loadUsers(); 
      },
      (error) => {
        console.error('Error deleting user:', error);
      }
    );
    this.closeEditDialog();
  }
}