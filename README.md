# ESISA - Application de Gestion des Bases de Données avec ASP.NET C# et Oracle
<img src="https://media.geeksforgeeks.org/wp-content/uploads/20231104142236/Message-Passing.png" alt="Parc" width="1300" height="400"/>

# A

## Description du Projet

Ce projet repose sur une architecture client-serveur flexible où une machine locale et une machine virtuelle interagissent dynamiquement pour assurer la gestion et la synchronisation des données stockées dans une base de données relationnelle **Oracle**.
L’application, développée en **ASP.NET C#**, permet de **gérer, modifier et synchroniser** les informations entre les deux machines en **temps réel**, avec la possibilité d’inverser les rôles de **client et serveur**.

## Fonctionnalités Principales

**Gestion complète des données** (ajout, suppression, mise à jour).  
**Synchronisation en temps réel** entre la machine locale et la machine virtuelle.  
**Flexibilité des rôles** : une machine peut agir en tant que serveur ou client selon le contexte.  .  
**Sécurité renforcée** : utilisation de protocoles d’authentification et de chiffrement pour protéger l’accès aux données.  

## Architecture du Système

1. **Base de Données Oracle**  
   - Contient les tables **EMP** et **DEPT**.  
   - Les modifications effectuées sur une machine sont immédiatement visibles sur l’autre.  

2. **Interconnexion des Machines**  
   - Un **Bridge Réseau** assure la communication entre la machine locale et la machine virtuelle.  
   - L’accès aux services peut être réalisé via **via une adresse IP ou un pont réseau configuré sous VMware**.  

3. **Rôles Interchangeables**  
   - La **machine locale** peut être un **serveur** lorsqu’elle héberge la base de données.  
   - Elle peut également devenir un **client** si la machine virtuelle joue le rôle de serveur.  
   - Cette architecture garantit **une redondance des données** et une **continuité de service**.

## Technologies Utilisées

- **ASP.NET Core C#** – Développement de l’application  
- **Oracle Database** – Gestion des données  
- **Vmware/VirtualBox** – Machine virtuelle  
- **Bridge Réseau** – Communication entre les machines

## Formation Oracle : Concepts Appliqués

### 1. Gestion des Instances Oracle
- Compréhension des étapes de démarrage d’une base de données : **Shutdown → Nomount → Mount → Open**.
- Différences entre les fichiers **Pfile** (modifiable manuellement) et **SPfile** (modifiable dynamiquement).
- Création et gestion des fichiers **SPfile** pour automatiser la configuration des bases de données.

### 2. Journalisation et Redolog Files
- Rôle des fichiers **Redolog** dans la gestion des transactions (Commit, LogSwitch).
- Création, modification et suppression de groupes de journaux.
- Activation des modes **ARCHIVE** et **NOARCHIVE LOG** pour la gestion des sauvegardes.

### 3. Gestion des Tablespaces et des Fichiers de Données
- Création et gestion des **tablespaces** (System, Sysaux et non-systèmes).
- Redimensionnement et déplacement des **fichiers de données** (Datafiles).

### 4. Pratiques DBA (Database Administrator)
- Gestion des **utilisateurs DBA** et des **permissions** en fonction de l’état de la base.
- Mise en ligne et hors ligne des **tablespaces**, avec archivage et synchronisation manuels.

### 5. Journalisation Manuelle et Points de Contrôle
- Commandes SQL pour forcer les basculements (**LogSwitch**) et les points de contrôle (**Checkpoint**).
- Configuration de l’archivage manuel pour garantir la sécurité des données.

### 6. Exercices Pratiques
- Mise en œuvre des commandes SQL pour gérer les composants de la base.
- Utilisation des vues système (V$log, V$logfile, **DBA_tablespaces**) pour analyser les métadonnées.

© 2024 - **Application de gestion des base de donnée**  
© 2024 **ES-SAHLY Hamza**. Tous droits réservés.
