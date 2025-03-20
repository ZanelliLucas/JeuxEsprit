// Jeux-esprit.UI/Views/AddPlayerWindow.xaml.cs
using System;
using System.Windows;
using JeuxDesprit;

namespace JeuxDespritUI.Views
{
    /// <summary>
    /// Logique d'interaction pour AddPlayerWindow.xaml
    /// </summary>
    public partial class AddPlayerWindow : Window
    {
        private readonly DatabaseManager dbManager;
        public bool DialogResult { get; private set; }

        public AddPlayerWindow()
        {
            InitializeComponent();
            
            dbManager = new DatabaseManager();
        }

        private void RadioAmateur_Checked(object sender, RoutedEventArgs e)
        {
            // Afficher les champs spécifiques aux amateurs
            GridAmateurFields.Visibility = Visibility.Visible;
            GridProFields.Visibility = Visibility.Collapsed;
        }

        private void RadioProfessionnel_Checked(object sender, RoutedEventArgs e)
        {
            // Afficher les champs spécifiques aux professionnels
            GridAmateurFields.Visibility = Visibility.Collapsed;
            GridProFields.Visibility = Visibility.Visible;
        }

        private void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            // Récupérer les valeurs du formulaire
            string nom = TxtNom.Text;
            string email = TxtEmail.Text;
            string avatar = TxtAvatar.Text;
            bool isAmateur = RadioAmateur.IsChecked == true;

            // Valider les entrées
            if (string.IsNullOrEmpty(nom) || string.IsNullOrEmpty(email))
            {
                ShowError("Le nom et l'email sont requis.");
                return;
            }

            try
            {
                // Vérifier si l'email existe déjà
                if (dbManager.EmailExiste(email))
                {
                    ShowError("Un compte avec cet email existe déjà.");
                    return;
                }

                // Créer le joueur selon le type
                Joueur joueur;
                if (isAmateur)
                {
                    // Créer un joueur amateur
                    int niveauExperience = 1;
                    if (int.TryParse(TxtAmateurLevel.Text, out int level))
                    {
                        niveauExperience = Math.Clamp(level, 1, 5);
                    }
                    
                    joueur = new Amateur(nom, email, avatar, niveauExperience);
                }
                else
                {
                    // Créer un joueur professionnel
                    int classement = 0;
                    int partiesGagnees = 0;
                    
                    if (int.TryParse(TxtProRanking.Text, out int ranking))
                    {
                        classement = ranking;
                    }
                    
                    if (int.TryParse(TxtProWins.Text, out int wins))
                    {
                        partiesGagnees = wins;
                    }
                    
                    joueur = new Professionnel(nom, email, avatar, classement, partiesGagnees);
                }

                // Ajouter le joueur dans la base de données
                int idJoueur = dbManager.AjouterJoueur(joueur);

                if (idJoueur != -1)
                {
                    MessageBox.Show("Joueur ajouté avec succès !", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                    DialogResult = true;
                    this.Close();
                }
                else
                {
                    ShowError("Erreur lors de l'ajout du joueur.");
                }
            }
            catch (Exception ex)
            {
                ShowError($"Erreur : {ex.Message}");
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        private void ShowError(string message)
        {
            TxtError.Text = message;
            TxtError.Visibility = Visibility.Visible;
        }

        // Surcharge DialogResult pour permettre à la fenêtre parente de savoir si l'ajout a réussi
        public new bool? ShowDialog()
        {
            base.ShowDialog();
            return DialogResult;
        }
    }
}