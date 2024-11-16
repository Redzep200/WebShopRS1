import { Component, Input, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MyConfig } from 'src/app/my-config';
import { filter } from 'rxjs';

@Component({
  selector: 'app-countries-component',
  templateUrl: './countries-component.component.html',
  styleUrls: ['./countries-component.component.css']
})
export class CountriesComponentComponent implements OnInit {
  constructor(private http:HttpClient){}
  @Input()
  public pretraga:any
  public countries:any
  public filteredCountries:any
  public countryForEdit:any = {}
  public newCountryName:any


  ngOnInit(): void {
    this.http.get(MyConfig.APIurl+'/api/Country/GetAllCountries').subscribe(x=>{
      this.countries=x;
      this.filterCountries();
    })
  }

  editCountry(country:any):any{
this.countryForEdit=country;
  }

  createCountry():void{
  this.http.post(MyConfig.APIurl+'/api/Country/AddCountry?name='+this.newCountryName,[{}]).subscribe((response)=>{
    console.log('Create response',response);
  });
  this.getCountries();
  }

  deleteCountry(country:any): any{
    this.http.delete(MyConfig.APIurl+'/api/Country/DeleteCountry?id='+country.id).subscribe(
      (response) => {
        console.log('Delete response:', response);
        // Handle the response as needed
      },
      (error) => {
        console.error('Delete error:', error);
        // Handle the error as needed
      }
    );
    this.getCountries();
  }

  saveChanges() : any{
    this.http.put(MyConfig.APIurl+'/api/Country/UpdateCountry?id='+this.countryForEdit.id+'&name='+this.countryForEdit.name,[{}]).subscribe((response) => {
    console.log('Update response:', response);
    // Handle the response as needed
  },
  (error) => {
    console.error('Update error:', error);
    // Handle the error as needed
  });
  }

  getCountries():any{
    this.http.get(MyConfig.APIurl+'/api/Country/GetAllCountries').subscribe(x=>{
      this.countries=x;
    console.log(this.countries);
    })
    this.filterCountries();
  }

  filterCountries() : void{
    if(!this.pretraga){
      this.filteredCountries=this.countries;
    }
    else{
    this.filteredCountries = this.countries.filter((country:any) => 
      country.name.toLowerCase().includes(this.pretraga.toLowerCase())
    );
    }
  }

}
