using Lab2.Model;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Lab2.Service
{
    public class PongService: IPongService
    {
        private readonly ConcurrentDictionary<string, Player> UsersInLobby = new ConcurrentDictionary<string, Player>();

        public void UpdatePlayer(string player, string position, string connectionId)
        {
            UsersInLobby.AddOrUpdate(connectionId, new Player(connectionId, player, position), (key, oldPlayer)
                => new Player(key, player, position));
        }

        public List<Player> PlayerToList()
        {
            return UsersInLobby.Select(x => new Player(x.Key, x.Value.UserName!, x.Value.PlayerPosition!)).ToList();
        }

        public int GetPlayerCount()
        {
            return UsersInLobby.Count;  
        }

        public int GetPlayerPositionCount()
        {
            return UsersInLobby.Count(u => u.Value.PlayerPosition != "");
        }

        public IEnumerable<string?> GetPlayerPosition()
        {
            return UsersInLobby.Select(x => x.Value.PlayerPosition);
        }

        public ConcurrentDictionary<string, Player> GetConcurrentDictionary()
        {
            return UsersInLobby;
        }
    }
}
