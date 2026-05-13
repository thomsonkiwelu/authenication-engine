#!/usr/bin/env bash
# ============================================================
# TANAPA Conservation System — Full Server Setup Script
# Ubuntu 20.04 LTS
# Run once on a fresh server: sudo bash setup-server.sh
# ============================================================
set -euo pipefail

# ============================================================
# CONFIGURATION — fill these in before running
# ============================================================
SERVER_IP_OR_DOMAIN="YOUR_SERVER_IP_OR_DOMAIN"   # e.g. 192.168.1.10 or erp.tanapa.go.tz
GITHUB_USER="YOUR_GITHUB_USERNAME"
GITHUB_TOKEN="YOUR_GITHUB_PAT_TOKEN"              # GitHub Personal Access Token

FRONTEND_REPO="https://${GITHUB_USER}:${GITHUB_TOKEN}@github.com/Birage/tanapa-conservation.git"
BACKEND_REPO="https://${GITHUB_USER}:${GITHUB_TOKEN}@github.com/Birage/conservation-backend.git"
FRONTEND_BRANCH="Dev"
BACKEND_BRANCH="dev"

FRONTEND_DIST="/var/www/tanapa"
BACKEND_INSTALL="/opt/tanapa/backend"
BACKEND_PORT=5011
BACKEND_CONFIG_DIR="/etc/tanapa"
BACKEND_DLL="conservation-backend.dll"

DB_NAME=""
DB_USER="tanapa_user"
DB_PASS="CHANGE_TO_STRONG_PASSWORD"

JWT_SECRET="CHANGE_TO_STRONG_SECRET_MIN_32_CHARS"
JWT_ISSUER="http://${SERVER_IP_OR_DOMAIN}"
JWT_AUDIENCE="http://${SERVER_IP_OR_DOMAIN}"

# ============================================================
# HELPERS
# ============================================================
log() { echo -e "\n\033[1;32m==> $*\033[0m"; }
err() { echo -e "\033[1;31mERROR: $*\033[0m" >&2; exit 1; }

[[ "$(id -u)" -eq 0 ]] || err "Run this script as root (sudo bash setup-server.sh)"
[[ "$SERVER_IP_OR_DOMAIN" == "YOUR_SERVER_IP_OR_DOMAIN" ]] && err "Set SERVER_IP_OR_DOMAIN before running"
[[ "$GITHUB_TOKEN" == "YOUR_GITHUB_PAT_TOKEN" ]] && err "Set GITHUB_TOKEN before running"
[[ "$DB_PASS" == "CHANGE_TO_STRONG_PASSWORD" ]] && err "Set DB_PASS before running"
[[ "$JWT_SECRET" == "CHANGE_TO_STRONG_SECRET_MIN_32_CHARS" ]] && err "Set JWT_SECRET before running"

# ============================================================
# SYSTEM PACKAGES
# ============================================================
log "Updating system packages..."
apt-get update -y
apt-get install -y curl wget git unzip software-properties-common apt-transport-https ca-certificates python3

# ============================================================
# NODE.JS 20 LTS
# ============================================================
log "Installing Node.js 20 LTS..."
curl -fsSL https://deb.nodesource.com/setup_20.x | bash -
apt-get install -y nodejs
node -v && npm -v

# ============================================================
# .NET 8 SDK
# ============================================================
log "Installing .NET 8 SDK..."
wget -q https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O /tmp/packages-microsoft-prod.deb
dpkg -i /tmp/packages-microsoft-prod.deb
rm /tmp/packages-microsoft-prod.deb
apt-get update -y
apt-get install -y dotnet-sdk-8.0
dotnet --version

# ============================================================
# NGINX
# ============================================================
log "Installing Nginx..."
apt-get install -y nginx
systemctl enable nginx

# ============================================================
# POSTGRESQL
# ============================================================
log "Installing PostgreSQL..."
apt-get install -y postgresql postgresql-contrib
systemctl enable postgresql
systemctl start postgresql

# ============================================================
# ANGULAR CLI
# ============================================================
log "Installing Angular CLI globally..."
npm install -g @angular/cli --silent

# ============================================================
# DIRECTORIES
# ============================================================
log "Creating directories..."
mkdir -p "${FRONTEND_DIST}" "${BACKEND_INSTALL}" "${BACKEND_CONFIG_DIR}"

# ============================================================
# CLONE BACKEND (early) — used for DB_NAME detection
# ============================================================
log "Cloning backend (branch: ${BACKEND_BRANCH})..."
rm -rf /tmp/tanapa-backend
git clone -b "${BACKEND_BRANCH}" "${BACKEND_REPO}" /tmp/tanapa-backend

if [[ -z "${DB_NAME}" ]]; then
  log "Detecting database name from backend appsettings.json..."
  BACKEND_APPSETTINGS_PATH="/tmp/tanapa-backend/appsettings.json"
  if [[ ! -f "${BACKEND_APPSETTINGS_PATH}" ]]; then
    BACKEND_APPSETTINGS_PATH="/tmp/tanapa-backend/appsettings.Development.json"
  fi
  [[ -f "${BACKEND_APPSETTINGS_PATH}" ]] || err "No backend appsettings file found to detect DB_NAME. Looked for appsettings.json and appsettings.Development.json. Set DB_NAME manually in this script."
  DB_NAME="$(python3 - "${BACKEND_APPSETTINGS_PATH}" <<'PY'
import json
import re
import sys

path = sys.argv[1]
with open(path, 'r', encoding='utf-8') as f:
    data = json.load(f)

cs = (data.get('ConnectionStrings') or {}).get('DefaultConnection') or ''
m = re.search(r'Database=([^;]+)', cs, flags=re.IGNORECASE)
if not m:
    sys.exit(2)
print(m.group(1))
PY
)"
  [[ -n "${DB_NAME}" ]] || err "Could not detect DB_NAME from backend appsettings.json. Set DB_NAME manually in this script."
fi

log "Setting up database user and database (DB_NAME=${DB_NAME})..."
sudo -u postgres psql -v ON_ERROR_STOP=1 <<SQL
DO \$\$
BEGIN
  IF NOT EXISTS (SELECT 1 FROM pg_roles WHERE rolname = '${DB_USER}') THEN
    CREATE ROLE "${DB_USER}" LOGIN PASSWORD '${DB_PASS}';
  ELSE
    ALTER ROLE "${DB_USER}" WITH PASSWORD '${DB_PASS}';
  END IF;
END\$\$;

SELECT format('CREATE DATABASE %I OWNER %I;', '${DB_NAME}', '${DB_USER}')
WHERE NOT EXISTS (SELECT 1 FROM pg_database WHERE datname='${DB_NAME}')
\gexec
GRANT ALL PRIVILEGES ON DATABASE "${DB_NAME}" TO "${DB_USER}";
SQL

# Grant schema ownership once db exists
sudo -u postgres psql -d "${DB_NAME}" -v ON_ERROR_STOP=1 <<SQL
ALTER SCHEMA public OWNER TO "${DB_USER}";
GRANT ALL ON SCHEMA public TO "${DB_USER}";
SQL

# ============================================================
# PRODUCTION CONFIG — appsettings.Production.json
# ============================================================
log "Creating appsettings.Production.json..."
cat > "${BACKEND_CONFIG_DIR}/appsettings.Production.json" <<APPSETTINGS
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=${DB_NAME};Username=${DB_USER};Password=${DB_PASS}"
  },
  "JwtSettings": {
    "SecretKey": "${JWT_SECRET}",
    "Issuer": "${JWT_ISSUER}",
    "Audience": "${JWT_AUDIENCE}",
    "AccessTokenExpireMinutes": 60
  },
  "AllowedHosts": "*",
  "Logging": {
    "LogLevel": {
      "Default": "Warning",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
APPSETTINGS
chmod 640 "${BACKEND_CONFIG_DIR}/appsettings.Production.json"

# ============================================================
# CLONE & BUILD FRONTEND
# ============================================================
log "Cloning frontend (branch: ${FRONTEND_BRANCH})..."
rm -rf /tmp/tanapa-frontend
git clone -b "${FRONTEND_BRANCH}" "${FRONTEND_REPO}" /tmp/tanapa-frontend

log "Building Angular frontend..."
cd /tmp/tanapa-frontend
npm ci --silent
npm run build
rm -rf "${FRONTEND_DIST:?}"/*
if [[ -d "dist/angular-app/browser" ]]; then
  cp -r dist/angular-app/browser/. "${FRONTEND_DIST}/"
else
  cp -r dist/angular-app/. "${FRONTEND_DIST}/"
fi

# ============================================================
# CLONE & BUILD BACKEND
# ============================================================
log "Copying production appsettings into backend..."
cp "${BACKEND_CONFIG_DIR}/appsettings.Production.json" \
   /tmp/tanapa-backend/appsettings.Production.json

log "Publishing .NET backend..."
cd /tmp/tanapa-backend
dotnet publish -c Release -o /tmp/tanapa-backend-publish

log "Deploying backend binaries..."
rm -rf "${BACKEND_INSTALL:?}"/*
cp -r /tmp/tanapa-backend-publish/. "${BACKEND_INSTALL}/"

log "Running database migrations and seeders..."
ASPNETCORE_ENVIRONMENT=Production \
  dotnet "${BACKEND_INSTALL}/${BACKEND_DLL}" --migrate-and-seed || true

# ============================================================
# SYSTEMD SERVICE
# ============================================================
log "Creating systemd service for backend..."
cat > /etc/systemd/system/tanapa-backend.service <<SERVICE
[Unit]
Description=TANAPA Conservation Backend API
After=network.target postgresql.service

[Service]
WorkingDirectory=${BACKEND_INSTALL}
ExecStart=/usr/bin/dotnet ${BACKEND_INSTALL}/${BACKEND_DLL}
Restart=always
RestartSec=10
KillSignal=SIGINT
SyslogIdentifier=tanapa-backend
User=www-data
Group=www-data
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=ASPNETCORE_URLS=http://localhost:${BACKEND_PORT}
Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=false

[Install]
WantedBy=multi-user.target
SERVICE

chown -R www-data:www-data "${BACKEND_INSTALL}" "${FRONTEND_DIST}"
systemctl daemon-reload
systemctl enable tanapa-backend
systemctl start tanapa-backend

# ============================================================
# NGINX CONFIG
# ============================================================
log "Configuring Nginx..."
cat > /etc/nginx/sites-available/tanapa <<NGINX
server {
    listen 80;
    server_name ${SERVER_IP_OR_DOMAIN};

    root ${FRONTEND_DIST};
    index index.html;

    # Angular — HTML5 pushstate routing
    location / {
        try_files \$uri \$uri/ /index.html;
    }

    # Backend API reverse proxy
    location /api/ {
        proxy_pass         http://localhost:${BACKEND_PORT}/api/;
        proxy_http_version 1.1;
        proxy_set_header   Host              \$host;
        proxy_set_header   X-Real-IP         \$remote_addr;
        proxy_set_header   X-Forwarded-For   \$proxy_add_x_forwarded_for;
        proxy_set_header   X-Forwarded-Proto \$scheme;
        proxy_read_timeout 90;
    }

    # File uploads served from backend wwwroot
    location /uploads/ {
        proxy_pass http://localhost:${BACKEND_PORT}/uploads/;
    }

    client_max_body_size 50M;
    gzip on;
    gzip_types text/plain text/css application/json application/javascript text/xml application/xml;
}
NGINX

ln -sf /etc/nginx/sites-available/tanapa /etc/nginx/sites-enabled/tanapa
rm -f /etc/nginx/sites-enabled/default
nginx -t && systemctl reload nginx

# ============================================================
# CLEANUP
# ============================================================
log "Cleaning up temp files..."
rm -rf /tmp/tanapa-frontend /tmp/tanapa-backend /tmp/tanapa-backend-publish

# ============================================================
# DONE
# ============================================================
echo ""
echo "======================================================"
echo " Setup complete!"
echo "======================================================"
echo " Frontend : http://${SERVER_IP_OR_DOMAIN}"
echo " API      : http://${SERVER_IP_OR_DOMAIN}/api"
echo ""
echo " Useful commands:"
echo "   sudo systemctl status tanapa-backend"
echo "   sudo journalctl -u tanapa-backend -f"
echo "   sudo systemctl restart tanapa-backend"
echo "======================================================"
