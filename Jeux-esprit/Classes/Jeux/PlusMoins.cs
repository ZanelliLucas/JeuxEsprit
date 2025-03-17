using System;
using System.Diagnostics;

namespace JeuxDesprit
{
    /// <summary>
    /// Classe pour le jeu du Plus ou Moins
    /// </summary>
    public class PlusMoins : Jeux
    {
        private readonly Random random;
        private readonly int borneMin;
        private readonly int borneMax;
        private int nombreEssaisMax;

        /// <summary>
        /// Constructeur de la classe PlusMoins
        /// </summary>
        /// <param name="borneMin">Borne minimale pour le nombre à deviner</param>
        /// <param name="borneMax">Borne maximale pour le nombre à deviner</param>
        public PlusMoins(int borneMin = 1, int borneMax = 100) 
            : base("Plus ou Moins", "Devinez un nombre entre deux bornes", 1)
        {
            this.random = new Random();
            this.borneMin = borneMin;
            this.borneMax = borneMax;
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
                    nombreEssaisMax = 7;
                    break;
                case "difficile":
                    nombreEssaisMax = 5;
                    break;
                case "expert":
                    nombreEssaisMax = 3;
                    break;
                default:
                    nombreEssaisMax = 10;
                    break;
            }
        }

        /// <summary>
        /// Joue une partie du jeu du Plus ou Moins
        /// </summary>
        /// <param name="joueur">Le joueur qui joue la partie</param>
        /// <param name="niveau">Niveau de difficulté</param>
        /// <returns>True si la partie est gagnée, False sinon</returns>
        public bool jouerPlusMoins(Joueur joueur, string niveau)
        {
            SetNiveauDifficulte(niveau);
            int nombreMystere = random.Next(borneMin, borneMax + 1);
            int essais = 0;
            bool gagne = false;
            int proposition;

            Console.WriteLine($"Bienvenue dans le jeu du Plus ou Moins, {joueur.GetNom()}!");
            Console.WriteLine($"Je pense à un nombre entre {borneMin} et {borneMax}.");
            Console.WriteLine($"Vous avez {nombreEssaisMax} essais pour le trouver.");

            Stopwatch chrono = new Stopwatch();
            chrono.Start();

            while (essais < nombreEssaisMax && !gagne)
            {
                Console.Write($"Essai {essais + 1}/{nombreEssaisMax} - Votre proposition : ");
                if (!int.TryParse(Console.ReadLine(), out proposition))
                {
                    Console.WriteLine("Veuillez entrer un nombre valide.");
                    continue;
                }

                essais++;

                if (proposition == nombreMystere)
                {
                    gagne = true;
                    Console.WriteLine("Bravo ! Vous avez trouvé le nombre mystère !");
                }
                else if (proposition < nombreMystere)
                {
                    Console.WriteLine("C'est plus !");
                }
                else
                {
                    Console.WriteLine("C'est moins !");
                }
            }

            chrono.Stop();
            TimeSpan tempsEffectue = chrono.Elapsed;

            if (!gagne)
            {
                Console.WriteLine($"Dommage, vous avez épuisé vos {nombreEssaisMax} essais !");
                Console.WriteLine($"Le nombre mystère était : {nombreMystere}");
            }

            // Calculer le score en fonction du temps et du nombre d'essais
            int score = CalculerScore(gagne, essais, tempsEffectue);

            // Mettre à jour la date de dernière connexion
            SetDerniereConnexion(DateTime.Now);

            // Mettre à jour les statistiques dans la base de données
            // ...

            // Appeler les méthodes demandées
            // statutPartie(gagne ? "gagné" : "perdu"); // Cette méthode doit être appelée par le Programme principal
            resultatsJeu(score, tempsEffectue, 1); // On suppose que c'est la première partie

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
            int essaisScore = (nombreEssaisMax - nbEssais + 1) * 100;
            
            // Moins de points pour un temps plus long (max 30 secondes)
            int tempsScore = Math.Max(0, 500 - (int)(temps.TotalSeconds * 10));
            
            return baseScore + essaisScore + tempsScore;
        }
    }
}