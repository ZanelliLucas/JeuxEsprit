using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace JeuxDesprit
{
    /// <summary>
    /// Classe pour le jeu du Pendu
    /// </summary>
    public class Pendu : Jeux
    {
        private readonly List<string> motsFaciles = new List<string> { "chat", "chien", "arbre", "maison", "soleil", "jeu", "fleur" };
        private readonly List<string> motsMoyens = new List<string> { "ordinateur", "voiture", "piscine", "montagne", "valise", "piano" };
        private readonly List<string> motsDifficiles = new List<string> { "développement", "algorithme", "environnement", "communication", "interface" };
        private readonly List<string> motsExperts = new List<string> { "anticonstitutionnellement", "chronophotographie", "électroencéphalogramme", "psychophysiologique" };

        private int nombreEssaisMax;

        /// <summary>
        /// Constructeur de la classe Pendu
        /// </summary>
        public Pendu() 
            : base("Pendu", "Devinez un mot lettre par lettre avant d'être pendu", 1)
        {
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
                    nombreEssaisMax = 10;
                    break;
                case "moyen":
                    nombreEssaisMax = 8;
                    break;
                case "difficile":
                    nombreEssaisMax = 6;
                    break;
                case "expert":
                    nombreEssaisMax = 4;
                    break;
                default:
                    nombreEssaisMax = 10;
                    break;
            }
        }

        /// <summary>
        /// Choisit un mot aléatoire en fonction du niveau de difficulté
        /// </summary>
        /// <param name="niveau">Niveau de difficulté</param>
        /// <returns>Mot choisi</returns>
        private string ChoisirMot(string niveau)
        {
            Random random = new Random();
            switch (niveau.ToLower())
            {
                case "facile":
                    return motsFaciles[random.Next(motsFaciles.Count)];
                case "moyen":
                    return motsMoyens[random.Next(motsMoyens.Count)];
                case "difficile":
                    return motsDifficiles[random.Next(motsDifficiles.Count)];
                case "expert":
                    return motsExperts[random.Next(motsExperts.Count)];
                default:
                    return motsFaciles[random.Next(motsFaciles.Count)];
            }
        }

        /// <summary>
        /// Affiche le pendu en fonction du nombre d'erreurs
        /// </summary>
        /// <param name="erreurs">Nombre d'erreurs</param>
        private void AfficherPendu(int erreurs)
        {
            Console.WriteLine("\nÉtat du pendu :");
            switch (erreurs)
            {
                case 0:
                    Console.WriteLine("  ________");
                    Console.WriteLine("  |      |");
                    Console.WriteLine("  |       ");
                    Console.WriteLine("  |       ");
                    Console.WriteLine("  |       ");
                    Console.WriteLine("  |       ");
                    Console.WriteLine("__|__     ");
                    break;
                case 1:
                    Console.WriteLine("  ________");
                    Console.WriteLine("  |      |");
                    Console.WriteLine("  |      O");
                    Console.WriteLine("  |       ");
                    Console.WriteLine("  |       ");
                    Console.WriteLine("  |       ");
                    Console.WriteLine("__|__     ");
                    break;
                case 2:
                    Console.WriteLine("  ________");
                    Console.WriteLine("  |      |");
                    Console.WriteLine("  |      O");
                    Console.WriteLine("  |      |");
                    Console.WriteLine("  |       ");
                    Console.WriteLine("  |       ");
                    Console.WriteLine("__|__     ");
                    break;
                case 3:
                    Console.WriteLine("  ________");
                    Console.WriteLine("  |      |");
                    Console.WriteLine("  |      O");
                    Console.WriteLine("  |     /|");
                    Console.WriteLine("  |       ");
                    Console.WriteLine("  |       ");
                    Console.WriteLine("__|__     ");
                    break;
                case 4:
                    Console.WriteLine("  ________");
                    Console.WriteLine("  |      |");
                    Console.WriteLine("  |      O");
                    Console.WriteLine("  |     /|\\");
                    Console.WriteLine("  |       ");
                    Console.WriteLine("  |       ");
                    Console.WriteLine("__|__     ");
                    break;
                case 5:
                    Console.WriteLine("  ________");
                    Console.WriteLine("  |      |");
                    Console.WriteLine("  |      O");
                    Console.WriteLine("  |     /|\\");
                    Console.WriteLine("  |     / ");
                    Console.WriteLine("  |       ");
                    Console.WriteLine("__|__     ");
                    break;
                case 6:
                default:
                    Console.WriteLine("  ________");
                    Console.WriteLine("  |      |");
                    Console.WriteLine("  |      O");
                    Console.WriteLine("  |     /|\\");
                    Console.WriteLine("  |     / \\");
                    Console.WriteLine("  |       ");
                    Console.WriteLine("__|__     ");
                    break;
            }
        }

        /// <summary>
        /// Joue une partie du jeu du Pendu
        /// </summary>
        /// <param name="joueur">Le joueur qui joue la partie</param>
        /// <param name="niveau">Niveau de difficulté</param>
        /// <returns>True si la partie est gagnée, False sinon</returns>
        public bool jouerPendu(Joueur joueur, string niveau)
        {
            SetNiveauDifficulte(niveau);
            string motADeviner = ChoisirMot(niveau);
            char[] motDevoile = new char[motADeviner.Length];
            for (int i = 0; i < motDevoile.Length; i++)
                motDevoile[i] = '_';

            int erreurs = 0;
            List<char> lettresEssayees = new List<char>();
            bool gagne = false;

            Console.WriteLine($"Bienvenue dans le jeu du Pendu, {joueur.GetNom()}!");
            Console.WriteLine($"Vous avez {nombreEssaisMax} erreurs possibles avant d'être pendu.");
            
            Stopwatch chrono = new Stopwatch();
            chrono.Start();

            while (erreurs < nombreEssaisMax && !gagne)
            {
                Console.WriteLine("\nMot actuel : " + new string(motDevoile));
                Console.WriteLine("Lettres déjà essayées : " + string.Join(", ", lettresEssayees));
                
                AfficherPendu(erreurs);
                
                Console.Write("Proposez une lettre : ");
                string input = Console.ReadLine()?.ToLower();
                
                if (string.IsNullOrEmpty(input) || input.Length != 1 || !char.IsLetter(input[0]))
                {
                    Console.WriteLine("Veuillez entrer une lettre valide.");
                    continue;
                }

                char lettre = input[0];

                if (lettresEssayees.Contains(lettre))
                {
                    Console.WriteLine("Vous avez déjà essayé cette lettre !");
                    continue;
                }

                lettresEssayees.Add(lettre);

                if (motADeviner.Contains(lettre))
                {
                    Console.WriteLine("Bien joué ! La lettre est dans le mot.");
                    for (int i = 0; i < motADeviner.Length; i++)
                    {
                        if (motADeviner[i] == lettre)
                            motDevoile[i] = lettre;
                    }

                    // Vérifier si le mot est complet
                    if (!motDevoile.Contains('_'))
                    {
                        gagne = true;
                    }
                }
                else
                {
                    Console.WriteLine("Cette lettre n'est pas dans le mot !");
                    erreurs++;
                }
            }

            chrono.Stop();
            TimeSpan tempsEffectue = chrono.Elapsed;

            Console.WriteLine("\nMot final : " + new string(motDevoile));
            AfficherPendu(erreurs);

            if (gagne)
            {
                Console.WriteLine($"Félicitations ! Vous avez deviné le mot \"{motADeviner}\" !");
            }
            else
            {
                Console.WriteLine($"Dommage ! Le mot à deviner était \"{motADeviner}\".");
            }

            // Calculer le score en fonction du temps et du nombre d'erreurs
            int score = CalculerScore(gagne, erreurs, tempsEffectue);

            // Mettre à jour la date de dernière connexion
            SetDerniereConnexion(DateTime.Now);

            // Appeler les méthodes demandées
            resultatsJeu(score, tempsEffectue, 1); // On suppose que c'est la première partie

            return gagne;
        }

        /// <summary>
        /// Calcule le score en fonction du résultat, du nombre d'erreurs et du temps
        /// </summary>
        /// <param name="gagne">True si la partie est gagnée, False sinon</param>
        /// <param name="nbErreurs">Nombre d'erreurs commises</param>
        /// <param name="temps">Temps effectué</param>
        /// <returns>Score calculé</returns>
        private int CalculerScore(bool gagne, int nbErreurs, TimeSpan temps)
        {
            if (!gagne)
                return 0;

            // Base de 1000 points pour une victoire
            int baseScore = 1000;
            
            // Moins de points pour plus d'erreurs
            int erreurScore = (nombreEssaisMax - nbErreurs) * 100;
            
            // Moins de points pour un temps plus long (max 2 minutes)
            int tempsScore = Math.Max(0, 500 - (int)(temps.TotalSeconds * 5));
            
            return baseScore + erreurScore + tempsScore;
        }