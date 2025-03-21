// Jeux-esprit.UI/Views/MainWindow.xaml.cs
using System;
using System.Windows;
using JeuxDesprit;

namespace JeuxEspritUI.Views
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Joueur currentUser;
        private readonly DatabaseManager dbManager;

        public MainWindow(Joueur user)
        {
            InitializeComponent();
            
            this.currentUser = user ?? throw new ArgumentNullException(nameof(user), "L'utilisateur ne peut pas être null");
            this.dbManager = new DatabaseManager();
            
            // Afficher le nom de l'utilisateur connecté
            TxtUserName.Text = $"Bienvenue, {user.GetNom()}";
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            // Demander confirmation
            if (MessageBox.Show("Voulez-vous vraiment vous déconnecter ?", 
                                "Confirmation", 
                                MessageBoxButton.YesNo, 
                                MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }

        private void BtnPlay_Click(object sender, RoutedEventArgs e)
        {
            // Ouvrir la fenêtre des jeux
            var gameWindow = new GameSelectionWindow(currentUser);
            gameWindow.Owner = this;
            gameWindow.ShowDialog();
        }

        private void BtnPlayers_Click(object sender, RoutedEventArgs e)
        {
            // Ouvrir la fenêtre de gestion des joueurs
            var playersWindow = new PlayersWindow();
            playersWindow.Owner = this;
            playersWindow.ShowDialog();
        }

        private void BtnHistory_Click(object sender, RoutedEventArgs e)
        {
            // Ouvrir la fenêtre d'historique
            var historyWindow = new HistoryWindow(currentUser);
            historyWindow.Owner = this;
            historyWindow.ShowDialog();
        }

        private void BtnGameTypes_Click(object sender, RoutedEventArgs e)
        {
            // Ouvrir la fenêtre des types de jeu
            var gameTypesWindow = new GameTypesWindow();
            gameTypesWindow.Owner = this;
            gameTypesWindow.ShowDialog();
        }
    }
}