export interface Cliente {
    id?:       number;
    cedula:   string;
    nombre:   string;
    telefono: string;
    edad:     number;
    seguros?:  Seguro[];
}

export interface Seguro {
    id?:     number;
    nombre: string;
    codigo: string;
    suma:   number;
    prima:  number;
    clientes?:  Cliente[];
}

export interface ClienteSeguro {
    id:           number;
    listaSeguros: number[];
}
