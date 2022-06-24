using Lab2.Model;
using Xunit;
using Lab2.Service;

namespace TestServer
{
    public class ServiceTest
    {
        private static Player Creat(string id, string name, string position)
        {
            var player = new Player(id, name, position);
            return player;
        }

        [Fact]
        void TestUpdatePlayer()
        {
            //arrange
            var testPlayer = Creat("1", "Harry", "Left");
            PongService pongService = new();
            //act
            pongService.UpdatePlayer("Harry", "Left", "1");
            var checkPlayer = pongService.GetConcurrentDictionary()["1"];
            //assert
            Assert.Equal(testPlayer, checkPlayer);
        }

        [Fact]
        void TestPlayerToList()
        {
            //arrange
            var testPlayer = Creat("1", "Harry", "Left");
            PongService pongService = new();
            pongService.GetConcurrentDictionary().TryAdd("1", testPlayer);
            //act
            pongService.PlayerToList();
            var checkPlayer = pongService.PlayerToList()[0];
            //assert
            Assert.Equal(testPlayer, checkPlayer);
        }

        [Fact]
        void TestGetPlayerCount()
        {
            //arrange
            PongService pongService = new();
            var count = pongService.GetPlayerCount();
            //act
            var playerCount = pongService.GetConcurrentDictionary().Count;
            //assert
            Assert.Equal(count, playerCount);
        }

        [Fact]
        void TestGetPlayerPositionCount()
        {
            //arrange
            var testPlayer = Creat("1", "Harry", "Left");
            PongService pongService = new();
            pongService.GetConcurrentDictionary().TryAdd("1", testPlayer);
            //act
            var count = pongService.GetPlayerPositionCount();
            //assert
            Assert.Equal(count, pongService.GetConcurrentDictionary().Count);
        }
    }
}
