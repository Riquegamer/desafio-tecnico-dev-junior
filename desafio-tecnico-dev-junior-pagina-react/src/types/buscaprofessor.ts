export interface AlunoBuscaRequest {
  idioma: string;
  dias: string[];
  inicioFaixa: string;
  fimFaixa: string;
  duracaoMinutos: number;
}

export interface ProfessorElegivel {
  id: number;
  nome: string;
  aulasNaSemana: number;
  horarios: string[];
}

export interface ProfessorNaoElegivel {
  id: number;
  nome: string;
  motivo: string;
}

export interface BuscaProfessorResponse {
  professores: ProfessorElegivel[];
  naoElegiveis: ProfessorNaoElegivel[];
}