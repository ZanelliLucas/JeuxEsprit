using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace JeuxDesprit
{
    /// <summary>
    /// Classe gérant les connexions à la base de données
    /// </summary>
    public class DatabaseManager
    {
        private readonly string connectionString;

        /// <summary>
        /// Constructeur de la classe DatabaseManager
        /// </summary>
        /// <param name="server">Serveur MySQL</param>
        /// <param name="user">Nom d'utilisateur</param>
        /// <param name="database">Nom de la base de données</param>
        /// <param name="password">Mot de passe</param>
        public DatabaseManager(string server = "localhost", string user = "root", string database = "JeuxdEsprit", string password = "root")
        {
            this.connectionString = $"server={server};user={user};database={database};password={password};";
        }

        /// <summary>
        /// Obtient une connexion à la base de données
        /// </summary>
        /// <returns>Connexion MySQL</returns>
        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }

        /// <summary>
        /// Teste la connexion à la base de données
        /// </summary>
        /// <returns>True si la connexion est réussie, False sinon</returns>
        public bool TestConnection()
        {
            try
            {
                using (MySqlConnection connection = GetConnection())
                {
                    connection.Open();
                    Console.WriteLine("Connexion à la base de données réussie !");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la connexion à la base de données : {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Récupère la liste des niveaux de difficulté
        /// </summary>
        /// <returns>Liste des niveaux</returns>
        public List<string> GetNiveaux()
        {
            List<string> niveaux = new List<string>();
            try
            {
                using (MySqlConnection connection = GetConnection())
                {
                    connection.Open();
                    string query = "SELECT libelle FROM Niveau";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                niveaux.Add(reader.GetString(0));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la récupération des niveaux : {ex.Message}");
            }
            return niveaux;
        }

        /// <summary>
        /// Récupère la liste des joueurs
        /// </summary>
        /// <returns>Liste des joueurs (ID, nom)</returns>
        public Dictionary<int, string> GetJoueurs()
        {
            Dictionary<int, string> joueurs = new Dictionary<int, string>();
            try
            {
                using (MySqlConnection connection = GetConnection())
                {
                    connection.Open();
                    string query = "SELECT idJoueur, nom FROM Joueur";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                joueurs.Add(reader.GetInt32(0), reader.GetString(1));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la récupération des joueurs : {ex.Message}");
            }
            return joueurs;
        }

        /// <summary>
        /// Récupère les informations d'un joueur par son ID
        /// </summary>
        /// <param name="idJoueur">ID du joueur</param>
        /// <returns>Joueur (null si non trouvé)</returns>
        public Joueur GetJoueurById(int idJoueur)
        {
            try
            {
                using (MySqlConnection connection = GetConnection())
                {
                    connection.Open();
                    string query = "SELECT nom, email, avatar, type FROM Joueur WHERE idJoueur = @idJoueur";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@idJoueur", idJoueur);
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string nom = reader.GetString(0);
                                string email = reader.GetString(1);
                                string avatar = reader.GetString(2);
                                string type = reader.GetString(3);

                                if (type.ToLower() == "amateur")
                                {
                                    return new Amateur(nom, email, avatar);
                                }
                                else if (type.ToLower() == "professionnel")
                                {
                                    return new Professionnel(nom, email, avatar);
                                }
                                else
                                {
                                    return new Joueur(nom, email, avatar);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la récupération du joueur : {ex.Message}");
            }
            return new Joueur("Inconnu", "inconnu@example.com", "default_avatar.png");
        }

        /// <summary>
        /// Récupère la liste des jeux
        /// </summary>
        /// <returns>Liste des jeux (ID, nom)</returns>
        public Dictionary<int, string> GetJeux()
        {
            Dictionary<int, string> jeux = new Dictionary<int, string>();
            try
            {
                using (MySqlConnection connection = GetConnection())
                {
                    connection.Open();
                    string query = "SELECT idJeu, nom FROM Jeu";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                jeux.Add(reader.GetInt32(0), reader.GetString(1));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la récupération des jeux : {ex.Message}");
            }
            return jeux;
        }

        /// <summary>
        /// Récupère la liste des types de jeu
        /// </summary>
        /// <returns>Liste des types de jeu (ID, libellé)</returns>
        public Dictionary<int, string> GetTypesJeu()
        {
            Dictionary<int, string> typesJeu = new Dictionary<int, string>();
            try
            {
                using (MySqlConnection connection = GetConnection())
                {
                    connection.Open();
                    string query = "SELECT idType, libelle FROM Type";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                typesJeu.Add(reader.GetInt32(0), reader.GetString(1));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la récupération des types de jeu : {ex.Message}");
            }
            return typesJeu;
        }

        /// <summary>
        /// Ajoute un nouveau joueur dans la base de données
        /// </summary>
        /// <param name="joueur">Joueur à ajouter</param>
        /// <returns>ID du joueur ajouté (-1 en cas d'erreur)</returns>
        public int AjouterJoueur(Joueur joueur)
        {
            try
            {
                using (MySqlConnection connection = GetConnection())
                {
                    connection.Open();
                    string type = joueur is Amateur ? "amateur" : (joueur is Professionnel ? "professionnel" : "standard");
                    string query = "INSERT INTO Joueur (nom, email, avatar, type) VALUES (@nom, @email, @avatar, @type)";
                    
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@nom", joueur.GetNom());
                        command.Parameters.AddWithValue("@email", joueur.GetEmail());
                        command.Parameters.AddWithValue("@avatar", joueur.GetAvatar());
                        command.Parameters.AddWithValue("@type", type);
                        
                        command.ExecuteNonQuery();
                        return (int)command.LastInsertedId;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'ajout du joueur : {ex.Message}");
                return -1;
            }
        }

        /// <summary>
        /// Enregistre une partie dans la base de données
        /// </summary>
        /// <param name="idJeu">ID du jeu</param>
        /// <param name="idJoueur">ID du joueur</param>
        /// <param name="idNiveau">ID du niveau</param>
        /// <param name="statut">Statut de la partie (gagné/perdu)</param>
        /// <param name="score">Score obtenu</param>
        /// <param name="temps">Temps effectué</param>
        /// <returns>ID de la partie enregistrée (-1 en cas d'erreur)</returns>
        public int EnregistrerPartie(int idJeu, int idJoueur, int idNiveau, string statut, int score, TimeSpan temps)
        {
            try
            {
                using (MySqlConnection connection = GetConnection())
                {
                    connection.Open();
                    
                    // Ajouter la partie
                    string queryPartie = "INSERT INTO Partie (idJeu, statut, datePartie, score, temps) " +
                                         "VALUES (@idJeu, @statut, @datePartie, @score, @temps)";
                    
                    using (MySqlCommand command = new MySqlCommand(queryPartie, connection))
                    {
                        command.Parameters.AddWithValue("@idJeu", idJeu);
                        command.Parameters.AddWithValue("@statut", statut);
                        command.Parameters.AddWithValue("@datePartie", DateTime.Now);
                        command.Parameters.AddWithValue("@score", score);
                        command.Parameters.AddWithValue("@temps", temps.TotalSeconds);
                        
                        command.ExecuteNonQuery();
                        int idPartie = (int)command.LastInsertedId;
                        
                        // Lier la partie au joueur
                        string queryJouer = "INSERT INTO Jouer (idJoueur, idPartie, idNiveau) " +
                                           "VALUES (@idJoueur, @idPartie, @idNiveau)";
                        
                        using (MySqlCommand commandJouer = new MySqlCommand(queryJouer, connection))
                        {
                            commandJouer.Parameters.AddWithValue("@idJoueur", idJoueur);
                            commandJouer.Parameters.AddWithValue("@idPartie", idPartie);
                            commandJouer.Parameters.AddWithValue("@idNiveau", idNiveau);
                            
                            commandJouer.ExecuteNonQuery();
                        }
                        
                        return idPartie;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'enregistrement de la partie : {ex.Message}");
                return -1;
            }
        }
    }
}