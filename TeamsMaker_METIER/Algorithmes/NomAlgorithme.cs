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
        NOPT
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
                
                case NomAlgorithme.GLOUTONCOIRSSANT: res = "Algo glouton(niv 1)"; break;
                case NomAlgorithme.EXTREMESPREMIER: res = "Algorithme extremes en premier(niv 1)"; break;
                case NomAlgorithme.EQUILIBREPROGRESSIF: res = "Algorithme equilbre progressif(niv 1)"; break;
                case NomAlgorithme.NSWAP: res = "Algorithmes Nswap(niv 1)"; break;
                case NomAlgorithme.NOPT: res = "Algorithme NOPT(niv 1)"; break;
                case NomAlgorithme.ROLESTRAT: res = "Algo stratifié par role(niv 2)"; break;
                case NomAlgorithme.ROLEADAPTATIF: res = "Algo adaptatif par rôle(niv 3)"; break;

            }
            return res;
        }
    }
}
