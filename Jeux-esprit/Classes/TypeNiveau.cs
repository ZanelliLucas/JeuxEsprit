using System;

namespace JeuxDesprit
{
    /// <summary>
    /// Classe représentant le niveau de difficulté d'un jeu
    /// </summary>
    public class TypeNiveau
    {
        private string libelle; // facile, moyen, difficile, expert

        /// <summary>
        /// Constructeur de la classe TypeNiveau
        /// </summary>
        /// <param name="libelle">Libellé du niveau</param>
        public TypeNiveau(string libelle = "facile")
        {
            this.libelle = ValidateNiveau(libelle);
        }

        // Getters et Setters
        public string GetLibelle() { return libelle; }
        public void SetLibelle(string libelle) { this.libelle = ValidateNiveau(libelle); }

        /// <summary>
        /// Valide et normalise le libellé du niveau
        /// </summary>
        /// <param name="niveau">Niveau proposé</param>
        /// <returns>Niveau validé</returns>
        private string ValidateNiveau(string niveau)
        {
            if (string.IsNullOrEmpty(niveau))
            {
                Console.WriteLine("Niveau non spécifié. Le niveau est défini à 'facile' par défaut.");
                return "facile";
            }
            
            string niveauLower = niveau.ToLower();
            switch (niveauLower)
            {
                case "facile":
                case "moyen":
                case "difficile":
                case "expert":
                    return niveauLower;
                default:
                    Console.WriteLine($"Niveau '{niveau}' non reconnu. Le niveau est défini à 'facile' par défaut.");
                    return "facile";
            }
        }

        /// <summary>
        /// Demande et affiche le niveau de la partie choisie
        /// </summary>
        /// <returns>Niveau choisi</returns>
        public string statutNiveau()
        {
            Console.WriteLine("\nChoisissez un niveau de difficulté :");
            Console.WriteLine("1 - Facile");
            Console.WriteLine("2 - Moyen");
            Console.WriteLine("3 - Difficile");
            Console.WriteLine("4 - Expert");
            Console.Write("Votre choix (1-4) : ");

            string niveauChoisi = "facile";
            bool choixValide = false;

            while (!choixValide)
            {
                string? input = Console.ReadLine() ?? "";
                switch (input)
                {
                    case "1":
                        niveauChoisi = "facile";
                        choixValide = true;
                        break;
                    case "2":
                        niveauChoisi = "moyen";
                        choixValide = true;
                        break;
                    case "3":
                        niveauChoisi = "difficile";
                        choixValide = true;
                        break;
                    case "4":
                        niveauChoisi = "expert";
                        choixValide = true;
                        break;
                    default:
                        Console.Write("Choix invalide. Veuillez entrer un nombre entre 1 et 4 : ");
                        break;
                }
            }

            this.libelle = niveauChoisi;
            Console.WriteLine($"Niveau choisi : {niveauChoisi}\n");
            return niveauChoisi;
        }
    }
}