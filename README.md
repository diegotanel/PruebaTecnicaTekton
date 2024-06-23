# ProductAPI

## Descripción
API REST para la gestión de productos utilizando .NET 8, siguiendo principios SOLID y patrones de diseño CQRS.

## Arquitectura de capas
Se ha separado las responsabilidades en distintas capas
- Controllers para la gestión de peticiones HTTP
- Servicios y modelos para definir las reglas de negocio
- Repositorios para manejar el acceso a la bases de datos
- Capa de soporte para las responsabilidades transversales

## Patrones y Arquitectura
- **Repository Pattern** para la capa de acceso a datos.
- **CQRS** para separar las operaciones de lectura y escritura.
- **DTOs** para transferir datos entre capas.
- **LazyCache** para el caching de productos.
- **Middleware** para el logging del tiempo de respuesta.
- **TDD** para asegurar la calidad del código.
- **Inyección de dependencias** para desacoplar los componentes por medio de Interfaces
- **Aspect-oriented programming (AOP)** para las responsabilidades transversales

## Base de datos
Configurar la base de datos en `appsettings.json`.
Se ha optado cómo base de datos SQLite, ya que para la prueba técnica se quiere tener la menor catidad de dependencias posibles.

## ORM
Se utiliza Entity Framework cómo ORM para gestionar el acceso a la base de datos. 

## Swagger
Para visualizar e interactuar con las APIs en desarrollo.
Se ha documentado los Endpoints cona el tipo de respuesta y el código de estado HTTP que retorna.

## API Externa
Se utiliza MockApi para obtención de datos externos.
En este caso, se obtiene cómo respuesta un número del 1 al 99, que se utiliza para el precio de descuento.

## Cache
Se ha implementado LazyCache para conservar el producto en cache. 
El tiempo de expiración es de 5 minutos.

## Logueo
Se almacena en un archivo local el logueo con el tiempo de respuesta de las peticiones.