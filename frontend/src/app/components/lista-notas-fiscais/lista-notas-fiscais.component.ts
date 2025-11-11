import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Subscription } from 'rxjs';
import { FaturamentoService } from '../../services/faturamento.service';
import { NotaFiscal } from '../../models/notaFiscal.model';
import { MatPaginator } from '@angular/material/paginator';

@Component({
  selector: 'app-lista-notas-fiscais',
  templateUrl: './lista-notas-fiscais.component.html',
  styleUrl: './lista-notas-fiscais.component.scss'
})
export class ListaNotasFiscaisComponent implements OnInit, OnDestroy {

  @ViewChild('paginatorAbertas') set paginatorAbertas(paginator: MatPaginator) {
    if (paginator) {
      this.dataSourceAbertas.paginator = paginator;
    }
  }

  @ViewChild('paginatorFechadas') set paginatorFechadas(paginator: MatPaginator) {
    if (paginator) {
      this.dataSourceFechadas.paginator = paginator;
    }
  }

  public todasAsNotas: NotaFiscal[] = [];
  
  public dataSourceAbertas = new MatTableDataSource<NotaFiscal>();
  public dataSourceFechadas = new MatTableDataSource<NotaFiscal>();

  public displayedColumnsAbertas: string[] = ['numero', 'itens', 'acoes'];
  public displayedColumnsFechadas: string[] = ['numero', 'itens', 'status'];

  public isLoading = true;
  public erroApi: string | null = null;
  public imprimindoNotaNum: number | null = null;

  private subscriptions = new Subscription();

  constructor(
    private faturamentoService: FaturamentoService,
    private _snackBar: MatSnackBar
  ) { }

  ngOnInit(): void {
    this.carregarNotas();
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }

  carregarNotas(): void {
    this.isLoading = true;
    this.erroApi = null;

    const sub = this.faturamentoService.getNotasFiscais().subscribe({
      next: (notas) => {
        this.todasAsNotas = notas;
        this.separarNotas();
        this.isLoading = false;
      },
      error: (err) => {
        this.erroApi = err.message;
        this.isLoading = false;
        console.error(err);
      }
    });

    this.subscriptions.add(sub);
  }

  separarNotas(): void {
    this.dataSourceAbertas.data = this.todasAsNotas.filter(n => n.status === 'Aberta');
    this.dataSourceFechadas.data = this.todasAsNotas.filter(n => n.status === 'Fechada');
  }

  onImprimir(numeroNota: number): void {
    this.imprimindoNotaNum = numeroNota;
    this.erroApi = null;

    const sub = this.faturamentoService.imprimirNotaFiscal(numeroNota).subscribe({
      next: (notaImpressa) => {
        this.openSnackBar(`Nota ${notaImpressa.numero} processada e fechada!`, 'Sucesso');
        
        this.carregarNotas(); 
        
        this.imprimindoNotaNum = null;
      },
      error: (err) => {
        this.openSnackBar(err.message, 'Erro'); 
        this.imprimindoNotaNum = null;
      }
    });

    this.subscriptions.add(sub);
  }

  openSnackBar(message: string, action: string): void {
    this._snackBar.open(message, action, {
      duration: 3000,
      verticalPosition: 'top'
    });
  }
}