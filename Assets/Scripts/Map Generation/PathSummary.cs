using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class PathSummary : MonoBehaviour
{
    
    public TextMeshProUGUI biomeDescription, positiveFaction, negativeFaction;
    public string desertDescription, grasslandsDescription, volcanoDescription;
    public string proAcademy, antiAcademy, proChurch, antiChurch, proGuild, antiGuild;
    // Start is called before the first frame update
    private void Awake()
    {
        grasslandsDescription = "These Marshlands, once peaceful, are now teeming with evil creatures." +
            "\n\n\n Surrounded by water, the terrain here is winding and wicked." +
            "\n\n\n Many of the Seraphs you'll find here are nimble and swift.";

        desertDescription = "A barren desert stretches into the horizon. " +
            "\n\n\n The terrain here is vast and full of hostile creatures." +
            "\n\n\n Many of the Seraphs here are toxic and michievous.";


        volcanoDescription = "A rocky, volcanic land lies ahead, bubbling with liquid hot magma." +
            "\n\n\n This Domain is full of Narrow corridors and treacherous arenas." +
            "\n\n\n Many of the Seraphs here are destructive and unstable.";
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
