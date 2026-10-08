# StudentApp Backend

## Compte initial seedé

Au démarrage de `StudentApp.ApiMinimal`, l'application crée automatiquement (s'il n'existe pas) un utilisateur initial :

- Email : `admin@admin.fr`
- Mot de passe : `Admin@123`
- Rôle : `Admin`

## Détail des autorisations

La policy `AllowTeacher` autorise désormais les rôles `Teacher` **et** `Admin`.

En conséquence, le compte `admin@admin.fr` dispose des mêmes droits que `Teacher` sur les endpoints protégés par cette policy.
