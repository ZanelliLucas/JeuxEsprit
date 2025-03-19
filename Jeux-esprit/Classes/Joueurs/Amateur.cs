// Fichier: Amateur.cs
using System;

namespace JeuxDesprit
{
    /// <summary>
    /// Classe représentant un joueur amateur
    /// </summary>
    public class Amateur : Joueur
    {
        private int niveauExperience;

        /// <summary>
        /// Constructeur de la classe Amateur
        /// </summary>
        /// <param name="nom">Nom du joueur</param>
        /// <param name="email">Email du joueur</param>
        /// <param name="avatar">Avatar du joueur</param>
        /// <param name="niveauExperience">Niveau d'expérience du joueur (1-5)</param>
        public Amateur(string nom, string email, string avatar, int niveauExperience = 1) 
            : base(nom, email, avatar)
        {
            this.niveauExperience = Math.Clamp(niveauExperience, 1, 5);
        }

        // Getters et Setters spécifiques
        public int GetNiveauExperience() { return niveauExperience; }
        public void SetNiveauExperience(int niveauExperience) { this.niveauExperience = Math.Clamp(niveauExperience, 1, 5); }

        /// <summary>
        /// Affiche un message de bienvenue avec l'avatar et le niveau d'expérience du joueur
        /// </summary>
        public override void afficherJoueur()
        {
            Console.WriteLine($"Bienvenue, {nom} (Joueur Amateur) !");
            Console.WriteLine($"Avatar : {avatar}");
            Console.WriteLine($"Niveau d'expérience : {niveauExperience}/5");
        }
    }
}