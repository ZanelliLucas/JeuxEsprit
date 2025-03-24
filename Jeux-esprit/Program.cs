﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace JeuxDesprit
{
    class Program
    {
        private static DatabaseManager dbManager = null!;
        private static AuthManager authManager = null!;
        private static Joueur? joueurActuel;
        private static bool applicationRunning = true;
        private static bool estConnecte = false;

        static void Main(string[] args)
        {
            Console.WriteLine("Bienvenue dans l'application Jeux d'Esprit!");
            
            try
            {
                // Initialiser le gestionnaire de base de données
                dbManager = new DatabaseManager();
                
                // Initialiser le gestionnaire d'authentification
                authManager = new AuthManager(dbManager);
                
                // Tester la connexion à la base de données
                if (!dbManager.TestConnection())
                {
                    Console.WriteLine("Impossible de se connecter à la base de données. L'application va s'exécuter sans persistance des données.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur d'initialisation: {ex.Message}");
                Console.WriteLine("L'application fonctionnera en mode dégradé sans base de données.");
                // Création d'un DatabaseManager en mode mémoire
                dbManager = new DatabaseManager("localhost", "root", "JeuxdEsprit", "root");
                authManager = new AuthManager(dbManager);
            }
            
            // Afficher l'écran d'accueil avec connexion/inscription
            while (applicationRunning)
            {
                if (!estConnecte)
                {
                    AfficherEcranAccueil();
                }
                else
                {
                    AfficherMenuPrincipal();
                    TraiterChoixMenuPrincipal();
                }
            }
            
            Console.WriteLine("Merci d'avoir utilisé Jeux d'Esprit. À bientôt !");
        }

        /// <summary>
        /// Affiche l'écran d'accueil avec les options de connexion et d'inscription
        /// </summary>
        static void AfficherEcranAccueil()
        {
            Console.Clear();
            Console.WriteLine("\n========== JEUX D'ESPRIT ==========");
            Console.WriteLine("1- Se connecter");
            Console.WriteLine("2- Créer un compte");
            Console.WriteLine("3- Mode invité");
            Console.WriteLine("4- Quitter l'application");
            Console.WriteLine("===================================");
            Console.Write("Votre choix : ");
            
            string? choix = Console.ReadLine();
            
            switch (choix)
            {
                case "1":
                    SeConnecter();
                    break;
                case "2":
                    CreerCompte();
                    break;
                case "3":
                    ConnexionInvite();
                    break;
                case "4":
                    applicationRunning = false;
                    break;
                default:
                    Console.WriteLine("Choix invalide. Veuillez réessayer.");
                    Console.ReadKey();
                    break;
            }
        }

        /// <summary>
        /// Permet une connexion en mode invité pour tester l'application sans base de données
        /// </summary>
        static void ConnexionInvite()
        {
            joueurActuel = new Amateur("Invité", "invite@example.com", "default_avatar.png");
            Console.WriteLine("\nConnexion en mode invité réussie !");
            estConnecte = true;
            Console.ReadKey();
        }

        /// <summary>
        /// Gère le processus de connexion
        /// </summary>
        static void SeConnecter()
        {
            Console.Clear();
            Console.WriteLine("\n========== CONNEXION ==========");
            
            Console.Write("Email : ");
            string? email = Console.ReadLine() ?? "";
            
            Console.Write("Mot de passe : ");
            string motDePasse = MasquerMotDePasse();
            
            try
            {
                joueurActuel = authManager.Connexion(email, motDePasse);
                
                if (joueurActuel != null)
                {
                    Console.WriteLine("\nConnexion réussie !");
                    estConnecte = true;
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("\nÉchec de la connexion. Vérifiez vos informations.");
                    Console.ReadKey();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nErreur lors de la connexion : {ex.Message}");
                Console.WriteLine("Voulez-vous vous connecter en mode invité ? (O/N)");
                string? reponse = Console.ReadLine()?.ToUpper();
                if (reponse == "O")
                {
                    ConnexionInvite();
                }
                else
                {
                    Console.ReadKey();
                }
            }
        }

        /// <summary>
        /// Gère le processus de création de compte
        /// </summary>
        static void CreerCompte()
        {
            Console.Clear();
            Console.WriteLine("\n========== CRÉATION DE COMPTE ==========");
            
            Console.Write("Nom : ");
            string? nom = Console.ReadLine() ?? "";
            
            Console.Write("Email : ");
            string? email = Console.ReadLine() ?? "";
            
            Console.Write("Mot de passe : ");
            string motDePasse = MasquerMotDePasse();
            
            Console.Write("Avatar (description ou chemin) : ");
            string? avatar = Console.ReadLine();
            
            Console.WriteLine("Type de joueur :");
            Console.WriteLine("1 - Amateur");
            Console.WriteLine("2 - Professionnel");
            Console.Write("Votre choix : ");
            string? typeChoix = Console.ReadLine();
            
            string type = typeChoix == "2" ? "professionnel" : "amateur";
            
            try
            {
                int idJoueur = authManager.CreerCompte(nom, email, motDePasse, avatar ?? "default_avatar.png", type);
                
                if (idJoueur != -1)
                {
                    Console.WriteLine("\nCompte créé avec succès ! Vous pouvez maintenant vous connecter.");
                }
                else
                {
                    Console.WriteLine("\nÉchec de la création du compte. Veuillez réessayer.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nErreur lors de la création du compte : {ex.Message}");
            }
            
            Console.ReadKey();
        }

        /// <summary>
        /// Masque l'entrée du mot de passe en affichant des astérisques
        /// </summary>
        /// <returns>Mot de passe saisi</returns>
        static string MasquerMotDePasse()
        {
            string motDePasse = "";
            ConsoleKeyInfo key;
            
            do
            {
                key = Console.ReadKey(true);
                
                // Ignorer les touches spéciales comme Enter, Backspace, etc.
                if (key.Key != ConsoleKey.Enter && key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Escape)
                {
                    motDePasse += key.KeyChar;
                    Console.Write("*");
                }
                else if (key.Key == ConsoleKey.Backspace && motDePasse.Length > 0)
                {
                    motDePasse = motDePasse.Substring(0, motDePasse.Length - 1);
                    Console.Write("\b \b"); // Efface le dernier caractère
                }
            } while (key.Key != ConsoleKey.Enter);
            
            Console.WriteLine();
            return motDePasse;
        }

        /// <summary>
        /// Déconnecte l'utilisateur actuel
        /// </summary>
        static void Deconnecter()
        {
            joueurActuel = null;
            estConnecte = false;
            Console.WriteLine("\nVous avez été déconnecté avec succès.");
            Console.ReadKey();
        }

        /// <summary>
        /// Affiche le menu principal de l'application
        /// </summary>
        static void AfficherMenuPrincipal()
        {
            Console.Clear();
            if (joueurActuel != null)
            {
                Console.WriteLine($"\nUtilisateur connecté : {joueurActuel.GetNom()}");
            }
            
            Console.WriteLine("\n========== MENU PRINCIPAL ==========");
            Console.WriteLine("1- Jeux");
            Console.WriteLine("2- Joueurs");
            Console.WriteLine("3- Types de jeu");
            Console.WriteLine("4- Jouer");
            Console.WriteLine("5- Consulter l'historique des jeux");
            Console.WriteLine("6- Se déconnecter");
            Console.WriteLine("7- Quitter l'application");
            Console.WriteLine("====================================");
            Console.Write("Votre choix : ");
        }

        /// <summary>
        /// Traite le choix de l'utilisateur dans le menu principal
        /// </summary>
        static void TraiterChoixMenuPrincipal()
        {
            string? choix = Console.ReadLine();
            
            switch (choix)
            {
                case "1":
                    MenuJeux();
                    break;
                case "2":
                    MenuJoueurs();
                    break;
                case "3":
                    MenuTypesJeu();
                    break;
                case "4":
                    MenuJouer();
                    break;
                case "5":
                    MenuHistorique();
                    break;
                case "6":
                    Deconnecter();
                    break;
                case "7":
                    applicationRunning = false;
                    break;
                default:
                    Console.WriteLine("Choix invalide. Veuillez réessayer.");
                    Console.ReadKey();
                    break;
            }
        }

        /// <summary>
        /// Affiche et gère le menu des jeux
        /// </summary>
        static void MenuJeux()
        {
            bool menuJeuxRunning = true;
            
            while (menuJeuxRunning)
            {
                Console.Clear();
                Console.WriteLine("\n========== MENU JEUX ==========");
                Console.WriteLine("a. Ajouter un jeu");
                Console.WriteLine("b. Modifier un jeu");
                Console.WriteLine("c. Revenir au menu");
                Console.WriteLine("===============================");
                Console.Write("Votre choix : ");
                
                string? choix = Console.ReadLine()?.ToLower();
                
                switch (choix)
                {
                    case "a":
                        Console.WriteLine("Fonctionnalité 'Ajouter un jeu' non implémentée dans cette version.");
                        Console.ReadKey();
                        break;
                    case "b":
                        Console.WriteLine("Fonctionnalité 'Modifier un jeu' non implémentée dans cette version.");
                        Console.ReadKey();
                        break;
                    case "c":
                        menuJeuxRunning = false;
                        break;
                    default:
                        Console.WriteLine("Choix invalide. Veuillez réessayer.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        /// <summary>
        /// Affiche et gère le menu des joueurs
        /// </summary>
        static void MenuJoueurs()
        {
            bool menuJoueursRunning = true;
            
            while (menuJoueursRunning)
            {
                Console.Clear();
                Console.WriteLine("\n========== MENU JOUEURS ==========");
                Console.WriteLine("a. Ajouter un joueur");
                Console.WriteLine("b. Modifier un joueur");
                Console.WriteLine("c. Revenir au menu");
                Console.WriteLine("==================================");
                Console.Write("Votre choix : ");
                
                string? choix = Console.ReadLine()?.ToLower();
                
                switch (choix)
                {
                    case "a":
                        AjouterJoueur();
                        break;
                    case "b":
                        Console.WriteLine("Fonctionnalité 'Modifier un joueur' non implémentée dans cette version.");
                        Console.ReadKey();
                        break;
                    case "c":
                        menuJoueursRunning = false;
                        break;
                    default:
                        Console.WriteLine("Choix invalide. Veuillez réessayer.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        /// <summary>
        /// Ajoute un nouveau joueur
        /// </summary>
        static void AjouterJoueur()
        {
            Console.Clear();
            Console.WriteLine("\n=== AJOUTER UN JOUEUR ===");
            
            Console.Write("Nom : ");
            string nom = Console.ReadLine() ?? "";
            
            Console.Write("Email : ");
            string email = Console.ReadLine() ?? "";
            
            Console.Write("Avatar (description ou chemin) : ");
            string? avatar = Console.ReadLine();
            
            Console.WriteLine("Type de joueur :");
            Console.WriteLine("1 - Amateur");
            Console.WriteLine("2 - Professionnel");
            Console.Write("Votre choix : ");
            string? typeChoix = Console.ReadLine();
            
            Joueur nouveauJoueur;
            
            switch (typeChoix)
            {
                case "1":
                    Console.Write("Niveau d'expérience (1-5) : ");
                    if (int.TryParse(Console.ReadLine(), out int niveauExp))
                    {
                        nouveauJoueur = new Amateur(nom, email, avatar ?? "default_avatar.png", niveauExp);
                    }
                    else
                    {
                        nouveauJoueur = new Amateur(nom, email, avatar ?? "default_avatar.png");
                    }
                    break;
                case "2":
                    nouveauJoueur = new Professionnel(nom, email, avatar ?? "default_avatar.png");
                    break;
                default:
                    Console.WriteLine("Type invalide. Création d'un joueur standard.");
                    nouveauJoueur = new Joueur(nom, email, avatar ?? "default_avatar.png");
                    break;
            }
            
            try
            {
                int idJoueur = dbManager.AjouterJoueur(nouveauJoueur);
                
                if (idJoueur != -1)
                {
                    Console.WriteLine($"Joueur ajouté avec succès ! ID : {idJoueur}");
                }
                else
                {
                    Console.WriteLine("Erreur lors de l'ajout du joueur. Joueur créé en mémoire uniquement.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'ajout du joueur: {ex.Message}");
                Console.WriteLine("Joueur créé en mémoire uniquement.");
            }
            
            Console.ReadKey();
        }

        /// <summary>
        /// Affiche et gère le menu des types de jeu
        /// </summary>
        static void MenuTypesJeu()
        {
            bool menuTypesJeuRunning = true;
            
            while (menuTypesJeuRunning)
            {
                Console.Clear();
                Console.WriteLine("\n========== MENU TYPES DE JEU ==========");
                Console.WriteLine("a. Ajouter un type de jeu");
                Console.WriteLine("b. Modifier un type de jeu");
                Console.WriteLine("c. Consulter un type de jeu");
                Console.WriteLine("d. Revenir au menu");
                Console.WriteLine("=======================================");
                Console.Write("Votre choix : ");
                
                string? choix = Console.ReadLine()?.ToLower();
                
                switch (choix)
                {
                    case "a":
                        Console.WriteLine("Fonctionnalité 'Ajouter un type de jeu' non implémentée dans cette version.");
                        Console.ReadKey();
                        break;
                    case "b":
                        Console.WriteLine("Fonctionnalité 'Modifier un type de jeu' non implémentée dans cette version.");
                        Console.ReadKey();
                        break;
                    case "c":
                        ConsulterTypesJeu();
                        break;
                    case "d":
                        menuTypesJeuRunning = false;
                        break;
                    default:
                        Console.WriteLine("Choix invalide. Veuillez réessayer.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        /// <summary>
        /// Consulte les types de jeu disponibles
        /// </summary>
        static void ConsulterTypesJeu()
        {
            Console.Clear();
            Console.WriteLine("\n=== TYPES DE JEU DISPONIBLES ===");
            
            try
            {
                Dictionary<int, string> typesJeu = dbManager.GetTypesJeu();
                
                if (typesJeu.Count > 0)
                {
                    foreach (var type in typesJeu)
                    {
                        Console.WriteLine($"ID: {type.Key}, Libellé: {type.Value}");
                    }
                }
                else
                {
                    Console.WriteLine("Aucun type de jeu trouvé ou erreur de connexion à la base de données.");
                    Console.WriteLine("Types de jeu par défaut : Cérébral, Tactique");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la récupération des types de jeu: {ex.Message}");
                Console.WriteLine("Types de jeu par défaut : Cérébral, Tactique");
            }
            
            Console.ReadKey();
        }

        /// <summary>
        /// Affiche et gère le menu pour jouer
        /// </summary>
        static void MenuJouer()
        {
            if (joueurActuel == null)
            {
                Console.WriteLine("Erreur: Aucun joueur connecté.");
                Console.ReadKey();
                return;
            }
            
            Console.Clear();
            // Présenter la bienvenue au joueur
            joueurActuel.afficherJoueur();
            
            // Créer la collection de jeux
            var jeux = new List<string>() { "Plus ou Moins", "Pendu", "César", "Vigenère" };
            
            Console.WriteLine("\n=== JEUX DISPONIBLES ===");
            for (int i = 0; i < jeux.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {jeux[i]}");
            }
            
            // Demander le choix du jeu
            Console.Write("\nChoisissez un jeu (1-4) : ");
            if (!int.TryParse(Console.ReadLine(), out int choixJeu) || choixJeu < 1 || choixJeu > jeux.Count)
            {
                Console.WriteLine("Choix invalide. Retour au menu principal.");
                Console.ReadKey();
                return;
            }
            
            // Récupérer le jeu choisi
            string jeuChoisi = jeux[choixJeu - 1];
            
            // Instancier le jeu approprié
            Jeux? jeu = null;
            switch (jeuChoisi)
            {
                case "Plus ou Moins":
                    jeu = new PlusMoins();
                    break;
                case "Pendu":
                    jeu = new Pendu();
                    break;
                case "César":
                    jeu = new Cesar();
                    break;
                case "Vigenère":
                    jeu = new Vigenere();
                    break;
            }
            
            if (jeu == null)
            {
                Console.WriteLine("Erreur lors de la création du jeu. Retour au menu principal.");
                Console.ReadKey();
                return;
            }
            
            // Afficher les informations du jeu
            jeu.affichageInfoJeu();
            
            // Demander le niveau de jeu
            TypeNiveau typeNiveau = new TypeNiveau();
            string niveau = typeNiveau.statutNiveau();
            
            // Créer une nouvelle partie
            Partie partie = new Partie();
            
            // Exécuter le jeu approprié
            bool gagne = false;
            try
            {
                switch (jeuChoisi)
                {
                    case "Plus ou Moins":
                        gagne = ((PlusMoins)jeu).jouerPlusMoins(joueurActuel, niveau);
                        break;
                    case "Pendu":
                        gagne = ((Pendu)jeu).jouerPendu(joueurActuel, niveau);
                        break;
                    case "César":
                        gagne = ((Cesar)jeu).jouerCesar(joueurActuel, niveau);
                        break;
                    case "Vigenère":
                        gagne = ((Vigenere)jeu).jouerVigenere(joueurActuel, niveau);
                        break;
                }
                
                // Mettre à jour et afficher le statut de la partie
                partie.SetStatut(gagne ? "gagné" : "perdu");
                partie.statutPartie();
                
                // Si le joueur est professionnel et a gagné, incrémenter ses parties gagnées
                if (gagne && joueurActuel is Professionnel professionnel)
                {
                    professionnel.IncrementPartiesGagnees();
                }
                
                // Enregistrer la partie dans la base de données
                try
                {
                    int idJeu = dbManager.GetJeux().FirstOrDefault(j => j.Value == jeuChoisi).Key;
                    if (idJeu != 0)
                    {
                        int idJoueur = dbManager.GetJoueurs().FirstOrDefault(j => j.Value == joueurActuel.GetNom()).Key;
                        if (idJoueur == 0) // Joueur pas encore dans la BDD
                        {
                            idJoueur = dbManager.AjouterJoueur(joueurActuel);
                        }
                        
                        if (idJoueur != -1)
                        {
                            // Déterminer l'ID du niveau
                            int idNiveau = 1; // Par défaut facile
                            switch (niveau)
                            {
                                case "moyen": idNiveau = 2; break;
                                case "difficile": idNiveau = 3; break;
                                case "expert": idNiveau = 4; break;
                            }
                            
                            // Enregistrer la partie
                            dbManager.EnregistrerPartie(idJeu, idJoueur, idNiveau, partie.GetStatut(), partie.GetScore(), partie.GetTempsEffectue());
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur lors de l'enregistrement de la partie : {ex.Message}");
                    Console.WriteLine("La partie n'a pas été enregistrée dans la base de données.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'exécution du jeu : {ex.Message}");
            }
            
            Console.WriteLine("\nAppuyez sur une touche pour revenir au menu principal...");
            Console.ReadKey();
        }

        /// <summary>
        /// Affiche et gère le menu de l'historique des jeux
        /// </summary>
        static void MenuHistorique()
        {
            bool menuHistoriqueRunning = true;
            
            while (menuHistoriqueRunning)
            {
                Console.Clear();
                Console.WriteLine("\n========== HISTORIQUE DES JEUX ==========");
                Console.WriteLine("a. Consulter le nombre de victoires à une date donnée");
                Console.WriteLine("b. Consulter le nombre de victoires pour un joueur donné");
                Console.WriteLine("c. Consulter le nombre de victoires pour un type d'épreuve donné");
                Console.WriteLine("d. Consulter le nombre de victoires pour un joueur, un type d'épreuve et une date donnés");
                Console.WriteLine("e. Consulter le nombre de parties jouées pour un type de jeu donné");
                Console.WriteLine("f. Consulter le nombre de parties jouées à une date et un type d'épreuve donnés");
                Console.WriteLine("g. Consulter le nombre de parties jouées par joueur à une date donnée");
                Console.WriteLine("h. Revenir au menu");
                Console.WriteLine("==========================================");
                Console.Write("Votre choix : ");
                
                string? choix = Console.ReadLine()?.ToLower();
                
                switch (choix)
                {
                    case "a":
                        ConsulterVictoiresParDate();
                        break;
                    case "b":
                        ConsulterVictoiresParJoueur();
                        break;
                    case "c":
                        ConsulterVictoiresParType();
                        break;
                    case "d":
                        ConsulterVictoiresParJoueurTypeDate();
                        break;
                    case "e":
                        ConsulterPartiesParType();
                        break;
                    case "f":
                        ConsulterPartiesParDateEtType();
                        break;
                    case "g":
                        ConsulterPartiesParJoueurEtDate();
                        break;
                    case "h":
                        menuHistoriqueRunning = false;
                        break;
                    default:
                        Console.WriteLine("Choix invalide. Veuillez réessayer.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        /// <summary>
        /// Consulte le nombre de victoires à une date donnée
        /// </summary>
        static void ConsulterVictoiresParDate()
        {
            Console.Clear();
            Console.WriteLine("\n=== VICTOIRES PAR DATE ===");
            
            try
            {
                Console.Write("Entrez la date (format JJ/MM/AAAA) : ");
                if (DateTime.TryParse(Console.ReadLine(), out DateTime date))
                {
                    // Utiliser une instance temporaire de Jeux pour appeler la méthode
                    Jeux jeux = new Jeux("temp", "temp", 1);
                    jeux.afficherNbVictoireAUneDate(date);
                }
                else
                {
                    Console.WriteLine("Format de date invalide.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur : {ex.Message}");
            }
            
            Console.ReadKey();
        }

        /// <summary>
        /// Consulte le nombre de victoires pour un joueur donné
        /// </summary>
        static void ConsulterVictoiresParJoueur()
        {
            Console.Clear();
            Console.WriteLine("\n=== VICTOIRES PAR JOUEUR ===");
            
            try
            {
                Dictionary<int, string> joueurs = dbManager.GetJoueurs();
                
                if (joueurs.Count > 0)
                {
                    Console.WriteLine("Liste des joueurs :");
                    foreach (var joueur in joueurs)
                    {
                        Console.WriteLine($"{joueur.Key} - {joueur.Value}");
                    }
                    
                    Console.Write("\nEntrez l'ID du joueur : ");
                    if (int.TryParse(Console.ReadLine(), out int idJoueur) && joueurs.ContainsKey(idJoueur))
                    {
                        // Utiliser une instance temporaire de Jeux pour appeler la méthode
                        Jeux jeux = new Jeux("temp", "temp", 1);
                        jeux.afficherNbVictoireJoueur(idJoueur);
                    }
                    else
                    {
                        Console.WriteLine("ID de joueur invalide.");
                    }
                }
                else
                {
                    Console.WriteLine("Aucun joueur trouvé ou erreur de connexion à la base de données.");
                    Console.WriteLine("Si vous êtes en mode invité, les statistiques ne sont pas disponibles.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur : {ex.Message}");
            }
            
            Console.ReadKey();
        }

        /// <summary>
        /// Consulte le nombre de victoires pour un type d'épreuve donné
        /// </summary>
        static void ConsulterVictoiresParType()
        {
            Console.Clear();
            Console.WriteLine("\n=== VICTOIRES PAR TYPE D'ÉPREUVE ===");
            
            try
            {
                Dictionary<int, string> typesJeu = dbManager.GetTypesJeu();
                
                if (typesJeu.Count > 0)
                {
                    Console.WriteLine("Liste des types d'épreuve :");
                    foreach (var type in typesJeu)
                    {
                        Console.WriteLine($"{type.Key} - {type.Value}");
                    }
                    
                    Console.Write("\nEntrez l'ID du type d'épreuve : ");
                    if (int.TryParse(Console.ReadLine(), out int idType) && typesJeu.ContainsKey(idType))
                    {
                        // Utiliser une instance temporaire de Jeux pour appeler la méthode
                        Jeux jeux = new Jeux("temp", "temp", 1);
                        jeux.afficherNbVictoireParType(idType);
                    }
                    else
                    {
                        Console.WriteLine("ID de type d'épreuve invalide.");
                    }
                }
                else
                {
                    Console.WriteLine("Aucun type d'épreuve trouvé ou erreur de connexion à la base de données.");
                    Console.WriteLine("Types d'épreuve par défaut : Cérébral, Tactique");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur : {ex.Message}");
                Console.WriteLine("Impossible de récupérer les types d'épreuve depuis la base de données.");
            }
            
            Console.ReadKey();
        }

        /// <summary>
        /// Consulte le nombre de parties jouées pour un type de jeu donné
        /// </summary>
        static void ConsulterPartiesParType()
        {
            Console.Clear();
            Console.WriteLine("\n=== PARTIES JOUÉES PAR TYPE DE JEU ===");
            
            try
            {
                Dictionary<int, string> typesJeu = dbManager.GetTypesJeu();
                
                if (typesJeu.Count > 0)
                {
                    Console.WriteLine("Liste des types de jeu :");
                    foreach (var type in typesJeu)
                    {
                        Console.WriteLine($"{type.Key} - {type.Value}");
                    }
                    
                    Console.Write("\nEntrez l'ID du type de jeu : ");
                    if (int.TryParse(Console.ReadLine(), out int idType) && typesJeu.ContainsKey(idType))
                    {
                        // Utiliser une instance temporaire de Jeux pour appeler la méthode
                        Jeux jeux = new Jeux("temp", "temp", 1);
                        jeux.afficherNbPartieJoueeParType(idType);
                    }
                    else
                    {
                        Console.WriteLine("ID de type de jeu invalide.");
                    }
                }
                else
                {
                    Console.WriteLine("Aucun type de jeu trouvé ou erreur de connexion à la base de données.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur : {ex.Message}");
            }
            
            Console.ReadKey();
        }

        /// <summary>
        /// Consulte le nombre de parties jouées à une date et un type d'épreuve donnés
        /// </summary>
        static void ConsulterPartiesParDateEtType()
        {
            Console.Clear();
            Console.WriteLine("\n=== PARTIES JOUÉES PAR DATE ET TYPE D'ÉPREUVE ===");
            
            try
            {
                Dictionary<int, string> typesJeu = dbManager.GetTypesJeu();
                
                if (typesJeu.Count > 0)
                {
                    Console.Write("Entrez la date (format JJ/MM/AAAA) : ");
                    if (!DateTime.TryParse(Console.ReadLine(), out DateTime date))
                    {
                        Console.WriteLine("Format de date invalide.");
                        Console.ReadKey();
                        return;
                    }
                    
                    Console.WriteLine("\nListe des types d'épreuve :");
                    foreach (var type in typesJeu)
                    {
                        Console.WriteLine($"{type.Key} - {type.Value}");
                    }
                    
                    Console.Write("\nEntrez l'ID du type d'épreuve : ");
                    if (int.TryParse(Console.ReadLine(), out int idType) && typesJeu.ContainsKey(idType))
                    {
                        // Utiliser une instance temporaire de Jeux pour appeler la méthode
                        Jeux jeux = new Jeux("temp", "temp", 1);
                        jeux.afficherNbPartieJoueeAUneDateParType(date, idType);
                    }
                    else
                    {
                        Console.WriteLine("ID de type d'épreuve invalide.");
                    }
                }
                else
                {
                    Console.WriteLine("Aucun type d'épreuve trouvé ou erreur de connexion à la base de données.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur : {ex.Message}");
            }
            
            Console.ReadKey();
        }

        /// <summary>
        /// Consulte le nombre de parties jouées par joueur à une date donnée
        /// </summary>
        static void ConsulterPartiesParJoueurEtDate()
        {
            Console.Clear();
            Console.WriteLine("\n=== PARTIES JOUÉES PAR JOUEUR ET DATE ===");
            
            try
            {
                Dictionary<int, string> joueurs = dbManager.GetJoueurs();
                
                if (joueurs.Count > 0)
                {
                    Console.WriteLine("Liste des joueurs :");
                    foreach (var joueur in joueurs)
                    {
                        Console.WriteLine($"{joueur.Key} - {joueur.Value}");
                    }
                    
                    Console.Write("\nEntrez l'ID du joueur : ");
                    if (!int.TryParse(Console.ReadLine(), out int idJoueur) || !joueurs.ContainsKey(idJoueur))
                    {
                        Console.WriteLine("ID de joueur invalide.");
                        Console.ReadKey();
                        return;
                    }
                    
                    Console.Write("\nEntrez la date (format JJ/MM/AAAA) : ");
                    if (DateTime.TryParse(Console.ReadLine(), out DateTime date))
                    {
                        // Utiliser une instance temporaire de Jeux pour appeler la méthode
                        Jeux jeux = new Jeux("temp", "temp", 1);
                        jeux.afficherNbPartieJoueeJoueurAUneDate(idJoueur, date);
                    }
                    else
                    {
                        Console.WriteLine("Format de date invalide.");
                    }
                }
                else
                {
                    Console.WriteLine("Aucun joueur trouvé ou erreur de connexion à la base de données.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur : {ex.Message}");
            }
            
            Console.ReadKey();
        }
    }
}