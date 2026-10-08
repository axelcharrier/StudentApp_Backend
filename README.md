# StudentApp Backend
_This repository is one of the two part of the project, you will find the other part [just here](https://github.com/axelcharrier/ProjetAngular)._

## API REST
This API follows the REST rules with different layers, you will find a README file on each layer directory.

For a better comprehension of the structure : 

(Aspire) <-> MinimalApi <-> Application <-> Infrastructure <-> Domain

## Orchestration 
This project is run by Aspire to organise the application start

## Initialisation 
To clone this project :
```git clone https://https://github.com/axelcharrier/StudentApp_Backend```
```cd StudentApp_Backend```

**Init your secrets to configure your database connection string**

```dotnet ef database update```
```dotnet start```

## For more 
- [Aspire layer]()
- [MinimalApi layer]()
- [Application layer]()
- [Infrastructure layer]()
- [Domain layer]()

## Compte initial seedé

Au démarrage de `StudentApp.ApiMinimal`, l'application crée automatiquement (s'il n'existe pas) un utilisateur initial :

- Email : `admin@admin.fr`
- Mot de passe : `Admin@123`
- Rôle : `Admin`

## Détail des autorisations

La policy `AllowTeacher` autorise désormais les rôles `Teacher` **et** `Admin`.

En conséquence, le compte `admin@admin.fr` dispose des mêmes droits que `Teacher` sur les endpoints protégés par cette policy.
