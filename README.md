# 🧾 Subscriptions API

API REST para la gestión de facturas y suscripciones desarrollada con .NET 9.0.

## 📦 Instalación

```bash
git clone <repository-url>
cd subscriptions-api
```

## ▶️ Ejecución

**Docker:**
```bash
docker build -t subscriptions-api .
docker run -d -p 8081:8080 --name subscriptions --env-file .env subscriptions-api
```

## 📡 Endpoints

- `GET /api/v1/invoices` - Listar facturas
- `GET /api/v1/invoices/{id}` - Obtener factura
- `POST /api/v1/invoices` - Crear factura
- `PATCH /api/v1/invoices/{id}` - Actualizar factura

**Swagger:** `http://localhost:8081/swagger`  
**Métricas:** `http://localhost:8081/metrics`

---

**Desarrollado con ❤️ usando .NET 9.0**
