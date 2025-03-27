using System;

namespace JeuxDesprit
{
    /// <summary>
    /// Classe représentant un joueur
    /// </summary>
    public class Joueur
    {
        protected string nom;
        protected string email;
        protected string avatar;

        /// <summary>
        /// Constructeur de la classe Joueur
        /// </summary>
        /// <param name="nom">Nom du joueur</param>
        /// <param name="email">Email du joueur</param>
        /// <param name="avatar">Avatar du joueur (chemin vers une image ou description textuelle)</param>
        public Joueur(string nom, string email, string avatar)
        {
            this.nom = nom;
            this.email = email;
            this.avatar = avatar;
        }

        // Getters et Setters
        public string GetNom() { return nom; }
        public void SetNom(string nom) { this.nom = nom; }

        public string GetEmail() { return email; }
        public void SetEmail(string email) { this.email = email; }

        public string GetAvatar() { return avatar; }
        public void SetAvatar(string avatar) { this.avatar = avatar; }

        /// <summary>
        /// Affiche un message de bienvenue avec l'avatar du joueur
        /// </summary>
        public virtual void afficherJoueur()
        {
            Console.WriteLine($"Bienvenue, {nom} !");
            Console.WriteLine($"Avatar : {avatar}");
            Console.WriteLine($"Email : {email}");
        }
    }
}