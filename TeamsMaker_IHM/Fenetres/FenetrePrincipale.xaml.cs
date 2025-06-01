using System.Media;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TeamsMaker_IHM.Ecrans;
using TeamsMaker_IHM.Ecrans.Realisations;
using TeamsMaker_IHM.Fenetres;

namespace TeamsMaker_IHM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class FenetrePrincipale : Window, IChangementEcran
    {
        #region --- Attributs ---
        private Ecran? ecran;           //Ecran de l'application en cours
        #endregion

        #region --- Constructeurs ---
        /// <summary>
        /// Constructeur
        /// </summary>
        public FenetrePrincipale()
        {
            InitializeComponent();

            this.ecran = null;

            //Gestion de la taille
            this.Height = (System.Windows.SystemParameters.PrimaryScreenHeight * 80) / 100;
            this.Width = (this.Height * 1600) / 900-55;

            //Chargement de l'écran
            this.ChangerEcran(new EcranTestAlgo(this));
        }
        #endregion

        #region --- Méthodes ---
        public void ChangerEcran(Ecran nouvelEcran)
        {
            if (this.ecran != null) this.ecran.Fermeture();
            this.ecran = nouvelEcran;
            this.GridEcran.Children.Clear();
            this.GridEcran.Children.Add(nouvelEcran);
        }

        //Fermeture de la fenetre
        private void FenetrePrincipale_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.ecran != null) this.ecran.Fermeture();
        }


        //Pression sur une touche du clavier
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.ecran != null)
            {
                ecran.OnKeyPress(e.Key);
            }
        }
        #endregion
    }
}