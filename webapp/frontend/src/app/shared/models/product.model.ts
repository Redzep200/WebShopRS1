export interface Product {
  productId: number;
  categoryId: number;
  //category?: Category;
  name: string;
  description: string;
  price: number;
  manufacturerId: number;
  // manufacturer?: Manufacturer;
  isDeleted: boolean;
  deletionDate?: Date;
  imageString: string;
}
