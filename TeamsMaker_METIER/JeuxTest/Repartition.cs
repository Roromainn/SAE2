using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsMaker_METIER.Personnages;
using TeamsMaker_METIER.Problemes;

namespace TeamsMaker_METIER.JeuxTest
{
    /// <summary>
    /// Répartition
    /// </summary>
    public class Repartition
    {
        #region --- Attributs ---
        public JeuTest jeuTest;        //Jeu de test d'origine
        private List<Equipe> equipes;   //Liste des équipes réalisées
        #endregion

        #region --- Propriétés ---
        /// <summary>
        /// Liste de tous les personnages 
        /// </summary>
        public Personnage[] Personnages => this.jeuTest.Personnages;

        /// <summary>
        /// Liste de toutes les équipes
        /// </summary>
        public Equipe[] Equipes => this.equipes.ToArray();

        /// <summary>
        /// Liste des personnages sans équipe
        /// </summary>
        public Personnage[] PersonnagesSansEquipe
        {
            get
            {
                List<Personnage> personnages = new List<Personnage>(this.jeuTest.Personnages);
                foreach(Equipe equipe in this.equipes)
                {
                    foreach(Personnage membre in equipe.Membres)
                    {
                        personnages.Remove(membre);
                    }
                }
                return personnages.ToArray();   
            }
        }

        /// <summary>
        /// Score de la répartition
        /// </summary>
        public double Score
        {
            get
            {
                double res = 0;
                foreach (Equipe equipe in this.equipes)
                {
                    if (equipe.DernierScoreCalcule == -1) res = -1;
                    else if(res != -1) res += equipe.DernierScoreCalcule;
                }
                if (res != -1) res += PersonnagesSansEquipe.Count() * 100;
                return res;
            }
        }
        #endregion

        #region --- Constructeurs ---
        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="jeuTest">Jeu de test d'origine</param>
        public Repartition(JeuTest jeuTest)
        {
            this.jeuTest = jeuTest;
            this.equipes = new List<Equipe>();
        }
        #endregion

        #region --- Méthodes ---
        /// <summary>
        /// Ajoute une équipe
        /// </summary>
        /// <param name="equipe">Equipe à ajouter</param>
        public void AjouterEquipe(Equipe equipe)
        {
            this.equipes.Add(equipe);
        }


        /// <summary>
        /// Lance les calculs des scores des équipes
        /// </summary>
        /// <param name="probleme">Problème évalué</param>
        public void LancerEvaluation(Probleme probleme)
        {
            foreach (Equipe equipe in this.equipes) equipe.Score(probleme);
        }
        #endregion
    }
}
