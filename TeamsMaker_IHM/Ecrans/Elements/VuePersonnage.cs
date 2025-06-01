using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using TeamsMaker_VM.VueModeles;

namespace TeamsMaker_IHM.Ecrans.Elements
{
    /// <summary>
    /// Vue d'un personnage
    /// </summary>
    public class VuePersonnage : DockPanel
    {
        #region --- Attributs ---
        public VMPersonnage vueModele;
        private TextBlock labelNiveaux;
        #endregion

        #region --- Constructeurs ---
        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="vueModele">Vue modèle du personnage</param>
        public VuePersonnage(VMPersonnage vueModele)
        {
            this.vueModele = vueModele;
            this.vueModele.PropertyChanged += VueModele_PropertyChanged;
            
            this.labelNiveaux = new TextBlock();
            this.labelNiveaux.TextAlignment = System.Windows.TextAlignment.Center;
            this.labelNiveaux.Width = 200;
            this.labelNiveaux.FontSize = 25; 
            this.labelNiveaux.Text = this.vueModele.Niveaux;
            DockPanel.SetDock(labelNiveaux, Dock.Bottom);
            this.Children.Add(labelNiveaux);
            this.Children.Add(new Image() { Source = this.vueModele.Image });

            this.Width = 200;
            this.Margin = new System.Windows.Thickness(10);
        }

        private void VueModele_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Niveaux") this.labelNiveaux.Text = this.vueModele.Niveaux;
        }
        #endregion
    }
}
