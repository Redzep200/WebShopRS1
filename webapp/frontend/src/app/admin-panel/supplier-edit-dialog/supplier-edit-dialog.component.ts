import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-supplier-edit-dialog',
  templateUrl: './supplier-edit-dialog.component.html',
  styleUrls: ['./supplier-edit-dialog.component.css'],
})
export class SupplierEditDialogComponent {
  editedSupplier: any;

  constructor(
    public dialogRef: MatDialogRef<SupplierEditDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    this.editedSupplier = { ...data.supplier };
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

  onSaveClick(): void {
    this.dialogRef.close(this.editedSupplier);
  }
}