import { Component, inject, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { IResponse, Product } from '../../../models/shopping-model/shopping-cart.model';
import { ShoppingMasterService } from '../../../services/shopping/shopping-master.service';
import { Router } from '@angular/router';
import { MasterService } from '../../../services/master/master.service';

@Component({
  selector: 'app-add-new-product',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './add-new-product.component.html',
  styleUrl: './add-new-product.component.css'
})
export class AddNewProductComponent implements OnInit {
  product: Product = new Product();
  router = inject(Router);
  shoppingSrc = inject(ShoppingMasterService);
  masterSrc = inject(MasterService);
  ngOnInit() {
    this.masterSrc.user$.subscribe(userObj => {
      if (userObj === null || (userObj && userObj.userName !== "admin")) {
        this.router.navigate(['/login']);
      }
    })
  }
  createProduct(input: HTMLInputElement) {
    debugger;
    if (input.files && input.files.length > 0) {
      const file = input.files[0];
      const formData = new FormData();
      formData.append('file', file);
      this.shoppingSrc.uploadImage(formData).subscribe((res) => {
        if (res.imageUrl) {
          this.product.image = res.imageUrl;
          this.shoppingSrc.addNewProduct(this.product).subscribe((res: IResponse) => {
            alert(res.statusMessage);
            this.goToHome();
          })
        }
      })

    }
  }

  goToHome() {
    this.router.navigate(['/shopping-cart']);
  }
}
