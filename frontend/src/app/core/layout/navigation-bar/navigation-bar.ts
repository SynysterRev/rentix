import { Component, ElementRef, QueryList, Renderer2, ViewChildren } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { LucideAngularModule, House, ChartLine, Building2, Settings, HandCoins, PanelRight } from 'lucide-angular'
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-navigation-bar',
  imports: [
    RouterLink,
    RouterLinkActive,
    LucideAngularModule,
    CommonModule
],
  templateUrl: './navigation-bar.html',
  styleUrl: './navigation-bar.scss',
})
export class NavigationBar {
  readonly House = House;
  readonly ChartLine = ChartLine;
  readonly Building2 = Building2;
  readonly Settings = Settings;
  readonly HandCoins = HandCoins;
  readonly PanelRight = PanelRight;

  isVisible = true;
}
