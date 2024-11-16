import { Supplier } from "./supplier.model";

export interface Store {
    id?: number;
    name: string;
    address: string;
    latitude: number;
    longitude: number;
    cityId: number;
    city: {
      id: number;
      name: string;
      zipCode: string;
      countryId: number;
    };
    supplierIds: number[];
    suppliers?: Supplier[];
    storeSuppliers?: StoreSupplier[];
    imageString?: string;
  }

  export interface StoreSupplier {
    storeId: number;
    supplierId: number;
    supplier?: {
      id: number;
      name: string;
    }
}

