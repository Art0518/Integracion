# Fix para Stored Procedure `sp_b_generar_factura_simple`

## ?? Problema

Los campos `estadoFactura` y `estadoReserva` están llegando vacíos en la respuesta del POST `/api/facturas/emitir`:

```json
{
  "estadoFactura": "",
  "estadoReserva": ""
}
```

## ? Solución

El SP original solo retornaba valores hardcodeados. Ahora el SP modificado:

1. **Declara variables** para los estados:
   ```sql
   DECLARE @EstadoFactura VARCHAR(50);
   DECLARE @EstadoReserva VARCHAR(50);
   ```

2. **Consulta el estado REAL** de la factura recién creada:
   ```sql
   SELECT @EstadoFactura = Estado
   FROM facturacion.Factura
   WHERE IdFactura = @IdFactura;
   ```

3. **Consulta el estado REAL** de la reserva:
   ```sql
   SELECT @EstadoReserva = Estado
   FROM reservas.Reserva
   WHERE IdReserva = @IdReserva;
   ```

4. **Devuelve los estados** en el SELECT final:
   ```sql
   SELECT
 ...
    @EstadoFactura AS EstadoFactura,
     @EstadoReserva AS EstadoReserva;
 ```

## ?? Cómo aplicar el fix

### Opción 1: Desde SQL Server Management Studio (SSMS)

1. Abre SSMS y conéctate a tu servidor:
   ```
   Server: db31553.public.databaseasp.net
   Database: db31553
 User: db31553
   Password: 0520ARTU
   ```

2. Abre el archivo `sp_b_generar_factura_simple_FIXED.sql`

3. Ejecuta el script (F5 o botón Execute)

4. Verifica que se ejecutó correctamente

### Opción 2: Desde Azure Data Studio

1. Conecta a la base de datos con las credenciales arriba

2. Abre el archivo SQL

3. Ejecuta el script

### Opción 3: Desde la terminal con sqlcmd

```bash
sqlcmd -S db31553.public.databaseasp.net -d db31553 -U db31553 -P 0520ARTU -i Scripts/sp_b_generar_factura_simple_FIXED.sql
```

## ?? Probar el cambio

Después de ejecutar el script, prueba el POST de nuevo:

```bash
POST https://factura-production-7d28.up.railway.app/api/facturas/emitir
Content-Type: application/json

{
  "idReserva": 118,
  "email": "Arturo1232123123123@gmail.com",
  "nombre": "Arturo",
  "tipoIdentificacion": "Cedula",
  "identificacion": "1750442501",
  "valor": 40
}
```

**Respuesta esperada:**
```json
{
  "idFactura": 125,
  "estadoFactura": "Emitida",  // ? Ya no vacío
  "estadoReserva": "Confirmada", // ? Ya no vacío
  ...
}
```

## ?? Columnas agregadas en el SELECT del SP

| Campo Antiguo | Campo Nuevo | Descripción |
|---|---|---|
| `'Emitida' AS Estado` | `@EstadoFactura AS EstadoFactura` | Estado REAL de `facturacion.Factura` |
| ? No existía | `@EstadoReserva AS EstadoReserva` | Estado REAL de `reservas.Reserva` |
| ? No existía | `@Email AS Correo` | Email del usuario |
| ? No existía | `@Nombre AS Nombre` | Nombre del usuario |
| ? No existía | `@TipoIdentificacion AS TipoIdentificacion` | Tipo de identificación |
| ? No existía | `@Identificacion AS Identificacion` | Número de identificación |
| `@FechaFactura AS Fecha` | `@FechaFactura AS FechaGeneracion` | Fecha (renombrada para coincidir con el DTO) |

## ?? No se requieren cambios en el código C#

El código del microservicio ya está preparado para recibir estos campos:

```csharp
EstadoFactura = row.Table.Columns.Contains("EstadoFactura") && row["EstadoFactura"] != DBNull.Value 
    ? row["EstadoFactura"].ToString() ?? string.Empty 
    : string.Empty,
EstadoReserva = row.Table.Columns.Contains("EstadoReserva") && row["EstadoReserva"] != DBNull.Value 
    ? row["EstadoReserva"].ToString() ?? string.Empty 
    : string.Empty
```

? Solo necesitas ejecutar el script SQL en la base de datos.
