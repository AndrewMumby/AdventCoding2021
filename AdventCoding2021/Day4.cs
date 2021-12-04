using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCoding2021
{
    internal class Day4
    {
        public static string A(string input)
        {
            string[] lines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            // first line is the called numbers
            string[] numberString = lines[0].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            List<int> numbers = numberString.Select(x => Convert.ToInt32(x)).ToList();

            List<BingoCard> cards = CreateCards(input);

            // Now run the numbers
            foreach (int number in numbers)
            {
                foreach (BingoCard card in cards)
                {
                    if (card.Draw(number))
                    {
                        return (card.GetScore() * number).ToString();
                    }
                }
            }
            return "No Winner";
        }

        public static string B(string input)
        {
            string[] lines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            // first line is the called numbers
            string[] numberString = lines[0].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            List<int> numbers = numberString.Select(x => Convert.ToInt32(x)).ToList();

            List<BingoCard> cards = CreateCards(input);

            // Run the numbers
            int numberEntry = 0;
            while (cards.Count > 1)
            {
                int number = numbers[numberEntry];
                List<BingoCard> remainingCards = new List<BingoCard>();
                foreach (BingoCard card in cards)
                {
                    if (!card.Draw(number))
                    {
                        remainingCards.Add(card);
                    }
                }
                cards = remainingCards;
                numberEntry++;
            }
            // Just one card left now. Run it till it wins
            BingoCard selectedCard = cards[0];
            while (!selectedCard.Draw(numbers[numberEntry]))
            {
                numberEntry++;
            }
            return (selectedCard.GetScore() * numbers[numberEntry]).ToString();


        }

        private static List<BingoCard> CreateCards(string input)
        {
            string[] lines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            // the rest is the cards
            int i = 2;
            List<BingoCard> cards = new List<BingoCard>();
            List<string> cardLines = new List<string>();
            while (i < lines.Length)
            {
                string line = lines[i];
                if (line != "")
                {
                    cardLines.Add(line);
                }
                else
                {
                    cards.Add(new BingoCard(cardLines.ToArray()));
                    cardLines = new List<string>();
                }
                i++;
            }
            cards.Add(new BingoCard(cardLines.ToArray()));
            return cards;
        }
    }
    internal class BingoCard
    {
        Dictionary<int, IntVector2> cardNumbers;
        Dictionary<IntVector2, bool> cardMarked;
        int maxX;
        int maxY;
        public BingoCard(string[] input)
        {
            maxX = 0;
            maxY = 0;
            cardNumbers = new Dictionary<int, IntVector2>();
            cardMarked = new Dictionary<IntVector2, bool>();
            /*
             22 13 17 11  0
              8  2 23  4 24
             21  9 14 16  7
              6 10  3 18  5
              1 12 20 15 19
            */
            int y = 0;
            foreach (string line in input)
            {
                int x = 0;
                string[] parts = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string part in parts)
                {
                    IntVector2 location = new IntVector2(x, y);
                    cardNumbers.Add(Convert.ToInt32(part), location);
                    cardMarked.Add(location, false);
                    maxX = Math.Max(x, maxX);
                    x++;
                }
                maxY = Math.Max(y, maxY);
                y++;
            }
        }

        public bool Draw(int number)
        {
            IntVector2 location = null;
            if (cardNumbers.ContainsKey(number))
            {
                location = cardNumbers[number];
                cardMarked[location] = true;
                return IsWinner();
            }
            return false;
        }

        public bool IsWinner()
        {
            bool allMarked;
            //The horizontals
            for (int y = 0; y <= maxY; y++)
            {
                allMarked = true;
                for (int x = 0; x <= maxX; x++)
                {
                    allMarked = allMarked && cardMarked[new IntVector2(x, y)];
                }
                if (allMarked)
                {
                    return true;
                }
            }
            //The verticals
            for (int x = 0; x <= maxX; x++)
            {
                allMarked = true;
                for (int y = 0; y <= maxY; y++)
                {
                    allMarked = allMarked && cardMarked[new IntVector2(x, y)];
                }
                if (allMarked)
                {
                    return true;
                }
            }
            /*
            //The diagonals
            allMarked = true;
            for (int i = 0; i <= maxY; i++)
            {
                allMarked = allMarked && cardMarked[new IntVector2(i, i)];
            }
            if (allMarked)
            {
                return true;
            }

            allMarked = true;
            for (int i = 0; i <= maxY; i++)
            {
                allMarked = allMarked && cardMarked[new IntVector2(i, maxY-i)];
            }
            if (allMarked)
            {
                return true;
            }
            */

            return false;
        }

        public int GetScore()
        {
            int score = 0;
            List<IntVector2> unmarkedNumbers = new List<IntVector2>();
            foreach(KeyValuePair<IntVector2, bool> pairs in cardMarked)
            {
                if (pairs.Value == false)
                {
                    unmarkedNumbers.Add(pairs.Key);
                }
            }
            foreach(KeyValuePair<int ,IntVector2> pair in cardNumbers)
            {
                if (unmarkedNumbers.Contains(pair.Value))
                {
                    score += pair.Key;
                }
            }
            return score;
        }
    }
}
