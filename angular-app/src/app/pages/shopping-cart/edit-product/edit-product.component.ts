import { Component, inject, OnInit } from '@angular/core';
import { IResponse, Product } from '../../../models/shopping-model/shopping-cart.model';
import { ShoppingMasterService } from '../../../services/shopping/shopping-master.service';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-edit-product',
  standalone: true,
  imports: [RouterLink, FormsModule],
  templateUrl: './edit-product.component.html',
  styleUrl: './edit-product.component.css'
})
export class EditProductComponent implements OnInit {
  product: Product = new Product();
  updateProductId: string = "";
  isProductExists: boolean = true;
  shoppingSrc = inject(ShoppingMasterService);
  activateRoute = inject(ActivatedRoute);
  router = inject(Router);

  ngOnInit(): void {
    this.activateRoute.params.subscribe((res: any) => {
      this.updateProductId = res.id;
      this.getProductById();
    })
  }

  getProductById() {
    this.shoppingSrc.getProductById(this.updateProductId).subscribe({
      next: (res: IResponse) => {
        if (res.statusCode === 200) {
          this.product = res.product;
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

  // imageChange(event: Event, imageRef: HTMLImageElement) {
  //   const input = event.target as HTMLInputElement;
  //   if (input) {
  //     const reader = new FileReader();
  //     reader.onload = function (e) {
  //       imageRef.src = e.target.result;
  //     };
  //     reader.readAsDataURL(input.files[0]);
  //   }
  // }


  imageChange(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files[0]) {
      const file = input.files[0];
      const reader = new FileReader();

      reader.onload = () => {
        const previewImage = document.querySelector('#previewImage') as HTMLImageElement;
        if (previewImage) {
          previewImage.src = reader.result as string;
        }
      };

      reader.readAsDataURL(file);
    }
  }

  updateProduct(input: HTMLInputElement) {
    if (input.files && input.files.length > 0) {
      const file = input.files[0];
      const formData = new FormData();
      formData.append('file', file);
      this.shoppingSrc.uploadImage(formData, this.updateProductId).subscribe((res) => {
        if (res.imageUrl) {
          this.product.image = res.imageUrl;
          this.shoppingSrc.updateProduct(this.product).subscribe({
            next: (res: IResponse) => {
              if (res.statusCode === 200) {
                alert(res.statusMessage);
                this.router.navigate(['/shopping-cart']);
              }
            },
            error: (err) => {
              alert("API Error");
            }
          })
        }
      })

    }
  }
}
