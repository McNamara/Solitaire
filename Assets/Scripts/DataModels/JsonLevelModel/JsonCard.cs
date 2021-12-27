using System.Collections.Generic;
using JetBrains.Annotations;

namespace DataModels.JsonLevelModel
{
    public class JsonCard
    {
        public int Id { get; set; }
        
        public float X { get; set; }
        public float Y { get; set; }
        public float Angle { get; set; }
        public int Layer { get; set; }
        
        public int Type  { get; set; }
        
        public int? Suit { get; set; }
        public int? Rank { get; set; }

        [CanBeNull] public List<int> Blockers { get; set; } = new List<int>();
        [CanBeNull] public List<int> Blocking { get; set; } = new List<int>();
    }
}