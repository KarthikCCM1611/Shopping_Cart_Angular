import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { CartProduct, IResponse, Product } from '../../../models/shopping-model/shopping-cart.model';
import { ShoppingMasterService } from '../../../services/shopping/shopping-master.service';
import { MasterService } from '../../../services/master/master.service';
import { IUser } from '../../../models/master/user';

@Component({
  selector: 'app-landing-page',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './landing-page.component.html',
  styleUrl: './landing-page.component.css'
})
export class LandingPageComponent implements OnInit {
  products: Product[] = [];
  shoppingSrc = inject(ShoppingMasterService);
  masterSrc = inject(MasterService);
  router = inject(Router);
  dataNotAvailable: boolean = false;
  userObj!: IUser;
  ngOnInit(): void {
    this.masterSrc.user$.subscribe(userObj => {
      if (userObj) {
        this.userObj = userObj;
        this.getAllProducts();
      }
    })
  }
  getAllProducts() {
    this.shoppingSrc.getAllProducts().subscribe((res: IResponse) => {
      // debugger;
      if (res.listOfProducts) {
        this.products = res.listOfProducts;
      }
      else {
        this.dataNotAvailable = true;
      }
    })
  }

  addToCart(product: Product) {
    var cartProduct: CartProduct = new CartProduct();
    cartProduct.id = product.id;
    cartProduct.image = product.image;
    cartProduct.name = product.name;
    cartProduct.price = product.price;
    cartProduct.userId = this.userObj.id;
    this.shoppingSrc.addToCart(cartProduct).subscribe((res: IResponse) => {
      alert(res.statusMessage);
    })
  }
}
