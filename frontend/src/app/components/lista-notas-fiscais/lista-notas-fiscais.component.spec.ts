import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ListaNotasFiscaisComponent } from './lista-notas-fiscais.component';

describe('ListaNotasFiscaisComponent', () => {
  let component: ListaNotasFiscaisComponent;
  let fixture: ComponentFixture<ListaNotasFiscaisComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ListaNotasFiscaisComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ListaNotasFiscaisComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
