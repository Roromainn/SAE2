using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamsMaker_METIER.Algorithmes
{
    /// <summary>
    /// Liste des noms d'algorithmes
    /// </summary>
    public enum NomAlgorithme
    {
        GLOUTONCOIRSSANT,
        ROLEADAPTATIF,
        ROLESTRAT,
        EXTREMESPREMIER,
        EQUILIBREPROGRESSIF,
        NSWAP,
        NOPT,
        ALGOTEST,

    }


    public static class NomAlgorithmeExt
    {
        /// <summary>
        /// Affichage du nom de l'algorithme
        /// </summary>
        /// <param name="algo">NomAlgorithme</param>
        /// <returns>La chaine de caractères à afficher</returns>
        public static string Affichage(this NomAlgorithme algo)
        {
            string res = "Algorithme non nommé :(";
            switch(algo)
            {
                
                case NomAlgorithme.GLOUTONCOIRSSANT: res = "Algo glouton"; break;
                case NomAlgorithme.ROLEADAPTATIF: res = "Algo adaptatif par rôle"; break;
                case NomAlgorithme.ROLESTRAT: res = "Algo stratifié par role"; break;
                case NomAlgorithme.EXTREMESPREMIER: res = "Algorithme extremes en premier"; break;
                case NomAlgorithme.EQUILIBREPROGRESSIF: res = "Algorithme equilbre progressif"; break;
                case NomAlgorithme.NSWAP: res = "Algorithmes Nswap"; break;
                case NomAlgorithme.NOPT: res = "Algorithme NOPT"; break;
            }
            return res;
        }
    }
}
