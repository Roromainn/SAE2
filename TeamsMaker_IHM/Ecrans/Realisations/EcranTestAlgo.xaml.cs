using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using TeamsMaker_IHM.Ecrans.Elements;
using TeamsMaker_IHM.Fenetres;
using TeamsMaker_METIER.Personnages.Classes;
using TeamsMaker_VM.Ressources;
using TeamsMaker_VM.VueModeles;

namespace TeamsMaker_IHM.Ecrans.Realisations
{
    /// <summary>
    /// Logique d'interaction pour EcranTestAlgo.xaml
    /// </summary>
    public partial class EcranTestAlgo : Ecran
    {
        #region --- Attributs ---
        private VMEcranTestAlgo vueModele;
        #endregion

        #region --- Constructeurs ---
        public EcranTestAlgo(IChangementEcran fenetre) : base(fenetre)
        {
            this.vueModele = new VMEcranTestAlgo();
            this.vueModele.PropertyChanged += VueModele_PropertyChanged;
            this.DataContext = this.vueModele;

            InitializeComponent();

            //Images
            this.ImageMode.Source = ImageManager.GetImage("PICTO_PROBLEME");
            this.ImageAlgorithme.Source = ImageManager.GetImage("PICTO_ALGORITHME");
            this.ImageJeuDeTest.Source = ImageManager.GetImage("PICTO_EQUIPE");
            this.ImageScore.Source = ImageManager.GetImage("PICTO_SCORE");

        }
        #endregion

        //Selection d'un problème
        private void SelectionProbleme(object sender, SelectionChangedEventArgs e)
        {
            this.vueModele.SelectionProbleme(this.ComboProbleme.SelectedIndex);
            this.Score.Text = this.vueModele.Score.ToString();
        }

        //Selection d'un algorithme
        private void SelectionAlgorithme(object sender, SelectionChangedEventArgs e)
        {
            this.vueModele.SelectionAlgorithme(this.ComboAlgorithme.SelectedIndex);
            this.DockScore.Visibility = Visibility.Hidden;
        }
        private void SelectionJeuTest(object sender, SelectionChangedEventArgs e)
        {
            this.DockScore.Visibility = Visibility.Hidden;
        }

        //Lancement des calculs
        private void LancerCalcul(object sender, RoutedEventArgs e)
        {
            this.vueModele.LancerCalculAlgorithme();
            this.Score.Text = this.vueModele.Score.ToString();
            this.DockScore.Visibility = Visibility.Visible;
        }

        //Mise à jour de l'affichage
        private void MiseAJourAffichage()
        {
            this.WrapPersonnage.Children.Clear();
            foreach(VMPersonnage personnage in this.vueModele.VMPersonnages)
            {
                this.WrapPersonnage.Children.Add(new VuePersonnage(personnage));
            }

            int numeroEquipe = 1;
            this.StackEquipes.Children.Clear();
            foreach (VMEquipe equipe in this.vueModele.VMEquipes)
            {
                this.StackEquipes.Children.Add(new VueEquipe(equipe,"Equipe n°"+numeroEquipe.ToString()));
                numeroEquipe++;
            }

            if (this.vueModele.TempsExecution != -1)
            {
                MessageBox.Show("Temps d'exécution : " + this.vueModele.TempsExecution.ToString() + "ms", "Temps d'exécution", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        #region --- Observation ---
        //Changement du vue modèle
        private void VueModele_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Repartition") this.MiseAJourAffichage();
        }
        #endregion

    }
}
