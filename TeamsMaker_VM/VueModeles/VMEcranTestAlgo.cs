using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TeamsMaker_METIER.Algorithmes;
using TeamsMaker_METIER.JeuxTest;
using TeamsMaker_METIER.JeuxTest.Parseurs;
using TeamsMaker_METIER.Personnages;
using TeamsMaker_METIER.Problemes;

namespace TeamsMaker_VM.VueModeles
{
    /// <summary>
    /// Vue Modèle de l'écran de TestAlgo
    /// </summary>
    public class VMEcranTestAlgo : INotifyPropertyChanged
    {
        #region --- Attributs ---
        //LIEN METIER
        private JeuTest? jeuTest;                       //Le jeu de test sélectionné
        private Algorithme? algorithme;                 //L'algorithme sélectionné
        private Probleme? probleme;                     //Le problème sélectionné
        private FabriqueAlgorithme fabriqueAlgorithmes; //Fabrique des algorithmes
        private Repartition repartition;                //Répartition calculée

        //ELEMENTS INTERNE
        private string nomFichierJeuTest;
        private ObservableCollection<string> listNomFichiersJeuTest;
        private List<string> listNomProblemes;
        private List<VMPersonnage> vmPersonnages;
        private List<VMEquipe> vmEquipes;
        #endregion

        #region --- Propriétés ---
        /// <summary>
        /// Nom du fichier de jeu de test sélectionné
        /// </summary>
        public string NomFichierJeuTest
        {
            get => this.nomFichierJeuTest;
            set
            {
                this.nomFichierJeuTest = value;
                this.MajJeuTest();
            }
        }

        /// <summary>
        /// Liste des noms de fichiers de test possible
        /// </summary>
        public ObservableCollection<string> ListNomFichiersJeuTest => this.listNomFichiersJeuTest;
        /// <summary>
        /// Liste des noms de problèmes
        /// </summary>
        public List<string> ListNomProblemes => this.listNomProblemes;

        /// <summary>
        /// Un problème est-il sélectionné ?
        /// </summary>
        public bool IsProblemeSelectionne => this.probleme != null;
        /// <summary>
        /// Un jeu test est-il sélectionné ?
        /// </summary>
        public bool IsJeuTestSelectionne => this.jeuTest != null;
        /// <summary>
        /// Un algorithme est-il sélectionné ?
        /// </summary>
        public bool IsAlgorithmeSelectionne => this.algorithme != null;

        /// <summary>
        /// Liste des algorithmes
        /// </summary>
        public string[] ListeAlgorithmes => this.fabriqueAlgorithmes.ListeAlgorithmes;

        /// <summary>
        /// Liste des VMPersonnages
        /// </summary>
        public VMPersonnage[] VMPersonnages => this.vmPersonnages.ToArray();

        /// <summary>
        /// Liste des VMEquipes
        /// </summary>
        public VMEquipe[] VMEquipes => this.vmEquipes.ToArray();

        /// <summary>
        /// Temps d'exécution de l'algorithme
        /// </summary>
        public long TempsExecution => this.algorithme?.TempsExecution ?? -1;

        /// <summary>
        /// Score de la répartition
        /// </summary>
        public string Score 
        {
            get {
                string res = "--";
                if(this.repartition != null && this.repartition.Score > -1) {
                    res = this.repartition.Score.ToString("F1");
                }
                return res;
            }
        }
        #endregion

        #region --- Constructeurs ---
        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        public VMEcranTestAlgo()
        {
            this.jeuTest = null;
            this.algorithme = null;
            this.probleme = null;
            this.nomFichierJeuTest = "";
            this.vmPersonnages = new List<VMPersonnage>();
            this.vmEquipes = new List<VMEquipe>();
            this.fabriqueAlgorithmes = new FabriqueAlgorithme();
            this.listNomFichiersJeuTest = new ObservableCollection<string>();
            this.listNomProblemes = Enum.GetValues(typeof(Probleme)).Cast<Probleme>().ToList().Select(p => p.Affichage()).ToList();

            this.ChargementListeFichiersJeuTest();
        }
        #endregion

        #region --- Méthodes ---
        private void ChargementListeFichiersJeuTest()
        {
            ListNomFichiersJeuTest.Clear();

            string repertoire = Path.Combine(Directory.GetCurrentDirectory(), "JeuxTest/Fichiers");
            if(!Directory.Exists(repertoire) ) Directory.CreateDirectory(repertoire);
            
            var jtFiles = Directory.GetFiles(repertoire, "*.jt").Select(Path.GetFileName);

            foreach (string jtFile in jtFiles) ListNomFichiersJeuTest.Add(jtFile);
        }

        /// <summary>
        /// Selection d'un problème
        /// </summary>
        /// <param name="selectedIndex">Indice du problème sélectionné</param>
        public void SelectionProbleme(int selectedIndex)
        {
            this.probleme = Enum.GetValues(typeof(Probleme)).Cast<Probleme>().ToArray()[selectedIndex];
            if (this.probleme != null)
            {
                foreach (VMPersonnage personnage in this.VMPersonnages) personnage.ChangeProbleme(this.probleme.Value);
                foreach (VMEquipe equipe in this.VMEquipes) equipe.ChangeProbleme(this.probleme.Value);
            }
            this.Notifier("IsProblemeSelectionne");
        }

        /// <summary>
        /// Selection d'un algorithme
        /// </summary>
        /// <param name="selectedIndex">Indice de l'algorithme sélectionné</param>
        public void SelectionAlgorithme(int selectedIndex)
        {
            var nomAlgorithme =  Enum.GetValues(typeof(NomAlgorithme)).Cast<NomAlgorithme>().ToArray()[selectedIndex];
            this.algorithme = this.fabriqueAlgorithmes.Creer(nomAlgorithme);
            this.Notifier("IsAlgorithmeSelectionne");
        }

        //Calcul les VM des personnages sans équipes et des équipes
        private void MiseAJourPersonnagesEtEquipes()
        {
            //Liste des personnages sans équipes
            this.vmPersonnages.Clear();
            foreach (Personnage personnage in this.repartition.PersonnagesSansEquipe)
            {
                this.vmPersonnages.Add(new VMPersonnage(personnage, this.probleme));
            }

            //Liste des équipes
            this.vmEquipes.Clear();
            foreach(Equipe equipe in this.repartition.Equipes)
            {
                this.vmEquipes.Add(new VMEquipe(equipe, this.probleme));
            }
            this.Notifier("Repartition");
        }

        //Mise à jour du jeu test
        private void MajJeuTest()
        {
            this.jeuTest = new Parseurs().Parser(this.nomFichierJeuTest);
            this.repartition = new Repartition(this.jeuTest);
            this.MiseAJourPersonnagesEtEquipes();

            this.Notifier("IsJeuTestSelectionne");
        }

        /// <summary>
        /// Lance les calculs si possible
        /// </summary>
        public void LancerCalculAlgorithme()
        {
            if(this.jeuTest != null && this.algorithme != null)
            {
                this.repartition = this.algorithme.Repartir(jeuTest);
                this.MiseAJourPersonnagesEtEquipes();
            }
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
