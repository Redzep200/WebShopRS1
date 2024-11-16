// import { DatePipe } from '@angular/common';
// import { HttpClient } from '@angular/common/http';
// import { Component, OnInit, Input } from '@angular/core';
// import { MyConfig } from 'src/app/my-config';


// @Component({
//   selector: 'app-coupons',
//   templateUrl: './coupons.component.html',
//   styleUrls: ['./coupons.component.css']
// })
// export class CouponsComponent implements OnInit{
//   constructor(private http:HttpClient, private datePipe:DatePipe){}
//   coupons:any
//   couponAmount:any
//   discount:any
//   expiryDate:any
//   couponForEdit:any = {}

//  ngOnInit():void{
// this.http.get(MyConfig.APIurl+'/api/Coupon/GetAllCoupons').subscribe((x:any)=>{
//   x.forEach((element:any) => {
//   element.expirationDate=this.datePipe.transform(element.expirationDate,'yyyy-MM-dd');
//   });
//   this.coupons=x;
// });
//  }

//  createCoupons(){
//   this.http.post(MyConfig.APIurl+'/api/Coupon/CreateNumberOfCoupons?numberOfCoupons='
//   +this.couponAmount+'&discountPerc='+this.discount
//   +'&expirationDate='+this.expiryDate+'%20',[{}]).subscribe((response)=>
//   console.log(response),
//   (error)=>
//   alert(error.error.text));
//   this.expiryDate=null;
//   this.discount=null;
//   this.couponAmount=null;
//  }

//  editCoupon(c:any){
//  this.couponForEdit=c;
//  this.couponForEdit.expirationDate=this.datePipe.transform(c.expirationDate,'yyyy-MM-dd');
//  console.log(c.expirationDate);
//  }

//  deleteCoupon(c:any){
//   this.http.delete(MyConfig.APIurl+'/api/Coupon/DeleteCoupon?id='+c.id).subscribe((response)=>
//   console.log(response),
//   (error)=>
//   console.log(error));
//  }

//  saveChanges(){
//   this.http.put(MyConfig.APIurl+'/api/Coupon/UpdateCoupon?id='+this.couponForEdit.id
//   +'&isActive='+this.couponForEdit.isActive
//   +'&expDate='+this.couponForEdit.expirationDate
//   +'&discountPerc='+this.couponForEdit.percentDiscount,[{}]).subscribe((response)=>
//   console.log(response),
//   (error)=>
//   console.log(error));
//  }
// }
