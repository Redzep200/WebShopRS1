import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavBarComponent } from './nav-bar/nav-bar.component';
import { FormsModule } from '@angular/forms';
import { LoginModalComponent } from '../login-modal/login-modal.component';
import { TranslateModule } from '@ngx-translate/core';

@NgModule({
  declarations: [NavBarComponent],
  imports: [CommonModule, FormsModule, TranslateModule],
  exports: [NavBarComponent],
})
export class CoreModule {}
