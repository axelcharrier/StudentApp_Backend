# Minimale API
_This is only a part of the entire project for more try the [global README](https://github.com/axelcharrier/StudentApp_Backend).

## Structure
This layer is the Api part, we use the "minimal" format.

### Endpoints
You will find the different endpoints in the directory "Endpoints".
There is 3 endpoints files : 
- AuthentificationEndpoints : manages the authentication process (login, logout, manage user...)
- StudentsEndpoints : manages the students' CRUD process (Create, Read, Update, Delete)
- UsersEndpoints : manages the users' process (Read, Update, Delete)

### Models 
Define some records to give a clean response in the endpoints

### Policies
Define access rules, these rules will be used to restrict access on the endpoints in relation to the user's roles.
