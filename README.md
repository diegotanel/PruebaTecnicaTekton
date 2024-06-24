# ProductosAPI

## Descripción
API REST para la gestión de productos utilizando .NET 8, siguiendo principios SOLID y patrones de diseño CQRS.

## Arquitectura de capas
Se ha separado las responsabilidades en distintas capas:
- Controllers para la gestión de peticiones HTTP.
- Servicios y modelos para definir las reglas de negocio.
- Repositorios para manejar el acceso a la bases de datos.
- Capa de soporte para las responsabilidades transversales.

## Patrones y Arquitectura
- **Repository Pattern** para la capa de acceso a datos.
- **CQRS** para separar las operaciones de lectura y escritura.
- **Mediator** para centraliza la comunicación de los objetos.
- **DTOs** para transferir datos entre capas.
- **LazyCache** para el caching de productos.
- **Middleware** para el logs del tiempo de respuesta de las peticiones y excepciones.
- **TDD** para asegurar la calidad del código.
- **Inyección de dependencias** para desacoplar los componentes por medio de Interfaces.
- **Aspect-oriented programming (AOP)** para las responsabilidades transversales.

## Clean code
Se aplico Clean code en la solución. Se realizaron las verificaciones utilizando lint.

## Base de datos
Se ha optado cómo base de datos SQLite, ya que para la prueba técnica se quiere tener la menor cantidad de dependencias posibles.
Se utiliza inyección de dependencias para la implementación de la base de datos, por lo que es trivial cambiarla por otra distinta.

## Configuraciones
Las configuraciones de la API son configurables por medio del archivo `appsettings.json`.
Las configuraciones actuales son:
- Base de datos
- Expiración de cache
- URL de MockApi

## ORM
Se utiliza Entity Framework cómo ORM para gestionar el acceso a la base de datos. 

## Swagger
Para visualizar e interactuar con las APIs en desarrollo.
Se ha documentado los Endpoints con el tipo de respuesta y el código de estado HTTP que retorna.

## API Externa
Se utiliza MockApi para obtención de datos externos.
En este caso, se obtiene cómo respuesta un número del 1 al 99, que se utiliza para el precio de descuento.

## Cache
Se ha implementado LazyCache para conservar el producto en cache. 
El tiempo de expiración esta configurado en 5 minutos.
Se utiliza inyección de dependencias para la implementación del cache, por lo que es trivial cambiarlo por otro distinto.
En un ambiente distribuido de microservicios sería una buena solución utilizar Redis.

## Logs
Se almacena en un archivo local el logueo con el tiempo de respuesta de las peticiones.
La implementación es por medio de un Middelware, por lo que es fácilmente remplazarlo por otro tipo de log.

## Excepciones
Se almacena en un archivo local todas las excepciones junto con el stacktrace.
La implementación es por medio de un Middelware, por lo que es fácilmente remplazarlo por otro tipo de log.

# Validaciones de parámetros de entrada de los endpoints
Se ha utilizado los Data Annotations para validar datos enviados a los endpoints.