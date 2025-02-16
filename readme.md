# MPIA2D - Projet de Steering Behaviors

## 🚀 Lancer le projet
1. **Téléchargez** le projet depuis Google Drive : [Lien vers la build et le projet](https://drive.google.com/drive/u/0/folders/1KG9eJOjIQ1QqYGZkMr8uXjhQge3Se_IP)
2. Vous y trouverez deux fichiers :
   - **MPIABuild** : La build du projet à exécuter.
   - **MPIA2D** : Le projet complet à ouvrir dans Unity.
3. **Pour tester la build**, décompressez **MPIABuild** et lancez l'exécutable.

---

## 🎮 Fonctionnalités
Ce projet implémente plusieurs comportements de **Steering** :

- **Seek** 🏹 → L'entité suit une cible. *(Cliquez pour changer la position de la cible.)*
- **Flee** 🏃‍♂️ → L'entité fuit la cible.
- **Pursuit** 🚔 → Prédit la position future de la cible et l’intercepte.
- **Evade** 😱 → Fuit en anticipant la position future de la cible.
- **Circuit** 🔄 → Suit un chemin en boucle.
- **One Way** ➡️ → Suit un chemin jusqu’à la fin et s’arrête progressivement.
- **Two Way** ↔️ → Suit un chemin dans un sens, puis revient en arrière une fois arrivé au bout, etc...

---

## 🕹 Commandes UI
- **Bouton "Suivant"** → Passe au comportement de steering suivant.
- **Bouton "Précédent"** → Revient au comportement précédent.
- **Bouton "Relancer"** → Redémarre l’animation en cours.
