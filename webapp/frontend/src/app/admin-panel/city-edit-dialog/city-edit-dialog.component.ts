import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-city-edit-dialog',
  templateUrl: './city-edit-dialog.component.html',
  styleUrls: ['./city-edit-dialog.component.css'],
})
export class CityEditDialogComponent {
  editedCity: any;
  
  constructor(
    public dialogRef: MatDialogRef<CityEditDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    this.editedCity = { ...data.city };
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

  onSaveClick(): void {
    this.dialogRef.close(this.editedCity);
  }
}