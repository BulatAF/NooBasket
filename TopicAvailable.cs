using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace NooBasket
{
    public static class TopicAvailable
    {
        private static bool[] _topics;
        private const int TotalTopics = 19;

        static TopicAvailable()
        {
            _topics = new bool[TotalTopics + 1]; 
            _topics[1] = true; 
        }
        public static bool IsAvailable(int number)
        {
            if (number < 1 || number > TotalTopics)
                return false;

            return _topics[number]; 
        }

        public static void CompleteTopic(int number)
        {
            if (number >= 1 && number < TotalTopics)
            {
                _topics[number + 1] = true;
            }
        }
    }
}