using UnityEngine;

namespace Utility
{
    public class Cameras : MonoBehaviour
    {
        public static Camera Main;

        private void Awake()
        {
            Main = Camera.main;
        }

        public static float GetDistanceMainToVectorZero()
        {
            return Main.transform.position.z - Vector3.zero.z;
        }
     
        //todo here
        public static float GetDistanceMainToGameBoard()
        {
            //return GameField.Instance.transform.position.z - Main.transform.position.z;
            return 0;
        }
        //todo зробити метод для координат рамки
    }
}