import { Routes } from '@angular/router';
import { NotFoundComponent } from './components/common/layout/not-found/not-found.component';
import { DashboardComponent } from './components/features/dashboard/dashboard.component';
import { SignupComponent } from './components/features/authentication/signup/signup.component';
import { LoginComponent } from './components/features/authentication/login/login.component';
import { ResetPasswordComponent } from './components/features/authentication/reset-password/reset-password.component';
import { ForgetPasswordComponent } from './components/features/authentication/forget-password/forget-password.component';
import { AnalyticsComponent } from './components/features/dashboard/childs/analytics/analytics.component';
import { BookingComponent } from './components/features/dashboard/childs/booking/booking.component';
import { CurrentPickupComponent } from './components/features/dashboard/childs/current-pickup/current-pickup.component';
import { MaintenanceComponent } from './components/features/dashboard/childs/maintenance/maintenance.component';
import { TransactionComponent } from './components/features/dashboard/childs/transaction/transaction.component';
import { VehicleComponent } from './components/features/dashboard/childs/vehicle/vehicle.component';

export const routes: Routes = [
    {
        path:"", 
        redirectTo:"dashboard",
        pathMatch: "full"
    },
    {
        
        path:"dashboard", 
        component : DashboardComponent,
        children:
        [
            {
                path:"analytics", 
                component : AnalyticsComponent
            },
            {
                path:"booking", 
                component : BookingComponent
            },
            {
                path:"current-pickup", 
                component : CurrentPickupComponent
            },
            {
                path:"recommendation", 
                component : MaintenanceComponent
            },
            {
                path:"maintenance", 
                component : MaintenanceComponent
            },
            {
                path:"transaction", 
                component : TransactionComponent
            },
            {
                path:"vehicle", 
                component : VehicleComponent
            }
        ] 
    },
    {
        path:"signup", 
        component : SignupComponent
    },
    {
        path:"login", 
        component : LoginComponent
    },
    {
        path:"reset-password", 
        component : ResetPasswordComponent
    },
    {
        path:"forget-password", 
        component : ForgetPasswordComponent
    }, 
    {
        path:"**", 
        component : NotFoundComponent
    }
];
