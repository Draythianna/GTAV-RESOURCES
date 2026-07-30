using System.Xml.Serialization;

namespace RageCoop.Resources.Race.Objects
{
    public struct RaceVector3
    {
        public float X;
        public float Y;
        public float Z;

        public GTA.Math.Vector3 ToGTA() => new GTA.Math.Vector3(X, Y, Z);
        public static RaceVector3 FromGTA(GTA.Math.Vector3 v) => new RaceVector3 { X = v.X, Y = v.Y, Z = v.Z };
    }

    [XmlRoot(ElementName = "Race")]
    public class Map
    {
        [XmlArrayItem(ElementName = "Vector3")]
        public RaceVector3[] Checkpoints;
        public SpawnPoint[] SpawnPoints;
        public string[] AvailableVehicles;
        public SavedProp[] DecorativeProps;
        public string Description;
        public string Name;
        public Map() { }
    }

    public class SpawnPoint
    {
        public RaceVector3 Position;
        public float Heading;
    }

    public class SavedProp
    {
        public RaceVector3 Position;
        public RaceVector3 Rotation;
        public int Hash;
        public bool Dynamic;
        public int Texture;
    }
}