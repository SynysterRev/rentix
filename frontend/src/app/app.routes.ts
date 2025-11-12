import { Routes } from '@angular/router';
import { Dashboard } from './core/layout/dashboard/dashboard';
import { RealEstateProperty } from './domains/real-estate/components/real-estate-property/real-estate-property';
import { Settings } from './domains/settings/components/settings/settings';
import { PropertyDetails } from './domains/real-estate/components/property-details/property-details';



export const routes: Routes = [
    { path: '', component: Dashboard },
    { path: 'dashboard', component: Dashboard },
    { path: 'real-estate-property', component: RealEstateProperty },
    { path: 'settings', component: Settings },
    { path: 'real-estate-property/:id', component: PropertyDetails}
];
