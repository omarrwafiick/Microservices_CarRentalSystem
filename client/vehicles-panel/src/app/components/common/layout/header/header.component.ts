import { Component } from '@angular/core';
import { AvatarComponent } from "../avatar/avatar.component";
import { SearchComponent } from "../search/search.component";

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [AvatarComponent, SearchComponent],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent {

}
