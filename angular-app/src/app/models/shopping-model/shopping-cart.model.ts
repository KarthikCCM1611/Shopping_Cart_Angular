export const SHOPPING_API = {
  GET_ALL_PRODUCTS: 'GetAllProducts',
  ADD_NEW_PRODUCT: 'AddNewProduct',
  UPDATE_PPRODUCT: 'UpdateProduct',
  DELETE_PRODUCT: 'DeleteProduct',
  GET_PRODUCT_BY_ID: 'GetProductById',
  ADD_TO_CART: 'AddToCart',
  Get_All_CART_PRODUCTS: 'GetAllCartProducts',
  DELETE_CART_PRODUCT: 'DeleteCartProduct',
  UPLOAD_IMAGE: 'UploadImage',
};

export class Product {
  id: string;
  name: string;
  image: string;
  price: number;
  constructor() {
    this.id = "";
    this.name = "";
    this.image = "";
    this.price = 0;
  }
}

export class CartProduct {
  id: string;
  userId: string;
  name: string;
  image: string;
  price: number;
  constructor() {
    this.id = "";
    this.userId = "";
    this.name = "";
    this.image = "";
    this.price = 0;
  }
}


export interface IResponse {
  statusCode: number;
  statusMessage: number;
  listOfProducts: Product[];
  listOfCartProducts: CartProduct[];
  product: Product;
  imageUrl: string;
}