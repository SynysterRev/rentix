import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-property-details',
  imports: [],
  templateUrl: './property-details.html',
  styleUrl: './property-details.scss',
})
export class PropertyDetails implements OnInit {
  id!: string;

  constructor(private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.id = this.route.snapshot.paramMap.get('id')!; // Get the 'id' from the route
    console.log('Dynamic ID:', this.id);
  }
}
