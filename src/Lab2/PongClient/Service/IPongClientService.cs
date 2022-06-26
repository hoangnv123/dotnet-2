using PongClient.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace PongClient.Service
{
    public interface IPongClientService
    {
        Task Connect();
        Task GetConnectedPlayers();
        Task<bool> Login(string playerName, string position);
        Task GetTakenGameSide(string position);
        Task FinishGame(GameScore gameScore);
        Task UpdatePlayerPosition(PlayerPosition playerPosition);
        Task StartGame();
        IObservable<string> PlayerJoined { get; }
    }
}
