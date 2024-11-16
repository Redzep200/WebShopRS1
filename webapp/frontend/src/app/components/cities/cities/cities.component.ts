import { HttpClient } from '@angular/common/http';
import { Component, Input, OnInit} from '@angular/core';
import { MyConfig } from 'src/app/my-config';

@Component({
  selector: 'app-cities',
  templateUrl: './cities.component.html',
  styleUrls: ['./cities.component.css']
})
export class CitiesComponent implements OnInit{
constructor(private http:HttpClient){}
@Input()
pretraga:any
cities:any
cityForEdit:any = {}
selectedCountryName:any = ''
cityName:any
zipCode:any
public countries:any

ngOnInit():void{
  this.getCities();
  this.getCountries();
  console.log(this.selectedCountryName);
}

getCities():any
{
  this.http.get(MyConfig.APIurl+'/api/City/GetAllCities').subscribe(x=>{
    this.cities=x;
    if(typeof this.pretraga === "string" && this.pretraga.trim() !== ""){
     this.cities= this.cities.filter((y:any)=>y.name.toLowerCase().includes(this.pretraga.toLowerCase()));
    }
  })
}

getCountries():any{
  this.http.get(MyConfig.APIurl+'/api/Country/GetAllCountries').subscribe(x=>{
    this.countries=x;
  })
}

deleteCity(s:any){
  this.http.delete(MyConfig.APIurl+'/api/City/DeleteCity?id='+s.id).subscribe(
    (response) => {
      console.log('Delete response:', response);
      // Handle the response as needed
    },
    (error) => {
      console.error('Delete error:', error);
      // Handle the error as needed
    }
  );
}
createCity(){
  const dropdownCountry = this.countries.find((country:any) => country.name === this.selectedCountryName)
  console.log("Id:"+dropdownCountry.id+", name:"+this.selectedCountryName);
  this.http.post(MyConfig.APIurl+'/api/City/CreateCity?countryid='+dropdownCountry.id+'&name='+this.cityName+'&zipcode='+this.zipCode,[{}])
  .subscribe((response)=>{
    console.log('Create response:', response);
  });
}

editCity(c:any){
  console.log("dugem pressed");
  this.cityForEdit=c;
  this.selectedCountryName=c.country.name;
}

saveChanges(){
  const dropdownCountry = this.countries.find((country:any) => country.name === this.selectedCountryName)
  this.http.put(MyConfig.APIurl+'/api/City/UpdateCity?id='+this.cityForEdit.id+'&countryid='+dropdownCountry.id+'&name='+this.cityForEdit.name+
  '&zipcode='+this.cityForEdit.zipCode,[{}]).subscribe((response) => {
    console.log('Update response:', response);
    // Handle the response as needed
  },
  (error) => {
    console.error('Update error:', error);
    // Handle the error as needed
  });
}
writeId(id:any):void{
  console.log(id);
}
}




