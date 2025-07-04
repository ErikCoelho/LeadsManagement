#!/bin/bash

echo "🚀 Lead Management - Back-end (API + Database)"
echo "============================================"

echo "📋 Verificando pré-requisitos..."
if ! command -v docker &> /dev/null; then
    echo "❌ Docker não encontrado. Instale o Docker primeiro."
    exit 1
fi

echo "✅ Docker encontrado!"
echo

echo "🛑 Parando containers anteriores..."
docker-compose down > /dev/null 2>&1

echo
echo "🐳 Construindo e iniciando Back-end..."
echo "  - SQL Server Database"
echo "  - .NET API"

docker-compose build --no-cache
docker-compose up -d

echo
echo "⏳ Aguardando serviços inicializarem..."
sleep 30

echo
echo "✅ Back-end iniciado com sucesso!"
echo
echo "🌐 Acessos disponíveis:"
echo "  API:      http://localhost:5000"
echo "  Swagger:  http://localhost:5000/swagger"
echo "  Health:   http://localhost:5000/health"
echo "  Database: localhost:1433 (sa/LeadManagement123!)"
echo
echo "📊 Para monitorar:"
echo "  docker-compose logs -f"
echo "  docker-compose ps"
echo
echo "🛑 Para parar:"
echo "  docker-compose down"
echo
echo "💡 Para conectar o front-end, use:"
echo "  API_URL=http://localhost:5000" 