# 🧾 API - Gestão Financeira

API REST para controle de lançamentos financeiros (receitas e despesas) com categorização por **centros de custo**. Desenvolvida com **.NET 3.1** e integrada ao frontend Angular disponível [aqui](https://github.com/deniis8/gestaoFinanceira).

---

## 🚀 Funcionalidades

- ✅ Cadastro de lançamentos financeiros
- ✅ Cadastro de centros de custo
- ✅ Edição e exclusão de registros
- ✅ Listagem de lançamentos e centros de custo
- ✅ Consulta por ID
- ✅ Próximo jogo do Palmeiras (`GET /api/jogospalmeiras/proximo`) e sincronização automática com o Supabase

---

## ⚽ Sincronização do jogo do Palmeiras com o Supabase

Um `BackgroundService` (`SincronizacaoJogosBackgroundService`) busca o próximo jogo e faz **upsert** na tabela `public.partida_futebol` do Supabase, por padrão às **07:00, 13:00 e 19:00** (hora local do servidor) e também quando a API sobe. Em caso de falha tenta até 3 vezes (5 min entre elas); a API nunca cai por causa disso.

- Chave do upsert: `UNIQUE (jogo, data_hora)` (`on_conflict=jogo,data_hora`).
- `data_hora` é gravada com fuso de Brasília (`-03:00`); `emissoras` vai separada por `|` (ex.: `Globo|SporTV|Premiere`).
- Jogo **sem horário definido** não é gravado (evita criar um horário falso e uma linha duplicada depois).
- A escrita usa a **service_role key** (ignora o RLS). Nunca coloque essa chave no `appsettings.json` versionado nem em app cliente.

Configuração no `appsettings.json` (ignorado pelo Git). A `Url` pode ser a do projeto ou a rota completa (`.../rest/v1/partida_futebol`):

```json
"Supabase": { "Url": "https://SEU-PROJETO.supabase.co", "ServiceRoleKey": "", "TabelaPartidas": "partida_futebol" },
"SincronizacaoJogos": { "Horarios": [ "07:00", "13:00", "19:00" ], "ExecutarNaInicializacao": true }
```

A `ServiceRoleKey` (Project Settings > API > `service_role`, ou uma *secret key*) pode ser preenchida no próprio `appsettings.json` local ou, de preferência, por variável de ambiente, que tem prioridade:

```bash
# ex.: no serviço systemd da API (Environment=) ou no shell
Supabase__ServiceRoleKey=eyJ...
```

A chave `anon`/*publishable* **não serve** aqui: ela só lê (o RLS bloqueia a escrita).

Sem `Url`/`ServiceRoleKey` a sincronização fica desativada (aparece um aviso no log). Confira também o fuso do Orange Pi (`timedatectl`), pois os horários usam a hora local do servidor.

---

## ⚙️ Tecnologias Utilizadas

- [.NET 3.1](https://dotnet.microsoft.com/en-us/download/dotnet/3.1)
- [Entity Framework Core](https://learn.microsoft.com/ef/core/)
- [SQL Server](https://www.microsoft.com/pt-br/sql-server)

---

### 1. Clonar o repositório

```bash
git clone https://github.com/deniis8/ApiGestaoFinanceira.git
cd ApiGestaoFinanceira
