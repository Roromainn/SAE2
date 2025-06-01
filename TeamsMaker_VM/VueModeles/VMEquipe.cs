using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsMaker_METIER.Personnages;
using TeamsMaker_METIER.Problemes;

namespace TeamsMaker_VM.VueModeles
{
    /// <summary>
    /// Vue modèle d'une équipe
    /// </summary>
    public class VMEquipe : INotifyPropertyChanged
    {
        #region --- Attributs ---
        private Equipe equipe;                  //L'équipe
        private Probleme probleme;              //Le problème
        private List<VMPersonnage> personnages; //les personnages
        #endregion

        #region --- Proprietes ---
        /// <summary>
        /// Liste des personnages
        /// </summary>
        public IEnumerable<VMPersonnage> VMPersonnages => this.personnages;

        /// <summary>
        /// Score de l'équipe
        /// </summary>
        public double Score => this.equipe.Score(probleme);

        /// <summary>
        /// L'équipe est-elle valide (au sens du problème)
        /// </summary>
        public bool EstValide => this.equipe.EstValide(this.probleme);
        #endregion

        #region --- Constructeurs ---
        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="equipe">L'équipe</param>
        /// <param name="probleme">Le problème</param>
        public VMEquipe(Equipe equipe, Probleme? probleme)
        {
            this.equipe = equipe;
            this.probleme = probleme??Probleme.SIMPLE;

            this.personnages = new List<VMPersonnage>();
            foreach (Personnage personnage in this.equipe.Membres) this.personnages.Add(new VMPersonnage(personnage, this.probleme));
        }

        /// <summary>
        /// Changement de problème
        /// </summary>
        /// <param name="probleme">Nouveau problème</param>

        public void ChangeProbleme(Probleme probleme)
        {
            foreach(VMPersonnage personnage in this.personnages) personnage.ChangeProbleme(probleme);
            this.probleme = probleme;
            this.Notifier("EstValide");
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
