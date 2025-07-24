import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { ChatComponent } from "../chat/chat.component";

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [RouterOutlet, ChatComponent],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent {

}
