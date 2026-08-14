using RageCoop.Server;
using RageCoop.Server.Scripting;

namespace RageCoop.Resources.Race.Objects
{
    public class Session
    {
        public State State;
        public Dictionary<Client, string> Votes = new();
        public DateTime NextEvent;

        public List<Player> Players = new();
        public Map Map;
        public long RaceStart;

        /// <summary>
        /// Rank all player's position
        /// </summary>
        public void Rank()
        {
            var checkPoints = Map.Checkpoints;
            var ordered=Players.OrderByDescending(x=>
            {
                double score = x.CheckpointsPassed;
                if (x.CheckpointsPassed<checkPoints.Length)
                {
                    if (x.Client.Player.LastVehicle!=null)
                    {
                        score-=x.Client.Player.LastVehicle.Position.DistanceTo(checkPoints[x.CheckpointsPassed].ToGTA())*0.000001;
                    }
                    else
                    {
                        score-=1;
                    }
                }
                return score;
            }).ToArray();
            for(int i = 0; i<ordered.Length; i++)
            {
                ordered[i].Ranking=(ushort)(i+1);
            }
        }
    }

    public class Player
    {
        public Client Client;
        public int VehicleHash;
        public int CheckpointsPassed;
        public ushort Ranking = 1;
        public ServerVehicle Vehicle;

        public Player(Client client)
        {
            Client = client;
            CheckpointsPassed = 0;
        }
    }
}
