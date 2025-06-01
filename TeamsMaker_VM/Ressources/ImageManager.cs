using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace TeamsMaker_VM.Ressources
{
    /// <summary>
    /// Manager pour les image
    /// </summary>
    public class ImageManager
    {
        #region --- Attributs ---
        //Dictionnaire des images chargées
        private Dictionary<string, BitmapImage> images;
        //Dictionnaire du chemin des images
        private Dictionary<string, string> adresses;

        // Instance du Singleton
        private static ImageManager instance;
        #endregion

        #region --- Propriétés ---
        /// <summary>
        /// Instance du singleton
        /// </summary>
        private static ImageManager Instance
        {
            get
            {
                if (instance == null) instance = new ImageManager();
                return instance;
            }
        }
        #endregion

        #region --- Constructeurs ---
        /// <summary>
        /// Constructeur
        /// </summary>
        private ImageManager()
        {
            //Initialisation
            adresses = new Dictionary<string, string>();
            images = new Dictionary<string, BitmapImage>();

            //Pictogrammes
            this.AjouterImage("PICTO_ALGORITHME", "Pictogrammes/Algorithme.png");
            this.AjouterImage("PICTO_EQUIPE", "Pictogrammes/Equipe.png");
            this.AjouterImage("PICTO_FONCTION", "Pictogrammes/Fonction.png");
            this.AjouterImage("PICTO_PROBLEME", "Pictogrammes/Probleme.png");
            this.AjouterImage("PICTO_SCORE", "Pictogrammes/Score.png");

            //Personnages
            this.AjouterImage("PERSO_ARCHER", "Personnages/Archer.png");
            this.AjouterImage("PERSO_BARBARE", "Personnages/Barbarian.png");
            this.AjouterImage("PERSO_BARDE", "Personnages/Bard.png");
            this.AjouterImage("PERSO_CLERC", "Personnages/Cleric.png");
            this.AjouterImage("PERSO_DRUIDE", "Personnages/Druid.png");
            this.AjouterImage("PERSO_MAGE", "Personnages/Mage.png");
            this.AjouterImage("PERSO_MOINE", "Personnages/Monk.png");
            this.AjouterImage("PERSO_PALADIN", "Personnages/Paladin.png");
            this.AjouterImage("PERSO_VOLEUR", "Personnages/Thief.png");
            this.AjouterImage("PERSO_WARLOCK", "Personnages/Warlock.png");
            this.AjouterImage("PERSO_GUERRIER", "Personnages/Warrior.png");
            this.AjouterImage("PERSO_SORCIER", "Personnages/Wizard.png");
        }
        #endregion

        #region --- Méthodes ---
        /// <summary>
        /// Ajoute une image dans la base des chemins
        /// </summary>
        /// <param name="nom">Nom de l'image</param>
        /// <param name="path">Chemin du fichier</param>
        private void AjouterImage(string nom, string path)
        {
            adresses[nom] = path;
        }

        /// <summary>
        /// Récupère une image
        /// </summary>
        /// <param name="nom">Nom de l'image</param>
        /// <returns>L'image pour WPF</returns>
        public static BitmapImage GetImage(string nom)
        {
            if (!Instance.images.ContainsKey(nom))
            {
                if (Instance.adresses.ContainsKey(nom))
                {
                    Uri uri = new Uri("pack://application:,,,/TeamsMaker_VM;component/Ressources/Images/" + Instance.adresses[nom]);
                    Instance.images[nom] = new BitmapImage(new Uri("pack://application:,,,/TeamsMaker_VM;component/Ressources/Images/" + Instance.adresses[nom]));
                    if (Instance.images[nom].CanFreeze) { Instance.images[nom].Freeze(); }
                }
            }
            return Instance.images[nom];
        }
        #endregion
    }
}

