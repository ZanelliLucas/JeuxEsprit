using System;
using System.Security.Cryptography;
using System.Text;

namespace JeuxDesprit
{
    /// <summary>
    /// Classe gérant l'authentification des utilisateurs
    /// </summary>
    public class AuthManager
    {
        private readonly DatabaseManager dbManager;

        /// <summary>
        /// Constructeur de la classe AuthManager
        /// </summary>
        /// <param name="dbManager">Gestionnaire de base de données</param>
        public AuthManager(DatabaseManager dbManager)
        {
            this.dbManager = dbManager;
        }

        /// <summary>
        /// Connecte un utilisateur en vérifiant ses identifiants
        /// </summary>
        /// <param name="email">Email de l'utilisateur</param>
        /// <param name="motDePasse">Mot de passe de l'utilisateur</param>
        /// <returns>Joueur connecté ou null si échec</returns>
        public Joueur? Connexion(string email, string motDePasse)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(motDePasse))
            {
                Console.WriteLine("L'email et le mot de passe sont requis.");
                return null;
            }
            
            // Hacher le mot de passe pour le comparer avec celui stocké en base
            string motDePasseHache = HashMotDePasse(motDePasse);
            
            // Vérifier les identifiants
            return dbManager.VerifierConnexion(email, motDePasseHache);
        }

        /// <summary>
        /// Crée un nouveau compte utilisateur
        /// </summary>
        /// <param name="nom">Nom de l'utilisateur</param>
        /// <param name="email">Email de l'utilisateur</param>
        /// <param name="motDePasse">Mot de passe de l'utilisateur</param>
        /// <param name="avatar">Avatar de l'utilisateur</param>
        /// <param name="type">Type de joueur (amateur/professionnel)</param>
        /// <returns>ID de l'utilisateur créé ou -1 si échec</returns>
        public int CreerCompte(string nom, string email, string motDePasse, string avatar, string type)
        {
            if (string.IsNullOrEmpty(nom) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(motDePasse))
            {
                Console.WriteLine("Le nom, l'email et le mot de passe sont requis.");
                return -1;
            }
            
            // Vérifier si l'email existe déjà
            if (dbManager.EmailExiste(email))
            {
                Console.WriteLine("Un compte avec cet email existe déjà.");
                return -1;
            }
            
            // Hacher le mot de passe
            string motDePasseHache = HashMotDePasse(motDePasse);
            
            // Créer le joueur
            Joueur joueur;
            if (type.ToLower() == "professionnel")
            {
                joueur = new Professionnel(nom, email, avatar ?? "default_avatar.png");
            }
            else
            {
                joueur = new Amateur(nom, email, avatar ?? "default_avatar.png");
            }
            
            // Ajouter le joueur avec son mot de passe haché
            return dbManager.AjouterJoueurAvecMotDePasse(joueur, motDePasseHache);
        }

        /// <summary>
        /// Hache un mot de passe en utilisant SHA256
        /// </summary>
        /// <param name="motDePasse">Mot de passe en clair</param>
        /// <returns>Mot de passe haché</returns>
        private string HashMotDePasse(string motDePasse)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                // Convertir le mot de passe en bytes
                byte[] bytes = Encoding.UTF8.GetBytes(motDePasse);
                
                // Hacher les bytes
                byte[] hash = sha256.ComputeHash(bytes);
                
                // Convertir le hash en string hexadécimal
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hash.Length; i++)
                {
                    builder.Append(hash[i].ToString("x2"));
                }
                
                return builder.ToString();
            }
        }
    }
}