import { Routes } from '@angular/router';
import { Dashboard } from './core/layout/dashboard/dashboard';
import { RealEstateProperty } from './core/layout/real-estate-property/real-estate-property';
import { Settings } from './core/layout/settings/settings';


export const routes: Routes = [
    { path: '', component: Dashboard },
    { path: 'dashboard', component: Dashboard },
    { path: 'real-estate-property', component: RealEstateProperty },
    { path: 'settings', component: Settings }
];
