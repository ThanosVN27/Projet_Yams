# Jeux de Yams

## Présentation

Ce projet est réalisé dans le cadre de la SAE P11/W11 à l'IUT Robert Schuman.  
Il s'agit d'une application permettant de jouer au jeu de Yam's (Yahtzee) en console (C#) et de visualiser les résultats via une interface web (HTML/CSS/JS).

## Membres du groupe

- Le Thanh Long
- Klausnitzer Jules

## Structure du projet

```
P11/
  yams.cs           # Code source du jeu en C#
  Parties/          # Sauvegardes des parties au format JSON
W11/
  Sae/
    Sae.html        # Page d'accueil du site web
    jeux.html       # Interface de visualisation d'une partie
    global.html     # Visualisation globale des rounds d'une partie
    regles.html     # Règles du jeu
    css/            # Feuilles de style
    img/            # Images utilisées sur le site
    js/             # Scripts JavaScript pour l'affichage dynamique
README.md           # Ce fichier
```

## Fonctionnalités

### Partie C# (P11)

- Jeu de Yam's en mode console pour 2 joueurs.
- Saisie des pseudos, gestion des tours, relances de dés, choix des challenges.
- Calcul automatique des scores et gestion du bonus.
- Sauvegarde automatique de chaque partie au format JSON dans `P11/Parties/`.

### Partie Web (W11)

- Visualisation des parties sauvegardées via une interface web moderne.
- Affichage des résultats par round, des scores, des dés et des challenges.
- Navigation entre les rounds, affichage global ou détaillé.
- Consultation des règles du jeu.

## Lancer le projet

### Partie C# (Jeu console)

1. Ouvre un terminal dans le dossier `P11/`.
2. Compile le fichier :
   ```sh
   csc yams.cs
   ```
3. Lance le jeu :
   ```sh
   ./yams.exe
   ```
4. À la fin de la partie, un fichier JSON est généré dans `P11/Parties/`.

### Partie Web

1. Ouvre le fichier `W11/Sae/Sae.html` dans un navigateur pour accéder à l'accueil.
2. Pour visualiser une partie, va sur `jeux.html` ou `global.html` et entre l'ID de la partie.
3. Les pages utilisent HTML, CSS et JavaScript, aucune installation n'est nécessaire.

## Dépendances

- .NET (pour compiler et exécuter le C#)
- Un navigateur web moderne

## Remarques

- Les fichiers JSON générés peuvent être utilisés pour l'affichage web.
- Le projet respecte la séparation des responsabilités entre la logique du jeu (C#) et l'interface utilisateur (Web).

---

© 2021-2024 IUT Robert Schuman – Tous droits réservés.
