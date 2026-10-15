import { useState } from 'react';
import { FormBusca } from './components/FormBusca';
import { ListaElegiveis } from './components/ListaElegiveis';
import { ListaNaoElegiveis } from './components/ListaNaoElegiveis';
import type { AlunoBuscaRequest, BuscaProfessorResponse } from './types/buscaprofessor';

export default function App() {
  const [resultado, setResultado] = useState<BuscaProfessorResponse | null>(null);
  const [carregando, setCarregando] = useState(false);
  const [erro, setErro] = useState<string | null>(null);

  const handleBuscar = async (dados: AlunoBuscaRequest) => {
    setCarregando(true);
    setErro(null);

    try {
      // Aponta para a API C# local
      const response = await fetch('http://localhost:5080/api/professores/busca', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(dados),
      });

      if (!response.ok) {
        const errorData = await response.json();
        throw new Error(errorData.message || 'Erro de validação ou resposta da API.');
      }

      const data: BuscaProfessorResponse = await response.json();
      setResultado(data);
    } catch (err: any) {
      setErro(err.message || 'Falha ao conectar com a API. Verifique se a API C# está rodando.');
    } finally {
      setCarregando(false);
    }
  };

  return (
<div className="min-h-screen bg-white py-12 px-4">
      <div className="max-w-3xl mx-auto space-y-10">
        <header className="text-center space-y-2">
          <h1 className="text-4xl font-extrabold text-slate-900 tracking-tight">Escola de Idiomas</h1>
          <p className="text-slate-500 font-medium">Sistema Inteligente de Alocação de Professores</p>
        </header>

        <FormBusca onBuscar={handleBuscar} carregando={carregando} />

        {erro && (
          <div className="bg-red-50 border border-red-200 text-red-700 px-4 py-3 rounded-lg text-sm">
            <strong>Erro:</strong> {erro}
          </div>
        )}

        {resultado && (
          <div className="space-y-6">
            <ListaElegiveis professores={resultado.professores} />
            <ListaNaoElegiveis professores={resultado.naoElegiveis} />
          </div>
        )}
      </div>
    </div>
  );
}