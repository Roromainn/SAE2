﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsMaker_METIER.Algorithmes.Realisations;
using TeamsMaker_METIER.Problemes;

namespace TeamsMaker_METIER.Algorithmes
{
    /// <summary>
    /// Fabrique des algorithmes
    /// </summary>
    public class FabriqueAlgorithme
    {
        #region --- Propriétés ---
        /// <summary>
        /// Liste des noms des algorithmes
        /// </summary>
        public string[] ListeAlgorithmes => Enum.GetValues(typeof(NomAlgorithme)).Cast<NomAlgorithme>().ToList().Select(nom => nom.Affichage()).ToArray();
        #endregion

        #region --- Méthodes ---
        /// <summary>
        /// Fabrique d'algorithme en fonction du nom de l'algorithme
        /// </summary>
        /// <param name="nomAlgorithme">Nom de l'algorithme</param>
        /// <returns></returns>
        public Algorithme? Creer(NomAlgorithme nomAlgorithme, Probleme probleme)
        {
            Algorithme res = null;
            switch(nomAlgorithme)
            {
                case NomAlgorithme.GLOUTONCOIRSSANT: res = new AlgorithmeGloutonCroissant(); break;
                case NomAlgorithme.ROLEADAPTATIF: res = new RoleAdaptatif(); break;
                case NomAlgorithme.ROLESTRAT: res = new RoleEtape(); break;
                case NomAlgorithme.EXTREMESPREMIER: res = new AlgorithmeExtremesEnPremier(); break;
                case NomAlgorithme.EQUILIBREPROGRESSIF: res = new AlgorithmeEquilibreProgressif(); break;
                case NomAlgorithme.NOPT: res = new AlgoNOpt(); break;
                case NomAlgorithme.NSWAP:
                    var algoInit = new AlgorithmeGloutonCroissant();
                    res = new AlgoNSwap(3, algoInit, probleme);
                    break;
                
            }
            return res;
        }

        public Algorithme? Creer(NomAlgorithme nomAlgorithme)
        {
            // Appelle la méthode existante avec un problème par défaut
            return Creer(nomAlgorithme, Probleme.SIMPLE);
        }
        #endregion

    }
}
