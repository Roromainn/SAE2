using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsMaker_METIER.Personnages.Classes;
using TeamsMaker_METIER.Problemes;

namespace TeamsMaker_METIER.Personnages
{
    public class Equipe
    {
        #region --- Attributs ---
        private List<Personnage> membres;   //Membres de l'équipe
        private double dernierScoreCalcule;    //Dernier score calculé
        #endregion

        #region --- Propriétés ---
        /// <summary>
        /// Membres de l'équipe
        /// </summary>
        public Personnage[] Membres => this.membres.ToArray();

        public double DernierScoreCalcule => this.dernierScoreCalcule;
        #endregion

        #region --- Constructeurs ---
        /// <summary>
        /// Constructeur
        /// </summary>
        public Equipe()
        {
            this.dernierScoreCalcule = -1;
            this.membres = new List<Personnage>();
        }
        #endregion

        #region --- Méthodes ---
        /// <summary>
        /// Ajoute un membre à l'équipe
        /// </summary>
        /// <param name="personnage">Personnage à ajouter</param>
        public void AjouterMembre(Personnage personnage)
        {
            this.membres.Add(personnage);
        }

        /// <summary>
        /// Test si l'équipe est valide pour le problème donné
        /// </summary>
        /// <param name="probleme">Le problème</param>
        /// <returns>La validité</returns>
        public bool EstValide(Probleme probleme)
        {
            bool res = false;
            switch(probleme)
            {
                case Probleme.SIMPLE: res = this.EstValide(); break;
                case Probleme.ROLEPRINCIPAL: res = this.EstValideRolePrincipalUniquement(); break;
                case Probleme.ROLESECONDAIRE: res = this.EstValideRolePrincipalEtSecondaire(); break;
            }
            return res;
        }

        /// <summary>
        /// Validité d'une équipe (version de base) : contient 4 membres ?
        /// </summary>
        /// <returns>L'équipe est-elle valide (version de base)</returns>
        private bool EstValide()
        {
            return this.membres.Count == 4;
        }

        /// <summary>
        /// Vérifie si une équipe est valide (en ne regardant que le rôle principal)
        /// </summary>
        /// <returns>L'équipe est-elle valide (version 1 seul rôle)</returns>
        private bool EstValideRolePrincipalUniquement()
        {
            bool res = this.EstValide();
            if (res)
            {
                List<Role> listeDesRoles = new List<Role>();
                foreach (Personnage personnage in this.membres) listeDesRoles.Add(personnage.RolePrincipal);
                res = this.ListeRolesValide(listeDesRoles);
            }
            return res;
        }

        /// <summary>
        /// Vérifie si une équipe est valide (en regardant les rôles principaux et secondaires)
        /// </summary>
        /// <returns>L'équipe est-elle valide (version 2 rôles)</returns>
        private bool EstValideRolePrincipalEtSecondaire()
        {
            bool res = this.EstValide();
            if(res)
            {
                res = false;
                for(int i = 0;i<16 && !res;i++)
                {
                    int num = i;
                    List<Role> listeDesRoles = new List<Role>();
                    for (int j = 0;j<4;j++)
                    {
                        if (num % 2 == 0) listeDesRoles.Add(this.membres[j].RolePrincipal);
                        else listeDesRoles.Add(this.membres[j].RoleSecondaire);
                        num /= 2;
                    }
                    res = this.ListeRolesValide(listeDesRoles);
                }
            }
            return res;
        }

        //Teste si la liste des rôles donnée en entrée contient bien 1 tank, 1 support et 2 dps.
        private bool ListeRolesValide(List<Role> roles)
        {
            Dictionary<Role, int> nbRoles = new Dictionary<Role, int>();
            foreach (Role role in Enum.GetValues(typeof(Role))) nbRoles[role] = 0;
            foreach (Role role in roles) nbRoles[role] += 1;

            return (nbRoles[Role.TANK] == 1) && (nbRoles[Role.SUPPORT] == 1) && (nbRoles[Role.DPS] == 2);
        }

        /// <summary>
        /// Calcul le score de l'équipe pour le problème donné
        /// </summary>
        /// <param name="probleme">Le problème</param>
        /// <returns>Le score</returns>
        public double Score(Probleme probleme)
        {
            double res = -1;
            if(this.EstValide(probleme))
            {
                switch (probleme)
                {
                    case Probleme.SIMPLE: res = this.Score(); break;
                    case Probleme.ROLEPRINCIPAL: res = this.Score(); break;
                    case Probleme.ROLESECONDAIRE: res = this.ScoreRolePrincipalEtSecondaire(); break;
                }
            }
            dernierScoreCalcule = res;
            return res;
        }

        //Score pour le problème simple
        private double Score()
        {
            List<int> niveaux = new List<int>();
            foreach (Personnage personnage in this.Membres) niveaux.Add(personnage.LvlPrincipal);
            return this.Evaluation(niveaux);
        }

        //Score pour le problème avec rôle principal
        private double ScoreRolePrincipalEtSecondaire()
        {
            double score = -1;
            for (int i = 0; i < 16; i++)
            {
                int num = i;
                List<Role> listeDesRoles = new List<Role>();
                List<int> niveaux = new List<int>();
                for (int j = 0; j < 4; j++)
                {
                    if (num % 2 == 0)
                    {
                        listeDesRoles.Add(this.membres[j].RolePrincipal);
                        niveaux.Add(this.membres[j].LvlPrincipal);
                    }
                    else
                    {
                        listeDesRoles.Add(this.membres[j].RoleSecondaire);
                        niveaux.Add(this.membres[j].LvlSecondaire);
                    }
                    num /= 2;
                }
                if(this.ListeRolesValide(listeDesRoles) && (this.Evaluation(niveaux) < score || score==-1)) score = this.Evaluation(niveaux);
            }
            return score;
        }

        //Evaluation d'une liste de niveaux
        private double Evaluation(List<int> niveau)
        {
            return (niveau.Average() - 50) * (niveau.Average() - 50);
        }
        #endregion
    }
}
