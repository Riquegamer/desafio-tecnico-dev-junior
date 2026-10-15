import React, { useEffect, useState } from "react";
import type { AlunoBuscaRequest } from "../types/buscaprofessor";

interface Props {
  onBuscar: (dados: AlunoBuscaRequest) => void;
  carregando: boolean;
}

const DIAS_OPCOES = [
  { id: "segunda", label: "Segunda" },
  { id: "terca", label: "Terça" },
  { id: "quarta", label: "Quarta" },
  { id: "quinta", label: "Quinta" },
  { id: "sexta", label: "Sexta" },
  { id: "sabado", label: "Sábado" },
  { id: "domingo", label: "Domingo" },
];

export const FormBusca: React.FC<Props> = ({ onBuscar, carregando }) => {
  const [idioma, setIdioma] = useState("Espanhol");
  const [idiomasDisponiveis, setIdiomasDisponiveis] = useState<string[]>([
    "Inglês",
    "Espanhol",
  ]);
  const [carregandoIdiomas, setCarregandoIdiomas] = useState<boolean>(true);
  const [dias, setDias] = useState<string[]>(["segunda"]);
  const [inicioFaixa, setInicioFaixa] = useState("18:00");
  const [fimFaixa, setFimFaixa] = useState("20:00");
  const [duracaoMinutos, setDuracaoMinutos] = useState<number>(60);

  useEffect(() => {
    fetch("http://localhost:5080/api/professores/idiomas")
      .then((res) => {
        if (!res.ok) throw new Error("Erro ao buscar idiomas");
        return res.json();
      })
      .then((data: string[]) => {
        if (data && data.length > 0) {
          setIdiomasDisponiveis(data);
          if (!data.includes(idioma)) {
            setIdioma(data[0]);
          }
        }
        setCarregandoIdiomas(false);
      })
      .catch((err) => {
        console.error(
          "Falha ao carregar idiomas do servidor, usando fallback:",
          err,
        );
        setCarregandoIdiomas(false);
      });
  }, []);

  const toggleDia = (diaId: string) => {
    setDias((prev) =>
      prev.includes(diaId) ? prev.filter((d) => d !== diaId) : [...prev, diaId],
    );
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    onBuscar({
      idioma,
      dias,
      inicioFaixa,
      fimFaixa,
      duracaoMinutos: Number(duracaoMinutos),
    });
  };

  return (
    <form
      onSubmit={handleSubmit}
      className="neu-card p-8 rounded-2xl space-y-6"
    >
      <h2 className="text-xl font-bold text-slate-800 pb-3">
        Filtros de Busca de Aula
      </h2>

      {/* Idioma e Duração */}
      <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
        <div>
          <label className="block text-sm font-semibold text-slate-700 mb-2">
            Idioma Desejado
          </label>
          <select
            value={idioma}
            onChange={(e) => setIdioma(e.target.value)}
            disabled={carregandoIdiomas}
            className="w-full px-4 py-3 rounded-xl neu-input focus:outline-none focus:ring-2 focus:ring-blue-400 text-slate-700 font-medium bg-white disabled:opacity-50"
          >
            {carregandoIdiomas ? (
              <option value="">Carregando idiomas...</option>
            ) : (
              idiomasDisponiveis.map((item) => (
                <option key={item} value={item}>
                  {item}
                </option>
              ))
            )}
          </select>
        </div>

        <div>
          <label className="block text-sm font-semibold text-slate-700 mb-2">
            Duração da Aula
          </label>
          <select
            value={duracaoMinutos}
            onChange={(e) => setDuracaoMinutos(Number(e.target.value))}
            className="w-full px-4 py-3 rounded-xl neu-input focus:outline-none focus:ring-2 focus:ring-blue-400 text-slate-700 font-medium bg-white"
          >
            <option value={30}>30 minutos</option>
            <option value={60}>60 minutos (1 hora)</option>
            <option value={90}>90 minutos (1h 30m)</option>
          </select>
        </div>
      </div>

      {/* Dias da Semana */}
      <div>
        <label className="block text-sm font-semibold text-slate-700 mb-3">
          Dias Preferidos (Horário Fixo)
        </label>
        <div className="flex flex-wrap gap-3">
          {DIAS_OPCOES.map((dia) => {
            const selecionado = dias.includes(dia.id);
            return (
              <button
                type="button"
                key={dia.id}
                onClick={() => toggleDia(dia.id)}
                className={`px-4 py-2 rounded-xl text-sm font-semibold transition-all ${
                  selecionado
                    ? "neu-button-pressed text-blue-600 border border-blue-200"
                    : "neu-button text-slate-600 hover:text-slate-900"
                }`}
              >
                {dia.label}
              </button>
            );
          })}
        </div>
      </div>

      {/* Faixa de Horário */}
      <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
        <div>
          <label className="block text-sm font-semibold text-slate-700 mb-2">
            Início da Faixa
          </label>
          <input
            type="time"
            value={inicioFaixa}
            onChange={(e) => setInicioFaixa(e.target.value)}
            className="w-full px-4 py-3 rounded-xl neu-input focus:outline-none focus:ring-2 focus:ring-blue-400 text-slate-700 font-medium"
            required
          />
        </div>

        <div>
          <label className="block text-sm font-semibold text-slate-700 mb-2">
            Fim da Faixa
          </label>
          <input
            type="time"
            value={fimFaixa}
            onChange={(e) => setFimFaixa(e.target.value)}
            className="w-full px-4 py-3 rounded-xl neu-input focus:outline-none focus:ring-2 focus:ring-blue-400 text-slate-700 font-medium"
            required
          />
        </div>
      </div>

      <button
        type="submit"
        disabled={carregando}
        className="w-full neu-button hover:bg-blue-700 text-blue-700 font-bold py-3.5 rounded-xl transition-all disabled:opacity-50 shadow-lg shadow-blue-500/20"
      >
        {carregando ? "Buscando Professores..." : "Buscar Horários Disponíveis"}
      </button>
    </form>
  );
};
