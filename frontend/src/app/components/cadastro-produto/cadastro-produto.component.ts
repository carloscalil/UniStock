import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Subscription } from 'rxjs';
import { CriarProdutoDTO, Produto, AtualizarProdutoDTO } from '../../models/produto.model';
import { ProdutoService } from '../../services/produto.service';

import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-cadastro-produto',
  templateUrl: './cadastro-produto.component.html',
  styleUrls: ['./cadastro-produto.component.scss']
})
export class CadastroProdutoComponent implements OnInit, OnDestroy  {

  private _paginator: MatPaginator | null = null;
  @ViewChild(MatPaginator) set paginator(paginator: MatPaginator) {
    this._paginator = paginator;
    this.dataSource.paginator = paginator;
  }

  @ViewChild(MatSort) set sort(sort: MatSort) {
    this.dataSource.sort = sort;
  }

  public isLoading = true;
  public erroApi: string | null = null;

  public dataSource = new MatTableDataSource<Produto>();
  public displayedColumns: string[] = ['codigo', 'descricao', 'saldo', 'ativo', 'acoes'];

  public novoProduto: CriarProdutoDTO = {
    descricao: '',
    saldo: 0
  };

  public produtoEmEdicao: Produto | null = null;
  
  private subscriptions = new Subscription();

  constructor(private produtoService: ProdutoService, private snackBar: MatSnackBar) { }
  
  ngOnInit(): void {
    this.carregarProdutos();
  }
  
  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }


  carregarProdutos(): void {
  this.isLoading = true;
  this.erroApi = null;

  const sub = this.produtoService.getProdutos().subscribe({
    next: (produtosRecebidos) => {
      this.isLoading = false;
      this.dataSource.data = produtosRecebidos;

      this.dataSource.filterPredicate = (data: Produto, filtro: string) => {
        const texto = filtro.trim().toLowerCase();
        return (
          data.descricao.toLowerCase().includes(texto) ||
          data.codigo.toString().includes(texto) ||
          (data.ativo ? 'ativo' : 'inativo').includes(texto)
        );
      };
    },
    error: (erro) => {
      this.erroApi = erro.message;
      this.isLoading = false;
      console.error(erro);
    }
  });

  this.subscriptions.add(sub);
}

  onSubmit(form: NgForm): void {
    if (form.invalid) {
      return;
    }

    if (this.produtoEmEdicao) {
      this.atualizarProduto(form);
    } else {
      this.criarProduto(form);
    }
  }

  criarProduto(form: NgForm): void {
    this.erroApi = null;
    const sub = this.produtoService.criarProduto(this.novoProduto).subscribe({
      next: (produtoCriado) => {
        this.carregarProdutos();
        form.resetForm({ saldo: 0 });
        this.novoProduto.descricao = '';
        this.snackBar.open(`Produto "${produtoCriado.descricao}" criado com sucesso!`, 'Fechar', { duration: 3000 });
      },
      error: (erro) => {
        this.erroApi = erro.message;
        console.error(erro);
      }
    });
    this.subscriptions.add(sub);
  }


  atualizarProduto(form: NgForm): void {
    if (!this.produtoEmEdicao) return; 

    const dto: AtualizarProdutoDTO = {
      descricao: this.novoProduto.descricao,
      saldo: this.novoProduto.saldo
    };

    this.erroApi = null;
    const sub = this.produtoService.atualizarProduto(this.produtoEmEdicao.codigo, dto).subscribe({
      next: (produtoAtualizado) => {
        this.carregarProdutos();
        this.onCancelarEdicao(form);
        this.snackBar.open(`Produto "${produtoAtualizado.descricao}" atualizado com sucesso!`, 'Fechar', { duration: 3000 });
      },
      error: (erro) => {
        this.erroApi = erro.message;
        console.error(erro);
      }
    });
    this.subscriptions.add(sub);
  }

  onEditar(produto: Produto): void {
    this.produtoEmEdicao = produto;
    
    this.novoProduto.descricao = produto.descricao;
    this.novoProduto.saldo = produto.saldo;

    document.getElementById('descricao')?.focus();
  }

  onCancelarEdicao(form: NgForm): void {
    this.produtoEmEdicao = null;
    form.resetForm({ saldo: 0 });
    this.novoProduto.descricao = '';
  }

  

  aplicarFiltro(event: Event): void {
    const valor = (event.target as HTMLInputElement).value;
    this.dataSource.filter = valor.trim().toLowerCase();

    if (this._paginator) { 
      this._paginator.firstPage();
    }
  } 
}