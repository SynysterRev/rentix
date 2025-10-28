# Rentix

Create database with docker :
```bash
docker run --name postgres-rentix -e POSTGRES_PASSWORD=postgres -e POSTGRES_DB=rentix -p 5433:5432 -d postgres
```