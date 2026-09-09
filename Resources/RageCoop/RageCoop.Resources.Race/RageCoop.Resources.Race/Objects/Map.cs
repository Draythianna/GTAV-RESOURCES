using GTA;
using System.Xml.Serialization;

namespace RageCoop.Resources.Race.Objects
{
    /// <summary>
    /// SHVDN-free Vector3 for XML serialization on the server side.
    /// </summary>
    public struct SerializableVector3
    {
        public float X;
        public float Y;
        public float Z;

        public SerializableVector3(float x, float y, float z)
        {
            X = x; Y = y; Z = z;
        }

        public static implicit operator GTA.Math.Vector3(SerializableVector3 v)
            => new GTA.Math.Vector3(v.X, v.Y, v.Z);

        public static implicit operator SerializableVector3(GTA.Math.Vector3 v)
            => new SerializableVector3(v.X, v.Y, v.Z);
    }

    [XmlRoot(ElementName = "Race")]
    public class Map
    {
        [XmlArrayItem(ElementName = "Vector3")]
        public SerializableVector3[] Checkpoints;

        [XmlArrayItem(ElementName = "SpawnPoint")]
        public SpawnPoint[] SpawnPoints;

        [XmlArrayItem(ElementName = "VehicleHash")]
        public VehicleHash[] AvailableVehicles;

        [XmlArrayItem(ElementName = "SavedProp")]
        public SavedProp[] DecorativeProps;

        public string Description;
        public string Name;

        public Map() { }
    }

    public class SpawnPoint
    {
        public SerializableVector3 Position;
        public float Heading;
    }

    public class SavedProp
    {
        public SerializableVector3 Position;
        public SerializableVector3 Rotation;
        public int Hash;
        public bool Dynamic;
        public int Texture;
    }
}
