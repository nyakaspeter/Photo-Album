import { Component, OnInit, Input, ViewChild, ElementRef } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-text-input-modal',
  templateUrl: './text-input-modal.component.html',
  styleUrls: ['./text-input-modal.component.scss'],
})
export class TextInputModalComponent implements OnInit {
  @Input() title: string;
  @Input() placeholder: string;
  @Input() confirmText: string;
  @Input() cancelText: string;
  @Input() cancellable: boolean;

  text: string;

  constructor(private dialogRef: MatDialogRef<TextInputModalComponent>) {}

  ngOnInit() {}

  confirm = () => {
    this.dialogRef.close(this.text);
  };

  cancel = () => {
    this.dialogRef.close(false);
  };
}
