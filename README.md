# TaskTracker

Набор лабораторных работ (Tasks) по дисциплине “Основы новых информационных технологий” специальности "Информатика и вычислительная техника", МГТУ "СТАНКИН".

Стек: C#/.NET 10, ASP.NET Core (Razor Pages), EF Core (ORM), PostgreSQL, Docker, GitHub Actions, Nginx (round-robin load balancer).

---

## Содержимое

* **Task 1**: `TaskTracker.Web` — веб-приложение “Task Tracker” + EF Core ORM + PostgreSQL,
* **Task 2**: `docker-compose.lab2.yml`, `TaskTracker.Web/Dockerfile`, `.env` — контейнеризация (app и db отдельно),
* **Task 3**: `.github/workflows/ci.yml`, `TaskTracker.Web.Tests` — CI/CD через GitHub Actions + функциональный тест,
* **Task 4**: `docker-compose.lab4.yml`, `NodeInfo.Web`, `nginx/nginx.conf` — 3 ноды + Nginx round-robin, переключение "Нода 1/2/3".

---

## Task 1: запуск приложения локально (без контейнеров)

> Требуется запущенная PostgreSQL (можно и через Docker, см. ниже).

Запуск из Visual Studio: открыть решение и нажать **Run**.

Запуск через CLI:

```powershell
cd TaskTracker.Web
dotnet run --urls "http://127.0.0.1:5005"
```

Ссылки:

* UI: `http://127.0.0.1:5005/Tasks`
* Health: `http://127.0.0.1:5005/health`

---

## Task 2: запуск системы в Docker (Web + DB)

### 1) Создать `.env` (в корне репозитория)

Файл **.env** (не коммитится, должен быть в `.gitignore`):

```env
DB_NAME=tasktracker
DB_USER=taskuser
DB_PASSWORD=<ПАРОЛЬ>
DB_PORT=5432
DB_HOST=db

APP_PORT=8080
```

### 2) Запуск

```powershell
cd .
docker compose -f docker-compose.lab2.yml --env-file .env up -d --build
docker ps
```

Ссылки:

* UI: `http://127.0.0.1:8080/Tasks`
* Health: `http://127.0.0.1:8080/health`

Остановка:

```powershell
docker compose -f docker-compose.lab2.yml --env-file .env down
```

Остановка с удалением данных БД:

```powershell
docker compose -f docker-compose.lab2.yml --env-file .env down -v
```

---

## Task 3: CI/CD (GitHub Actions)

Workflow: `.github/workflows/ci.yml`

В CI выполняются:

* `dotnet build`
* `dotnet test` (функциональный тест `/health`)
* `docker compose up` + проверки `curl` для `/health` и `/Tasks`

Переменные среды из Task 2 вынесены в GitHub **Secrets/Variables**.

---

## Task 4: Nginx load balancer + 3 ноды (round-robin)

Композиция:

* `web1`, `web2`, `web3` — экземпляры `NodeInfo.Web` с разным `NODE_ID`
* `nginx` — балансировщик нагрузки (round-robin upstream)

Запуск:

```powershell
cd .
docker compose -p lab4 -f docker-compose.lab4.yml up -d --build
docker ps
```

Ссылка:

* `http://127.0.0.1:8081/`
  При обновлении страницы должна меняться надпись: **Нода 1 / Нода 2 / Нода 3**.

Остановка:

```powershell
docker compose -p lab4 -f docker-compose.lab4.yml down
```
