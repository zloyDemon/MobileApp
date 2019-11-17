using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Advice 
{
    public int AdviceId { get; set; }
    public string AdviceText { get; set; }

    public Advice(){}

    public Advice(int adviceId, string adviceText)
    {
        AdviceId = adviceId;
        AdviceText = adviceText;
    }
}
