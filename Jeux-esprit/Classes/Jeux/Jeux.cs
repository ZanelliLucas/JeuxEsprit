using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace JeuxDesprit
{
    /// <summary>
    /// Classe mère pour tous les jeux d'esprit
    /// </summary>
    public class Jeux
    {
        // Attributs
        protected string nom;
        protected string principeDuJeu;
        protected int nombreDeJoueurs;
        protected DateTime derniereConnexion;
        protected DatabaseManager dbManager;

        // Constructeur
        public Jeux(string nom, string principeDuJeu, int nombreDeJoueurs)
        {
            this.nom = nom;
            this.principeDuJeu = principeDuJeu;
            this.nombreDeJoueurs = nombreDeJoueurs;
            this.derniereConnexion = DateTime.Now;
            this.dbManager = new DatabaseManager();
        }

        // Getters et Setters
        public string GetNom() { return nom; }
        public void SetNom(string nom) { this.nom = nom; }

        public string GetPrincipeDuJeu() { return principeDuJeu; }
        public void SetPrincipeDuJeu(string principeDuJeu) { this.principeDuJeu = principeDuJeu; }

        public int GetNombreDeJoueurs() { return nombreDeJoueurs; }
        public void SetNombreDeJoueurs(int nombreDeJoueurs) { this.nombreDeJoueurs = nombreDeJoueurs; }

        public DateTime GetDerniereConnexion() { return derniereConnexion; }
        public void SetDerniereConnexion(DateTime derniereConnexion) { this.derniereConnexion = derniereConnexion; }

        // Méthodes
        /// <summary>
        /// Affiche les informations du jeu
        /// </summary>
        public void affichageInfoJeu()
        {
            Console.WriteLine("Informations sur le jeu :");
            Console.WriteLine($"Nom : {nom}");
            Console.WriteLine($"Principe du jeu : {principeDuJeu}");
            Console.WriteLine($"Nombre de joueurs : {nombreDeJoueurs}");
            Console.WriteLine($"Dernière connexion : {derniereConnexion}");
        }

        /// <summary>
        /// Affiche les résultats d'une partie
        /// </summary>
        /// <param name="score">Score obtenu</param>
        /// <param name="tempsEffectue">Temps effectué pour réaliser la partie</param>
        /// <param name="nbParties">Nombre de parties réalisées</param>
        public void resultatsJeu(int score, TimeSpan tempsEffectue, int nbParties)
        {
            Console.WriteLine("Résultats de la partie :");
            Console.WriteLine($"Score : {score}");
            Console.WriteLine($"Temps effectué : {tempsEffectue.TotalSeconds} secondes");
            Console.WriteLine($"Nombre de parties réalisées : {nbParties}");
        }

        /// <summary>
        /// Affiche le nombre de victoires à une date donnée
        /// </summary>
        /// <param name="date">Date de recherche</param>
        public void afficherNbVictoireAUneDate(DateTime date)
        {
            try
            {
                string query = "SELECT COUNT(*) FROM Partie WHERE DATE(datePartie) = @date AND statut = 'gagné'";
                MySqlConnection connection = dbManager.GetConnection();
                connection.Open();

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@date", date.Date);
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    Console.WriteLine($"Nombre de victoires le {date.ToShortDateString()} : {count}");
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la récupération des victoires : {ex.Message}");
            }
        }

        /// <summary>
        /// Affiche le nombre de victoires pour un joueur donné
        /// </summary>
        /// <param name="idJoueur">Identifiant du joueur</param>
        public void afficherNbVictoireJoueur(int idJoueur)
        {
            try
            {
                string query = "SELECT COUNT(*) FROM Jouer " +
                               "JOIN Partie ON Jouer.idPartie = Partie.idPartie " +
                               "WHERE Jouer.idJoueur = @idJoueur AND Partie.statut = 'gagné'";
                MySqlConnection connection = dbManager.GetConnection();
                connection.Open();

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idJoueur", idJoueur);
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    Console.WriteLine($"Nombre de victoires pour le joueur ID {idJoueur} : {count}");
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la récupération des victoires : {ex.Message}");
            }
        }

        /// <summary>
        /// Affiche le nombre de victoires pour un type d'épreuve donné
        /// </summary>
        /// <param name="idType">Identifiant du type d'épreuve</param>
        public void afficherNbVictoireParType(int idType)
        {
            try
            {
                string query = "SELECT COUNT(*) FROM Partie " +
                               "JOIN Jeu ON Partie.idJeu = Jeu.idJeu " +
                               "WHERE Jeu.idType = @idType AND Partie.statut = 'gagné'";
                MySqlConnection connection = dbManager.GetConnection();
                connection.Open();

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idType", idType);
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    Console.WriteLine($"Nombre de victoires pour le type d'épreuve ID {idType} : {count}");
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la récupération des victoires : {ex.Message}");
            }
        }

        /// <summary>
        /// Affiche le nombre de victoires pour un joueur donné, un type d'épreuve donné, à une date donnée
        /// </summary>
        /// <param name="idJoueur">Identifiant du joueur</param>
        /// <param name="idType">Identifiant du type d'épreuve</param>
        /// <param name="date">Date de recherche</param>
        public void afficherNbVictoireJoueurParTypeAUneDate(int idJoueur, int idType, DateTime date)
        {
            try
            {
                string query = "SELECT COUNT(*) FROM Jouer " +
                               "JOIN Partie ON Jouer.idPartie = Partie.idPartie " +
                               "JOIN Jeu ON Partie.idJeu = Jeu.idJeu " +
                               "WHERE Jouer.idJoueur = @idJoueur AND Jeu.idType = @idType " +
                               "AND DATE(Partie.datePartie) = @date AND Partie.statut = 'gagné'";
                MySqlConnection connection = dbManager.GetConnection();
                connection.Open();

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idJoueur", idJoueur);
                    command.Parameters.AddWithValue("@idType", idType);
                    command.Parameters.AddWithValue("@date", date.Date);
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    Console.WriteLine($"Nombre de victoires pour le joueur ID {idJoueur}, de type {idType}, le {date.ToShortDateString()} : {count}");
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la récupération des victoires : {ex.Message}");
            }
        }

        /// <summary>
        /// Affiche le nombre de parties jouées pour un type de jeu donné
        /// </summary>
        /// <param name="idType">Identifiant du type de jeu</param>
        public void afficherNbPartieJouéeParType(int idType)
        {
            try
            {
                string query = "SELECT COUNT(*) FROM Partie " +
                               "JOIN Jeu ON Partie.idJeu = Jeu.idJeu " +
                               "WHERE Jeu.idType = @idType";
                MySqlConnection connection = dbManager.GetConnection();
                connection.Open();

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idType", idType);
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    Console.WriteLine($"Nombre de parties jouées pour le type de jeu ID {idType} : {count}");
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la récupération des parties : {ex.Message}");
            }
        }

        /// <summary>
        /// Affiche le nombre de parties jouées à une date donnée et un type d'épreuve donné
        /// </summary>
        /// <param name="date">Date de recherche</param>
        /// <param name="idType">Identifiant du type d'épreuve</param>
        public void afficherNbPartieJouéeAUneDateParType(DateTime date, int idType)
        {
            try
            {
                string query = "SELECT COUNT(*) FROM Partie " +
                               "JOIN Jeu ON Partie.idJeu = Jeu.idJeu " +
                               "WHERE DATE(Partie.datePartie) = @date AND Jeu.idType = @idType";
                MySqlConnection connection = dbManager.GetConnection();
                connection.Open();

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@date", date.Date);
                    command.Parameters.AddWithValue("@idType", idType);
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    Console.WriteLine($"Nombre de parties jouées le {date.ToShortDateString()} pour le type de jeu ID {idType} : {count}");
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la récupération des parties : {ex.Message}");
            }
        }

        /// <summary>
        /// Affiche le nombre de parties jouées par joueur à une date donnée
        /// </summary>
        /// <param name="idJoueur">Identifiant du joueur</param>
        /// <param name="date">Date de recherche</param>
        public void afficherNbPartieJouéeJoueurAUneDate(int idJoueur, DateTime date)
        {
            try
            {
                string query = "SELECT COUNT(*) FROM Jouer " +
                               "JOIN Partie ON Jouer.idPartie = Partie.idPartie " +
                               "WHERE Jouer.idJoueur = @idJoueur AND DATE(Partie.datePartie) = @date";
                MySqlConnection connection = dbManager.GetConnection();
                connection.Open();

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idJoueur", idJoueur);
                    command.Parameters.AddWithValue("@date", date.Date);
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    Console.WriteLine($"Nombre de parties jouées par le joueur ID {idJoueur} le {date.ToShortDateString()} : {count}");
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la récupération des parties : {ex.Message}");
            }
        }
    }
}