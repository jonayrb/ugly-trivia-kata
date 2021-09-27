using System;

namespace Trivia {
    public class GameRunner {
        private static bool _notAWinner;

        public static void Main(string[] args) {
            var aGame = new Game();

            aGame.Add("Chet");
            aGame.Add("Pat");
            aGame.Add("Sue");

            var rand = new Random();

            do {
                aGame.Roll(rand.Next(5) + 1);

                _notAWinner = rand.Next(9) == 7 ? aGame.WrongAnswer() : aGame.WasCorrectlyAnswered();
            } while (_notAWinner);
        }
    }
}