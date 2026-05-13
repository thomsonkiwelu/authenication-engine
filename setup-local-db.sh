#!/usr/bin/env bash
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
APPSETTINGS_PATH="${SCRIPT_DIR}/conservation-backend/appsettings.json"
PROJECT_PATH="${SCRIPT_DIR}/conservation-backend/conservation-backend.csproj"

if [[ ! -f "${APPSETTINGS_PATH}" ]]; then
  echo "appsettings.json not found at: ${APPSETTINGS_PATH}" >&2
  exit 1
fi

if ! command -v psql >/dev/null 2>&1; then
  echo "psql is required but not found in PATH." >&2
  exit 1
fi

if ! command -v dotnet >/dev/null 2>&1; then
  echo "dotnet SDK is required but not found in PATH." >&2
  exit 1
fi

CONN_STR="$(python3 - <<'PY' "${APPSETTINGS_PATH}"
import json,sys
path=sys.argv[1]
with open(path,'r',encoding='utf-8') as f:
    data=json.load(f)
print(data['ConnectionStrings']['DefaultConnection'])
PY
)"

trim() {
  local s="$1"
  s="${s#${s%%[![:space:]]*}}"
  s="${s%${s##*[![:space:]]}}"
  printf '%s' "$s"
}

HOST=""
PORT=""
DB_NAME=""
DB_USER=""
DB_PASSWORD=""

IFS=';' read -r -a parts <<< "${CONN_STR}"
for part in "${parts[@]}"; do
  part="$(trim "$part")"
  [[ -z "$part" ]] && continue
  key="${part%%=*}"
  val="${part#*=}"
  key="$(trim "$key")"
  val="$(trim "$val")"

  lower_key="$(printf '%s' "$key" | tr '[:upper:]' '[:lower:]')"

  case "${lower_key}" in
    host) HOST="$val";;
    port) PORT="$val";;
    database) DB_NAME="$val";;
    username|user\ id|user) DB_USER="$val";;
    password) DB_PASSWORD="$val";;
  esac
done

if [[ -z "${HOST}" || -z "${PORT}" || -z "${DB_NAME}" || -z "${DB_USER}" ]]; then
  echo "Could not parse connection string from appsettings.json" >&2
  echo "Value: ${CONN_STR}" >&2
  exit 1
fi

DB_NAME_LIT="${DB_NAME//\'/\'\'}"
DB_NAME_IDENT="${DB_NAME//\"/\"\"}"
DB_USER_LIT="${DB_USER//\'/\'\'}"
DB_USER_IDENT="${DB_USER//\"/\"\"}"

PG_ADMIN_USER_WAS_SET=0
if [[ -n "${PG_ADMIN_USER+x}" ]]; then
  PG_ADMIN_USER_WAS_SET=1
fi

PG_ADMIN_USER="${PG_ADMIN_USER:-$(whoami)}"
PG_ADMIN_DB="${PG_ADMIN_DB:-postgres}"

PSQL_BASE=(psql -v ON_ERROR_STOP=1 -h "${HOST}" -p "${PORT}" -U "${PG_ADMIN_USER}" -d "${PG_ADMIN_DB}")

if [[ -n "${PG_ADMIN_PASSWORD:-}" ]]; then
  export PGPASSWORD="${PG_ADMIN_PASSWORD}"
fi

set +e
PSQL_PREFLIGHT_OUTPUT="$(${PSQL_BASE[@]} -c "SELECT 1" 2>&1)"
PSQL_PREFLIGHT_STATUS=$?
set -e

if [[ ${PSQL_PREFLIGHT_STATUS} -ne 0 ]]; then
  echo "Failed to connect to PostgreSQL for admin operations." >&2
  echo "Host: ${HOST}  Port: ${PORT}  Admin DB: ${PG_ADMIN_DB}  Admin User: ${PG_ADMIN_USER}" >&2

  if echo "${PSQL_PREFLIGHT_OUTPUT}" | grep -qi "role \"${PG_ADMIN_USER}\" does not exist"; then
    echo "The admin role '${PG_ADMIN_USER}' does not exist on this PostgreSQL instance." >&2
    if [[ ${PG_ADMIN_USER_WAS_SET} -eq 1 ]]; then
      echo "You set PG_ADMIN_USER explicitly. Try using your local superuser role instead (often your macOS username):" >&2
      echo "  PG_ADMIN_USER='$(whoami)' PG_ADMIN_DB='postgres' ./setup-local-db.sh" >&2
    else
      echo "Try setting PG_ADMIN_USER to an existing superuser role (often your macOS username)." >&2
      echo "Example:" >&2
      echo "  PG_ADMIN_USER='$(whoami)' PG_ADMIN_DB='postgres' ./setup-local-db.sh" >&2
    fi
  else
    echo "psql output:" >&2
    echo "${PSQL_PREFLIGHT_OUTPUT}" >&2
    echo "If your admin user requires a password, set PG_ADMIN_PASSWORD." >&2
  fi

  exit 1
fi

if [[ "${RESET_DB:-0}" == "1" ]]; then
  "${PSQL_BASE[@]}" -c "SELECT pg_terminate_backend(pid) FROM pg_stat_activity WHERE datname='${DB_NAME_LIT}' AND pid <> pg_backend_pid();" >/dev/null
  "${PSQL_BASE[@]}" -c "DROP DATABASE IF EXISTS \"${DB_NAME_IDENT}\";" >/dev/null
fi

"${PSQL_BASE[@]}" <<SQL
DO \$\$
BEGIN
  IF NOT EXISTS (SELECT 1 FROM pg_roles WHERE rolname = '${DB_USER_LIT}') THEN
    EXECUTE format('CREATE ROLE %I LOGIN', '${DB_USER}');
  END IF;

  EXECUTE format('ALTER ROLE %I WITH PASSWORD %L', '${DB_USER}', '${DB_PASSWORD}');
END
\$\$;
SQL

DB_EXISTS="$(${PSQL_BASE[@]} -tAc "SELECT 1 FROM pg_database WHERE datname='${DB_NAME_LIT}'")"
if [[ "${DB_EXISTS}" != "1" ]]; then
  "${PSQL_BASE[@]}" -c "CREATE DATABASE \"${DB_NAME_IDENT}\" OWNER \"${DB_USER_IDENT}\";" >/dev/null
fi

"${PSQL_BASE[@]}" -c "ALTER DATABASE \"${DB_NAME_IDENT}\" OWNER TO \"${DB_USER_IDENT}\";" >/dev/null
"${PSQL_BASE[@]}" -c "GRANT ALL PRIVILEGES ON DATABASE \"${DB_NAME_IDENT}\" TO \"${DB_USER_IDENT}\";" >/dev/null

PSQL_TARGET=(psql -v ON_ERROR_STOP=1 -h "${HOST}" -p "${PORT}" -U "${PG_ADMIN_USER}" -d "${DB_NAME}")

"${PSQL_TARGET[@]}" -c "ALTER SCHEMA public OWNER TO \"${DB_USER_IDENT}\";" >/dev/null
"${PSQL_TARGET[@]}" -c "GRANT ALL ON SCHEMA public TO \"${DB_USER_IDENT}\";" >/dev/null
"${PSQL_TARGET[@]}" -c "ALTER TABLE IF EXISTS public.\"__EFMigrationsHistory\" OWNER TO \"${DB_USER_IDENT}\";" >/dev/null
"${PSQL_TARGET[@]}" -c "GRANT ALL ON TABLE public.\"__EFMigrationsHistory\" TO \"${DB_USER_IDENT}\";" >/dev/null

unset PGPASSWORD || true

ASPNETCORE_ENVIRONMENT=Development dotnet run --project "${PROJECT_PATH}" -- --migrate-and-seed

echo "Done. Database '${DB_NAME}' is ready, migrations applied, and seeders executed."
