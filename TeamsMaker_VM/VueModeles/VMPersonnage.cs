using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using TeamsMaker_METIER.Personnages;
using TeamsMaker_METIER.Personnages.Classes;
using TeamsMaker_METIER.Problemes;
using TeamsMaker_VM.Ressources;

namespace TeamsMaker_VM.VueModeles
{
    public class VMPersonnage : INotifyPropertyChanged
    {
        #region --- Attributs ---
        private Personnage personnage;
        private Probleme probleme;
        #endregion

        #region --- Propriétés ---
        /// <summary>
        /// Image du personnage
        /// </summary>
        public ImageSource Image
        {
            get
            {
                ImageSource image = null;
                switch (this.personnage.Classe)
                {
                    case Classe.ARCHER: image = ImageManager.GetImage("PERSO_ARCHER"); break;
                    case Classe.BARBARE: image = ImageManager.GetImage("PERSO_BARBARE"); break;
                    case Classe.BARDE: image = ImageManager.GetImage("PERSO_BARDE"); break;
                    case Classe.CLERC: image = ImageManager.GetImage("PERSO_CLERC"); break;
                    case Classe.DRUIDE: image = ImageManager.GetImage("PERSO_DRUIDE"); break;
                    case Classe.MAGE: image = ImageManager.GetImage("PERSO_MAGE"); break;
                    case Classe.MOINE: image = ImageManager.GetImage("PERSO_MOINE"); break;
                    case Classe.PALADIN: image = ImageManager.GetImage("PERSO_PALADIN"); break;
                    case Classe.VOLEUR: image = ImageManager.GetImage("PERSO_VOLEUR"); break;
                    case Classe.WARLOCK: image = ImageManager.GetImage("PERSO_WARLOCK"); break;
                    case Classe.GUERRIER: image = ImageManager.GetImage("PERSO_GUERRIER"); break;
                    case Classe.SORCIER: image = ImageManager.GetImage("PERSO_SORCIER"); break;
                }
                return image;
            }
        }

        /// <summary>
        /// Affichage du niveau du joueur
        /// </summary>
        public string Niveaux 
        {
            get 
            {
                string res="";
                switch(this.probleme)
                {
                    case Probleme.SIMPLE: res = this.personnage.LvlPrincipal.ToString(); break;
                    case Probleme.ROLEPRINCIPAL: res = this.personnage.RolePrincipal.ToString()[0] + " : " + this.personnage.LvlPrincipal.ToString(); break;
                    case Probleme.ROLESECONDAIRE:
                        res = this.personnage.RolePrincipal.ToString()[0] + " : " + this.personnage.LvlPrincipal.ToString();
                        if (this.personnage.RoleSecondaire != Role.AUCUN)res += "   " + this.personnage.RoleSecondaire.ToString()[0] + " : " + this.personnage.LvlSecondaire.ToString();
                        break;
                }
                return res;
            }
        }
        #endregion

        #region --- Constructeurs ---
        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="personnage">Personnage (metier)</param>
        public VMPersonnage(Personnage personnage, Probleme? probleme)
        {
            this.probleme = probleme ?? Probleme.SIMPLE;
            this.personnage = personnage;
        }
        #endregion

        #region --- Méthodes ---
        public void ChangeProbleme(Probleme probleme)
        {
            this.probleme = probleme;
            this.Notifier("Niveaux");
        }
        #endregion

        #region --- Observation ---
        public event PropertyChangedEventHandler? PropertyChanged;
        private void Notifier(string message)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(message));
        }
        #endregion
    }
}
