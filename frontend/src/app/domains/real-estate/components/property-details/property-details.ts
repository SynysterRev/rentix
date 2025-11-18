import { Component, DestroyRef, inject, signal } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { PropertyDetailsDTO } from '../../models/property.model';
import { PropertyService } from '../../services/property';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { DatePipe } from '@angular/common';
import { LucideAngularModule, CircleAlert, MapPin, Euro, FileText, UsersRound, Mail, Phone, Calendar, CalendarOff, Trash2, UserRoundPlus, Pencil, Glasses, ArrowLeft } from "lucide-angular";
import { PropertyStatus } from '../../../../shared/models/property-status.model';
import { MatDialog } from '@angular/material/dialog';
import { RentPropertyDialog } from '../rent-property-dialog/rent-property-dialog';

@Component({
  selector: 'app-property-details',
  imports: [LucideAngularModule, DatePipe, RouterLink],
  templateUrl: './property-details.html',
  styleUrl: './property-details.scss',
})
export class PropertyDetails {

  propertyId: number = 0;
  isEditable: boolean = false;

  private propertyService = inject(PropertyService);
  private destroyRef = inject(DestroyRef);
  property = signal<PropertyDetailsDTO | null>(null);

  readonly MapPin = MapPin;
  readonly CircleAlert = CircleAlert;
  readonly Euro = Euro;
  readonly FileText = FileText;
  readonly UsersRound = UsersRound;
  readonly Mail = Mail;
  readonly Phone = Phone;
  readonly Calendar = Calendar;
  readonly CalendarOff = CalendarOff;
  readonly Trash2 = Trash2;
  readonly UserRoundPlus = UserRoundPlus;
  readonly Pencil = Pencil;
  readonly Glasses = Glasses;
  readonly ArrowLeft = ArrowLeft;

  constructor(private route: ActivatedRoute, private dialog: MatDialog) {
    this.route.paramMap.subscribe(params => {
      const newId = Number(params.get('id'));
      if (newId !== this.propertyId) {
        this.loadPropertyDetails(newId);
        this.propertyId = newId;
      }
    });
  }

  loadPropertyDetails(id: number) {
    this.propertyService.getPropertyDetails(id)
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe(response => {
        this.property.set(response);
        ///////////////////////////////////////////!--TODO UPDATE DE LISTE
      });
  }

  deleteProperty() {
    //Message popup de confirmation
    this.propertyService.deleteProperty(this.propertyId)
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe(response => {
        this.property.set(response);
        //Call le routerLink pour renvoyer sur la page "Mes biens"
      });
  }

  getPropertyStatus(): string {
    const labels = {
      [PropertyStatus.Available]: 'Disponible',
      [PropertyStatus.Rented]: 'Loué',
      [PropertyStatus.UnderMaintenance]: 'En maintenance',
      [PropertyStatus.Unavailable]: 'Indisponible'
    };

    return labels[this.property()?.propertyStatus!];
  }

  endLease() {
    console.log("Fin de la location - Actions à déterminer")
  }

  addReminder() {
    console.log("Ajouter un rappel - Actions à déterminer")
  }

  addTransaction() {
    console.log("Ajout d'une transaction - Actions à déterminer")
  }

  generateRentReceipt() {
    console.log("Generation d'une quittance - Actions à déterminer")
  }

  rentProperty() {
    const dialogRef = this.dialog.open(RentPropertyDialog, {
      data: { propertyId: this.propertyId },
      //Pour la taille il y a un calcul auto dans ce css .cdk-overlay-pane.mat-mdc-dialog-panel
      disableClose: true, // Prevent closing by clicking outside
    });

    dialogRef.beforeClosed().subscribe(() => 
      this.loadPropertyDetails(this.propertyId)
    )
  }

  updateRent() {
    console.log("updateRent - Actions à déterminer")
  }

  switchReadEditMode(){
    this.isEditable = !this.isEditable;
  }
}

