using Lab2.Model;
using Lab2.Service;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lab2
{
    public class PongGameHub : Hub
    {
        private readonly ILogger<PongGameHub> _logger;

        private readonly IPongService _pongService;

        public PongGameHub(IPongService pongService, ILogger<PongGameHub> logger)
        {
            _pongService = pongService;
            _logger = logger;
        }

        public async Task<bool> Login(string player, string position)
        {
            if (_pongService!.GetPlayerCount() > 2) return false;
            _logger.LogInformation($"Login: {_logger}", player);
            _pongService.UpdatePlayer(player, position, Context.ConnectionId);

            await Clients.All.SendAsync("GetConnectedUsers", _pongService.PlayerToList());
                  
            
            if (_pongService.GetPlayerCount() > 1 && _pongService.GetPlayerPositionCount() == 2)
            {
                var task = Task.Run(async () =>
                {
                    for (var i = 5; i >= 0; i--)
                    {
                        await Task.Delay(1000);
                        _logger.LogInformation("StartGameCountdown" + i);
                        await Clients.All.SendAsync("StartGameCountdown", i);
                    }
                });
                await Task.WhenAll(task);
                _logger.LogInformation("StartGame");
                await Clients.All.SendAsync("StartGame");
            }
            return true;
        }

        public async Task GetConnectedPlayers()
        {
            _logger.LogInformation($"GetConnectedPlayers {_pongService!.GetPlayerCount()}");
            await Clients.All.SendAsync("GetConnectedPlayers", _pongService.PlayerToList());
        }

        public async Task GetTakenGameSide()
        {
            _logger.LogInformation($"GetTakenGameSide {_pongService!.GetPlayerCount()}");
            await Clients.Caller.SendAsync("GetTakenGameSide", _pongService.GetPlayerPosition());
        }

        public async Task MakeGoals(GameScore gameScore)
        {
            if (gameScore.RightScore >= 10 || gameScore.LeftScore >= 10)
            {
                await Clients.All.SendAsync("FinishGame", gameScore);
                foreach (var player in _pongService!.GetConcurrentDictionary())
                {
                    player.Value.PlayerPosition = "";
                }
                await GetConnectedPlayers();
                return;
            }
        }

        public async Task UpdateGamePosition(BallPosition ballPosition)
        {
            _logger.LogInformation($"UpdateGamePosition");
            await Clients.Others.SendAsync("UpdateGamePosition", ballPosition);
        }

        public async Task UpdatePlayerPosition(PlayerPosition playerPosition)
        {
            _logger.LogInformation($"UpdatePlayerPosition");
            await Clients.Others.SendAsync("UpdatePlayerPosition", playerPosition);
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            _pongService!.GetConcurrentDictionary().Remove(Context.ConnectionId, out _);
            Clients.All.SendAsync("GetConnectedUsers", _pongService.PlayerToList());
            Clients.Others.SendAsync("GameLeft");
            return base.OnDisconnectedAsync(exception);
        }
    }
}
