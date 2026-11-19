# ProductosAPI

API REST en Visual Basic .NET 8, ASP.NET Core, Entity Framework Core 8 y SQL Server. Implementa CRUD de categorías y productos, DTOs validados, reglas de negocio, relación 1:N, Swagger y respuestas de error centralizadas.

## Requisitos y configuración

- .NET 8 SDK
- SQL Server o SQL Server Express
- Visual Studio 2022 (opcional)

1. Ejecuta `SQL/EmpresaDB.sql` en SQL Server Management Studio. Crea `EmpresaDB` y las tablas del proyecto; si las tablas ya existen, completa el esquema sin borrar las filas.
2. Ajusta `ConnectionStrings:DefaultConnection` en `ProductosAPI/appsettings.json` para tu instancia.
3. Desde `ProductosAPI/ProductosAPI`, ejecuta `dotnet run`.
4. Abre la URL indicada por la consola y agrega `/swagger`.

## Endpoints

| Método | Ruta | Resultado |
|---|---|---|
| GET | `/api/categorias` | Lista categorías |
| GET | `/api/categorias/{id}` | Categoría con sus productos |
| POST | `/api/categorias` | Registra categoría (`201`) |
| PUT | `/api/categorias/{id}` | Modifica categoría (`200`) |
| DELETE | `/api/categorias/{id}` | Elimina categoría vacía (`204`); conflicto si tiene productos (`409`) |
| GET | `/api/productos` | Lista productos con categoría |
| GET | `/api/productos/{id}` | Producto con categoría |
| GET | `/api/productos/categoria/{categoriaId}` | Productos de una categoría; `404` si categoría inexistente |
| POST | `/api/productos` | Registra producto (`201`) |
| PUT | `/api/productos/{id}` | Modifica producto (`200`) |
| DELETE | `/api/productos/{id}` | Elimina producto (`204`) |

### Ejemplos para Swagger

Crear categoría (`POST /api/categorias`):

```json
{ "nombre": "Papelería", "descripcion": "Útiles de oficina" }
```

Crear producto (`POST /api/productos`), usando el Id de categoría creado o uno existente:

```json
{ "nombre": "Cuaderno A4", "descripcion": "Tapa dura", "precio": 12.5, "stock": 30, "categoriaId": 4 }
```

Pruebas sugeridas en Swagger: consultar ambas listas y los recursos por Id; crear una categoría y un producto asociado; repetir un nombre de producto cambiando mayúsculas para verificar `409`; enviar un `categoriaId` inexistente para verificar `409`; intentar eliminar una categoría con productos para verificar `409`; modificar y eliminar un producto; enviar precio cero o stock negativo para verificar `400`. Las respuestas de error incluyen un mensaje público y nunca incluyen detalles internos.

## Validaciones y arquitectura

- Nombre de producto y categoría obligatorio, hasta 50 caracteres; descripción hasta 200.
- Precio mayor que cero; stock no negativo; categoría obligatoria y existente.
- Los servicios validan nombres duplicados sin distinguir mayúsculas/minúsculas y protegen la eliminación de categorías con productos. La base de datos también declara claves únicas y una FK con eliminación restringida.
- Los controllers dependen de servicios; los servicios usan repositorios; los repositorios acceden al `ApplicationDbContext`. DTOs separan solicitudes y respuestas.
- La relación `Categoria (1) -> Producto (N)` está configurada en `Data/ApplicationDbContext.vb`. El script SQL reconstruye el esquema sin requerir herramientas de migración.

## Evidencia de ejecución

La interfaz Swagger está disponible en `/swagger` al iniciar el proyecto. Los ejemplos anteriores describen los casos a ejecutar y los estados HTTP esperados; la captura de evidencias debe tomarse desde Swagger después de configurar una instancia SQL Server accesible.
