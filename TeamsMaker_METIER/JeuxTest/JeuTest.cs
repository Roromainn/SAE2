using System;
using System.Collections.Generic;
using System.IO;
using TeamsMaker_METIER.Personnages;

public class JeuTest
{
    #region --- Attributs ---
    private List<Personnage> personnages;
    #endregion

    #region --- Propriétés ---
    public Personnage[] Personnages => this.personnages.ToArray();
    #endregion

    #region --- Constructeurs ---
    public JeuTest()
    {
        this.personnages = new List<Personnage>();
    }

    public JeuTest(List<Personnage> personnages)
    {
        this.personnages = personnages;
    }
    #endregion

    #region --- Méthodes ---
    public void AjouterPersonnage(Personnage personnage)
    {
        this.personnages.Add(personnage);
    }

    /// <summary>
    /// Charge un fichier texte (.jt) et retourne un JeuTest
    /// </summary>
    public static JeuTest FromFile(string path)
    {
        var lignes = File.ReadAllLines(path);
        var personnages = new List<Personnage>();

        foreach (var ligne in lignes)
            personnages.Add(new Personnage(ligne));

        return new JeuTest(personnages);
    }
    #endregion
}

