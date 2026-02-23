# Infrastructure layer
_This is a part of a bigger project, for more see [global README](https://github.com/axelcharrier/StudentApp_Backend/)._

## Structure
This layer represents the repositories, it makes the link between database and services

### Abstractions 
Implements the repository's interface that define the required methods in the corresponding repository.

### Extension
Used to configure dbContext

### Migrations 
These files are auto generated with this command : ```dotnet ef migrations add <NAME>```
They are used to manipulate database structure.

### Persistence
Defines the database configuration

### Repositories
Here are the different repositories, they contains the methods that manipulate data inside the database.
