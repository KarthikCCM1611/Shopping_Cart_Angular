import { Component, inject, OnInit } from '@angular/core';
import { ShoppingMasterService } from '../../../services/shopping/shopping-master.service';
import { Observable } from 'rxjs';
import { IResponse, Product } from '../../../models/shopping-model/shopping-cart.model';
import { RouterLink } from '@angular/router';
import { MasterService } from '../../../services/master/master.service';
import { IUser } from '../../../models/master/user';

@Component({
  selector: 'app-view-cart',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './view-cart.component.html',
  styleUrl: './view-cart.component.css'
})
export class ViewCartComponent implements OnInit {
  userObj!: IUser;
  shoppingSrc = inject(ShoppingMasterService);
  masterSrc = inject(MasterService);
  cartProducts: Product[] = [];
  dataNotAvailable: boolean = false;
  constructor() {
  }

  ngOnInit(): void {
    this.masterSrc.user$.subscribe(userObj => {
      if (userObj) {
        this.userObj = userObj;
        this.getAllCartProducts();

      }
    })
  }

  getAllCartProducts() {
    this.shoppingSrc.getAllCartProducts(this.userObj.id).subscribe((res: IResponse) => {
      debugger;
      if (res.listOfCartProducts && res.listOfCartProducts.length > 0) {
        this.cartProducts = res.listOfCartProducts;
      }
      else {
        if (res.listOfCartProducts.length === 0) {
          this.cartProducts = [];
        }
        this.dataNotAvailable = true;
      }
    })
  }

  deleteCartProduct(id: string) {
    this.shoppingSrc.deleteCartProduct(id, this.userObj.id).subscribe({
      next: (res: IResponse) => {
        alert(res.statusMessage);
        this.getAllCartProducts();
      },
      error: (err: any) => {
        alert("API Error occurs")
      }
    })
  }
}
