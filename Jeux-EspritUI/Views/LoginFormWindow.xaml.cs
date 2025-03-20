// Jeux-esprit.UI/Views/LoginFormWindow.xaml.cs
using System;
using System.Windows;
using JeuxDesprit;

namespace JeuxDesprit.UI.Views
{
    /// <summary>
    /// Logique d'interaction pour LoginFormWindow.xaml
    /// </summary>
    public partial class LoginFormWindow : Window
    {
        private readonly AuthManager authManager;
        public Joueur? ConnectedUser { get; private set; }
        public bool DialogResult { get; private set; }

        public LoginFormWindow()
        {
            InitializeComponent();
            // Initialiser le gestionnaire d'authentification
            var dbManager = new DatabaseManager();
            authManager = new AuthManager(dbManager);
        }

        private void BtnConnect_Click(object sender, RoutedEventArgs e)
        {
            string email = TxtEmail.Text;
            string password = TxtPassword.Password;

            // Vérifier que les champs ne sont pas vides
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ShowError("Veuillez remplir tous les champs.");
                return;
            }

            // Tenter de se connecter
            ConnectedUser = authManager.Connexion(email, password);

            if (ConnectedUser != null)
            {
                DialogResult = true;
                this.Close();
            }
            else
            {
                ShowError("Échec de la connexion. Vérifiez vos informations.");
                TxtPassword.Password = "";
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

        // Surcharge DialogResult pour permettre à la fenêtre parente de savoir si la connexion a réussi
        public new bool? ShowDialog()
        {
            base.ShowDialog();
            return DialogResult;
        }
    }
}