using System;
using DataModels.JsonLevelModel;
using Enums.Levels;
using Newtonsoft.Json;
using UnityEngine;

namespace Utility
{
    public class JsonManager : GenericSingleton<JsonManager>
    {
        public LevelJsonInfo LoadJsonLevel(int levelToLoad)
        {
            string jsonLevel = LoadDataFromFile(levelToLoad);
            
            LevelJsonInfo levelInfo = new LevelJsonInfo();
            
            var settings = GetJsonSettings();
            
            JsonConvert.PopulateObject(jsonLevel, levelInfo, settings);

            return levelInfo;
        }

        private string LoadDataFromFile(int levelNumber)
        {
            string fileName;
            var levelsCount = Enum.GetNames(typeof(LevelsName)).Length;
            if (levelsCount < levelNumber)
            {
                Debug.Log($"Error on loading json. Cannot load level #{levelNumber}." +
                          $" Instead Loading last level in list.");
                fileName = Enum.GetName(typeof(LevelsName), levelsCount-1);
            }
            else
                fileName = Enum.GetName(typeof(LevelsName), levelNumber);
            var file = $"{Constants.Level.LevelsPathFolder}/{fileName}";
            var jsonLevel = Resources.Load<TextAsset>(file).text;

            return jsonLevel;
        }

        private JsonSerializerSettings GetJsonSettings()
        {
            var settings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore,
            };

            return settings;
        }
    }
}