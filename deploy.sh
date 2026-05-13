#!/usr/bin/env bash
# ============================================================
# TANAPA Conservation System — Re-deploy Script
# Run after setup-server.sh for subsequent deployments
# Usage: sudo bash deploy.sh [frontend|backend|both]
# Default: both
# ============================================================
set -euo pipefail

# ============================================================
# CONFIGURATION — must match setup-server.sh
# ============================================================
GITHUB_USER="YOUR_GITHUB_USERNAME"
GITHUB_TOKEN="YOUR_GITHUB_PAT_TOKEN"

FRONTEND_REPO="https://${GITHUB_USER}:${GITHUB_TOKEN}@github.com/Birage/tanapa-conservation.git"
BACKEND_REPO="https://${GITHUB_USER}:${GITHUB_TOKEN}@github.com/Birage/conservation-backend.git"
FRONTEND_BRANCH="Dev"
BACKEND_BRANCH="dev"

FRONTEND_DIST="/var/www/tanapa"
BACKEND_INSTALL="/opt/tanapa/backend"
BACKEND_CONFIG_DIR="/etc/tanapa"
BACKEND_DLL="conservation-backend.dll"

# ============================================================
# HELPERS
# ============================================================
log()  { echo -e "\n\033[1;32m==> $*\033[0m"; }
info() { echo -e "\033[0;36m    $*\033[0m"; }
err()  { echo -e "\033[1;31mERROR: $*\033[0m" >&2; exit 1; }

[[ "$(id -u)" -eq 0 ]] || err "Run as root: sudo bash deploy.sh"
[[ "$GITHUB_TOKEN" == "YOUR_GITHUB_PAT_TOKEN" ]] && err "Set GITHUB_TOKEN before running"

TARGET="${1:-both}"

deploy_frontend() {
    log "Deploying frontend (branch: ${FRONTEND_BRANCH})..."
    rm -rf /tmp/tanapa-frontend
    git clone -b "${FRONTEND_BRANCH}" "${FRONTEND_REPO}" /tmp/tanapa-frontend
    cd /tmp/tanapa-frontend
    npm ci --silent
    npm run build
    info "Replacing frontend files..."
    rm -rf "${FRONTEND_DIST:?}"/*
    cp -r dist/angular-app/browser/. "${FRONTEND_DIST}/"
    chown -R www-data:www-data "${FRONTEND_DIST}"
    rm -rf /tmp/tanapa-frontend
    info "Frontend deployed successfully."
}

deploy_backend() {
    log "Deploying backend (branch: ${BACKEND_BRANCH})..."

    [[ -f "${BACKEND_CONFIG_DIR}/appsettings.Production.json" ]] || \
        err "Production config not found: ${BACKEND_CONFIG_DIR}/appsettings.Production.json"

    rm -rf /tmp/tanapa-backend /tmp/tanapa-backend-publish
    git clone -b "${BACKEND_BRANCH}" "${BACKEND_REPO}" /tmp/tanapa-backend

    info "Copying production config..."
    cp "${BACKEND_CONFIG_DIR}/appsettings.Production.json" \
       /tmp/tanapa-backend/conservation-backend/appsettings.Production.json

    info "Publishing .NET backend..."
    cd /tmp/tanapa-backend/conservation-backend
    dotnet publish -c Release -o /tmp/tanapa-backend-publish

    info "Stopping backend service..."
    systemctl stop tanapa-backend

    info "Replacing backend binaries..."
    rm -rf "${BACKEND_INSTALL:?}"/*
    cp -r /tmp/tanapa-backend-publish/. "${BACKEND_INSTALL}/"
    chown -R www-data:www-data "${BACKEND_INSTALL}"

    info "Running database migrations..."
    ASPNETCORE_ENVIRONMENT=Production \
        dotnet "${BACKEND_INSTALL}/${BACKEND_DLL}" --migrate-and-seed || true

    info "Starting backend service..."
    systemctl start tanapa-backend
    sleep 2
    systemctl is-active --quiet tanapa-backend && info "Backend is running." || err "Backend failed to start — check: journalctl -u tanapa-backend -n 50"

    rm -rf /tmp/tanapa-backend /tmp/tanapa-backend-publish
}

# ============================================================
# EXECUTE
# ============================================================
case "$TARGET" in
    frontend) deploy_frontend ;;
    backend)  deploy_backend  ;;
    both)     deploy_frontend; deploy_backend ;;
    *)        err "Usage: sudo bash deploy.sh [frontend|backend|both]" ;;
esac

echo ""
echo "======================================================"
echo " Deployment complete! ($TARGET)"
echo "======================================================"
systemctl status tanapa-backend --no-pager || true
