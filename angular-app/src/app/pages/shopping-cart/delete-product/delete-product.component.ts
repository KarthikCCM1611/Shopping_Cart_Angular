import { Component, inject, OnInit } from '@angular/core';
import { IResponse, Product } from '../../../models/shopping-model/shopping-cart.model';
import { ShoppingMasterService } from '../../../services/shopping/shopping-master.service';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-delete-product',
  standalone: true,
  imports: [FormsModule, RouterLink],
  templateUrl: './delete-product.component.html',
  styleUrl: './delete-product.component.css'
})
export class DeleteProductComponent implements OnInit {
  product: Product = new Product();
  deleteProductId: string = "";
  isProductExists: boolean = true;
  shoppingSrc = inject(ShoppingMasterService);
  activateRoute = inject(ActivatedRoute);
  router = inject(Router);
  ngOnInit(): void {
    this.activateRoute.params.subscribe((res: any) => {
      this.deleteProductId = res.id;
      this.getProductById();
    })
  }

  getProductById() {
    this.shoppingSrc.getProductById(this.deleteProductId).subscribe({
      next: (res: IResponse) => {
        if (res.statusCode === 200) {
          this.product = res.product;
          // debugger;
        }
        else {
          this.isProductExists = false;
        }
      },
      error: (err) => {
        alert("API Error");
      }
    })
  }

  deleteProduct() {
    this.shoppingSrc.deleteProduct(this.deleteProductId).subscribe({
      next: (res: IResponse) => {
        if (res.statusCode === 200) {
          alert(res.statusMessage);
          // this.router.navigateByUrl("/shopping-cart");
          this.router.navigate(['/shopping-cart']);
        }
        else {
          this.isProductExists = false;
        }
      },
      error: (err) => {
        alert("API Error");
      }
    })
  }
}
