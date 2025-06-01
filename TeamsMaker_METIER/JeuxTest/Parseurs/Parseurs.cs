using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsMaker_METIER.Personnages;
using TeamsMaker_METIER.Personnages.Classes;

namespace TeamsMaker_METIER.JeuxTest.Parseurs
{
    public class Parseurs
    {
        //parse une ligne pour creeeer le perso associé
        private Personnage ParserLigne(string ligne)
        {
            string[] morceau = ligne.Split(' ');
            Classe classe = (Classe)Enum.Parse(typeof(Classe), morceau[0]);
            int levelPrincipal = Int32.Parse(morceau[1]);
            int levelSecondaire = Int32.Parse(morceau[2]);
            Personnage perso = new Personnage(classe, levelPrincipal, levelSecondaire);
            return perso;

        }

        /// <summary>
        /// parse le fichier .jt donne pour generer un jeu de test
        /// </summary>
        /// <param name="nomFichier">No du fichier .jt </param>
        /// <returns></returns>
        public JeuTest Parser(string nomFichier)
        {
            JeuTest jeuTest = new JeuTest();
            string cheminFichier = Path.Combine(Directory.GetCurrentDirectory(),
            "JeuxTest/Fichiers/" + nomFichier);
            using (StreamReader stream = new StreamReader(cheminFichier))
            {
                string ligne;
                while ((ligne = stream.ReadLine()) != null)
                {
                    jeuTest.AjouterPersonnage(ParserLigne(ligne));
                        
                }
            }
            return jeuTest;
        }
    }
}
