import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { CriarNotaFiscalDTO, NotaFiscal } from '../models/notaFiscal.model';
import { catchError, Observable, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class FaturamentoService {

  private apiUrl = `${environment.faturamentoApiUrl}/faturamento`;

  constructor(private http: HttpClient) { }

  getNotasFiscais(): Observable<NotaFiscal[]> {
    return this.http.get<NotaFiscal[]>(this.apiUrl)
      .pipe(
        catchError(this.handleError)
      );
  }


  criarNotaFiscal(dto: CriarNotaFiscalDTO): Observable<NotaFiscal> {
    return this.http.post<NotaFiscal>(this.apiUrl, dto)
      .pipe(
        catchError(this.handleError)
      );
  }

  imprimirNotaFiscal(numeroNota: number): Observable<NotaFiscal> {
    return this.http.post<NotaFiscal>(`${this.apiUrl}/${numeroNota}/imprimir`, null)
      .pipe(
        catchError(this.handleError)
      );
  }

  private handleError(error: any) {
    console.error('Ocorreu um erro na chamada da API de Faturamento:', error);
    if (error.error && typeof error.error === 'string') {
      return throwError(() => new Error(error.error));
    }
    return throwError(() => new Error('Erro ao comunicar com o serviço de faturamento.'));
  }
}
