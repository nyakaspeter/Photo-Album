import { Injectable } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { AlertModalComponent } from './alert-modal/alert-modal.component';
import { TextInputModalComponent } from './text-input-modal/text-input-modal.component';

@Injectable({
  providedIn: 'root',
})
export class ModalService {
  constructor(private dialog: MatDialog) {}

  alert = (
    message: string,
    title: string = '',
    cancellable: boolean = false,
    confirmText: string = 'Ok',
    cancelText: string = 'Cancel'
  ) => {
    const dialogRef = this.dialog.open(AlertModalComponent, {
      width: '500px',
      disableClose: true,
    });
    dialogRef.componentInstance.message = message;
    dialogRef.componentInstance.title = title;
    dialogRef.componentInstance.confirmText = confirmText;
    dialogRef.componentInstance.cancelText = cancelText;
    dialogRef.componentInstance.cancellable = cancellable;
    return dialogRef;
  };

  textinput = (
    placeholder: string,
    title: string = '',
    cancellable: boolean = true,
    confirmText: string = 'Ok',
    cancelText: string = 'Cancel'
  ) => {
    const dialogRef = this.dialog.open(TextInputModalComponent, {
      width: '500px',
      disableClose: true,
    });
    dialogRef.componentInstance.placeholder = placeholder;
    dialogRef.componentInstance.title = title;
    dialogRef.componentInstance.confirmText = confirmText;
    dialogRef.componentInstance.cancelText = cancelText;
    dialogRef.componentInstance.cancellable = cancellable;
    return dialogRef;
  };
}
