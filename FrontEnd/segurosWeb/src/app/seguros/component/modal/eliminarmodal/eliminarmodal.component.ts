import { Component, Inject  } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-eliminarmodal',
  templateUrl: './eliminarmodal.component.html',
  styles: [
  ]
})
export class EliminarmodalComponent {

  constructor(
    public dialogRef: MatDialogRef<EliminarmodalComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {}

  onAceptarClick(): void {
    this.dialogRef.close(true); // Envía true al cerrar para indicar confirmación
  }

  onCancelarClick(): void {
    this.dialogRef.close(false); // Envía false al cerrar para indicar cancelación
  }
}
