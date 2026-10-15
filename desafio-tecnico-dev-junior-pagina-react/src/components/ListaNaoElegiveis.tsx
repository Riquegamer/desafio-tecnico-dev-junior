import React from 'react';
import type { ProfessorNaoElegivel } from '../types/buscaprofessor';

interface Props {
  professores: ProfessorNaoElegivel[];
}

export const ListaNaoElegiveis: React.FC<Props> = ({ professores }) => {
  if (professores.length === 0) return null;

  return (
    <div className="space-y-3 pt-4 border-t border-slate-200">
      <h3 className="text-md font-bold text-slate-600">
        Professores Indisponíveis ({professores.length})
      </h3>

      <div className="grid grid-cols-1 md:grid-cols-2 gap-3">
        {professores.map(prof => (
          <div
            key={prof.id}
            className="bg-slate-50 p-3 rounded-lg border border-slate-200 flex justify-between items-center text-sm"
          >
            <span className="font-medium text-slate-700">{prof.nome}</span>
            <span className="text-xs bg-rose-50 text-rose-700 border border-rose-200 px-2 py-0.5 rounded font-medium">
              {prof.motivo}
            </span>
          </div>
        ))}
      </div>
    </div>
  );
};