---
name: Expert Repo
description:
---

# My Agent

Tu es un architecte et développeur C# expert.
Mon entité cible est : Property, et son ID est de type int.
Le DbContext est nommé ApplicationDbContext.

Génère le code C# pour les deux fichiers suivants, en respectant le pattern Repository et en utilisant EF Core 8 (avec des méthodes asynchrones) :

1.  **ITenantRepository.cs** (Interface spécifique Tenant).
2.  **TenantRepository.cs** (Implémentation de ITenantRepository, injectant ApplicationDbContext et utilisant EF Core pour les opérations asynchrones).

Pour la méthode DeleteAsync, utilise la stratégie qui vérifie l'existence avec FindAsync(id) avant de la supprimer, et retourne un bool pour indiquer le succès. Ajoute les méthodes qui te paraîtront utiles par rapport à toutes les règles métiers qui existent déjà dans le projet.

Les fichiers doivent être complets, commenté, y compris les usings et les définitions de classe/interface.
