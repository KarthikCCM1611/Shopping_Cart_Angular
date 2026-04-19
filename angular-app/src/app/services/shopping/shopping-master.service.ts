import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { CartProduct, IResponse, Product, SHOPPING_API } from '../../models/shopping-model/shopping-cart.model';
import { catchError, Observable, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ShoppingMasterService {

  private baseUrl = 'https://localhost:7168/api/shoppingcart';
  private GUID = "00000000-0000-0000-0000-000000000000";
  private http = inject(HttpClient);
  constructor() { }

  getAllProducts(): Observable<IResponse> {
    return this.http.get<IResponse>(`${this.baseUrl}/${SHOPPING_API.GET_ALL_PRODUCTS}`).pipe(
      catchError(this.handleError)
    );
  }

  addNewProduct(product: Product): Observable<IResponse> {
    product.id = this.GUID;
    return this.http.post<IResponse>(`${this.baseUrl}/${SHOPPING_API.ADD_NEW_PRODUCT}`, product).pipe(
      catchError(this.handleError)
    );
  }

  uploadImage(data: FormData, guid?: string): Observable<IResponse> {
    let id = guid ? guid : this.GUID;
    return this.http.post<IResponse>(`${this.baseUrl}/${SHOPPING_API.UPLOAD_IMAGE}?id=${id}`, data).pipe(
      catchError(this.handleError)
    );
  }

  getProductById(id: string): Observable<IResponse> {
    return this.http.get<IResponse>(`${this.baseUrl}/${SHOPPING_API.GET_PRODUCT_BY_ID}?id=${id}`).pipe(
      catchError(this.handleError)
    );
  }

  updateProduct(product: Product): Observable<IResponse> {
    return this.http.put<IResponse>(`${this.baseUrl}/${SHOPPING_API.UPDATE_PPRODUCT}`, product).pipe(
      catchError(this.handleError)
    );
  }

  deleteProduct(id: string): Observable<IResponse> {
    return this.http.delete<IResponse>(`${this.baseUrl}/${SHOPPING_API.DELETE_PRODUCT}/?id=${id}`).pipe(
      catchError(this.handleError)
    );
  }

  getAllCartProducts(userId: string): Observable<IResponse> {
    return this.http.get<IResponse>(`${this.baseUrl}/${SHOPPING_API.Get_All_CART_PRODUCTS}/?userId=${userId}`).pipe(
      catchError(this.handleError)
    );
  }

  addToCart(product: CartProduct): Observable<IResponse> {
    return this.http.post<IResponse>(`${this.baseUrl}/${SHOPPING_API.ADD_TO_CART}`, product).pipe(
      catchError(this.handleError)
    );
  }

  deleteCartProduct(id: string, userId: string): Observable<IResponse> {
    // return this.http.delete<void>(`${this.baseUrl}/${SHOPPING_API.DELETE_CART_PRODUCT}/id?=${id}`).pipe(
    //   catchError(this.handleError)
    // );
    return this.http.delete<IResponse>(`${this.baseUrl}/${SHOPPING_API.DELETE_CART_PRODUCT}?id=${id}&userId=${userId}`);
  }

  private handleError(error: HttpErrorResponse) {
    const errorDetails: string = (error.error && !(error.error instanceof ProgressEvent)) ? error.error : 'Something went wrong. Please try again later.'
    if (error.error instanceof ErrorEvent) {
      // Client-side or network error
      console.error('Client-side error:', error.error.message);
    } else {
      // Backend error
      console.error(
        `Server returned code ${error.status}, ` +
        `Message: ${errorDetails}`
      );
    }
    // Return a user-facing error message
    return throwError(() => new Error(errorDetails));
  }
}
