using System;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using PongClient.Model;

namespace PongClient.Service

{
    public class PongClientService : IPongClientService
    {
        public IObservable<string> PlayerLobby => _playerLobby;

        public async Task Connect()
        {
            _connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5000/gameroom")
                .Build();
            _connection.On<string>("StartGame", (message) => Console.WriteLine("StartGame"));
            _connection.On<string>(nameof(PlayerLobby), message => _playerLobby.OnNext(message));
            _connection.On<string>("GetTakenGameSide", mesage => Console.WriteLine("Get Side"));
            await _connection.StartAsync();
        }

        public Task GetConnectedPlayers(Player player)
        {
             return Connection.InvokeAsync("GetConnectedPlayers", player);
        }

        public Task<bool> Login(string playerName, string position)
        {
            return Connection.InvokeAsync<bool>("Login", playerName, position);
        }

        public Task GetTakenGameSide(PlayerPosition position)
        {
            return Connection.InvokeAsync("GetTakenGameSide", position);
        }

        public Task MakeGoals(GameScore gameScore)
        {
            return Connection.InvokeAsync("FinishGame", gameScore);
        }

        public Task UpdatePlayerPosition(PlayerPosition playerPosition)
        {
            return Connection.InvokeAsync("UpdatePlayerPosition", playerPosition);
        }

        private HubConnection Connection => _connection ?? throw new InvalidOperationException();
        private HubConnection? _connection;
        private Subject<string> _playerLobby = new();
    }
}
