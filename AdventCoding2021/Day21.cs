using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCoding2021
{
    internal class Day21
    {
        public static string A (string input)
        {
            string[] lines = input.Split(new string[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);
            int player1Start = int.Parse(lines[0].Substring(28));
            int player2Start = int.Parse(lines[1].Substring(28));
            DiceGame game = new DiceGame(player1Start, player2Start);
            return game.PlayTillWin().ToString();

        }

        public static string B (string input)
        {
            string[] lines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            int player1Start = int.Parse(lines[0].Substring(28));
            int player2Start = int.Parse(lines[1].Substring(28));
            DiracDiceGame game = new DiracDiceGame(player1Start, player2Start);
            return game.MaxWinCount().ToString();

        }
    }

    internal class DiracDiceGame
    {
        Dictionary<DiracDiceGameState, DiracDiceWinCount> cheatSheet;
        DiracDiceGameState startState;
        int[] possibleDiceRollTotals;
        public DiracDiceGame(int player1Start, int player2Start)
        {
            startState = new DiracDiceGameState(0, 0, 0, player1Start, player2Start);
            cheatSheet = new Dictionary<DiracDiceGameState, DiracDiceWinCount>();
            possibleDiceRollTotals = new int[3 * 3 * 3];
            int i = 0;
            for (int a = 1; a <= 3; a++)
            {
                for (int b = 1; b <= 3; b++)
                {
                    for (int c = 1; c <= 3; c++)
                    {
                        possibleDiceRollTotals[i] = a + b + c;
                        i++;
                    }
                }
            }
        }

        internal long MaxWinCount()
        {
            DiracDiceWinCount wins = Branch(startState);
            return Math.Max(wins.player1WinCount, wins.player2WinCount);
        }

        private DiracDiceWinCount Branch(DiracDiceGameState state)
        {
            // check to see if we've done this one already
            if (cheatSheet.ContainsKey(state))
            {
                return cheatSheet[state];
            }

            // check to see if someone's won
            else if (state.playerScores[0]>= 21)
            {
                return new DiracDiceWinCount(1, 0);

            }
            else if (state.playerScores[1] >= 21)
            {
                return new DiracDiceWinCount(0, 1);
            }
            else
            {
                DiracDiceWinCount winCount = new DiracDiceWinCount(0, 0);
                foreach (int diceRoll in possibleDiceRollTotals)
                {
                    int turn = state.turn;
                    int[] playerPos = new int[2];
                    playerPos[0] = state.playerPos[0];
                    playerPos[1] = state.playerPos[1];
                    playerPos[turn] = playerPos[turn] + diceRoll;
                    while (playerPos[turn] > 10)
                    {
                        playerPos[turn] -= 10;
                    }
                    int[] playerScores = new int[2];
                    playerScores[0] = state.playerScores[0];
                    playerScores[1] = state.playerScores[1];
                    playerScores[turn] = state.playerScores[turn] + playerPos[turn];
                    turn = (state.turn + 1) % 2;

                    DiracDiceGameState newState = new DiracDiceGameState(turn, playerScores, playerPos);
                    winCount = DiracDiceWinCount.Add(winCount, Branch(newState));
                }
                cheatSheet.Add(state, winCount);
                //Console.WriteLine(state.ToString() + " " + winCount.ToString());
                return winCount;
            }
        }
    }

    internal class DiracDiceGameState
    {
        public int turn;
        public int[] playerScores;
        public int[] playerPos;

        public DiracDiceGameState(int turn, int player1Score, int player2Score, int player1Pos, int player2Pos)
        {
            this.turn = turn;
            this.playerScores = new int[] { player1Score, player2Score };
            this.playerPos = new int[] {player1Pos, player2Pos};
        }

        public DiracDiceGameState(int turn, int[] playerScores, int[] playerPos)
        {
            this.turn = turn;
            this.playerScores = playerScores;
            this.playerPos = playerPos;
        }

        public override int GetHashCode()
        {
            return turn.GetHashCode() ^ playerScores[0].GetHashCode() ^ playerScores[1].GetHashCode() ^ playerPos[0].GetHashCode() ^ playerPos[1].GetHashCode();
        }

        public override bool Equals(object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                DiracDiceGameState v = (DiracDiceGameState)obj;
                return turn == v.turn && playerScores[0] == v.playerScores[0] && playerScores[1] == v.playerScores[1] && playerPos[0] == v.playerPos[0] && playerPos[1] == v.playerPos[1];
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(turn);
            sb.Append(" ");
            foreach (int pos in playerPos)
            {
                sb.Append(pos);
                sb.Append(" ");
            }
            foreach (int score in playerScores)
            {
                sb.Append(score);
                sb.Append(" ");
            }
            return sb.ToString();

        }

    }

    internal struct DiracDiceWinCount
    {
        public long player1WinCount;
        public long player2WinCount;

        public DiracDiceWinCount(long player1WinCount, long player2WinCount)
        {
            this.player1WinCount = player1WinCount;
            this.player2WinCount = player2WinCount;
        }

        public static DiracDiceWinCount Add(DiracDiceWinCount a, DiracDiceWinCount b)
        {
            return new DiracDiceWinCount(a.player1WinCount + b.player1WinCount, a.player2WinCount + b.player2WinCount);
        }

        public override string ToString()
        {
            return player1WinCount + " " + player2WinCount;
        }
    }

    internal class DiceGame
    {
        int dice;
        int[] playerScores;
        int[] playerSpaces;
        int rollCount;
        int turn;

        public DiceGame(int player1Start, int player2Start)
        {
            dice = 0;
            playerScores = new int[2];
            playerSpaces = new int[2];
            playerSpaces[0] = player1Start;
            playerSpaces[1] = player2Start;
            rollCount = 0;
            turn = 0;
        }

        public long PlayTillWin()
        {
            while (!HasWinner())
            {
                TakeTurn();
            }

            return playerScores.Min() * rollCount;
        }

        private void TakeTurn()
        {
            // roll the dice 3 times and add up the amount
            int total = RollDice() + RollDice() + RollDice();
            // Move forward that many squares
            playerSpaces[turn] += total;
            // Wrap around to the start
            while (playerSpaces[turn] > 10)
            {
                playerSpaces[turn] -= 10;
            }
            // Add the value of the place they landed on to their score
            playerScores[turn] += playerSpaces[turn];
            turn = (turn+1) % 2;
        }

        private bool HasWinner()
        { 
            return playerScores.Max() >= 1000;
        }

        private int RollDice()
        {
            dice++;
            if (dice > 100)
            {
                dice = 1;
            }
            rollCount++;
            return dice;
        }
    }
}
