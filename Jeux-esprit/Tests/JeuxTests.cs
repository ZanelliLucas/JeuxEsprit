using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JeuxDesprit;

namespace JeuxDespritTests
{
    [TestClass]
    public class JeuxTests
    {
        [TestMethod]
        public void TestConstructeurJeux()
        {
            // Arrange
            string nom = "Test Jeu";
            string principe = "Principe du jeu de test";
            int nbJoueurs = 2;

            // Act
            Jeux jeu = new Jeux(nom, principe, nbJoueurs);

            // Assert
            Assert.AreEqual(nom, jeu.GetNom());
            Assert.AreEqual(principe, jeu.GetPrincipeDuJeu());
            Assert.AreEqual(nbJoueurs, jeu.GetNombreDeJoueurs());
            Assert.IsTrue((DateTime.Now - jeu.GetDerniereConnexion()).TotalSeconds < 1);
        }

        [TestMethod]
        public void TestPlusMoinsConstructeur()
        {
            // Act
            PlusMoins jeu = new PlusMoins();

            // Assert
            Assert.AreEqual("Plus ou Moins", jeu.GetNom());
            Assert.AreEqual("Devinez un nombre entre deux bornes", jeu.GetPrincipeDuJeu());
            Assert.AreEqual(1, jeu.GetNombreDeJoueurs());
        }

        [TestMethod]
        public void TestPenduConstructeur()
        {
            // Act
            Pendu jeu = new Pendu();

            // Assert
            Assert.AreEqual("Pendu", jeu.GetNom());
            Assert.AreEqual("Devinez un mot lettre par lettre avant d'être pendu", jeu.GetPrincipeDuJeu());
            Assert.AreEqual(1, jeu.GetNombreDeJoueurs());
        }

        [TestMethod]
        public void TestCesarConstructeur()
        {
            // Act
            Cesar jeu = new Cesar();

            // Assert
            Assert.AreEqual("Chiffrement de César", jeu.GetNom());
            Assert.AreEqual("Déchiffrez ou chiffrez un message en utilisant le chiffrement de César", jeu.GetPrincipeDuJeu());
            Assert.AreEqual(1, jeu.GetNombreDeJoueurs());
        }

        [TestMethod]
        public void TestVigenereConstructeur()
        {
            // Act
            Vigenere jeu = new Vigenere();

            // Assert
            Assert.AreEqual("Chiffrement de Vigenère", jeu.GetNom());
            Assert.AreEqual("Déchiffrez un message en utilisant le chiffrement de Vigenère", jeu.GetPrincipeDuJeu());
            Assert.AreEqual(1, jeu.GetNombreDeJoueurs());
        }
    }
}