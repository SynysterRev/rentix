import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NavigationBar } from "./core/layout/navigation-bar/navigation-bar";

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, NavigationBar],
  templateUrl: './app.html',
  styleUrl: '../styles/styles.scss',
  
})
export class App {
  protected title = 'frontend';
}
