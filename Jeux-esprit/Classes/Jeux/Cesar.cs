using System;
using System.Diagnostics;
using System.Text;

namespace JeuxDesprit
{
    /// <summary>
    /// Classe pour le jeu du chiffrement de César
    /// </summary>
    public class Cesar : Jeux
    {
        private readonly Random random;
        private int decalageMax;

        /// <summary>
        /// Constructeur de la classe Cesar
        /// </summary>
        public Cesar() 
            : base("Chiffrement de César", "Déchiffrez ou chiffrez un message en utilisant le chiffrement de César", 1)
        {
            this.random = new Random();
            SetNiveauDifficulte("facile");
        }

        /// <summary>
        /// Définit le niveau de difficulté du jeu
        /// </summary>
        /// <param name="niveau">Niveau de difficulté (facile, moyen, difficile, expert)</param>
        private void SetNiveauDifficulte(string niveau)
        {
            switch (niveau.ToLower())
            {
                case "facile":
                    decalageMax = 5;
                    break;
                case "moyen":
                    decalageMax = 10;
                    break;
                case "difficile":
                    decalageMax = 15;
                    break;
                case "expert":
                    decalageMax = 25;
                    break;
                default:
                    decalageMax = 5;
                    break;
            }
        }

        /// <summary>
        /// Chiffre un message avec un décalage donné
        /// </summary>
        /// <param name="message">Message à chiffrer</param>
        /// <param name="decalage">Décalage à appliquer</param>
        /// <returns>Message chiffré</returns>
        private string Chiffrer(string message, int decalage)
        {
            StringBuilder result = new StringBuilder();

            foreach (char c in message)
            {
                if (char.IsLetter(c))
                {
                    char offset = char.IsUpper(c) ? 'A' : 'a';
                    result.Append((char)((((c - offset) + decalage) % 26) + offset));
                }
                else
                {
                    result.Append(c);
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// Déchiffre un message avec un décalage donné
        /// </summary>
        /// <param name="messageChiffre">Message chiffré</param>
        /// <param name="decalage">Décalage appliqué</param>
        /// <returns>Message déchiffré</returns>
        private string Dechiffrer(string messageChiffre, int decalage)
        {
            return Chiffrer(messageChiffre, 26 - (decalage % 26));
        }

        /// <summary>
        /// Joue une partie du jeu du chiffrement de César
        /// </summary>
        /// <param name="joueur">Le joueur qui joue la partie</param>
        /// <param name="niveau">Niveau de difficulté</param>
        /// <returns>True si la partie est gagnée, False sinon</returns>
        public bool jouerCesar(Joueur joueur, string niveau)
        {
            SetNiveauDifficulte(niveau);
            
            string[] messages = {
                "Le chiffrement de Cesar est une technique simple",
                "La cryptographie est une science passionnante",
                "Ce message secret doit etre dechiffre",
                "Le code a ete casse par les ennemis",
                "Envoyez des renforts immediatement"
            };

            string messageOriginal = messages[random.Next(messages.Length)];
            int decalage = random.Next(1, decalageMax + 1);
            string messageChiffre = Chiffrer(messageOriginal, decalage);

            Console.WriteLine($"Bienvenue dans le jeu du Chiffrement de César, {joueur.GetNom()}!");
            Console.WriteLine("Vous devez déchiffrer le message suivant en trouvant le décalage utilisé.");
            Console.WriteLine($"Niveau: {niveau} (décalage max: {decalageMax})");
            Console.WriteLine($"\nMessage chiffré: {messageChiffre}");

            Stopwatch chrono = new Stopwatch();
            chrono.Start();

            bool gagne = false;
            int essais = 0;
            int maxEssais = 3;

            while (essais < maxEssais && !gagne)
            {
                Console.Write($"\nEssai {essais + 1}/{maxEssais} - Entrez le décalage que vous pensez avoir été utilisé (1-{decalageMax}): ");
                if (!int.TryParse(Console.ReadLine(), out int decalagePropose))
                {
                    Console.WriteLine("Veuillez entrer un nombre valide.");
                    continue;
                }

                if (decalagePropose < 1 || decalagePropose > decalageMax)
                {
                    Console.WriteLine($"Le décalage doit être entre 1 et {decalageMax}.");
                    continue;
                }

                essais++;

                if (decalagePropose == decalage)
                {
                    gagne = true;
                    Console.WriteLine("\nBravo ! Vous avez trouvé le bon décalage !");
                    Console.WriteLine($"Message déchiffré: {messageOriginal}");
                }
                else
                {
                    string tentative = Dechiffrer(messageChiffre, decalagePropose);
                    Console.WriteLine($"Ce n'est pas le bon décalage. Voici ce que ça donne: {tentative}");
                }
            }

            chrono.Stop();
            TimeSpan tempsEffectue = chrono.Elapsed;

            if (!gagne)
            {
                Console.WriteLine($"\nDommage ! Vous n'avez pas trouvé le bon décalage ({decalage}).");
                Console.WriteLine($"Le message original était: {messageOriginal}");
            }

            // Calculer le score
            int score = CalculerScore(gagne, essais, tempsEffectue);

            // Mettre à jour la date de dernière connexion
            SetDerniereConnexion(DateTime.Now);

            // Appeler les méthodes demandées
            resultatsJeu(score, tempsEffectue, 1);

            return gagne;
        }

        /// <summary>
        /// Calcule le score en fonction du résultat, du nombre d'essais et du temps
        /// </summary>
        /// <param name="gagne">True si la partie est gagnée, False sinon</param>
        /// <param name="nbEssais">Nombre d'essais effectués</param>
        /// <param name="temps">Temps effectué</param>
        /// <returns>Score calculé</returns>
        private int CalculerScore(bool gagne, int nbEssais, TimeSpan temps)
        {
            if (!gagne)
                return 0;

            // Base de 1000 points pour une victoire
            int baseScore = 1000;
            
            // Moins de points pour plus d'essais
            int essaisScore = (4 - nbEssais) * 200;
            
            // Moins de points pour un temps plus long (max 2 minutes)
            int tempsScore = Math.Max(0, 500 - (int)(temps.TotalSeconds * 5));
            
            return baseScore + essaisScore + tempsScore;
        }
    }
}