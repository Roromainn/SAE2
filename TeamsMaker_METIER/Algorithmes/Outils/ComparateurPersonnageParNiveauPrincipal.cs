using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsMaker_METIER.Personnages;

namespace TeamsMaker_METIER.Algorithmes.Outils
{
    /// <summary>
    /// Comparer pour trier les personnages par niveau principal.
    /// </summary>
    internal class ComparateurPersonnageParNiveauPrincipal : Comparer<Personnage>
    {
        /// <summary>
        /// Compare deux personnages en fonction de leur niveau principal.
        /// </summary>
        /// <param name="x">premier personnage</param>
        /// <param name="y">deuxieme personnage</param>
        /// <returns></returns>
        public override int Compare(Personnage x, Personnage y)
        {
            return x.LvlPrincipal - y.LvlPrincipal;
        }
        
    }
}
