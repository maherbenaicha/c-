# ENIT Hub

Plateforme de gestion des événements et activités de l'École Nationale d'Ingénieurs de Tunis.

## Nouvelles fonctionnalités ajoutées

### 🔧 Admin (DepartmentAdmin)
- ✅ Créer des activités avec **photo**
- ✅ **Modifier** une activité (titre, description, dates, lieu, photo)
- ✅ **Supprimer** une activité (et tous ses commentaires automatiquement)

### 🎓 Étudiant
- ✅ **Changer sa photo de profil** (bouton dans le dashboard)
- ✅ **Commenter** les événements de son département et des autres
- ✅ **Supprimer ses propres commentaires**
- ✅ Voir les photos des événements

### 🎨 Interface
- ✅ Nom remplacé **UniHub → ENIT Hub** partout
- ✅ Charte graphique ENIT (bleu #1a3c6e, rouge #e63946)

## Migration de base de données

Après avoir mis à jour le projet, appliquer la migration :

```bash
cd UniHub.DAL
dotnet ef database update --startup-project ../UniHub.UI
```

Ou via la console du Gestionnaire de packages dans Visual Studio :
```
Update-Database
```

## Structure des nouvelles tables/colonnes

### Table `Identity.Activities`
- `EventPhoto` (varbinary(max)) — photo de l'événement
- `EventPhotoContentType` (nvarchar(100)) — type MIME de la photo

### Table `Identity.User`
- `ProfilePictureContentType` (nvarchar(100)) — type MIME de la photo de profil

### Nouvelle table `Identity.ActivityComments`
- `Id` (int, PK)
- `Content` (nvarchar(1000))
- `CreatedDate` (datetime2)
- `ActivityId` (FK → Activities)
- `UserId` (FK → User)
