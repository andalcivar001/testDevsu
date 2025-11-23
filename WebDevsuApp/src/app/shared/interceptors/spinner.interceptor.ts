// spinner.interceptor.ts
import {
  HttpHandlerFn,
  HttpInterceptorFn,
  HttpRequest,
} from '@angular/common/http';
import { inject } from '@angular/core';
import { finalize } from 'rxjs';
import { SpinnerService } from '../services/spinner.service';

let totalRequests = 0;

export const SpinnerInterceptor: HttpInterceptorFn = (
  request: HttpRequest<unknown>,
  next: HttpHandlerFn
) => {
  const spinnerService = inject(SpinnerService);

  totalRequests++;
  spinnerService.show();

  return next(request).pipe(
    finalize(() => {
      totalRequests--;
      if (totalRequests === 0) {
        spinnerService.hide();
      }
    })
  );
};
