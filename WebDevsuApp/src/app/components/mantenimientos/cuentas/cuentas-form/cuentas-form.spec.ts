import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CuentasForm } from './cuentas-form';

describe('CuentasForm', () => {
  let component: CuentasForm;
  let fixture: ComponentFixture<CuentasForm>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CuentasForm]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CuentasForm);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
