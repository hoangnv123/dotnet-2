using Lab2.Model;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Lab2.Service
{
    public interface IPongService
    {
        void UpdatePlayer(string player, string position, string connectionId);
        List<Player> PlayerToList();
        int GetPlayerCount();
        int GetPlayerPositionCount();
        IEnumerable<string?> GetPlayerPosition();
        ConcurrentDictionary<string, Player> GetConcurrentDictionary();
    }
}
