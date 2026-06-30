using UnityEngine;

namespace FSM
{
    public enum Comparison { Less, LessEqual, Greater, GreaterEqual, Equal, NotEqual }

    public static class ComparisonExtensions
    {
        public static bool Compare(this Comparison comparison, float measured, float threshold)
        {
            switch (comparison)
            {
                case Comparison.Less:         
                    return measured <  threshold;
                case Comparison.LessEqual:    
                    return measured <= threshold;
                case Comparison.Greater:     
                    return measured >  threshold;
                case Comparison.GreaterEqual: 
                    return measured >= threshold;
                case Comparison.Equal:        
                    return Mathf.Approximately(measured, threshold);
                case Comparison.NotEqual:     
                    return !Mathf.Approximately(measured, threshold);
                default:                      
                    return false;
            }
        }
    }
}
