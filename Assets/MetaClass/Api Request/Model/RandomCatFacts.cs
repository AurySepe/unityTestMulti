using System;


[Serializable]
public class RandomCatFacts
{
    public String fact;

    public int length; 

    public override string ToString()
    {
        return $"fact : {fact}, lenght : {length}";
    }
}
