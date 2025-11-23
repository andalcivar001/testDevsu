import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-dialog-confirm',
  imports: [],
  templateUrl: './dialog-confirm.html',
  styleUrl: './dialog-confirm.scss',
})
export class DialogConfirm {
  @Input() show: boolean = false;

  @Output() onConfirm = new EventEmitter<void>();
  @Output() onCancel = new EventEmitter<void>();

  confirm() {
    this.onConfirm.emit();
    this.show = false;
  }

  cancel() {
    this.onCancel.emit();
    this.show = false;
  }
}
