import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { CadastroProdutoComponent } from './components/cadastro-produto/cadastro-produto.component';
import { CadastroNotaFiscalComponent } from './components/cadastro-nota-fiscal/cadastro-nota-fiscal.component';
import { ListaNotasFiscaisComponent } from './components/lista-notas-fiscais/lista-notas-fiscais.component';

import { MaterialModule } from './angular-material.module';

@NgModule({
  declarations: [
    AppComponent,
    CadastroProdutoComponent,
    CadastroNotaFiscalComponent,
    ListaNotasFiscaisComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    ReactiveFormsModule,
    FormsModule,
    MaterialModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
