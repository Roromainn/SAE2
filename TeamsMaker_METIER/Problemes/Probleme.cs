using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamsMaker_METIER.Problemes
{
    /// <summary>
    /// Liste des problèmes
    /// </summary>
    public enum Probleme
    {
        SIMPLE,ROLEPRINCIPAL,ROLESECONDAIRE
    }

    public static class ProblemeExt
    {
        public static string Affichage(this Probleme probleme)
        {
            string res = "";
            switch(probleme)
            {
                case Probleme.SIMPLE: res = "Problème simple"; break;
                case Probleme.ROLEPRINCIPAL: res = "Problème avec rôle principal"; break;
                case Probleme.ROLESECONDAIRE: res = "Problème avec rôle secondaire"; break;
            }
            return res;
        }
    }
}
