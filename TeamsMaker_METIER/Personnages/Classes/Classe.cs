using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamsMaker_METIER.Personnages.Classes
{
    /// <summary>
    /// Liste des classes des personnages
    /// </summary>
    public enum Classe
    {
        ARCHER, BARBARE, BARDE, CLERC, DRUIDE, MAGE, MOINE, PALADIN, VOLEUR, WARLOCK, GUERRIER, SORCIER
    }

    /// <summary>
    /// Classe d'extension pour l'énumération
    /// </summary>
    public static class ClasseExt
    {
        /// <summary>
        /// Role principal de la classe
        /// </summary>
        /// <param name="classe">La classe</param>
        /// <returns>Son rôle principal</returns>
        public static Role RolePrincipal(this Classe classe)
        {
            Role role = Role.AUCUN;
            switch (classe)
            {
                case Classe.BARBARE:
                case Classe.PALADIN:
                case Classe.GUERRIER:
                    role = Role.TANK; 
                    break;
                case Classe.ARCHER:
                case Classe.VOLEUR:
                case Classe.MAGE:
                case Classe.SORCIER:
                case Classe.WARLOCK:
                case Classe.MOINE:
                    role = Role.DPS;
                    break;
                case Classe.BARDE:
                case Classe.CLERC:
                case Classe.DRUIDE:
                    role = Role.SUPPORT;
                    break;
            }
            return role;
        }

        /// <summary>
        /// Rôle secondaire de la classe
        /// </summary>
        /// <param name="classe">La classe</param>
        /// <returns>Son rôle secondaire</returns>
        public static Role RoleSecondaire(this Classe classe)
        {
            Role role = Role.AUCUN;
            switch (classe)
            {
                case Classe.DRUIDE:
                case Classe.MOINE:
                    role = Role.TANK; 
                    break;
                case Classe.GUERRIER:
                case Classe.BARBARE:
                    role = Role.DPS;
                    break;
                case Classe.WARLOCK:
                case Classe.SORCIER:
                case Classe.PALADIN:
                    role = Role.SUPPORT;
                    break;
            }
            return role;
        }
    }
}