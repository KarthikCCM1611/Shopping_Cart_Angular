import { Routes } from '@angular/router';
import { TodoListComponent } from './pages/todo-list/todo-list.component';
import { LandingPageComponent } from './pages/shopping-cart/landing-page/landing-page.component';
import { AddNewProductComponent } from './pages/shopping-cart/add-new-product/add-new-product.component';
import { EditProductComponent } from './pages/shopping-cart/edit-product/edit-product.component';
import { DeleteProductComponent } from './pages/shopping-cart/delete-product/delete-product.component';
import { ViewCartComponent } from './pages/shopping-cart/view-cart/view-cart.component';
import { NotFoundComponent } from './pages/not-found/not-found.component';
import { LoginComponent } from './pages/login/login.component';
import { RegisterComponent } from './pages/register/register.component';
import { HomeComponent } from './pages/home/home.component';

export const routes: Routes = [
    {
        path: "",
        redirectTo: "",
        pathMatch: "full"
    },
    {
        path: "home",
        component: HomeComponent
    },
    {
        path: "login",
        component: LoginComponent
    },
    {
        path: "register",
        component: RegisterComponent
    },
    {
        path: "todo",
        component: TodoListComponent
    },
    {
        path: "shopping-cart",
        children: [
            { path: "", component: LandingPageComponent },
            { path: "add-new-product", component: AddNewProductComponent },
            { path: "edit-product/:id", component: EditProductComponent },
            { path: "delete-product/:id", component: DeleteProductComponent },
            { path: "view-cart", component: ViewCartComponent },
            { path: "**", component: NotFoundComponent },
        ]
    },
    { path: "**", component: NotFoundComponent },
];
