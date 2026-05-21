# E-Commerce API — Arquitectura Limpia (.NET 8)

Este proyecto consiste en el desarrollo de una API REST para un sistema de E-Commerce, construida bajo los lineamientos de la **Arquitectura Limpia (Clean Architecture)** y utilizando **Entity Framework Core** con **SQLite** para la persistencia de datos.

Desarrollado para la asignatura **Backend (Optativa II)** de la Tecnicatura Universitaria en Desarrollo de Software de la Universidad Católica de Cuyo (UCC).

---

## 🏗️ Estructura de la Solución

El proyecto está dividido estrictamente en capas independientes para aislar la lógica del negocio de los componentes de infraestructura:

- **`ECommerce.Domain`**: Capa central que contiene las entidades puras del dominio (`Product`, `User`, `Order`, `OrderItem`), enums y reglas de negocio, aislada de cualquier framework.
- **`ECommerce.Application`**: Define los contratos e interfaces (`IProductRepository`, `IUserRepository`, `IOrderRepository`) que regulan las operaciones del sistema.
- **`ECommerce.Infrastructure`**: Implementa el acceso a datos mediante el `ApplicationDbContext`, configuraciones explícitas de tablas con **Fluent API** y los repositorios reales.
- **`ECommerce.Api`**: Punto de entrada de la aplicación, contiene los DTOs, los controladores expuestos (`Products`, `Users`, `Orders`) y la inyección de dependencias general.

---

## 🛠️ Tecnologías y Herramientas Utilizadas

- **SDK:** .NET 8.0
- **ORM:** Entity Framework Core 8.0.*
- **Motor de Base de Datos:** SQLite
- **Documentación de Endpoints:** Swagger UI nativo de .NET 8
- **IDE:** Visual Studio Code

---

## 🚀 Requisitos e Instalación

Siga estos pasos para clonar, configurar y ejecutar el proyecto localmente en su entorno:

### 1. Clonar el repositorio
```bash
git clone <URL_DE_TU_REPOSITORIO_GITHUB>
cd ECommerce