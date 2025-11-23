import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReporteMovimientos } from './reporte-movimientos';

describe('ReporteMovimientos', () => {
  let component: ReporteMovimientos;
  let fixture: ComponentFixture<ReporteMovimientos>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ReporteMovimientos]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ReporteMovimientos);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
