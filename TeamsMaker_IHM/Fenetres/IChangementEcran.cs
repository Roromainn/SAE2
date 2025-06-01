using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsMaker_IHM.Ecrans;

namespace TeamsMaker_IHM.Fenetres
{
    /// <summary>
    /// Interface pour les classes permettant un changement d'écran
    /// </summary>
    public interface IChangementEcran
    {
        /// <summary>
        /// Changement d'écran
        /// </summary>
        /// <param name="nouvelEcran">Nouvel écran</param>
        void ChangerEcran(Ecran nouvelEcran);
    }
}
