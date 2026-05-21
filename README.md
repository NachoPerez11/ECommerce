# E-Commerce API — Arquitectura Limpia (.NET 8)
Este proyecto consiste en el desarrollo de una API REST para un
sistema de E-Commerce, construida bajo los lineamientos de la
**Arquitectura Limpia (Clean Architecture)** y utilizando **Entity

Framework Core** con **SQLite** para la persistencia de datos.
Desarrollado para la asignatura **Backend (Optativa II)** de la
Tecnicatura Universitaria en Desarrollo de Software de la
Universidad Católica de Cuyo (UCC).
---
## ️ Estructura de la Solución
El proyecto está dividido estrictamente en capas independientes
para aislar la lógica del negocio de los componentes de
infraestructura:
- **`ECommerce.Domain`**: Capa central que contiene las entidades
puras del dominio (`Product`, `User`, `Order`, `OrderItem`), enums
y reglas de negocio, aislada de cualquier framework o dependencia
externa.
- **`ECommerce.Application`**: Define los contratos e interfaces
(`IProductRepository`, `IUserRepository`, `IOrderRepository`) que
regulan las operaciones de los servicios del sistema.
- **`ECommerce.Infrastructure`**: Implementa el acceso a datos
mediante el `ApplicationDbContext`, configuraciones explícitas de
tablas con **Fluent API** y los repositorios concretos encargados
de persistir los datos.
- **`ECommerce.Api`**: Punto de entrada de la aplicación, contiene
los DTOs, los controladores expuestos (`Products`, `Users`,
`Orders`) y la configuración de la inyección de dependencias
general.
---
## 🚀 Tecnologías y Herramientas Utilizadas
- **SDK:** .NET 8.0
- **ORM:** Entity Framework Core 8.0.*
- **Motor de Base de Datos:** SQLite
- **Documentación de Endpoints:** Swagger UI nativo de .NET 8
- **IDE:** Visual Studio Code
---
## 🚀 Requisitos e Instalación

Siga estos pasos para clonar, configurar y ejecutar el proyecto
localmente en su entorno:
### 1. Clonar el repositorio
```bash
git clone <URL_DE_TU_REPOSITORIO_GITHUB>
cd ECommerce
```

### 2. Restaurar dependencias de NuGet
```bash
dotnet restore
```

### 3. Ejecutar las Migraciones (Creación de la Base de Datos)
El historial de migraciones se encuentra incluido en la capa de
Infraestructura. Ejecute el siguiente comando desde la raíz del
proyecto para generar automáticamente el archivo físico de la base
de datos `ecommerce.db` con sus respectivas tablas indexadas:
```powershell
dotnet ef database update --project ECommerce.Infrastructure
--startup-project ECommerce.Api
```

*Nota: Asegúrese de contar con la herramienta global `dotnet-ef`
instalada en su sistema en su versión compatible con .NET 8.*
### 4. Compilar y arrancar la API
```bash
dotnet run --project ECommerce.Api
```

---
## 💡 Uso e Interacción con la API
Una vez iniciado el servidor local, la interfaz interactiva de
**Swagger UI** estará disponible en su navegador en la siguiente
URL de desarrollo por defecto:
🔗

**[https://localhost:7123/swagger/index.html](https://localhost:712
3/swagger/index.html)** *(Nota: Verifique el puerto asignado en su
terminal si la URL local varía).*
### Flujo de Pruebas Recomendado en Swagger:
1. **Crear un Usuario:** Diríjase al endpoint `POST
/api/Users/register` para registrar una nueva cuenta y copie el
`id` (GUID) generado en la respuesta HTTP 201.
2. **Crear Productos:** Ingrese al endpoint `POST /api/Products` e
inserte algunos artículos de prueba con su respectivo precio y
stock disponible. Copie los IDs de los productos creados.
3. **Generar una Orden de Compra:** Vaya al endpoint `POST
/api/Orders`, pegue el ID del usuario creado previamente y
configure el listado de productos con sus cantidades. El sistema
validará y descontará de forma automática las existencias del stock
en la tabla de productos mediante las reglas embebidas del Dominio.
4. **Consultar Datos:** Utilice los endpoints `GET` para verificar
cómo impactaron las relaciones, cálculos de subtotales e
inclusiones en cascada dentro de la base de datos local SQLite.