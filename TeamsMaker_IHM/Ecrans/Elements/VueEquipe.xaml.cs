using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TeamsMaker_VM.VueModeles;

namespace TeamsMaker_IHM.Ecrans.Elements
{
    /// <summary>
    /// Logique d'interaction pour VueEquipe.xaml
    /// </summary>
    public partial class VueEquipe : UserControl
    {
        #region --- Attributs ---
        private VMEquipe vueModele;
        #endregion

        #region --- Constructeurs ---
        public VueEquipe(VMEquipe vueModele, string nomEquipe)
        {
            this.vueModele = vueModele;
            this.vueModele.PropertyChanged += VueModele_PropertyChanged;
            InitializeComponent();

            this.NomEquipe.Text = nomEquipe;
            foreach(VMPersonnage personnage in this.vueModele.VMPersonnages) this.PanelPersonnage.Children.Add(new VuePersonnage(personnage));

            this.MiseAjJour();
        }

        //Mise à jour du métier
        private void VueModele_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "EstValide") this.MiseAjJour();
        }
        #endregion

        #region --- Méthodes ---
        private void MiseAjJour()
        {
            switch(this.vueModele.EstValide)
            {
                case true: this.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#ADEBAD"); break;
                case false: this.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFB2B2"); break;
            }
            if (this.vueModele.Score > -1) this.Score.Text = this.vueModele.Score.ToString("F1");
            else this.Score.Text = "--";
        }
        #endregion



    }
}
