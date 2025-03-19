// Fichier: Professionnel.cs
using System;

namespace JeuxDesprit
{
    /// <summary>
    /// Classe représentant un joueur professionnel
    /// </summary>
    public class Professionnel : Joueur
    {
        private int classement;
        private int partiesGagnees;

        /// <summary>
        /// Constructeur de la classe Professionnel
        /// </summary>
        /// <param name="nom">Nom du joueur</param>
        /// <param name="email">Email du joueur</param>
        /// <param name="avatar">Avatar du joueur</param>
        /// <param name="classement">Classement du joueur</param>
        /// <param name="partiesGagnees">Nombre de parties gagnées</param>
        public Professionnel(string nom, string email, string avatar, int classement = 0, int partiesGagnees = 0) 
            : base(nom, email, avatar)
        {
            this.classement = classement;
            this.partiesGagnees = partiesGagnees;
        }

        // Getters et Setters spécifiques
        public int GetClassement() { return classement; }
        public void SetClassement(int classement) { this.classement = classement; }

        public int GetPartiesGagnees() { return partiesGagnees; }
        public void SetPartiesGagnees(int partiesGagnees) { this.partiesGagnees = partiesGagnees; }

        /// <summary>
        /// Incrémente le nombre de parties gagnées et met à jour le classement
        /// </summary>
        public void IncrementPartiesGagnees()
        {
            partiesGagnees++;
            // Mettre à jour le classement en fonction des parties gagnées
            if (partiesGagnees % 5 == 0)
            {
                classement++;
                Console.WriteLine($"Félicitations ! Votre classement est maintenant de {classement} !");
            }
        }

        /// <summary>
        /// Affiche un message de bienvenue avec l'avatar, le classement et les statistiques du joueur
        /// </summary>
        public override void afficherJoueur()
        {
            Console.WriteLine($"Bienvenue, {nom} (Joueur Professionnel) !");
            Console.WriteLine($"Avatar : {avatar}");
            Console.WriteLine($"Classement : {classement}");
            Console.WriteLine($"Parties gagnées : {partiesGagnees}");
        }
    }
}