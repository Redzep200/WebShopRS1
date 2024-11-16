import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-manufacturer-edit-dialog',
  templateUrl: './manufacturer-edit-dialog.component.html',
  styleUrls: ['./manufacturer-edit-dialog.component.css'],
})
export class ManufacturerEditDialogComponent {
  editedManufacturer: any;

  constructor(
    public dialogRef: MatDialogRef<ManufacturerEditDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    this.editedManufacturer = { ...data.manufacturer };
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

  onSaveClick(): void {
    this.dialogRef.close(this.editedManufacturer);
  }
}