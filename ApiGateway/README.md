# ApiGateway - Café San Juan

Gateway unificado para los microservicios del sistema Café San Juan.

## ?? Características

- **Proxy reverso** para 4 microservicios independientes
- **Swagger UI** organizado por servicios (Usuario, Factura, Busqueda, Reserva)
- **CORS habilitado** para permitir peticiones desde cualquier origen
- **Logging integrado** para debugging
- **Health checks** para monitoreo

## ?? Microservicios Integrados

| Servicio | URL Base | Endpoints |
|----------|----------|-----------|
| **Factura** | https://factura-production-7d28.up.railway.app/ | 3 endpoints |
| **Usuario** | https://usuario-production1.up.railway.app/ | 2 endpoints |
| **Reserva** | https://reservas-production1.up.railway.app/ | 5 endpoints |
| **Búsqueda** | https://busqueda-production-70a5.up.railway.app/ | 1 endpoint |

## ??? Endpoints Disponibles

### Gateway Info
- `GET /api/info` - Información del gateway y servicios
- `GET /api/health` - Health check

### Usuario
- `POST /api/usuarios/registrar` - Registrar nuevo usuario
- `GET /api/usuarios/listar` - Listar usuarios con paginación

### Factura
- `POST /api/facturas/emitir` - Emitir factura
- `GET /api/facturas/{idReserva}` - Obtener factura
- `GET /api/facturas/{idReserva}/pdf` - Obtener PDF de factura

### Búsqueda
- `GET /api/mesas/buscar` - Buscar mesas disponibles

### Reserva
- `POST /api/reservas/hold` - Crear pre-reserva
- `POST /api/reservas/confirmar` - Confirmar reserva
- `GET /api/reservas/{idReserva}` - Obtener detalles de reserva
- `PUT /api/reservas/cancelar/{idReserva}` - Cancelar reserva
- `POST /api/reservas/disponibilidad` - Validar disponibilidad

## ?? Ejecutar Localmente

```bash
cd ApiGateway
dotnet run
```

El gateway estará disponible en: `http://localhost:8080`

## ?? Documentación Swagger

Una vez ejecutado, accede a Swagger UI en la raíz:
```
http://localhost:8080
```

La documentación está organizada por tags para facilitar la navegación:
- ??? **Gateway** - Información del sistema
- ??? **Usuario** - Gestión de usuarios
- ??? **Factura** - Facturación
- ??? **Busqueda** - Búsqueda de mesas
- ??? **Reserva** - Gestión de reservas

## ?? Desplegar en Railway

1. El proyecto ya está configurado para Railway con puerto dinámico
2. Crear nuevo proyecto en Railway
3. Conectar repositorio
4. Railway detectará automáticamente .NET 8
5. El gateway se desplegará automáticamente

## ?? Configuración

Editar `appsettings.json` para modificar URLs de microservicios o timeouts:

```json
{
  "Microservices": {
    "Factura": {
      "BaseUrl": "https://factura-production-7d28.up.railway.app/",
      "Timeout": 30
    }
 // ...
  }
}
```

## ?? Notas Importantes

- ? **Todos los endpoints** redirigen a los microservicios desplegados en Railway
- ? **No hay lógica de negocio** en el gateway - solo proxy
- ? **Las respuestas** son las mismas que retornan los microservicios originales
- ? **Los códigos de estado HTTP** se preservan
- ? **Swagger** funciona de manera unificada

## ?? Ejemplo de Uso

### Registrar Usuario
```bash
curl -X POST http://localhost:8080/api/usuarios/registrar \
  -H "Content-Type: application/json" \
  -d '{
    "nombre": "Juan",
 "apellido": "Pérez",
"email": "juan@email.com",
    "tipo_identificacion": "CEDULA",
    "identificacion": "1234567890"
  }'
```

### Buscar Mesas
```bash
curl http://localhost:8080/api/mesas/buscar
```

### Crear Pre-Reserva
```bash
curl -X POST http://localhost:8080/api/reservas/hold \
  -H "Content-Type: application/json" \
  -d '{
    "id_mesa": "1",
    "fecha": "2024-12-25T19:00:00",
    "numero_clientes": 4,
    "duracionHoldSegundos": 600
  }'
```

## ?? Ventajas del ApiGateway

1. **Punto único de entrada** - Los clientes solo necesitan conocer una URL
2. **Documentación centralizada** - Swagger unificado para todos los servicios
3. **Simplificación de CORS** - Configurado una sola vez
4. **Monitoreo centralizado** - Logs en un solo lugar
5. **Versionado simplificado** - Fácil mantener versiones de API
6. **Seguridad** - Puede agregar autenticación/autorización centralizada

## ????? Autor

Desarrollado para el sistema de reservas Café San Juan

## ?? Licencia

Proyecto académico - Universidad
