import React from 'react';
import type { ProfessorElegivel } from '../types/buscaprofessor';

interface Props {
  professores: ProfessorElegivel[];
}

export const ListaElegiveis: React.FC<Props> = ({ professores }) => {
  if (professores.length === 0) {
    return (
      <div className="bg-amber-50 border border-amber-200 text-amber-800 p-4 rounded-xl text-center">
        Nenhum professor disponível atende a todos os critérios e horários selecionados.
      </div>
    );
  }

  return (
    <div className="space-y-4">
      <h3 className="text-lg font-bold text-slate-800 flex items-center gap-2">
        <span className="bg-emerald-500 text-white w-7 h-7 rounded-full inline-flex items-center justify-center text-xs font-bold shadow-md shadow-emerald-500/30">
          ✓
        </span>
        Professores Disponíveis ({professores.length})
      </h3>

      <div className="grid grid-cols-1 gap-4">
        {professores.map(prof => (
          <div
            key={prof.id}
            className="neu-card p-6 rounded-2xl transition-all hover:translate-y-[-2px]"
          >
            <div className="flex justify-between items-start mb-4">
              <div>
                <h4 className="text-xl font-bold text-slate-800">{prof.nome}</h4>
                <p className="text-xs font-medium text-slate-500 mt-0.5">
                  Carga atual: <span className="text-slate-700 font-bold">{prof.aulasNaSemana} aula(s)</span> esta semana
                </p>
              </div>
              <span className="bg-emerald-50 text-emerald-700 border border-emerald-200 text-xs px-3 py-1 rounded-full font-bold">
                Elegível
              </span>
            </div>

            <div>
              <p className="text-xs font-bold text-slate-500 uppercase tracking-wider mb-2">Horários livres de início:</p>
              <div className="flex flex-wrap gap-2">
                {prof.horarios.map((h, i) => (
                  <span
                    key={i}
                    className="neu-button px-3.5 py-1.5 rounded-lg text-sm font-mono font-bold text-slate-700"
                  >
                    {h}
                  </span>
                ))}
              </div>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
};