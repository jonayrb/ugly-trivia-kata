﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Trivia {
    public class Game {
        private readonly List<string> _players = new List<string>();

        private readonly int[] _places = new int[6];
        private readonly int[] _purses = new int[6];

        private readonly bool[] _inPenaltyBox = new bool[6];

        private readonly LinkedList<string> _popQuestions = new LinkedList<string>();
        private readonly LinkedList<string> _scienceQuestions = new LinkedList<string>();
        private readonly LinkedList<string> _sportsQuestions = new LinkedList<string>();
        private readonly LinkedList<string> _rockQuestions = new LinkedList<string>();

        private int _currentPlayer;
        private bool _isGettingOutOfPenaltyBox;

        public Game() {
            for (var i = 0; i < 50; i++) {
                GenerateInitialData(i);
            }
        }

        private void GenerateInitialData(int i) {
            _popQuestions.AddLast("Pop Question " + i);
            _scienceQuestions.AddLast(("Science Question " + i));
            _sportsQuestions.AddLast(("Sports Question " + i));
            _rockQuestions.AddLast(CreateRockQuestion(i));
        }

        public string CreateRockQuestion(int index) {
            return "Rock Question " + index;
        }

        public bool Add(string playerName) {
            _players.Add(playerName);
            _places[HowManyPlayers()] = 0;
            _purses[HowManyPlayers()] = 0;
            _inPenaltyBox[HowManyPlayers()] = false;

            Console.WriteLine(playerName + " was added");
            Console.WriteLine("They are player number " + _players.Count);
            return true;
        }

        public int HowManyPlayers() {
            return _players.Count;
        }

        public void Roll(int roll) {
            Console.WriteLine(_players[_currentPlayer] + " is the current player");
            Console.WriteLine("They have rolled a " + roll);

            if (_inPenaltyBox[_currentPlayer]) {
                RollInPenaltyBox(roll);
            }
            else {
                NotRollInPenaltyBox(roll);
            }
        }

        private void RollInPenaltyBox(int roll) {
            if (roll % 2 != 0) {
                _isGettingOutOfPenaltyBox = true;

                Console.WriteLine(_players[_currentPlayer] + " is getting out of the penalty box");
                _places[_currentPlayer] = _places[_currentPlayer] + roll;
                if (_places[_currentPlayer] > 11) _places[_currentPlayer] = _places[_currentPlayer] - 12;

                Console.WriteLine(_players[_currentPlayer]
                                  + "'s new location is "
                                  + _places[_currentPlayer]);
                Console.WriteLine("The category is " + CurrentCategory());
                AskQuestion();
            }
            else {
                Console.WriteLine(_players[_currentPlayer] + " is not getting out of the penalty box");
                _isGettingOutOfPenaltyBox = false;
            }
        }

        private void NotRollInPenaltyBox(int roll) {
            _places[_currentPlayer] = _places[_currentPlayer] + roll;
            if (_places[_currentPlayer] > 11) _places[_currentPlayer] = _places[_currentPlayer] - 12;

            Console.WriteLine(_players[_currentPlayer]
                              + "'s new location is "
                              + _places[_currentPlayer]);
            Console.WriteLine("The category is " + CurrentCategory());
            AskQuestion();
        }

        private void AskQuestion() {
            var currentCategory = CurrentCategory();
            switch (currentCategory) {
                case "Pop":
                    Console.WriteLine(_popQuestions.First());
                    _popQuestions.RemoveFirst();
                    break;
                case "Science":
                    Console.WriteLine(_scienceQuestions.First());
                    _scienceQuestions.RemoveFirst();
                    break;
                case "Sports":
                    Console.WriteLine(_sportsQuestions.First());
                    _sportsQuestions.RemoveFirst();
                    break;
                case "Rock":
                    Console.WriteLine(_rockQuestions.First());
                    _rockQuestions.RemoveFirst();
                    break;
            }
        }

        private string CurrentCategory() {
            switch (_places[_currentPlayer]) {
                case 0:
                case 4:
                case 8:
                    return "Pop";
                case 1:
                case 5:
                case 9:
                    return "Science";
                case 2:
                case 6:
                case 10:
                    return "Sports";
                default:
                    return "Rock";
            }
        }

        public bool WasCorrectlyAnswered() {
            if (!_inPenaltyBox[_currentPlayer]) return AnswerIsCorrect();
            if (_isGettingOutOfPenaltyBox) {
                return GetWinnerOutOfPenaltyBox();
            }

            _currentPlayer++;
            if (_currentPlayer == _players.Count) _currentPlayer = 0;
            return true;
        }

        private bool AnswerIsCorrect() {
            Console.WriteLine("Answer was corrent!!!!");
            _purses[_currentPlayer]++;
            Console.WriteLine(_players[_currentPlayer]
                              + " now has "
                              + _purses[_currentPlayer]
                              + " Gold Coins.");

            var winner = DidPlayerWin();
            _currentPlayer++;
            if (_currentPlayer == _players.Count) _currentPlayer = 0;

            return winner;
        }

        private bool GetWinnerOutOfPenaltyBox() {
            Console.WriteLine("Answer was correct!!!!");
            _purses[_currentPlayer]++;
            Console.WriteLine(_players[_currentPlayer]
                              + " now has "
                              + _purses[_currentPlayer]
                              + " Gold Coins.");

            var winner = DidPlayerWin();
            _currentPlayer++;
            if (_currentPlayer == _players.Count) _currentPlayer = 0;

            return winner;
        }

        public bool WrongAnswer() {
            Console.WriteLine("Question was incorrectly answered");
            Console.WriteLine(_players[_currentPlayer] + " was sent to the penalty box");
            _inPenaltyBox[_currentPlayer] = true;

            _currentPlayer++;
            if (_currentPlayer == _players.Count) _currentPlayer = 0;
            return true;
        }
        private bool DidPlayerWin() {
            return _purses[_currentPlayer] != 6;
        }
    }
}
