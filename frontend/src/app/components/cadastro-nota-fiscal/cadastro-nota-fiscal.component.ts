import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatTableDataSource } from '@angular/material/table';
import { Observable, Subscription, startWith, map } from 'rxjs';
import { Produto } from '../../models/produto.model';
import { FaturamentoService } from '../../services/faturamento.service';
import { ProdutoService } from '../../services/produto.service';
import { CriarNotaFiscalDTO, ItemNotaDTO } from '../../models/notaFiscal.model';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-cadastro-nota-fiscal',
  templateUrl: './cadastro-nota-fiscal.component.html',
  styleUrls: ['./cadastro-nota-fiscal.component.scss']
})
export class CadastroNotaFiscalComponent implements OnInit, OnDestroy {

  private _paginator: MatPaginator | null = null;
    @ViewChild(MatPaginator) set paginator(paginator: MatPaginator) {
      this._paginator = paginator;
      this.dataSource.paginator = paginator;
    }
  
    @ViewChild(MatSort) set sort(sort: MatSort) {
      this.dataSource.sort = sort;
    }

  itemForm: FormGroup;
  todosOsProdutos: Produto[] = [];
  filteredProdutos$!: Observable<Produto[]>;

  itensDaNota: ItemNotaDTO[] = [];
  dataSource = new MatTableDataSource<ItemNotaDTO>();
  displayedColumns: string[] = ['codigo', 'descricao', 'quantidade', 'acoes'];

  isLoadingProdutos = true;
  isSavingNota = false;

  public itemEmEdicao: ItemNotaDTO | null = null;
  private subscriptions = new Subscription();

  constructor( private fb: FormBuilder, private produtoService: ProdutoService,private faturamentoService: FaturamentoService, private _snackBar: MatSnackBar) {
    this.itemForm = this.fb.group({
      produto: [null, Validators.required],
      quantidade: [1, [Validators.required, Validators.min(1)]]
    });
  }

  ngOnInit(): void {
    this.carregarProdutos();
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }

  validarProdutoSelecionado(control: any): { [key: string]: any } | null {
    return (typeof control.value === 'object' && control.value !== null) 
      ? null 
      : { produtoInvalido: true };
  }

  carregarProdutos(): void {
    this.isLoadingProdutos = true;
    const prodSub = this.produtoService.getProdutos().subscribe({
      next: (produtos) => {
        this.todosOsProdutos = produtos;
        this.setupAutocomplete();
        this.isLoadingProdutos = false;
      },
      error: (err) => {
        this.openSnackBar(`Erro ao carregar produtos: ${err.message}`, 'Erro');
        this.isLoadingProdutos = false;
      }
    });
    this.subscriptions.add(prodSub);
  }

  setupAutocomplete(): void {
    this.filteredProdutos$ = this.itemForm.get('produto')!.valueChanges.pipe(
      startWith(''),
      map(value => (typeof value === 'string' ? value : value?.descricao)),
      map(descricao => (descricao ? this._filterProdutos(descricao) : this.todosOsProdutos.slice()))
    );
  }

  private _filterProdutos(descricao: string): Produto[] {
    const filterValue = descricao.toLowerCase();
    return this.todosOsProdutos.filter(prod => prod.descricao.toLowerCase().includes(filterValue));
  }

  displayProdutoFn(produto: Produto): string {
    return produto && produto.descricao ? produto.descricao : '';
  }

  adicionarOuAtualizarItem(): void {
    if (this.itemForm.invalid) {
      if (this.itemForm.get('produto')?.hasError('produtoInvalido')) {
        this.openSnackBar('Por favor, selecione um produto válido da lista.', 'Erro');
      };
    }

    const produtoSelecionado: Produto = this.itemForm.value.produto;
    const quantidade: number = this.itemForm.value.quantidade;

    if (this.itemEmEdicao) {
      const item = this.itensDaNota.find(item => item.produtoCodigo === this.itemEmEdicao!.produtoCodigo);
      if (item) {
        item.produtoCodigo = produtoSelecionado.codigo;
        item.quantidade = quantidade;
      }
    } else {
      const itemExistente = this.itensDaNota.find(item => item.produtoCodigo === produtoSelecionado.codigo);

      if (itemExistente) {
        itemExistente.quantidade += quantidade;
      } else {
        this.itensDaNota.push({
          produtoCodigo: produtoSelecionado.codigo,
          quantidade: quantidade
        });
      }
    }

    this.dataSource.data = [...this.itensDaNota];
    this.onCancelarEdicao();
  }

  onEditarItem(item: ItemNotaDTO): void {
    this.itemEmEdicao = item;

    const produtoParaEditar = this.todosOsProdutos.find(p => p.codigo === item.produtoCodigo);
    
    if (produtoParaEditar) {
      this.itemForm.setValue({
        produto: produtoParaEditar,
        quantidade: item.quantidade
      });
    }
  }

  onCancelarEdicao(): void {
    this.itemEmEdicao = null;
    this.itemForm.reset({ quantidade: 1 });
  }

  removerItem(codigoProduto: number): void {
    this.itensDaNota = this.itensDaNota.filter(item => item.produtoCodigo !== codigoProduto);
    this.dataSource.data = [...this.itensDaNota];

    if (this.itemEmEdicao && this.itemEmEdicao.produtoCodigo === codigoProduto) {
      this.onCancelarEdicao();
    }
  }

  onSubmitNota(): void {
    if (this.itensDaNota.length === 0) {
      this.openSnackBar("A nota fiscal deve conter pelo menos um item.", 'Erro');
      return;
    }

    this.isSavingNota = true;

    const dto: CriarNotaFiscalDTO = {
      itens: this.itensDaNota
    };

    const notaSub = this.faturamentoService.criarNotaFiscal(dto).subscribe({
      next: (notaCriada) => {
        this.openSnackBar(`Nota Fiscal N° ${notaCriada.numero} criada com sucesso!`, 'Sucesso');
        this.isSavingNota = false;
        
        this.itensDaNota = [];
        this.dataSource.data = [];
        this.onCancelarEdicao();
      },
      error: (err) => {
        this.openSnackBar(`Erro ao salvar nota: ${err.message}`, 'Erro');
        this.isSavingNota = false;
      }
    });
    this.subscriptions.add(notaSub);
  }

  getDescricaoProduto(codigo: number): string {
    return this.todosOsProdutos.find(p => p.codigo === codigo)?.descricao || 'Produto não encontrado';
  }

  openSnackBar(message: string, action: string): void {
    this._snackBar.open(message, action, {
      duration: 3000,
      verticalPosition: 'top'
    });
  }
}