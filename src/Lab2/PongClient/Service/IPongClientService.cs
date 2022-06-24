using PongClient.Model;
using System;
using System.Threading.Tasks;

namespace PongClient.Service
{
    public interface IPongClientService
    {
        Task Connect();
        Task GetConnectedPlayers(Player player);
        Task<bool> Login(string playerName, string position);
        Task GetTakenGameSide(PlayerPosition position);
        Task MakeGoals(GameScore gameScore);
        Task UpdatePlayerPosition(PlayerPosition playerPosition);
        IObservable<string> PlayerLobby { get; }
    }
}
