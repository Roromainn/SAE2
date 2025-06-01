using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsMaker_METIER.Personnages;
using TeamsMaker_METIER.Personnages.Classes;

namespace TeamsMaker_METIER.JeuxTest
{
    public class JeuTest
    {
        #region --- Attributs ---
        private List<Personnage> personnages;   //Les personnages
        #endregion

        #region --- Propriétés ---
        /// <summary>
        /// Les personnages
        /// </summary>
        public Personnage[] Personnages => this.personnages.ToArray();
        #endregion

        #region --- Constructeurs ---
        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        public JeuTest()
        {
            this.personnages = new List<Personnage>();

            
        }
        #endregion

        #region --- Méthodes ---
        /// <summary>
        /// Ajout d'un personnage
        /// </summary>
        /// <param name="personnage">Le personnage à ajouter</param>
        public void AjouterPersonnage(Personnage personnage)
        {
            this.personnages.Add(personnage);
        }
        #endregion
    }
}
