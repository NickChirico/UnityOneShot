using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seraph : MonoBehaviour
{
    public enum Genome { None, Rupture, Siphon, Contaminate, Surge, Calcify }
    public Genome seraphType;

    [Range(0,100)] public int level_Blood;
    [Range(0,100)] public int level_Carapace;
    [Range(0,100)] public int level_Temperment;

    public string desc_Blood;
    public string desc_Carapace;
    public string desc_Temperment;

    private void Start()
    {
        desc_Blood = DetermineDescription(level_Blood);
        desc_Carapace = DetermineDescription(level_Carapace);
        desc_Temperment = DetermineDescription(level_Temperment);
        this.GetComponent<SpriteRenderer>().color = DetermineTypeColor(seraphType);
    }

    private Color DetermineTypeColor(Genome g)
    {
        return g switch
        {
            Genome.Rupture => Color.red,
            Genome.Siphon => Color.green,
            Genome.Contaminate => Color.magenta,
            Genome.Surge => Color.blue,
            Genome.Calcify => Color.yellow,
            _ => Color.black,
        };
    }
    private string DetermineDescription(int level)
    {
        if (level > 66)
            return "High";
        else if (level > 33)
            return "Moderate";
        else
            return "Low";
    }
}
