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