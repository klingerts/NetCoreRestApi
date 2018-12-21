# Code Challenge
Author: Klinger Silva 
***
--Best viewed with a markdown viewer
***
### Issues found on original solution
- Vulnerability to SQL injection
- No data sanitization to prevent XSS in case data is accessed via browsers
- No validation
- No automated tests of any kind, and also no documentation
- Connection string is hard coded potentially exposing secrets to source control
- Transaction control not implemented which can lead to data integrity issues 
- Database connections are never disposed causing potential connection pool starvation 
- Deleting productOption rows require multiple roundtrips to database (lacks sql set operation). 
- Pagination not implemented which can lead to memory issues and scalability problems.
- High coupling between controllers and data access making it difficult to automate tests
- The single controller has too much responsability
- Model is also concerned with data access
- Multiple classes in one file (sometimes it is ok, but only for special cases)
- Data models shared between Api and data access.
***
### Refactoring and improvements  
The solution presented here fixes all the issues pointed above.

Below is an example of how to use pagination:
`/products?offset=0&limit=2  `
`/products?offset=3&limit=5  `
--_where_: offset = number of records to skip  
--_limit_: number of rows to retrieve  

***
### The solution
New app is architected in a layered fashion with a very minor DDD flavor.  
Security, reliability, extensibility, maintainability, readability, and testability 
concerns were kept in mind at all times. 
OO design principles were applied to achieve good separation of concerns, loose coupling,
and high cohesion between classes and modules.  
The app has 6 modules: `Api`, `Application`, `Abstractions`, `DataAccess`, `Common`, and `Setup`.  
Queries are clearly separated from operations that change data.  
The `Api` module consumes services/queries exposed as interfaces by the `Application` module 
and has no direct dependencies on data access.  
In order to modify data the `Api` module uses services implemented by the `Application` module. This prevents the `Api` from bypassing control/validation implemented at the `Application` module.  
The `Application` module is responsible for coordination (services), validation via fluent validation, and transaction control 
via UnitOfWork.  
The modules `Application` and `DataAccess` take a dependency on the `Abstraction` module. This module contains interfaces only.  
The `DataAccess` module implements interfaces exposed by the `Abstraction` and `Application` modules.  
The communication with the database is implemented with __Dapper__  
All queries are stored in a resource file within (`SQLStatements`) the data access module making it very easy to see all queries at a glance.  
The data access classes uses **Dapper** indirectly via a proxy class. This is done to enhance testability.  

All modules take a dependency to the `Common` module. The `Common` module contains DTOs and classes used by all modules.
A\ `Setup` module is used to encapsulate application specific dependency injection configuration and take a dependency on all modules.  
I chose __Dapper__ because it is a light, fast, productive, and easy to use micro ORM. It supports bulk operations, store procs, AND makes the task of preventing SQL injection simple and fast even without the use of store procs.  
The app configuration can be completely isolated (using environment variables perhaps) properly protecting secrets and simplifying deployment automation.
The self hosting features of Asp.Net Core make it simpler to deploy in containers and apply microservices architecture principles.
Asp.net Core also promotes good dependency management and versioning and it embraces the dependency injection pattern vastly improving testability.  
In addition Asp.Net Core is multiplatform allowing for additional deployment options.
Rest api documentation is available via __Swagger__.  

***
### Diagram:  
![Component Diagram](https://www.plantuml.com/plantuml/svg/RP91ReCm44NtdC9YrLeAKdi052jiAbMLMj9LaeM1IM89RCiUsaNLktSmJa41hppVxtiyNzvwb0xxhaqIRHfWwGUSPOt6xQ_AivJz98nMRJfhD6XaAUV2AiwF-a5ucxq1ifrnm9wpfyUKYPBHwPh5jggMg8pcF69s1QiyEkfEcBznbLzoEzDr_pUDnH8g0NiRcR1V8hoGA4LTW_V3H7aXhGKN78L8Vc-HxC7ZbCNXHIDSN1ZcD2gVMk5fu1kwhvAUFENxAtLrN-2EEZeFHJmDAEN4DetvrfiyX36ln-UIEX4Kmvo8GmVmNfpxebI8DQMtiVTKK1igiYoVlpPsSUfonUYubyWNfHREWqBv9LlGpV2p_W00)

[Created with PlantText UML](https://www.planttext.com)
***
### Tech stack:  
|Use|Name|
|----|----|
|Framework version|Asp.Net Core 2.1
|Language|C#
|Package management|Nuget
|Data Access|Dapper
|Unhandled exception logging|(*)ElmahCore. **Endpoint:** _/errors_
|Additional runtime dependencies| AutoMapper, FluentValidation, HTMLSanitizer, Scrutor (service discovery), Ben.Demystifier (exception and stacktrace serialization)
|Api documentation|(*)Swagger (Swashbuckle.AspNetCore nuget package). **Endpoint:** _/swagger/v2/swagger.json_
|Testing tools: |Nunit 3, Moq, AutoFixture, FluentAssertions  

(\*) Swagger and Elmah can be deactivated via configuration on appsettings.

***
**_Thank you for considering my application and for reading!_** :smiley: 
