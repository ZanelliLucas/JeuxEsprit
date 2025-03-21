// Jeux-esprit.UI/Views/RegisterWindow.xaml.cs
using System;
using System.Windows;
using JeuxDesprit;

namespace JeuxEspritUI.Views
{
    /// <summary>
    /// Logique d'interaction pour RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        private readonly AuthManager authManager;

        public RegisterWindow()
        {
            InitializeComponent();
            
            // Initialiser le gestionnaire d'authentification
            var dbManager = new DatabaseManager();
            authManager = new AuthManager(dbManager);
        }

        private void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            // Récupérer les valeurs du formulaire
            string nom = TxtNom.Text;
            string email = TxtEmail.Text;
            string password = TxtPassword.Password;
            string confirmPassword = TxtConfirmPassword.Password;
            string avatar = TxtAvatar.Text;
            string type = RadioProfessionnel.IsChecked == true ? "professionnel" : "amateur";

            // Valider les entrées
            if (string.IsNullOrEmpty(nom) || string.IsNullOrEmpty(email) || 
                string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
            {
                ShowError("Veuillez remplir tous les champs obligatoires.");
                return;
            }

            if (password != confirmPassword)
            {
                ShowError("Les mots de passe ne correspondent pas.");
                return;
            }

            // Créer le compte
            int result = authManager.CreerCompte(nom, email, password, avatar, type);

            if (result != -1)
            {
                MessageBox.Show("Compte créé avec succès ! Vous pouvez maintenant vous connecter.", 
                                "Succès", 
                                MessageBoxButton.OK, 
                                MessageBoxImage.Information);
                this.Close();
            }
            else
            {
                ShowError("Échec de la création du compte. Veuillez réessayer.");
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ShowError(string message)
        {
            TxtError.Text = message;
            TxtError.Visibility = Visibility.Visible;
        }
    }
}