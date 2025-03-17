using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JeuxDesprit;

namespace JeuxDespritTests
{
    [TestClass]
    public class JoueurTests
    {
        [TestMethod]
        public void TestConstructeurJoueur()
        {
            // Arrange
            string nom = "Test Joueur";
            string email = "test@example.com";
            string avatar = "avatar_test.png";

            // Act
            Joueur joueur = new Joueur(nom, email, avatar);

            // Assert
            Assert.AreEqual(nom, joueur.GetNom());
            Assert.AreEqual(email, joueur.GetEmail());
            Assert.AreEqual(avatar, joueur.GetAvatar());
        }

        [TestMethod]
        public void TestConstructeurAmateur()
        {
            // Arrange
            string nom = "Test Amateur";
            string email = "amateur@example.com";
            string avatar = "avatar_amateur.png";
            int niveauExp = 3;

            // Act
            Amateur amateur = new Amateur(nom, email, avatar, niveauExp);

            // Assert
            Assert.AreEqual(nom, amateur.GetNom());
            Assert.AreEqual(email, amateur.GetEmail());
            Assert.AreEqual(avatar, amateur.GetAvatar());
            Assert.AreEqual(niveauExp, amateur.GetNiveauExperience());
        }

        [TestMethod]
        public void TestConstructeurProfessionnel()
        {
            // Arrange
            string nom = "Test Pro";
            string email = "pro@example.com";
            string avatar = "avatar_pro.png";
            int classement = 5;
            int partiesGagnees = 10;

            // Act
            Professionnel pro = new Professionnel(nom, email, avatar, classement, partiesGagnees);

            // Assert
            Assert.AreEqual(nom, pro.GetNom());
            Assert.AreEqual(email, pro.GetEmail());
            Assert.AreEqual(avatar, pro.GetAvatar());
            Assert.AreEqual(classement, pro.GetClassement());
            Assert.AreEqual(partiesGagnees, pro.GetPartiesGagnees());
        }

        [TestMethod]
        public void TestIncrementPartiesGagnees()
        {
            // Arrange
            Professionnel pro = new Professionnel("Test Pro", "pro@example.com", "avatar_pro.png", 1, 4);

            // Act
            pro.IncrementPartiesGagnees();

            // Assert
            Assert.AreEqual(5, pro.GetPartiesGagnees());
            Assert.AreEqual(2, pro.GetClassement());
        }

        [TestMethod]
        public void TestSetterGetterJoueur()
        {
            // Arrange
            Joueur joueur = new Joueur("Nom Initial", "email@initial.com", "avatar_initial.png");
            string nouveauNom = "Nouveau Nom";
            string nouvelEmail = "nouvel@email.com";
            string nouvelAvatar = "nouvel_avatar.png";

            // Act
            joueur.SetNom(nouveauNom);
            joueur.SetEmail(nouvelEmail);
            joueur.SetAvatar(nouvelAvatar);

            // Assert
            Assert.AreEqual(nouveauNom, joueur.GetNom());
            Assert.AreEqual(nouvelEmail, joueur.GetEmail());
            Assert.AreEqual(nouvelAvatar, joueur.GetAvatar());
        }
    }
}