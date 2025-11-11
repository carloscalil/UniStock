import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { catchError, Observable, throwError } from 'rxjs';
import { AtualizarProdutoDTO, CriarProdutoDTO, Produto } from '../models/produto.model';

@Injectable({
  providedIn: 'root'
})
export class ProdutoService {

  private apiUrl = `${environment.estoqueApiUrl}/produtos`;

  constructor(private http: HttpClient) { }


  getProdutos(): Observable<Produto[]> {
    return this.http.get<Produto[]>(this.apiUrl)
      .pipe(
        catchError(this.handleError)
      );
  }

  criarProduto(produto: CriarProdutoDTO): Observable<Produto> {
    return this.http.post<Produto>(this.apiUrl, produto)
      .pipe(
        catchError(this.handleError)
      );
  }

  atualizarProduto(codigo: number, produto: AtualizarProdutoDTO): Observable<Produto> {
    return this.http.put<Produto>(`${this.apiUrl}/${codigo}`, produto)
      .pipe(catchError(this.handleError));
  }

  private handleError(error: any) {
    console.error('Ocorreu um erro na chamada da API de Estoque:', error);
    return throwError(() => new Error('Erro ao comunicar com o serviço de estoque. Tente novamente mais tarde.'));
  }
}
