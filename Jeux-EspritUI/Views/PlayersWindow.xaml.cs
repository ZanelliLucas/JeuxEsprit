// Jeux-esprit.UI/Views/PlayersWindow.xaml.cs
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using JeuxDesprit;

namespace JeuxDesprit.UI.Views
{
    /// <summary>
    /// Logique d'interaction pour PlayersWindow.xaml
    /// </summary>
    public partial class PlayersWindow : Window
    {
        private readonly DatabaseManager dbManager;
        private Dictionary<int, string> playersList;
        private Joueur currentPlayer;
        private int currentPlayerId;
        private bool editMode = false;

        public PlayersWindow()
        {
            InitializeComponent();
            
            dbManager = new DatabaseManager();
            
            // Charger la liste des joueurs
            LoadPlayers();
        }

        private void LoadPlayers()
        {
            try
            {
                playersList = dbManager.GetJoueurs();
                
                // Mettre à jour la liste
                LstPlayers.Items.Clear();
                
                if (playersList.Count > 0)
                {
                    foreach (var player in playersList)
                    {
                        LstPlayers.Items.Add(new ListBoxItem 
                        { 
                            Content = player.Value, 
                            Tag = player.Key 
                        });
                    }
                }
                else
                {
                    MessageBox.Show("Aucun joueur trouvé dans la base de données.", 
                                   "Information", 
                                   MessageBoxButton.OK, 
                                   MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des joueurs : {ex.Message}", 
                               "Erreur", 
                               MessageBoxButton.OK, 
                               MessageBoxImage.Error);
            }
        }

        private void LstPlayers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LstPlayers.SelectedItem != null)
            {
                // Sortir du mode édition si besoin
                if (editMode)
                {
                    ToggleEditMode(false);
                }
                
                // Récupérer l'ID du joueur sélectionné
                ListBoxItem selectedItem = (ListBoxItem)LstPlayers.SelectedItem;
                currentPlayerId = (int)selectedItem.Tag;
                
                // Charger les détails du joueur
                try
                {
                    currentPlayer = dbManager.GetJoueurById(currentPlayerId);
                    
                    if (currentPlayer != null)
                    {
                        // Afficher les détails généraux
                        TxtPlayerName.Text = currentPlayer.GetNom();
                        TxtPlayerEmail.Text = currentPlayer.GetEmail();
                        TxtPlayerAvatar.Text = currentPlayer.GetAvatar();
                        
                        // Déterminer le type et afficher les détails spécifiques
                        if (currentPlayer is Amateur amateur)
                        {
                            TxtPlayerType.Text = "Amateur";
                            TxtAmateurLevel.Text = amateur.GetNiveauExperience().ToString();
                            
                            // Afficher les statistiques d'amateur
                            GridAmateurStats.Visibility = Visibility.Visible;
                            GridProStats.Visibility = Visibility.Collapsed;
                            GridProWins.Visibility = Visibility.Collapsed;
                        }
                        else if (currentPlayer is Professionnel pro)
                        {
                            TxtPlayerType.Text = "Professionnel";
                            TxtProRanking.Text = pro.GetClassement().ToString();
                            TxtProWins.Text = pro.GetPartiesGagnees().ToString();
                            
                            // Afficher les statistiques de pro
                            GridAmateurStats.Visibility = Visibility.Collapsed;
                            GridProStats.Visibility = Visibility.Visible;
                            GridProWins.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            TxtPlayerType.Text = "Standard";
                            
                            // Masquer toutes les statistiques spécifiques
                            GridAmateurStats.Visibility = Visibility.Collapsed;
                            GridProStats.Visibility = Visibility.Collapsed;
                            GridProWins.Visibility = Visibility.Collapsed;
                        }
                        
                        // Activer les boutons d'édition et de suppression
                        BtnEditPlayer.IsEnabled = true;
                        BtnDeletePlayer.IsEnabled = true;
                    }
                    else
                    {
                        MessageBox.Show($"Joueur ID {currentPlayerId} non trouvé.", 
                                       "Erreur", 
                                       MessageBoxButton.OK, 
                                       MessageBoxImage.Warning);
                        
                        // Désactiver les boutons d'édition et de suppression
                        BtnEditPlayer.IsEnabled = false;
                        BtnDeletePlayer.IsEnabled = false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur lors du chargement des détails du joueur : {ex.Message}", 
                                   "Erreur", 
                                   MessageBoxButton.OK, 
                                   MessageBoxImage.Error);
                }
            }
            else
            {
                // Aucun joueur sélectionné, vider les champs
                ClearPlayerDetails();
                
                // Désactiver les boutons d'édition et de suppression
                BtnEditPlayer.IsEnabled = false;
                BtnDeletePlayer.IsEnabled = false;
            }
        }

        private void ClearPlayerDetails()
        {
            TxtPlayerName.Text = string.Empty;
            TxtPlayerEmail.Text = string.Empty;
            TxtPlayerAvatar.Text = string.Empty;
            TxtPlayerType.Text = string.Empty;
            TxtAmateurLevel.Text = string.Empty;
            TxtProRanking.Text = string.Empty;
            TxtProWins.Text = string.Empty;
            
            currentPlayer = null;
            currentPlayerId = 0;
        }

        private void ToggleEditMode(bool enable)
        {
            editMode = enable;
            
            // Activer/désactiver les champs
            TxtPlayerName.IsEnabled = enable;
            TxtPlayerEmail.IsEnabled = enable;
            TxtPlayerAvatar.IsEnabled = enable;
            
            // Pour les champs spécifiques selon le type
            if (currentPlayer is Amateur)
            {
                TxtAmateurLevel.IsEnabled = enable;
            }
            else if (currentPlayer is Professionnel)
            {
                TxtProRanking.IsEnabled = enable;
                TxtProWins.IsEnabled = enable;
            }
            
            // Changer la visibilité des boutons
            if (enable)
            {
                BtnEditPlayer.Visibility = Visibility.Collapsed;
                BtnDeletePlayer.Visibility = Visibility.Collapsed;
                BtnSavePlayer.Visibility = Visibility.Visible;
                BtnCancelEdit.Visibility = Visibility.Visible;
            }
            else
            {
                BtnEditPlayer.Visibility = Visibility.Visible;
                BtnDeletePlayer.Visibility = Visibility.Visible;
                BtnSavePlayer.Visibility = Visibility.Collapsed;
                BtnCancelEdit.Visibility = Visibility.Collapsed;
            }
        }

        private void BtnAddPlayer_Click(object sender, RoutedEventArgs e)
        {
            // Ouvrir la fenêtre d'ajout de joueur
            var addPlayerWindow = new AddPlayerWindow();
            addPlayerWindow.Owner = this;
            
            if (addPlayerWindow.ShowDialog() == true)
            {
                // Recharger la liste des joueurs
                LoadPlayers();
            }
        }

        private void BtnEditPlayer_Click(object sender, RoutedEventArgs e)
        {
            if (currentPlayer == null)
            {
                MessageBox.Show("Veuillez sélectionner un joueur à modifier.", 
                               "Sélection requise", 
                               MessageBoxButton.OK, 
                               MessageBoxImage.Warning);
                return;
            }
            
            // Passer en mode édition
            ToggleEditMode(true);
        }

        private void BtnSavePlayer_Click(object sender, RoutedEventArgs e)
        {
            // Vérifier les entrées
            if (string.IsNullOrEmpty(TxtPlayerName.Text) || string.IsNullOrEmpty(TxtPlayerEmail.Text))
            {
                MessageBox.Show("Le nom et l'email sont requis.", 
                               "Erreur de validation", 
                               MessageBoxButton.OK, 
                               MessageBoxImage.Warning);
                return;
            }
            
            try
            {
                // Mettre à jour les informations du joueur
                currentPlayer.SetNom(TxtPlayerName.Text);
                currentPlayer.SetEmail(TxtPlayerEmail.Text);
                currentPlayer.SetAvatar(TxtPlayerAvatar.Text);
                
                // Mettre à jour les informations spécifiques
                if (currentPlayer is Amateur amateur)
                {
                    if (int.TryParse(TxtAmateurLevel.Text, out int level))
                    {
                        amateur.SetNiveauExperience(level);
                    }
                }
                else if (currentPlayer is Professionnel pro)
                {
                    if (int.TryParse(TxtProRanking.Text, out int ranking))
                    {
                        pro.SetClassement(ranking);
                    }
                    
                    if (int.TryParse(TxtProWins.Text, out int wins))
                    {
                        pro.SetPartiesGagnees(wins);
                    }
                }
                
                // Mettre à jour dans la base de données
                // Note: Cette fonctionnalité n'est pas implémentée dans le backend existant
                // Il faudrait ajouter une méthode UpdateJoueur dans DatabaseManager
                MessageBox.Show("Les modifications ont été enregistrées en mémoire.\nNote: La mise à jour dans la base de données n'est pas implémentée dans cette version.", 
                               "Information", 
                               MessageBoxButton.OK, 
                               MessageBoxImage.Information);
                
                // Mettre à jour l'élément dans la liste
                if (LstPlayers.SelectedItem != null)
                {
                    ListBoxItem item = (ListBoxItem)LstPlayers.SelectedItem;
                    item.Content = currentPlayer.GetNom();
                }
                
                // Sortir du mode édition
                ToggleEditMode(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la mise à jour du joueur : {ex.Message}", 
                               "Erreur", 
                               MessageBoxButton.OK, 
                               MessageBoxImage.Error);
            }
        }

        private void BtnCancelEdit_Click(object sender, RoutedEventArgs e)
        {
            // Recharger les détails du joueur pour annuler les modifications
            if (LstPlayers.SelectedItem != null)
            {
                LstPlayers_SelectionChanged(LstPlayers, null);
            }
            
            // Sortir du mode édition
            ToggleEditMode(false);
        }

        private void BtnDeletePlayer_Click(object sender, RoutedEventArgs e)
        {
            if (currentPlayer == null)
            {
                MessageBox.Show("Veuillez sélectionner un joueur à supprimer.", 
                               "Sélection requise", 
                               MessageBoxButton.OK, 
                               MessageBoxImage.Warning);
                return;
            }
            
            // Demander confirmation
            if (MessageBox.Show($"Êtes-vous sûr de vouloir supprimer le joueur {currentPlayer.GetNom()} ?", 
                              "Confirmation", 
                              MessageBoxButton.YesNo, 
                              MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    // Supprimer de la base de données
                    // Note: Cette fonctionnalité n'est pas implémentée dans le backend existant
                    // Il faudrait ajouter une méthode DeleteJoueur dans DatabaseManager
                    MessageBox.Show("La suppression dans la base de données n'est pas implémentée dans cette version.", 
                                   "Information", 
                                   MessageBoxButton.OK, 
                                   MessageBoxImage.Information);
                    
                    // Supprimer de la liste
                    if (LstPlayers.SelectedItem != null)
                    {
                        LstPlayers.Items.Remove(LstPlayers.SelectedItem);
                    }
                    
                    // Vider les détails
                    ClearPlayerDetails();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur lors de la suppression du joueur : {ex.Message}", 
                                   "Erreur", 
                                   MessageBoxButton.OK, 
                                   MessageBoxImage.Error);
                }
            }
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            // Recharger la liste des joueurs
            LoadPlayers();
            
            // Vider les détails
            ClearPlayerDetails();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            // Fermer la fenêtre
            this.Close();
        }
    }
}