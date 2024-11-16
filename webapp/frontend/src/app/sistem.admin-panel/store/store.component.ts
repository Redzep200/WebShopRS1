import { Component, OnInit, ViewChild } from '@angular/core';
import { GoogleMap } from '@angular/google-maps';
import { StoreService } from 'src/app/services/store.service';
import { Store } from 'src/app/shared/models/store.model';
import { CityService } from 'src/app/services/city.service';
import { City } from 'src/app/shared/models/city.model';
import { Country } from 'src/app/shared/models/country.model';
import { CountryService } from 'src/app/services/country.service';
import { catchError, switchMap } from 'rxjs/operators';
import { of } from 'rxjs';
import { StoreImageService } from 'src/app/services/storeImage.service';

@Component({
  selector: 'app-store',
  templateUrl: './store.component.html',
  styleUrls: ['./store.component.css']
})
export class StoreComponent implements OnInit {
  @ViewChild(GoogleMap) map!: GoogleMap;
  
  stores: Store[] = [];
  newStore: Store = this.initializeNewStore();
  countries: Country[] = [];
  selectedStore: Store | null = null;
  
  mapOptions: google.maps.MapOptions = {
    center: { lat: 43.3438, lng: 17.8078 }, // Koordinate Mostara
    zoom: 12
  };

  constructor(
    private storeService: StoreService,
    private cityService: CityService,
    private countryService: CountryService,
    private storeImageService: StoreImageService
  ) {}

  ngOnInit() {
    this.loadStores();
    this.loadCountries();
  }

  initializeNewStore(): Store {
    return {
      name: '', 
      address: '', 
      latitude: 0, 
      longitude: 0, 
      cityId: 0, 
      city: { id: 0, name: '', zipCode: '', countryId: 0 },
      supplierIds: [],
      storeSuppliers: []
    };
  }

  loadStores() {
    this.storeService.getAllStores().subscribe(
      (stores) => {
        this.stores = stores;
        this.updateMapMarkers();
        console.log('Prodavnice:', this.stores);
      },
      (error) => console.error('Greška pri pronalaženju prodavnica:', error)
    );
  }

  loadCountries() {
    this.countryService.getAllCountries().subscribe(
      (countries) => {
        this.countries = countries;
      },
      (error) => console.error('Greška pri pronalaženju država:', error)
    );
  }

 

  addStore() {
    if (!this.newStore.name) {
      console.error('Obavezno je unijeti ime prodavnice');
      return;
    }

    this.newStore.supplierIds = this.newStore.supplierIds || [];

    this.storeService.createStore(this.newStore).subscribe(
      () => {
        this.loadStores();
        this.resetNewStore();
      },
      (error) => {
        console.error('Greška pri dodavanju prodavnice:', error);
        if (error.error && error.error.errors) {
          Object.keys(error.error.errors).forEach(key => {
            console.log(`${key}: ${error.error.errors[key]}`);
          });
        }
      }
    );
  }

  deleteStore(id: number | undefined) {
    if (id !== undefined) {
      this.storeService.deleteStore(id).subscribe(
        () => this.loadStores(),
        (error) => console.error('Greška pri brisanju prodavnice:', error)
      );
    } else {
      console.error('Prodavnica ne postoji');
    }
  }

  updateMapMarkers() {
    if (this.map && this.map.googleMap) {
      this.stores.forEach(store => {
        const marker = new google.maps.Marker({
          position: { lat: store.latitude, lng: store.longitude },
          map: this.map.googleMap,
          title: store.name
        });
  
        marker.addListener('click', () => {
          this.onStoreSelected(store);
        });
      });
  
      if (this.stores.length > 0) {
        this.map.googleMap.setCenter({ lat: this.stores[0].latitude, lng: this.stores[0].longitude });
      }
    }
  }

  onMapClick(event: google.maps.MapMouseEvent) {
    if (event.latLng) {
      this.newStore.latitude = event.latLng.lat();
      this.newStore.longitude = event.latLng.lng();
      this.getAddressFromLatLng(event.latLng);
    }
  }

  getAddressFromLatLng(latLng: google.maps.LatLng) {
    const geocoder = new google.maps.Geocoder();
    geocoder.geocode({ location: latLng }, (results, status) => {
      if (status === 'OK' && results && results[0]) {
        this.newStore.address = results[0].formatted_address;
        let cityName = '';
        let zipCode = '';
        let countryName = '';

        for (let component of results[0].address_components) {
          if (component.types.includes('locality')) {
            cityName = component.long_name;
          } else if (component.types.includes('administrative_area_level_2') && !cityName) {
            cityName = component.long_name;
          }
          if (component.types.includes('postal_code')) {
            zipCode = component.long_name;
          }
          if (component.types.includes('country')) {
            countryName = component.long_name;
          }
        }

        if (!this.newStore.city) {
          this.newStore.city = { id: 0, name: '', zipCode: '', countryId: 0 };
        }

        this.newStore.city.name = cityName;
        this.newStore.city.zipCode = zipCode || '';

        this.getOrCreateCountryAndCity(countryName, cityName, zipCode);
      }
    });
  }

  getOrCreateCountryAndCity(countryName: string, cityName: string, zipCode: string) {
    this.countryService.getOrCreateCountryByName(countryName).pipe(
      switchMap(country => {
        this.newStore.city.countryId = country.id;
        return this.cityService.getAllCities().pipe(
          switchMap(cities => {
            const existingCity = cities.find(city => 
              city.name.toLowerCase() === cityName.toLowerCase() && 
              city.countryId === country.id
            );
            if (existingCity) {
              return of(existingCity);
            } else {
              const newCity: Partial<City> = {
                name: cityName,
                countryId: country.id,
                zipCode: zipCode || ''
              };
              return this.cityService.addCity(newCity as City).pipe(
                switchMap(response => {
                  if (response.success) {
                    return of(response.city);
                  } else {
                    console.error('Greška pri dodavanju grada:', response.message);
                    return of(null);
                  }
                }),
                catchError(error => {
                  console.error('Greška pri dodavanju grada:', error);
                  return of(null);
                })
              );
            }
          })
        );
      })
    ).subscribe(
      city => {
        if (city) {
          this.updateNewStoreCity(city);
        } else {
          console.error('Greška pri dodavanju/pronalaženju grada');
        }
      },
      error => console.error('greška pri dodavanju ili pronalaženju grada:', error)
    );
  }

  updateNewStoreCity(city: City) {
    this.newStore.city = {
      id: city.id,
      name: city.name,
      zipCode: city.zipCode || '',
      countryId: city.countryId
    };
    this.newStore.cityId = city.id;
  }

  resetNewStore() {
    this.newStore = this.initializeNewStore();
  }

  convertToBase64(file: File): Promise<string> {
    return new Promise((resolve, reject) => {
      const reader = new FileReader();
      reader.readAsDataURL(file);
      reader.onload = () => resolve(reader.result as string);
      reader.onerror = error => reject(error);
    });
  }


  uploadImage(productId: number, base64Image: string) {
    const payload = {
      productId: productId,
      image: base64Image
    };
  }

  onStoreSelected(store: Store) {
    this.selectedStore = store;
    if (store.id !== undefined) {
      this.loadStoreImage(store.id);
    }
  }

  loadStoreImage(storeId: number) {
    this.storeImageService.getStoreImage(storeId).subscribe(
      (imageData) => {
        if (this.selectedStore && this.selectedStore.id === storeId) {
          this.selectedStore.imageString = `data:${imageData.imageFormat};base64,${imageData.imageByteArray}`;
        }
      },
      error => console.error('Greška pri hvatanju slike prodavnice:', error)
    );
  }

  onStoreImageSelected(event: Event, storeId: number | undefined) {
    if (storeId === undefined) {
      console.error('Store ID je nedefinisan');
      return;
    }

    const file = (event.target as HTMLInputElement).files?.[0];
    if (file) {
      this.convertToBase64(file).then(base64 => {
        this.storeImageService.addStoreImage(storeId, base64).subscribe(
          () => console.log('Slika uspješno dodana'),
          error => console.error('Greška pri dodavanju slike:', error)
        );
      });
    }
  }
}