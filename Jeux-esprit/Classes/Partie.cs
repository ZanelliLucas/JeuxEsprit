using System;

namespace JeuxDesprit
{
    /// <summary>
    /// Classe représentant une partie de jeu
    /// </summary>
    public class Partie
    {
        private string statut; // "gagné" ou "perdu"
        private DateTime datePartie;
        private int score;
        private TimeSpan tempsEffectue;

        /// <summary>
        /// Constructeur de la classe Partie
        /// </summary>
        /// <param name="statut">Statut de la partie (gagné ou perdu)</param>
        public Partie(string statut = "")
        {
            this.statut = statut;
            this.datePartie = DateTime.Now;
            this.score = 0;
            this.tempsEffectue = TimeSpan.Zero;
        }

        // Getters et Setters
        public string GetStatut() { return statut; }
        public void SetStatut(string statut) { this.statut = statut; }

        public DateTime GetDatePartie() { return datePartie; }
        public void SetDatePartie(DateTime datePartie) { this.datePartie = datePartie; }

        public int GetScore() { return score; }
        public void SetScore(int score) { this.score = score; }

        public TimeSpan GetTempsEffectue() { return tempsEffectue; }
        public void SetTempsEffectue(TimeSpan tempsEffectue) { this.tempsEffectue = tempsEffectue; }

        /// <summary>
        /// Affiche le statut de la partie
        /// </summary>
        public void statutPartie()
        {
            if (string.IsNullOrEmpty(statut))
            {
                Console.WriteLine("Partie en cours...");
            }
            else if (statut.ToLower() == "gagné" || statut.ToLower() == "gagne")
            {
                Console.WriteLine("Partie gagnée ! Félicitations !");
            }
            else if (statut.ToLower() == "perdu")
            {
                Console.WriteLine("Partie perdue. Meilleure chance la prochaine fois !");
            }
            else
            {
                Console.WriteLine($"Statut de la partie : {statut}");
            }
        }

        /// <summary>
        /// Met à jour les informations de la partie avec le résultat
        /// </summary>
        /// <param name="gagne">Indique si la partie est gagnée ou perdue</param>
        /// <param name="score">Score obtenu</param>
        /// <param name="tempsEffectue">Temps effectué pour jouer la partie</param>
        public void MettreAJour(bool gagne, int score, TimeSpan tempsEffectue)
        {
            this.statut = gagne ? "gagné" : "perdu";
            this.score = score;
            this.tempsEffectue = tempsEffectue;
            this.datePartie = DateTime.Now;
        }
    }
}