using System;
using System.Diagnostics;
using System.Text;

namespace JeuxDesprit
{
    /// <summary>
    /// Classe pour le jeu du chiffrement de Vigenère
    /// </summary>
    public class Vigenere : Jeux
    {
        private readonly Random random;
        private readonly string[] messagesPossibles;
        private readonly string[] clesPossibles;

        /// <summary>
        /// Constructeur de la classe Vigenere
        /// </summary>
        public Vigenere() 
            : base("Chiffrement de Vigenère", "Déchiffrez un message en utilisant le chiffrement de Vigenère", 1)
        {
            this.random = new Random();

            // Messages possibles pour le jeu
            this.messagesPossibles = new string[] {
                "Le chiffre de Vigenere est une methode de chiffrement par substitution",
                "La cryptographie protege nos donnees personnelles",
                "Ce message est code avec une cle secrete",
                "Les espions utilisent des codes sophistiques",
                "Transmettez ces informations confidentielles avec prudence"
            };

            // Clés possibles pour le chiffrement
            this.clesPossibles = new string[] {
                "CRYPT", "SECRET", "ENIGMA", "CODE", "CLE", 
                "ALGORITHME", "SECURITE", "MYSTERE", "VIGENERE"
            };
        }

        /// <summary>
        /// Chiffre un message avec la méthode de Vigenère
        /// </summary>
        /// <param name="message">Message à chiffrer</param>
        /// <param name="cle">Clé de chiffrement</param>
        /// <returns>Message chiffré</returns>
        private string Chiffrer(string message, string cle)
        {
            StringBuilder resultat = new StringBuilder();
            int cleIndex = 0;
            
            foreach (char c in message)
            {
                if (char.IsLetter(c))
                {
                    // Normaliser les caractères (tout en majuscule pour le calcul)
                    char offset = char.IsUpper(c) ? 'A' : 'a';
                    char cleChar = char.ToUpper(cle[cleIndex % cle.Length]);
                    int decalage = cleChar - 'A';
                    
                    // Appliquer le chiffrement de Vigenère
                    char chiffre = (char)(((c - offset + decalage) % 26) + offset);
                    resultat.Append(chiffre);
                    
                    // Avancer dans la clé
                    cleIndex++;
                }
                else
                {
                    // Conserver les caractères non alphabétiques
                    resultat.Append(c);
                }
            }
            
            return resultat.ToString();
        }

        /// <summary>
        /// Déchiffre un message avec la méthode de Vigenère
        /// </summary>
        /// <param name="messageChiffre">Message chiffré</param>
        /// <param name="cle">Clé de chiffrement</param>
        /// <returns>Message déchiffré</returns>
        private string Dechiffrer(string messageChiffre, string cle)
        {
            StringBuilder resultat = new StringBuilder();
            int cleIndex = 0;
            
            foreach (char c in messageChiffre)
            {
                if (char.IsLetter(c))
                {
                    // Normaliser les caractères
                    char offset = char.IsUpper(c) ? 'A' : 'a';
                    char cleChar = char.ToUpper(cle[cleIndex % cle.Length]);
                    int decalage = cleChar - 'A';
                    
                    // Appliquer le déchiffrement de Vigenère
                    char dechiffre = (char)(((c - offset - decalage + 26) % 26) + offset);
                    resultat.Append(dechiffre);
                    
                    // Avancer dans la clé
                    cleIndex++;
                }
                else
                {
                    // Conserver les caractères non alphabétiques
                    resultat.Append(c);
                }
            }
            
            return resultat.ToString();
        }

        /// <summary>
        /// Sélectionne un niveau de difficulté approprié pour la clé
        /// </summary>
        /// <param name="niveau">Niveau de difficulté (facile, moyen, difficile, expert)</param>
        /// <returns>Clé sélectionnée</returns>
        private string SelectionnerCle(string niveau)
        {
            switch (niveau.ToLower())
            {
                case "facile":
                    // Clés courtes pour le niveau facile
                    return clesPossibles[random.Next(0, 3)];
                case "moyen":
                    // Clés moyennes
                    return clesPossibles[random.Next(2, 5)];
                case "difficile":
                    // Clés longues
                    return clesPossibles[random.Next(4, 7)];
                case "expert":
                    // Clés très longues
                    return clesPossibles[random.Next(6, clesPossibles.Length)];
                default:
                    return clesPossibles[0];
            }
        }

        /// <summary>
        /// Joue une partie du jeu du chiffrement de Vigenère
        /// </summary>
        /// <param name="joueur">Le joueur qui joue la partie</param>
        /// <param name="niveau">Niveau de difficulté</param>
        /// <returns>True si la partie est gagnée, False sinon</returns>
        public bool jouerVigenere(Joueur joueur, string niveau)
        {
            string message = messagesPossibles[random.Next(messagesPossibles.Length)];
            string cle = SelectionnerCle(niveau);
            string messageChiffre = Chiffrer(message, cle);

            Console.WriteLine($"Bienvenue dans le jeu du Chiffrement de Vigenère, {joueur.GetNom()}!");
            Console.WriteLine("Dans ce jeu, vous devez trouver la clé qui a été utilisée pour chiffrer un message.");
            Console.WriteLine($"Niveau: {niveau}");
            Console.WriteLine("\nVoici le message chiffré:");
            Console.WriteLine(messageChiffre);
            
            // Donner un indice en fonction du niveau
            if (niveau.ToLower() == "facile")
            {
                Console.WriteLine($"\nIndice: La clé contient {cle.Length} lettres.");
            }
            else if (niveau.ToLower() == "moyen")
            {
                Console.WriteLine($"\nIndice: La première lettre de la clé est '{cle[0]}'.");
            }

            Stopwatch chrono = new Stopwatch();
            chrono.Start();

            int essais = 0;
            int maxEssais = GetMaxEssais(niveau);
            bool gagne = false;

            while (essais < maxEssais && !gagne)
            {
                Console.Write($"\nEssai {essais + 1}/{maxEssais} - Entrez la clé que vous pensez avoir été utilisée: ");
                string cleProposee = Console.ReadLine()?.ToUpper() ?? "";
                
                if (string.IsNullOrEmpty(cleProposee))
                {
                    Console.WriteLine("Veuillez entrer une clé valide.");
                    continue;
                }

                essais++;

                if (cleProposee == cle)
                {
                    gagne = true;
                    Console.WriteLine("\nFélicitations ! Vous avez trouvé la bonne clé !");
                    Console.WriteLine($"Message déchiffré: {message}");
                }
                else
                {
                    // Déchiffrer avec la clé proposée pour montrer le résultat
                    string tentative = Dechiffrer(messageChiffre, cleProposee);
                    Console.WriteLine($"Ce n'est pas la bonne clé. Voici ce que ça donne:");
                    Console.WriteLine(tentative);
                }
            }

            chrono.Stop();
            TimeSpan tempsEffectue = chrono.Elapsed;

            if (!gagne)
            {
                Console.WriteLine($"\nDommage ! Vous n'avez pas trouvé la bonne clé qui était: {cle}");
                Console.WriteLine($"Le message original était: {message}");
            }

            // Calculer le score
            int score = CalculerScore(gagne, essais, tempsEffectue, niveau);

            // Mettre à jour la date de dernière connexion
            SetDerniereConnexion(DateTime.Now);

            // Appeler les méthodes demandées
            resultatsJeu(score, tempsEffectue, 1);

            return gagne;
        }

        /// <summary>
        /// Calcule le score en fonction du résultat, du nombre d'essais, du temps et du niveau
        /// </summary>
        /// <param name="gagne">True si la partie est gagnée, False sinon</param>
        /// <param name="nbEssais">Nombre d'essais effectués</param>
        /// <param name="temps">Temps effectué</param>
        /// <param name="niveau">Niveau de difficulté</param>
        /// <returns>Score calculé</returns>
        private int CalculerScore(bool gagne, int nbEssais, TimeSpan temps, string niveau)
        {
            if (!gagne)
                return 0;

            // Base de 1000 points pour une victoire
            int baseScore = 1000;
            
            // Facteur de difficulté
            int facteurDifficulte = GetFacteurDifficulte(niveau);
            
            // Moins de points pour plus d'essais
            int maxEssais = GetMaxEssais(niveau);
            int essaisScore = (maxEssais - nbEssais + 1) * 100;
            
            // Moins de points pour un temps plus long (max 3 minutes)
            int tempsScore = Math.Max(0, 500 - (int)(temps.TotalSeconds * 3));
            
            return (baseScore + essaisScore + tempsScore) * facteurDifficulte / 100;
        }

        /// <summary>
        /// Retourne le nombre maximum d'essais en fonction du niveau
        /// </summary>
        /// <param name="niveau">Niveau de difficulté</param>
        /// <returns>Nombre maximum d'essais</returns>
        private int GetMaxEssais(string niveau)
        {
            switch (niveau.ToLower())
            {
                case "facile":
                    return 5;
                case "moyen":
                    return 4;
                case "difficile":
                    return 3;
                case "expert":
                    return 2;
                default:
                    return 5;
            }
        }

        /// <summary>
        /// Retourne le facteur de difficulté en fonction du niveau
        /// </summary>
        /// <param name="niveau">Niveau de difficulté</param>
        /// <returns>Facteur de difficulté (en pourcentage)</returns>
        private int GetFacteurDifficulte(string niveau)
        {
            switch (niveau.ToLower())
            {
                case "facile":
                    return 100;
                case "moyen":
                    return 150;
                case "difficile":
                    return 200;
                case "expert":
                    return 300;
                default:
                    return 100;
            }
        }
    }
}