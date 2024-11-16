import { Component, OnInit, ViewChild } from '@angular/core';
import { GoogleMap, MapMarker } from '@angular/google-maps';
import { StoreService } from '../services/store.service';
import { CountryService } from '../services/country.service';
import { CityService } from '../services/city.service';
import { Store } from '../shared/models/store.model';
import { Country } from '../shared/models/country.model';
import { City } from '../shared/models/city.model';
import { forkJoin } from 'rxjs';
import { StoreImageService } from '../services/storeImage.service';

@Component({
  selector: 'app-visit-us',
  templateUrl: './visit.us.component.html',
  styleUrls: ['./visit.us.component.css']
})
export class VisitUsComponent implements OnInit {
  @ViewChild(GoogleMap) map!: GoogleMap;

  countries: Country[] = [];
  cities: City[] = [];
  stores: Store[] = [];
  selectedStore: Store | null = null;

  mapOptions: google.maps.MapOptions = {
    center: { lat: 44.0, lng: 18.0 },
    zoom: 7
  };

  constructor(
    private storeService: StoreService,
    private countryService: CountryService,
    private cityService: CityService,
    private storeImageService: StoreImageService
  ) {}

  ngOnInit() {
    forkJoin({
      countries: this.countryService.getAllCountries(),
      cities: this.cityService.getAllCities(),
      stores: this.storeService.getAllStores()
    }).subscribe(
      ({ countries, cities, stores }) => {
        this.countries = countries;
        this.cities = cities;
        this.stores = stores;
        this.updateMapMarkers();
      },
      error => console.error('Greška pri hvatanju podataka:', error)
    );
  }

  loadCountries() {
    this.countryService.getAllCountries().subscribe(
      (countries) => {
        this.countries = countries;
      },
      (error) => console.error('Greška pri hvatanju država::', error)
    );
  }

  loadCities(countryId: number) {
    this.cityService.getAllCities().subscribe(
      (cities) => {
        this.cities = cities.filter(city => city.countryId === countryId);
      },
      (error) => console.error('Greška pri hvatanju gradova:', error)
    );
  }

  loadStores() {
    this.storeService.getAllStores().subscribe(
      (stores) => {
        this.stores = stores;
        this.updateMapMarkers();
      },
      (error) => console.error('Greška pri hvatanju prodavnica:', error)
    );
  }

  updateMapMarkers() {
    this.stores.forEach(store => {
      const marker = new google.maps.Marker({
        position: { lat: store.latitude, lng: store.longitude },
        map: this.map.googleMap,
        title: store.name
      });

      marker.addListener('click', () => {
        this.selectedStore = store;
      });
    });
  }

  onCountrySelect(event: Event) {
    const select = event.target as HTMLSelectElement;
    const countryId = parseInt(select.value, 10);
    if (!isNaN(countryId)) {
      this.loadCities(countryId);
    }
  }

  getCityName(store: Store): string {
    const city = this.cities.find(c => c.id === store.cityId);
    return city ? city.name : 'N/A';
  }

  getCountryName(store: Store): string {
    const city = this.cities.find(c => c.id === store.cityId);
    if (city) {
      const country = this.countries.find(c => c.id === city.countryId);
      return country ? country.name : 'N/A';
    }
    return 'N/A';
  }

  loadStoreImage(storeId: number) {
    
    this.storeImageService.getStoreImage(storeId).subscribe(
      (imageData) => {
        console.log(imageData)
        if (this.selectedStore && this.selectedStore.id === storeId) {
          this.selectedStore.imageString = `${imageData[0].imageFormat}${imageData[0].imageByteArray}`;
        }
      },
      error => console.error('Greška pri hvataanju slike:', error)
    );
  }

  onStoreSelect(store: Store) {
    this.selectedStore = store;
    if (store.id !== undefined) {
      this.loadStoreImage(store.id);
    }
  }
}
