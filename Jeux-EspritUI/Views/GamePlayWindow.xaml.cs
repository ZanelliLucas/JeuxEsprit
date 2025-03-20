// Jeux-esprit.UI/Views/GamePlayWindow.xaml.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Text;
using JeuxDesprit;

namespace JeuxDesprit.UI.Views
{
    /// <summary>
    /// Logique d'interaction pour GamePlayWindow.xaml
    /// </summary>
    public partial class GamePlayWindow : Window
    {
        private readonly Joueur currentUser;
        private readonly string gameType;
        private readonly string level;
        
        // Timer pour mesurer le temps de jeu
        private readonly DispatcherTimer gameTimer;
        private DateTime startTime;
        private TimeSpan elapsedTime;
        
        // Variables pour le jeu Plus ou Moins
        private int nombreMystere;
        private int minRange = 1;
        private int maxRange = 100;
        private int remainingAttemptsPlusMoins;
        
        // Variables pour le jeu du Pendu
        private string motADeviner;
        private char[] motDevoile;
        private List<char> lettresEssayees;
        private int erreursPendu;
        private int remainingAttemptsPendu;
        
        // Variables pour le jeu César
        private string messageOriginalCesar;
        private string messageChiffreCesar;
        private int decalageCesar;
        private int remainingAttemptsCesar;
        
        // Variables pour le jeu Vigenère
        private string messageOriginalVigenere;
        private string messageChiffreVigenere;
        private string cleVigenere;
        private int remainingAttemptsVigenere;
        
        // Variables communes
        private bool gameWon;
        private int score;
        private Random random;
        private DatabaseManager dbManager;

        public GamePlayWindow(Joueur user, string gameType, string level)
        {
            InitializeComponent();
            
            this.currentUser = user ?? throw new ArgumentNullException(nameof(user), "L'utilisateur ne peut pas être null");
            this.gameType = gameType ?? throw new ArgumentNullException(nameof(gameType), "Le type de jeu ne peut pas être null");
            this.level = level ?? throw new ArgumentNullException(nameof(level), "Le niveau ne peut pas être null");
            
            this.random = new Random();
            this.score = 0;
            this.gameWon = false;
            this.dbManager = new DatabaseManager();
            
            // Configurer le timer
            gameTimer = new DispatcherTimer();
            gameTimer.Interval = TimeSpan.FromSeconds(1);
            gameTimer.Tick += GameTimer_Tick;
            
            // Initialiser l'interface selon le jeu
            InitializeGame();
            
            // Démarrer le chronomètre
            startTime = DateTime.Now;
            gameTimer.Start();
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            elapsedTime = DateTime.Now - startTime;
            TxtTimer.Text = $"Temps: {elapsedTime.Minutes:00}:{elapsedTime.Seconds:00}";
        }

        private void InitializeGame()
        {
            // Configurer les informations générales
            TxtPlayerInfo.Text = $"Joueur: {currentUser.GetNom()}";
            TxtLevel.Text = $"Niveau: {level}";
            
            // Initialiser le jeu spécifique
            switch (gameType)
            {
                case "PlusMoins":
                    InitializePlusMoins();
                    break;
                case "Pendu":
                    InitializePendu();
                    break;
                case "Cesar":
                    InitializeCesar();
                    break;
                case "Vigenere":
                    InitializeVigenere();
                    break;
                default:
                    MessageBox.Show($"Type de jeu non reconnu: {gameType}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    this.Close();
                    break;
            }
        }

        #region Plus ou Moins
        private void InitializePlusMoins()
        {
            TxtGameTitle.Text = "PLUS OU MOINS";
            TabPlusMoins.Visibility = Visibility.Visible;
            GameTabControl.SelectedItem = TabPlusMoins;
            
            // Configurer selon le niveau
            int maxNombreEssais = 10;
            switch (level.ToLower())
            {
                case "facile":
                    minRange = 1;
                    maxRange = 100;
                    maxNombreEssais = 10;
                    break;
                case "moyen":
                    minRange = 1;
                    maxRange = 200;
                    maxNombreEssais = 7;
                    break;
                case "difficile":
                    minRange = 1;
                    maxRange = 500;
                    maxNombreEssais = 5;
                    break;
                case "expert":
                    minRange = 1;
                    maxRange = 1000;
                    maxNombreEssais = 3;
                    break;
            }
            
            // Générer le nombre mystère
            nombreMystere = random.Next(minRange, maxRange + 1);
            
            // Mettre à jour l'interface
            TxtPlusMoinsRange.Text = $"Nombre entre {minRange} et {maxRange}";
            remainingAttemptsPlusMoins = maxNombreEssais;
            TxtPlusMoinsRemaining.Text = $"Essais restants: {remainingAttemptsPlusMoins}";
            TxtPlusMoinsHint.Text = "En attente de votre première proposition...";
        }

        private void BtnPlusMoinsSubmit_Click(object sender, RoutedEventArgs e)
        {
            // Vérifier qu'il reste des essais
            if (remainingAttemptsPlusMoins <= 0 || gameWon)
                return;
                
            // Récupérer la proposition
            if (!int.TryParse(TxtPlusMoinsGuess.Text, out int proposition))
            {
                MessageBox.Show("Veuillez entrer un nombre valide.", "Erreur de saisie", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            
            // Vérifier si la proposition est dans les bornes
            if (proposition < minRange || proposition > maxRange)
            {
                MessageBox.Show($"Veuillez entrer un nombre entre {minRange} et {maxRange}.", "Erreur de saisie", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            
            // Décrémenter le nombre d'essais
            remainingAttemptsPlusMoins--;
            TxtPlusMoinsRemaining.Text = $"Essais restants: {remainingAttemptsPlusMoins}";
            
            // Vérifier la proposition
            if (proposition == nombreMystere)
            {
                gameWon = true;
                // Calculer le score (plus d'essais restants = plus de points)
                score = 1000 + (remainingAttemptsPlusMoins * 100) - (int)(elapsedTime.TotalSeconds * 5);
                if (score < 0) score = 0;
                
                TxtPlusMoinsHint.Text = "BRAVO ! Vous avez trouvé le nombre mystère !";
                TxtPlusMoinsHint.Background = new SolidColorBrush(Colors.LightGreen);
                TxtScore.Text = $"Score: {score}";
                TxtGameStatus.Text = "Partie gagnée !";
                
                // Mettre à jour les stats si c'est un joueur pro
                if (currentUser is Professionnel professionnel && gameWon)
                {
                    professionnel.IncrementPartiesGagnees();
                }
                
                // Désactiver les contrôles
                TxtPlusMoinsGuess.IsEnabled = false;
                BtnPlusMoinsSubmit.IsEnabled = false;
            }
            else if (proposition < nombreMystere)
            {
                TxtPlusMoinsHint.Text = "C'est PLUS !";
                minRange = Math.Max(minRange, proposition + 1);
                TxtPlusMoinsRange.Text = $"Nombre entre {minRange} et {maxRange}";
            }
            else
            {
                TxtPlusMoinsHint.Text = "C'est MOINS !";
                maxRange = Math.Min(maxRange, proposition - 1);
                TxtPlusMoinsRange.Text = $"Nombre entre {minRange} et {maxRange}";
            }
            
            // Vérifier s'il reste des essais
            if (remainingAttemptsPlusMoins <= 0 && !gameWon)
            {
                TxtPlusMoinsHint.Text = $"PERDU ! Le nombre mystère était {nombreMystere}.";
                TxtPlusMoinsHint.Background = new SolidColorBrush(Colors.LightCoral);
                TxtGameStatus.Text = "Partie perdue !";
                
                // Désactiver les contrôles
                TxtPlusMoinsGuess.IsEnabled = false;
                BtnPlusMoinsSubmit.IsEnabled = false;
            }
            
            // Effacer le champ de texte
            TxtPlusMoinsGuess.Text = "";
            TxtPlusMoinsGuess.Focus();
        }
        #endregion

        #region Pendu
        private void InitializePendu()
        {
            TxtGameTitle.Text = "PENDU";
            TabPendu.Visibility = Visibility.Visible;
            GameTabControl.SelectedItem = TabPendu;
            
            // Configurer selon le niveau
            string[] motsFaciles = { "chat", "chien", "arbre", "maison", "soleil", "jeu", "fleur" };
            string[] motsMoyens = { "ordinateur", "voiture", "piscine", "montagne", "valise", "piano" };
            string[] motsDifficiles = { "developpement", "algorithme", "environnement", "communication", "interface" };
            string[] motsExperts = { "anticonstitutionnellement", "chronophotographie", "electroencephalogramme", "psychophysiologique" };
            
            int maxNombreEssais = 10;
            string[] mots;
            
            // Sélectionner la liste de mots selon le niveau
            switch (level.ToLower())
            {
                case "facile":
                    mots = motsFaciles;
                    maxNombreEssais = 10;
                    break;
                case "moyen":
                    mots = motsMoyens;
                    maxNombreEssais = 8;
                    break;
                case "difficile":
                    mots = motsDifficiles;
                    maxNombreEssais = 6;
                    break;
                case "expert":
                    mots = motsExperts;
                    maxNombreEssais = 4;
                    break;
                default:
                    mots = motsFaciles;
                    maxNombreEssais = 10;
                    break;
            }
            
            // Choisir un mot aléatoire
            motADeviner = mots[random.Next(mots.Length)];
            
            // Initialiser le mot dévoilé avec des tirets
            motDevoile = new char[motADeviner.Length];
            for (int i = 0; i < motDevoile.Length; i++)
            {
                motDevoile[i] = '_';
            }
            
            // Initialiser les variables
            lettresEssayees = new List<char>();
            erreursPendu = 0;
            remainingAttemptsPendu = maxNombreEssais;
            
            // Mettre à jour l'interface
            UpdatePenduDisplay();
            TxtPenduRemaining.Text = $"Essais restants: {remainingAttemptsPendu}";
            DrawPendu();
        }

        private void UpdatePenduDisplay()
        {
            // Afficher le mot avec des espaces entre les lettres
            StringBuilder sb = new StringBuilder();
            foreach (char c in motDevoile)
            {
                sb.Append(c).Append(' ');
            }
            TxtPenduWord.Text = sb.ToString();
            
            // Afficher les lettres déjà essayées
            sb.Clear();
            foreach (char c in lettresEssayees)
            {
                sb.Append(c).Append(' ');
            }
            TxtPenduLetters.Text = sb.ToString();
        }

        private void DrawPendu()
        {
            // Effacer le canvas
            CanvasPendu.Children.Clear();
            
            // Dessiner le pendu selon le nombre d'erreurs
            // Base
            var baseLine = new Line
            {
                X1 = 10,
                Y1 = 240,
                X2 = 190,
                Y2 = 240,
                Stroke = Brushes.Black,
                StrokeThickness = 2
            };
            CanvasPendu.Children.Add(baseLine);
            
            if (erreursPendu >= 1)
            {
                // Poteau vertical
                var verticalLine = new Line
                {
                    X1 = 50,
                    Y1 = 240,
                    X2 = 50,
                    Y2 = 20,
                    Stroke = Brushes.Black,
                    StrokeThickness = 2
                };
                CanvasPendu.Children.Add(verticalLine);
            }
            
            if (erreursPendu >= 2)
            {
                // Poutre horizontale
                var horizontalLine = new Line
                {
                    X1 = 50,
                    Y1 = 20,
                    X2 = 150,
                    Y2 = 20,
                    Stroke = Brushes.Black,
                    StrokeThickness = 2
                };
                CanvasPendu.Children.Add(horizontalLine);
            }
            
            if (erreursPendu >= 3)
            {
                // Corde
                var ropeLine = new Line
                {
                    X1 = 150,
                    Y1 = 20,
                    X2 = 150,
                    Y2 = 50,
                    Stroke = Brushes.Black,
                    StrokeThickness = 2
                };
                CanvasPendu.Children.Add(ropeLine);
            }
            
            if (erreursPendu >= 4)
            {
                // Tête
                var head = new Ellipse
                {
                    Width = 30,
                    Height = 30,
                    Stroke = Brushes.Black,
                    StrokeThickness = 2,
                    Margin = new Thickness(135, 50, 0, 0)
                };
                CanvasPendu.Children.Add(head);
            }
            
            if (erreursPendu >= 5)
            {
                // Corps
                var bodyLine = new Line
                {
                    X1 = 150,
                    Y1 = 80,
                    X2 = 150,
                    Y2 = 150,
                    Stroke = Brushes.Black,
                    StrokeThickness = 2
                };
                CanvasPendu.Children.Add(bodyLine);
            }
            
            if (erreursPendu >= 6)
            {
                // Bras gauche
                var leftArm = new Line
                {
                    X1 = 150,
                    Y1 = 100,
                    X2 = 120,
                    Y2 = 120,
                    Stroke = Brushes.Black,
                    StrokeThickness = 2
                };
                CanvasPendu.Children.Add(leftArm);
            }
            
            if (erreursPendu >= 7)
            {
                // Bras droit
                var rightArm = new Line
                {
                    X1 = 150,
                    Y1 = 100,
                    X2 = 180,
                    Y2 = 120,
                    Stroke = Brushes.Black,
                    StrokeThickness = 2
                };
                CanvasPendu.Children.Add(rightArm);
            }
            
            if (erreursPendu >= 8)
            {
                // Jambe gauche
                var leftLeg = new Line
                {
                    X1 = 150,
                    Y1 = 150,
                    X2 = 130,
                    Y2 = 190,
                    Stroke = Brushes.Black,
                    StrokeThickness = 2
                };
                CanvasPendu.Children.Add(leftLeg);
            }
            
            if (erreursPendu >= 9)
            {
                // Jambe droite
                var rightLeg = new Line
                {
                    X1 = 150,
                    Y1 = 150,
                    X2 = 170,
                    Y2 = 190,
                    Stroke = Brushes.Black,
                    StrokeThickness = 2
                };
                CanvasPendu.Children.Add(rightLeg);
            }
            
            if (erreursPendu >= 10)
            {
                // Visage (croix)
                var face1 = new Line
                {
                    X1 = 145,
                    Y1 = 60,
                    X2 = 155,
                    Y2 = 70,
                    Stroke = Brushes.Red,
                    StrokeThickness = 2
                };
                CanvasPendu.Children.Add(face1);
                
                var face2 = new Line
                {
                    X1 = 155,
                    Y1 = 60,
                    X2 = 145,
                    Y2 = 70,
                    Stroke = Brushes.Red,
                    StrokeThickness = 2
                };
                CanvasPendu.Children.Add(face2);
            }
        }

        private void BtnPenduSubmit_Click(object sender, RoutedEventArgs e)
        {
            // Vérifier qu'il reste des essais
            if (remainingAttemptsPendu <= 0 || gameWon)
                return;
                
            // Récupérer la lettre proposée
            string input = TxtPenduGuess.Text.ToLower();
            
            if (string.IsNullOrEmpty(input) || input.Length != 1 || !char.IsLetter(input[0]))
            {
                MessageBox.Show("Veuillez entrer une lettre valide.", "Erreur de saisie", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            
            char lettre = input[0];
            
            // Vérifier si la lettre a déjà été essayée
            if (lettresEssayees.Contains(lettre))
            {
                MessageBox.Show("Vous avez déjà essayé cette lettre !", "Lettre déjà proposée", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            
            // Ajouter la lettre aux lettres essayées
            lettresEssayees.Add(lettre);
            
            // Vérifier si la lettre est dans le mot
            bool lettreTrouvee = false;
            for (int i = 0; i < motADeviner.Length; i++)
            {
                if (motADeviner[i] == lettre)
                {
                    motDevoile[i] = lettre;
                    lettreTrouvee = true;
                }
            }
            
            // Mettre à jour l'interface
            if (lettreTrouvee)
            {
                // Vérifier si le mot est complet
                if (!motDevoile.Contains('_'))
                {
                    gameWon = true;
                    // Calculer le score
                    score = 1000 + (remainingAttemptsPendu * 100) - (erreursPendu * 50) - (int)(elapsedTime.TotalSeconds * 5);
                    if (score < 0) score = 0;
                    
                    TxtGameStatus.Text = "Partie gagnée ! Vous avez deviné le mot !";
                    TxtScore.Text = $"Score: {score}";
                    
                    // Mettre à jour les stats si c'est un joueur pro
                    if (currentUser is Professionnel professionnel && gameWon)
                    {
                        professionnel.IncrementPartiesGagnees();
                    }
                    
                    // Désactiver les contrôles
                    TxtPenduGuess.IsEnabled = false;
                    BtnPenduSubmit.IsEnabled = false;
                }
            }
            else
            {
                // Incrémenter le nombre d'erreurs
                erreursPendu++;
                remainingAttemptsPendu--;
                TxtPenduRemaining.Text = $"Essais restants: {remainingAttemptsPendu}";
                
                // Mettre à jour le dessin du pendu
                DrawPendu();
                
                // Vérifier s'il reste des essais
                if (remainingAttemptsPendu <= 0)
                {
                    TxtGameStatus.Text = $"Partie perdue ! Le mot était: {motADeviner}";
                    
                    // Révéler le mot
                    for (int i = 0; i < motADeviner.Length; i++)
                    {
                        motDevoile[i] = motADeviner[i];
                    }
                    
                    // Désactiver les contrôles
                    TxtPenduGuess.IsEnabled = false;
                    BtnPenduSubmit.IsEnabled = false;
                }
            }
            
            UpdatePenduDisplay();
            
            // Effacer le champ de texte
            TxtPenduGuess.Text = "";
            TxtPenduGuess.Focus();
        }
        #endregion

        #region César
        private void InitializeCesar()
        {
            TxtGameTitle.Text = "CHIFFREMENT DE CÉSAR";
            TabCesar.Visibility = Visibility.Visible;
            GameTabControl.SelectedItem = TabCesar;
            
            // Messages possibles
            string[] messages = {
                "Le chiffrement de Cesar est une technique simple",
                "La cryptographie est une science passionnante",
                "Ce message secret doit etre dechiffre",
                "Le code a ete casse par les ennemis",
                "Envoyez des renforts immediatement"
            };
            
            // Configurer selon le niveau
            int decalageMax = 5;
            switch (level.ToLower())
            {
                case "facile":
                    decalageMax = 5;
                    remainingAttemptsCesar = 3;
                    break;
                case "moyen":
                    decalageMax = 10;
                    remainingAttemptsCesar = 3;
                    break;
                case "difficile":
                    decalageMax = 15;
                    remainingAttemptsCesar = 2;
                    break;
                case "expert":
                    decalageMax = 25;
                    remainingAttemptsCesar = 2;
                    break;
            }
            
            // Choisir un message aléatoire
            messageOriginalCesar = messages[random.Next(messages.Length)];
            
            // Générer un décalage aléatoire
            decalageCesar = random.Next(1, decalageMax + 1);
            
            // Chiffrer le message
            messageChiffreCesar = ChiffrerCesar(messageOriginalCesar, decalageCesar);
            
            // Mettre à jour l'interface
            TxtCesarMessage.Text = messageChiffreCesar;
            TxtCesarRemaining.Text = $"Essais restants: {remainingAttemptsCesar}";
            TxtCesarResult.Text = "En attente de votre proposition...";
        }

        private string ChiffrerCesar(string message, int decalage)
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

        private string DechiffrerCesar(string messageChiffre, int decalage)
        {
            return ChiffrerCesar(messageChiffre, 26 - (decalage % 26));
        }

        private void BtnCesarSubmit_Click(object sender, RoutedEventArgs e)
        {
            // Vérifier qu'il reste des essais
            if (remainingAttemptsCesar <= 0 || gameWon)
                return;
                
            // Récupérer le décalage proposé
            if (!int.TryParse(TxtCesarGuess.Text, out int decalagePropose))
            {
                MessageBox.Show("Veuillez entrer un nombre valide.", "Erreur de saisie", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            
            // Décrémenter le nombre d'essais
            remainingAttemptsCesar--;
            TxtCesarRemaining.Text = $"Essais restants: {remainingAttemptsCesar}";
            
            // Vérifier la proposition
            if (decalagePropose == decalageCesar)
            {
                gameWon = true;
                // Calculer le score
                score = 1000 + (remainingAttemptsCesar * 200) - (int)(elapsedTime.TotalSeconds * 5);
                if (score < 0) score = 0;
                
                TxtCesarResult.Text = $"BRAVO ! Vous avez trouvé le bon décalage ({decalageCesar}) !\nMessage déchiffré: {messageOriginalCesar}";
                TxtCesarResult.Background = new SolidColorBrush(Colors.LightGreen);
                TxtScore.Text = $"Score: {score}";
                TxtGameStatus.Text = "Partie gagnée !";
                
                // Mettre à jour les stats si c'est un joueur pro
                if (currentUser is Professionnel professionnel && gameWon)
                {
                    professionnel.IncrementPartiesGagnees();
                }
                
                // Désactiver les contrôles
                TxtCesarGuess.IsEnabled = false;
                BtnCesarSubmit.IsEnabled = false;
            }
            else
            {
                // Déchiffrer avec le décalage proposé pour montrer le résultat
                string tentative = DechiffrerCesar(messageChiffreCesar, decalagePropose);
                TxtCesarResult.Text = $"Ce n'est pas le bon décalage. Voici ce que ça donne avec un décalage de {decalagePropose}:\n{tentative}";
                
                // Vérifier s'il reste des essais
                if (remainingAttemptsCesar <= 0)
                {
                    TxtCesarResult.Text += $"\n\nPERDU ! Le bon décalage était: {decalageCesar}.\nLe message original était: {messageOriginalCesar}";
                    TxtCesarResult.Background = new SolidColorBrush(Colors.LightCoral);
                    TxtGameStatus.Text = "Partie perdue !";
                    
                    // Désactiver les contrôles
                    TxtCesarGuess.IsEnabled = false;
                    BtnCesarSubmit.IsEnabled = false;
                }
            }
            
            // Effacer le champ de texte
            TxtCesarGuess.Text = "";
            TxtCesarGuess.Focus();
        }
        #endregion

        #region Vigenère
        private void InitializeVigenere()
        {
            TxtGameTitle.Text = "CHIFFREMENT DE VIGENÈRE";
            TabVigenere.Visibility = Visibility.Visible;
            GameTabControl.SelectedItem = TabVigenere;
            
            // Messages possibles
            string[] messages = {
                "Le chiffre de Vigenere est une methode de chiffrement par substitution",
                "La cryptographie protege nos donnees personnelles",
                "Ce message est code avec une cle secrete",
                "Les espions utilisent des codes sophistiques",
                "Transmettez ces informations confidentielles avec prudence"
            };
            
            // Clés possibles
            string[] cles = {
                "CRYPT", "SECRET", "ENIGMA", "CODE", "CLE", 
                "ALGORITHME", "SECURITE", "MYSTERE", "VIGENERE"
            };
            
            // Configurer selon le niveau
            int maxEssais = 5;
            int indexMaxCle = 3; // Pour facile
            string indice = "";
            
            switch (level.ToLower())
            {
                case "facile":
                    indexMaxCle = 3; // Clés courtes
                    maxEssais = 5;
                    break;
                case "moyen":
                    indexMaxCle = 5;
                    maxEssais = 4;
                    break;
                case "difficile":
                    indexMaxCle = 7;
                    maxEssais = 3;
                    break;
                case "expert":
                    indexMaxCle = cles.Length - 1;
                    maxEssais = 2;
                    break;
            }
            
            // Choisir un message aléatoire
            messageOriginalVigenere = messages[random.Next(messages.Length)];
            
            // Choisir une clé selon le niveau
            cleVigenere = cles[random.Next(indexMaxCle)];
            
            // Chiffrer le message
            messageChiffreVigenere = ChiffrerVigenere(messageOriginalVigenere, cleVigenere);
            
            // Configurer l'indice selon le niveau
            if (level.ToLower() == "facile")
            {
                indice = $"Indice: La clé contient {cleVigenere.Length} lettres.";
            }
            else if (level.ToLower() == "moyen")
            {
                indice = $"Indice: La première lettre de la clé est '{cleVigenere[0]}'.";
            }
            else
            {
                indice = "Sans indice. À vous de trouver la clé.";
            }
            
            // Mettre à jour l'interface
            TxtVigenereMessage.Text = messageChiffreVigenere;
            TxtVigenereHint.Text = indice;
            remainingAttemptsVigenere = maxEssais;
            TxtVigenereRemaining.Text = $"Essais restants: {remainingAttemptsVigenere}";
            TxtVigenereResult.Text = "En attente de votre proposition...";
        }

        private string ChiffrerVigenere(string message, string cle)
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

        private string DechiffrerVigenere(string messageChiffre, string cle)
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

        private void BtnVigenereSubmit_Click(object sender, RoutedEventArgs e)
        {
            // Vérifier qu'il reste des essais
            if (remainingAttemptsVigenere <= 0 || gameWon)
                return;
                
            // Récupérer la clé proposée
            string cleProposee = TxtVigenereGuess.Text.ToUpper();
            
            if (string.IsNullOrEmpty(cleProposee))
            {
                MessageBox.Show("Veuillez entrer une clé valide.", "Erreur de saisie", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            
            // Décrémenter le nombre d'essais
            remainingAttemptsVigenere--;
            TxtVigenereRemaining.Text = $"Essais restants: {remainingAttemptsVigenere}";
            
            // Vérifier la proposition
            if (cleProposee == cleVigenere)
            {
                gameWon = true;
                // Calculer le score
                int facteurDifficulte = 1;
                switch (level.ToLower())
                {
                    case "facile": facteurDifficulte = 1; break;
                    case "moyen": facteurDifficulte = 2; break;
                    case "difficile": facteurDifficulte = 3; break;
                    case "expert": facteurDifficulte = 4; break;
                }
                
                score = (1000 + (remainingAttemptsVigenere * 200) - (int)(elapsedTime.TotalSeconds * 3)) * facteurDifficulte;
                if (score < 0) score = 0;
                
                TxtVigenereResult.Text = $"BRAVO ! Vous avez trouvé la bonne clé ({cleVigenere}) !\nMessage déchiffré: {messageOriginalVigenere}";
                TxtVigenereResult.Background = new SolidColorBrush(Colors.LightGreen);
                TxtScore.Text = $"Score: {score}";
                TxtGameStatus.Text = "Partie gagnée !";
                
                // Mettre à jour les stats si c'est un joueur pro
                if (currentUser is Professionnel professionnel && gameWon)
                {
                    professionnel.IncrementPartiesGagnees();
                }
                
                // Désactiver les contrôles
                TxtVigenereGuess.IsEnabled = false;
                BtnVigenereSubmit.IsEnabled = false;
            }
            else
            {
                // Déchiffrer avec la clé proposée pour montrer le résultat
                string tentative = DechiffrerVigenere(messageChiffreVigenere, cleProposee);
                TxtVigenereResult.Text = $"Ce n'est pas la bonne clé. Voici ce que ça donne avec la clé '{cleProposee}':\n{tentative}";
                
                // Vérifier s'il reste des essais
                if (remainingAttemptsVigenere <= 0)
                {
                    TxtVigenereResult.Text += $"\n\nPERDU ! La bonne clé était: {cleVigenere}.\nLe message original était: {messageOriginalVigenere}";
                    TxtVigenereResult.Background = new SolidColorBrush(Colors.LightCoral);
                    TxtGameStatus.Text = "Partie perdue !";
                    
                    // Désactiver les contrôles
                    TxtVigenereGuess.IsEnabled = false;
                    BtnVigenereSubmit.IsEnabled = false;
                }
            }
            
            // Effacer le champ de texte
            TxtVigenereGuess.Text = "";
            TxtVigenereGuess.Focus();
        }
        #endregion

        private void BtnRestartGame_Click(object sender, RoutedEventArgs e)
        {
            // Enregistrer la partie terminée (si nécessaire)
            if (gameWon || remainingAttemptsPlusMoins <= 0 || remainingAttemptsPendu <= 0 || 
                remainingAttemptsCesar <= 0 || remainingAttemptsVigenere <= 0)
            {
                // Ici vous pourriez sauvegarder le résultat de la partie dans la base de données
                try
                {
                    string statut = gameWon ? "gagné" : "perdu";
                    
                    // Optionnel: Enregistrer dans la base de données
                    // dbManager.EnregistrerPartie(...);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur lors de l'enregistrement de la partie : {ex.Message}", 
                                   "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            
            // Réinitialiser le jeu
            score = 0;
            gameWon = false;
            TxtScore.Text = "Score: 0";
            TxtGameStatus.Text = "Partie en cours...";
            
            // Réinitialiser le chronomètre
            startTime = DateTime.Now;
            
            // Réinitialiser le jeu spécifique
            InitializeGame();
        }

        private void BtnQuitGame_Click(object sender, RoutedEventArgs e)
        {
            // Demander confirmation si la partie est en cours
            bool partieEnCours = !(gameWon || remainingAttemptsPlusMoins <= 0 || remainingAttemptsPendu <= 0 || 
                                  remainingAttemptsCesar <= 0 || remainingAttemptsVigenere <= 0);
            
            if (partieEnCours)
            {
                if (MessageBox.Show("Êtes-vous sûr de vouloir quitter la partie en cours ?", 
                                  "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                {
                    return;
                }
            }
            
            // Arrêter le chronomètre
            gameTimer.Stop();
            
            // Fermer la fenêtre
            this.Close();
        }
    }
}