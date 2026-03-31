import { useEffect, useState } from 'react'
import type { RelatorioMedias } from './types'

const API_URL = '/api/seguro/relatorio/medias'

const brl = (value: number) =>
  value.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })

const pct = (value: number) =>
  `${value.toLocaleString('pt-BR', { minimumFractionDigits: 2 })}%`

interface CardProps {
  label: string
  value: string
  sub?: string
}

function Card({ label, value, sub }: CardProps) {
  return (
    <div className="bg-white rounded-2xl shadow p-6 flex flex-col gap-1 border border-slate-100">
      <span className="text-sm text-slate-500 font-medium">{label}</span>
      <span className="text-2xl font-bold text-slate-800">{value}</span>
      {sub && <span className="text-xs text-slate-400">{sub}</span>}
    </div>
  )
}

export default function App() {
  const [dados, setDados] = useState<RelatorioMedias | null>(null)
  const [carregando, setCarregando] = useState(true)
  const [erro, setErro] = useState<string | null>(null)

  const carregar = () => {
    setCarregando(true)
    setErro(null)
    fetch(API_URL)
      .then(r => {
        if (!r.ok) throw new Error(`HTTP ${r.status}`)
        return r.json() as Promise<RelatorioMedias>
      })
      .then(setDados)
      .catch((e: Error) => setErro(e.message))
      .finally(() => setCarregando(false))
  }

  useEffect(() => { carregar() }, [])

  return (
    <div className="min-h-screen bg-slate-50 flex flex-col">
      {/* Header */}
      <header className="bg-blue-700 text-white py-6 px-8 shadow">
        <div className="max-w-5xl mx-auto flex items-center justify-between">
          <div>
            <h1 className="text-2xl font-bold tracking-tight">🚗 Seguro de Veículos</h1>
            <p className="text-blue-200 text-sm mt-1">Relatório de Médias Aritméticas</p>
          </div>
          <button
            onClick={carregar}
            className="bg-white text-blue-700 font-semibold px-4 py-2 rounded-lg text-sm hover:bg-blue-50 transition"
          >
            ↺ Atualizar
          </button>
        </div>
      </header>

      {/* Content */}
      <main className="flex-1 max-w-5xl mx-auto w-full px-6 py-10">
        {carregando && (
          <div className="flex justify-center items-center py-24">
            <div className="animate-spin rounded-full h-12 w-12 border-4 border-blue-600 border-t-transparent" />
          </div>
        )}

        {erro && !carregando && (
          <div className="bg-red-50 border border-red-200 rounded-xl p-6 text-center">
            <p className="text-red-600 font-semibold text-lg">⚠️ Erro ao carregar dados</p>
            <p className="text-red-400 text-sm mt-1">{erro}</p>
            <button
              onClick={carregar}
              className="mt-4 bg-red-600 text-white px-4 py-2 rounded-lg text-sm hover:bg-red-700 transition"
            >
              Tentar novamente
            </button>
          </div>
        )}

        {dados && !carregando && (
          <>
            <div className="mb-8">
              <div className="inline-flex items-center gap-2 bg-blue-100 text-blue-700 px-4 py-2 rounded-full text-sm font-semibold">
                📊 Total de seguros registrados: <span className="text-lg font-bold">{dados.totalSeguros}</span>
              </div>
            </div>

            {dados.totalSeguros === 0 ? (
              <div className="bg-white rounded-2xl shadow p-12 text-center border border-slate-100">
                <p className="text-slate-400 text-lg">Nenhum seguro registrado ainda.</p>
                <p className="text-slate-300 text-sm mt-2">Use a API para cadastrar seguros.</p>
              </div>
            ) : (
              <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-5">
                <Card label="Média Valor do Veículo" value={brl(dados.mediaValorVeiculo)} sub="Média aritmética dos valores" />
                <Card label="Média Taxa de Risco" value={pct(dados.mediaTaxaRisco)} sub="Constante didática: 2,5%" />
                <Card label="Média Prêmio de Risco" value={brl(dados.mediaPremioRisco)} sub="TaxaRisco × ValorVeículo" />
                <Card label="Média Prêmio Puro" value={brl(dados.mediaPremioPuro)} sub="PrêmioRisco × (1 + 3%)" />
                <Card label="Média Prêmio Comercial" value={brl(dados.mediaPremioComercial)} sub="PrêmioPuro × (1 + 5%) — Valor do Seguro" />
              </div>
            )}

            <div className="mt-8 bg-blue-50 border border-blue-100 rounded-xl p-4 text-xs text-blue-600">
              <strong>Legenda do cálculo:</strong> Taxa de Risco = (VV × 5) / (2 × VV) = 2,5% &nbsp;|&nbsp;
              Margem de Segurança = 3% &nbsp;|&nbsp; Lucro = 5%
            </div>
          </>
        )}
      </main>

      <footer className="text-center text-xs text-slate-400 py-4">
        Seguro Veículos © {new Date().getFullYear()} — Clean Architecture · .NET 10 · React · TailwindCSS
      </footer>
    </div>
  )
}

