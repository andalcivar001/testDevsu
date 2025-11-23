// spinner.service.ts
import { Injectable, signal } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class SpinnerService {
  // Usar la API de signals de Angular 17
  private isLoadingSignal = signal<boolean>(false);

  isLoading() {
    return this.isLoadingSignal.asReadonly();
  }

  show() {
    this.isLoadingSignal.set(true);
  }

  hide() {
    this.isLoadingSignal.set(false);
  }
}
