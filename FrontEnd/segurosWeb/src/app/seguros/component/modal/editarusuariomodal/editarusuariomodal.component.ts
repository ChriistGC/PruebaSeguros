import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Cliente } from 'src/app/seguros/interface/seguros.interface';

@Component({
  selector: 'app-editarusuariomodal',
  templateUrl: './editarusuariomodal.component.html',
  styles: [
  ]
})
export class EditarusuariomodalComponent {

  editedData: Cliente = { ...this.data };

  constructor(
    public dialogRef: MatDialogRef<EditarusuariomodalComponent>,
    @Inject(MAT_DIALOG_DATA) public data: Cliente
  ) {}

  onCancelarClick(): void {
    this.dialogRef.close();
  }

  onGuardarClick(): void {    
    this.dialogRef.close(this.editedData);
  }
}
