@echo off
echo 🚀 Lead Management - Back-end (API + Database)
echo ============================================

echo 📋 Verificando pré-requisitos...
docker --version >nul 2>&1
if %errorlevel% neq 0 (
    echo ❌ Docker não encontrado. Instale o Docker Desktop primeiro.
    pause
    exit /b 1
)

echo ✅ Docker encontrado!
echo.

echo 🛑 Parando containers anteriores...
docker-compose down >nul 2>&1

echo.
echo 🐳 Construindo e iniciando Back-end...
echo   - SQL Server Database
echo   - .NET API

docker-compose build --no-cache
docker-compose up -d

echo.
echo ⏳ Aguardando serviços inicializarem...
timeout /t 30 /nobreak >nul

echo.
echo ✅ Back-end iniciado com sucesso!
echo.
echo 🌐 Acessos disponíveis:
echo   API:      http://localhost:5000
echo   Swagger:  http://localhost:5000/swagger
echo   Health:   http://localhost:5000/health
echo   Database: localhost:1434 (sa/LeadManagement123!)
echo.
echo 📊 Para monitorar:
echo   docker-compose logs -f
echo   docker-compose ps
echo.
echo 🛑 Para parar:
echo   docker-compose down
echo.
echo 💡 Para conectar o front-end, use:
echo   API_URL=http://localhost:5000
echo.
pause 