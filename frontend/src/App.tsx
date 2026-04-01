import { useEffect, useState } from 'react'
import type { RelatorioMedias } from './types'

const API_URL = import.meta.env.VITE_API_URL
  ? `${import.meta.env.VITE_API_URL}/api/seguro/relatorio/medias`
  : '/api/seguro/relatorio/medias'

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
    const delay = new Promise<void>(resolve => setTimeout(resolve, 2000))
    const request = fetch(API_URL)
      .then(r => {
        if (!r.ok) throw new Error(`HTTP ${r.status}`)
        return r.json() as Promise<RelatorioMedias>
      })
    Promise.all([request, delay])
      .then(([data]) => setDados(data))
      .catch((e: Error) => setErro(e.message))
      .finally(() => setCarregando(false))
  }

  useEffect(() => { carregar() }, [])

  return (
    <div className="min-h-screen bg-slate-50 flex flex-col">
      {/* Header */}
      <header
        className="relative text-white py-10 px-8 shadow-lg overflow-hidden"
        style={{ backgroundImage: "url('/seguro_banner_IA.png')", backgroundSize: 'cover', backgroundPosition: 'center' }}
      >
        <div className="absolute inset-0 bg-black/50" />
        <div className="relative max-w-5xl mx-auto flex items-center justify-between">
          <div>
            <h1 className="text-3xl font-bold tracking-tight drop-shadow">🚗 Seguro de Veículos</h1>
            <p className="text-blue-200 text-sm mt-1 drop-shadow">Relatório de Médias Aritméticas</p>
          </div>
          <button
            onClick={carregar}
            disabled={carregando}
            className="flex items-center gap-2 bg-white/90 text-blue-700 font-semibold px-5 py-2.5 rounded-xl text-sm shadow hover:bg-white hover:scale-105 active:scale-95 transition-all duration-200 disabled:opacity-60 disabled:cursor-not-allowed"
          >
            {carregando ? (
              <svg className="animate-spin h-4 w-4 text-blue-700" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
                <circle className="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="4" />
                <path className="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z" />
              </svg>
            ) : (
              <svg className="h-4 w-4" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor" strokeWidth={2}>
                <path strokeLinecap="round" strokeLinejoin="round" d="M4 4v5h.582m15.356 2A8.001 8.001 0 004.582 9m0 0H9m11 11v-5h-.581m0 0a8.003 8.003 0 01-15.357-2m15.357 2H15" />
              </svg>
            )}
            {carregando ? 'Carregando…' : 'Atualizar'}
          </button>
        </div>
      </header>

      {/* Content */}
      <main className="flex-1 max-w-5xl mx-auto w-full px-6 py-10">
        {carregando && !dados && (
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
          </>
        )}
      </main>

      <footer className="text-center text-xs text-slate-400 py-4">
        Seguro Veículos © {new Date().getFullYear()} — Clean Architecture · .NET 10 · React · TailwindCSS
      </footer>
    </div>
  )
}

