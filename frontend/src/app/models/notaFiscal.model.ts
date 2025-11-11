export interface CriarNotaFiscalDTO {
    itens: ItemNotaDTO[];
}

export interface ItemNotaDTO {
    produtoCodigo: number;
    quantidade: number;
}

export interface NotaFiscal {
    numero: number;
    status: string;
    itens: ItemNotaFiscalDTO[];
}

export interface ItemNotaFiscalDTO {
    produtoCodigo: number;
    quantidade: number;
}