using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using PongClient.Model;

namespace PongClient.Service

{
    public class PongClientService : IPongClientService
    {
        public IObservable<string> PlayerJoined => _playerJoined;
        public ObservableCollection<Player> Players { get; set; } = new();

        public async Task Connect()
        {
            _connection = new HubConnectionBuilder()
                .WithUrl(" http://localhost:5000/gameroom")
                .Build();
            _connection.On<string>("StartGame", (message) => Console.WriteLine("StartGame"));
            _connection.On<string>(nameof(PlayerJoined), message => _playerJoined.OnNext(message));
            _connection.On<string>("GetTakenGameSide", mesage => Console.WriteLine("Get Side"));
            _connection.On<string>("UpdateGamePosition", (message) => {
                Console.WriteLine("UpdateGamePosition incoming");
                var position = new Ball();
            });

            await _connection.StartAsync();
        }

        public Task GetConnectedPlayers()
        {
             return Connection.InvokeAsync("GetConnectedPlayers", Players);
        }

        public Task<bool> Login(string playerName, string position)
        {
            return Connection.InvokeAsync<bool>("Login", playerName, position);
        }

        public Task GetTakenGameSide(string position)
        {
            return Connection.InvokeAsync("GetTakenGameSide", position);
        }

        public Task FinishGame(GameScore gameScore)
        {
            return Connection.InvokeAsync("FinishGame", gameScore);
        }

        public Task UpdatePlayerPosition(PlayerPosition playerPosition)
        {
            return Connection.InvokeAsync("UpdatePlayerPosition", playerPosition);
        }

        public Task StartGame()
        {
            return Connection.InvokeAsync("StartGame");
        }

        private HubConnection Connection => _connection ?? throw new InvalidOperationException();
        private HubConnection? _connection;
        private Subject<string> _playerJoined = new();
    }
}
