using System.Collections.Generic;
using Card.Controller;
using DataModels.Card;

namespace Utility
{
    public class GenericDictionary
    {
        //delegate void Clicked(PlayingCardModel cardModel);
        private Dictionary<int, object> dictionary = new Dictionary<int, object>();

        public void Add<T>(int key, T value) where T : class
        {
            dictionary.Add(key, value);
        }

        public dynamic GetClassModel(int key)
        {
            if (dictionary.ContainsKey(key))
                switch (dictionary[key])
                {
                    case PlayingCardModel _:
                        return (PlayingCardModel) dictionary[key];
                    case ArrowsCardModel _:
                        return (ArrowsCardModel) dictionary[key];
                    default:
                        return null;
                }

            return null;
        }
        
        public T GetValue<T>(int key) where T : class
        {
            return dictionary[key] as T;
        }
        
        public void Remove(int key)
        {
            dictionary.Remove(key);
        }
        
        public void Clear()
        {
            dictionary.Clear();
        }
        
        public int GetCount()
        {
            return dictionary.Count;
        }
    }
}