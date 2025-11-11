export interface Produto {
    codigo: number;
    descricao: string;
    saldo: number;
    ativo: boolean;
}

export interface CriarProdutoDTO {
    descricao: string;
    saldo: number;
}

export interface AtualizarProdutoDTO {
    descricao: string;
    saldo: number;
}