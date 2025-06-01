using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using TeamsMaker_IHM.Fenetres;

namespace TeamsMaker_IHM.Ecrans
{/// <summary>
 /// Classe abstraite représentant un écran pour la fenêtre principale
 /// </summary>
    public abstract class Ecran : UserControl
    {
        #region --- Attributs ---
        private IChangementEcran fenetre;   //Fenetre portant l'écran
        #endregion

        #region --- Propriétés ---
        /// <summary>
        /// Fenetre contenant l'écran
        /// </summary>
        protected IChangementEcran Fenetre => this.fenetre;
        #endregion

        #region --- Constructeurs ---
        public Ecran(IChangementEcran fenetre)
        {
            this.fenetre = fenetre;
        }
        #endregion

        #region --- Méthodes ---
        /// <summary>
        /// Change l'écran affiché par la fenêtre de cet écran
        /// </summary>
        /// <param name="nouvelEcran">Le nouvel écran</param>
        public void ChangerEcran(Ecran nouvelEcran)
        {
            if (fenetre != null) fenetre.ChangerEcran(nouvelEcran);
        }

        /// <summary>
        /// Méthode appelée en cas de pression d'une touche à cet écran
        /// </summary>
        /// <param name="key">Touche pressée</param>
        public virtual void OnKeyPress(Key key)
        {

        }

        /// <summary>
        /// Action réalisée à la fermeture de l'écran
        /// </summary>
        public virtual void Fermeture()
        {

        }

        /// <summary>
        /// Donne le focus à l'écran
        /// </summary>
        public virtual void GetFocus()
        {
        }
        #endregion
    }
}
