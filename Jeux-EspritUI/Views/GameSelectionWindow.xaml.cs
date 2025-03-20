// Jeux-esprit.UI/Views/GameSelectionWindow.xaml.cs
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using JeuxDesprit;

namespace JeuxDespritUI.Views
{
    /// <summary>
    /// Logique d'interaction pour GameSelectionWindow.xaml
    /// </summary>
    public partial class GameSelectionWindow : Window
    {
        private readonly Joueur currentUser;
        private readonly Dictionary<string, string> gameDescriptions;

        public GameSelectionWindow(Joueur user)
        {
            InitializeComponent();
            
            this.currentUser = user ?? throw new ArgumentNullException(nameof(user), "L'utilisateur ne peut pas être null");
            
            // Initialiser les descriptions des jeux
            gameDescriptions = new Dictionary<string, string>
            {
                { "PlusMoins", "Devinez un nombre entre deux bornes. À chaque proposition, le jeu vous indique si le nombre mystère est plus grand ou plus petit." },
                { "Pendu", "Devinez un mot lettre par lettre avant d'être pendu. À chaque erreur, une partie du pendu est dessinée." },
                { "Cesar", "Déchiffrez un message en utilisant le chiffrement de César, une technique de chiffrement par décalage." },
                { "Vigenere", "Déchiffrez un message en utilisant le chiffrement de Vigenère, une méthode de chiffrement par substitution plus complexe que César." }
            };
        }

        private void LstGames_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LstGames.SelectedItem != null)
            {
                ListBoxItem selectedItem = (ListBoxItem)LstGames.SelectedItem;
                string gameTag = selectedItem.Tag.ToString();
                
                // Mettre à jour le titre et la description
                TxtGameTitle.Text = selectedItem.Content.ToString();
                TxtGameDescription.Text = gameDescriptions.ContainsKey(gameTag) ? gameDescriptions[gameTag] : "Aucune description disponible.";
                
                // Activer le bouton Jouer
                BtnStartGame.IsEnabled = true;
            }
            else
            {
                TxtGameTitle.Text = "Sélectionnez un jeu";
                TxtGameDescription.Text = "Cliquez sur un jeu pour voir sa description.";
                BtnStartGame.IsEnabled = false;
            }
        }

        private void BtnStartGame_Click(object sender, RoutedEventArgs e)
        {
            if (LstGames.SelectedItem == null)
            {
                MessageBox.Show("Veuillez sélectionner un jeu.", "Sélection requise", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Récupérer le jeu sélectionné
            ListBoxItem selectedItem = (ListBoxItem)LstGames.SelectedItem;
            string gameTag = selectedItem.Tag.ToString();
            
            // Récupérer le niveau de difficulté
            string level = "facile";
            if (RadioMedium.IsChecked == true) level = "moyen";
            else if (RadioHard.IsChecked == true) level = "difficile";
            else if (RadioExpert.IsChecked == true) level = "expert";

            // Lancer le jeu sélectionné
            var gameWindow = new GamePlayWindow(currentUser, gameTag, level);
            gameWindow.Owner = this;
            gameWindow.ShowDialog();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}