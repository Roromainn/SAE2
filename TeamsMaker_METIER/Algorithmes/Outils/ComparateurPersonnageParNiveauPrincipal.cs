using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsMaker_METIER.Personnages;

namespace TeamsMaker_METIER.Algorithmes.Outils
{
    internal class ComparateurPersonnageParNiveauPrincipal : Comparer<Personnage>
    {
        public override int Compare(Personnage x, Personnage y)
        {
            return x.LvlPrincipal - y.LvlPrincipal;
        }
        
    }
}
