export interface Supplier {
    id: number;
    name: string;
    cityId: number;
    city?: {
      id: number;
      name: string;
      zipCode: string;
      countryId: number;
    };
    adress: string;
    contactPhone: string;
    email: string;
  }