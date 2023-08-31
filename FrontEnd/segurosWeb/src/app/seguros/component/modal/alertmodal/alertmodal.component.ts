import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-alertmodal',
  templateUrl: './alertmodal.component.html',
  styles: [
  ]
})
export class AlertmodalComponent {

  constructor(
    public dialogRef: MatDialogRef<AlertmodalComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {}

  onAceptarClick(): void {
    this.dialogRef.close();
  }
}
