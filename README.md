# InventarioApiVB - API REST de Administración de Inventario

Proyecto desarrollado en **Visual Basic .NET (.NET 8)** con **ASP.NET Core Web API**, **Entity Framework Core 8**, **SQL Server** y **Swagger**, siguiendo una arquitectura en capas con inyección de dependencias.

---

## 1. Requisitos previos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- **SQL Server** (LocalDB, SQL Server Express o SQL Server Developer)
- **Visual Studio 2022** (v17.8 o superior) o **VS Code**

---

## 2. Arquitectura del proyecto

```text
InventarioApiVB/
├── InventarioApiVB.sln
├── README.md
├── Scripts/
│   └── InventarioDB.sql                # Script SQL Server de creación de BD
└── InventarioApiVB/
    ├── Controllers/                    # Endpoints REST y respuestas HTTP
    │   ├── CategoriasController.vb
    │   └── ProductosController.vb
    ├── Services/                       # Reglas de negocio y mapeo DTO <-> Entidad
    │   ├── ICategoriaService.vb
    │   ├── CategoriaService.vb
    │   ├── IProductoService.vb
    │   └── ProductoService.vb
    ├── Repositories/                   # Acceso a datos con EF Core e Include()
    │   ├── ICategoriaRepository.vb
    │   ├── CategoriaRepository.vb
    │   ├── IProductoRepository.vb
    │   └── ProductoRepository.vb
    ├── Models/                         # Entidades del dominio (1:N)
    │   ├── Categoria.vb
    │   └── Producto.vb
    ├── DTOs/                           # Objetos de transferencia y validaciones
    │   ├── CategoriaCreateDto.vb
    │   ├── CategoriaUpdateDto.vb
    │   ├── CategoriaResponseDto.vb
    │   ├── CategoriaConProductosDto.vb
    │   ├── ProductoCreateDto.vb
    │   ├── ProductoUpdateDto.vb
    │   ├── ProductoResponseDto.vb
    │   ├── ProductoSimpleDto.vb
    │   └── ErrorResponseDto.vb
    ├── Data/                           # DbContext y Fluent API
    │   └── InventarioDbContext.vb
    ├── Migrations/                     # Migraciones de Entity Framework Core 8
    ├── Exceptions/                     # Excepciones personalizadas (404, 409)
    │   ├── NotFoundException.vb
    │   └── BusinessRuleException.vb
    ├── Middleware/                     # Manejo global centralizado de errores
    │   └── ExceptionHandlingMiddleware.vb
    ├── appsettings.json
    └── Program.vb                      # Configuración de DI, EF Core, Swagger y Pipeline
```

---

## 3. Configuración de la base de datos

### Paso 1: Cadena de conexión
Edite el archivo `InventarioApiVB/appsettings.json` según su instancia de SQL Server:

```json
"ConnectionStrings": {
  "InventarioDB": "Server=localhost;Database=InventarioDB;Trusted_Connection=True;TrustServerCertificate=True;"
}
```


### Paso 2: Crear la base de datos

- **Mediante Script SQL:**
  Ejecute el archivo `Scripts/InventarioDB.sql` en **SQL Server Management Studio (SSMS)**.

---

## 4. Ejecución del proyecto

1. Abra `InventarioApiVB.sln` en Visual Studio 2022 y presione **F5** (o ejecute `dotnet run --project InventarioApiVB` desde la terminal).
2. El navegador abrirá automáticamente la interfaz de **Swagger UI** en:
   - `https://localhost:54628/swagger/index.html`

---

## 5. Endpoints disponibles y guía de pruebas en Swagger

### Categorías (`/api/categorias`)
| Método | Ruta | Descripción | Códigos HTTP |
| :--- | :--- | :--- | :--- |
| `GET` | `/api/categorias` | Consultar todas las categorías | `200` |
| `GET` | `/api/categorias/{id}` | Consultar categoría por ID | `200`, `404` |
| `GET` | `/api/categorias/{id}/productos` | Consultar categoría incluyendo sus productos | `200`, `404` |
| `POST` | `/api/categorias` | Registrar una categoría | `201`, `400`, `409` |
| `PUT` | `/api/categorias/{id}` | Modificar una categoría | `200`, `400`, `404`, `409` |
| `DELETE` | `/api/categorias/{id}` | Eliminar categoría (si no tiene productos) | `204`, `404`, `409` |

### Productos (`/api/productos`)
| Método | Ruta | Descripción | Códigos HTTP |
| :--- | :--- | :--- | :--- |
| `GET` | `/api/productos` | Consultar todos los productos con su categoría | `200` |
| `GET` | `/api/productos/{id}` | Consultar producto por ID con su categoría | `200`, `404` |
| `GET` | `/api/productos/categoria/{categoriaId}` | Consultar productos por categoría | `200`, `404` |
| `POST` | `/api/productos` | Registrar un producto | `201`, `400`, `409` |
| `PUT` | `/api/productos/{id}` | Modificar un producto | `200`, `400`, `404`, `409` |
| `DELETE` | `/api/productos/{id}` | Eliminar un producto | `204`, `404` |

### Secuencia sugerida de pruebas (Sección 8 del Examen)
1. **Registrar categoría** (`POST /api/categorias`):
   ```json
   {
     "nombre": "Electrónica",
     "descripcion": "Dispositivos electrónicos y accesorios"
   }
   ```
   -> Respuesta esperada: `201 Created`.
2. **Registrar producto asociado** (`POST /api/productos`):
   ```json
   {
     "nombre": "Teclado Mecánico RGB",
     "descripcion": "Teclado retroiluminado switches rojos",
     "precio": 85.50,
     "stock": 15,
     "categoriaId": 1
   }
   ```
   -> Respuesta esperada: `201 Created`.
3. **Consultar todos los productos con su categoría** (`GET /api/productos`) -> `200 OK`.
4. **Consultar productos por categoría** (`GET /api/productos/categoria/1`) -> `200 OK`.
5. **Intentar registrar producto duplicado** (`POST /api/productos` con `"nombre": "teclado mecánico rgb"`) -> `409 Conflict`.
6. **Intentar registrar producto con categoría inexistente** (`POST /api/productos` con `"categoriaId": 999`) -> `409 Conflict`.
7. **Intentar eliminar categoría con productos asociados** (`DELETE /api/categorias/1`) -> `409 Conflict`.
8. **Modificar producto** (`PUT /api/productos/1`) -> `200 OK`.
9. **Eliminar producto** (`DELETE /api/productos/1`) -> `204 No Content`.

10. Colección bruno para pruebas y evidencias: disponible en "Inventario API Tests"
