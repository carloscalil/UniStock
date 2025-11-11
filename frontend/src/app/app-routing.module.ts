import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CadastroProdutoComponent } from './components/cadastro-produto/cadastro-produto.component';
import { CadastroNotaFiscalComponent } from './components/cadastro-nota-fiscal/cadastro-nota-fiscal.component';
import { ListaNotasFiscaisComponent } from './components/lista-notas-fiscais/lista-notas-fiscais.component';

const routes: Routes = [
  { path: 'produtos', component: CadastroProdutoComponent },
  { path: 'notas/cadastro', component: CadastroNotaFiscalComponent },
  { path: 'notas/lista', component: ListaNotasFiscaisComponent },
  { path: '', redirectTo: '/produtos', pathMatch: 'full' },
  { path: '**', redirectTo: '/produtos' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
