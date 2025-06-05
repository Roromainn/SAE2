using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsMaker_METIER.JeuxTest;
using TeamsMaker_METIER.Personnages;

namespace TeamsMaker_METIER.Algorithmes.Realisations
{
    /// <summary>
    /// Algorithme de répartition des personnages selon la stratégie "Extrêmes en premier".
    /// Forme des équipes en prenant les 2 personnages les plus faibles et les 2 plus forts restants.
    /// </summary>
    public class AlgorithmeExtremesEnPremier : Algorithme
    {

        #region -- Méthodes -- 

        /// <summary>
        /// Répartit les personnages d’un jeu de test en équipes selon la méthode des extrêmes :
        /// à chaque itération, on forme une équipe avec les deux personnages ayant les plus faibles niveaux,
        /// et les deux avec les niveaux les plus élevés.
        /// </summary>
        /// <param name="jeuTest">Le jeu de test contenant les personnages à répartir</param>
        /// <returns>Une répartition contenant les équipes formées</returns>
        public override Repartition Repartir(JeuTest jeuTest)
        {
            // Création d'une copie triée des personnages par niveau principal croissant
            List<Personnage> listeRestante = new List<Personnage>(jeuTest.Personnages);
            listeRestante.Sort((p1, p2) => p1.LvlPrincipal.CompareTo(p2.LvlPrincipal));

            // Création de la répartition vide, basée sur le jeu de test
            Repartition repartition = new Repartition(jeuTest);

            // Tant qu'il reste au moins 4 personnages à répartir
            while (listeRestante.Count >= 4)
            {
                // Récupérer les deux personnages les plus faibles (en début de liste)
                Personnage faible1 = listeRestante[0];
                Personnage faible2 = listeRestante[1];

                // Récupérer les deux personnages les plus forts (en fin de liste)
                Personnage fort1 = listeRestante[listeRestante.Count - 1];
                Personnage fort2 = listeRestante[listeRestante.Count - 2];

                // Créer une nouvelle équipe avec ces 4 personnages
                Equipe equipe = new Equipe();
                equipe.AjouterMembre(faible1);
                equipe.AjouterMembre(faible2);
                equipe.AjouterMembre(fort1);
                equipe.AjouterMembre(fort2);

                // Ajouter l'équipe à la répartition
                repartition.AjouterEquipe(equipe);

                // Supprimer les membres utilisés de la liste restante
                listeRestante.RemoveAt(0); // faible1
                listeRestante.RemoveAt(0); // faible2 (devient le 1er après le retrait du précédent)
                listeRestante.RemoveAt(listeRestante.Count - 1); // fort2
                listeRestante.RemoveAt(listeRestante.Count - 1); // fort1
            }

            // Retourner la répartition complète
            return repartition;
        }

        #endregion

    }
}
