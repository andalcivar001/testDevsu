import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ToastrModule } from 'ngx-toastr';

// Angular Material
@NgModule({
  declarations: [],
  exports: [FormsModule, ReactiveFormsModule, ToastrModule, CommonModule],
})
export class SharedModule {}
