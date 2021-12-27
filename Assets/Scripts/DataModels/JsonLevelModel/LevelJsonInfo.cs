using System.Collections.Generic;

namespace DataModels.JsonLevelModel
{
    public class LevelJsonInfo
    {
         public List<JsonCard> BoardCards { get; set; } = new List<JsonCard>();
         
         public List<JsonCard> HandCards { get; set; } = new List<JsonCard>();
         public JsonCard PivotCard { get; set; }
         
         // TO DEL?
         public int BoardLayersCount { get; set; }
    }
}