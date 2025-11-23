import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { APP_ROUTES } from '../constant/routes-constant';

@Component({
  selector: 'app-layout',
  imports: [RouterModule],
  templateUrl: './layout.html',
  styleUrl: './layout.scss',
})
export class Layout {
  sidebarOpen = false;
  constant = APP_ROUTES;
  toggleSidebar() {
    this.sidebarOpen = !this.sidebarOpen;
  }

  closeSidebar() {
    this.sidebarOpen = false;
  }
}
