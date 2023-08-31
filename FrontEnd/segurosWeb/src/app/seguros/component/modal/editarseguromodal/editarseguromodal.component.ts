import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Seguro } from 'src/app/seguros/interface/seguros.interface';

@Component({
  selector: 'app-editarseguromodal',
  templateUrl: './editarseguromodal.component.html',
  styles: [
  ]
})
export class EditarseguromodalComponent {

  editedData: Seguro = { ...this.data };

  constructor(
    public dialogRef: MatDialogRef<EditarseguromodalComponent>,
    @Inject(MAT_DIALOG_DATA) public data: Seguro
  ) {}

  onCancelarClick(): void {
    this.dialogRef.close();
  }

  onGuardarClick(): void {
    this.dialogRef.close(this.editedData);
  }
}
