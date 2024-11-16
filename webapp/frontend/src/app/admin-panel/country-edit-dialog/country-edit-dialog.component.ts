import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-country-edit-dialog',
  templateUrl: './country-edit-dialog.component.html',
  styleUrls: ['./country-edit-dialog.component.css']
})
export class CountryEditDialogComponent {
  editedCountryName: string;

  constructor(
    public dialogRef: MatDialogRef<CountryEditDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    this.editedCountryName = data.country.name;
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

  saveChanges(): void {
    const editedCountry = {
      id: this.data.country.id,
      name: this.editedCountryName
    };

    this.dialogRef.close(editedCountry);
  }
}