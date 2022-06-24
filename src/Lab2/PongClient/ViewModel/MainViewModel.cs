﻿using PongClient.Model;
using PongClient.Service;
using System.Collections.ObjectModel;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PongClient.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private IPongClientService _pongService;

        public ObservableCollection<PlayerModel> Players { get; set; } = new();

        public enum GameSide
        {
            Left,
            Right
        }

        private GameSide _gameSide;

        public MainViewModel(IPongClientService pongService)
        {
            _pongService = pongService;
            pongService.PlayerLobby.Subscribe(login =>
            {
                Players.Add(new PlayerModel
                {
                    PlayerPosition = login
                });
            });
            
        }

    public bool _isLeft => _gameSide == GameSide.Left;

    public string FinishGame(GameScore score)
    {
        string gameResult;
        if ((_isLeft && score.LeftScore > score.RightScore && score.LeftScore == 10) ||
            (!_isLeft && score.LeftScore < score.RightScore && score.RightScore == 10))
        {
            gameResult = "Congratulations!\nYou Win!";
        }
        else if (score.LeftScore == score.RightScore)
        {
            gameResult = "The points are equal. FriendShip!";
        }
        else
        {
            gameResult = "You Lose!";
        }
        return gameResult;
    }
}
}
