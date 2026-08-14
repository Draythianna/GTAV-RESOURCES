using Newtonsoft.Json;
using RageCoop.Client.Scripting;
using System.Collections.Generic;
using System.IO;
using GTA;

namespace RageCoop.Resources.HandlingEnforcer.Client
{
    public class Main : ClientScript
    {
        private readonly Dictionary<int, HandlingData> HandlingDatamn = new Dictionary<int, HandlingData>();
        private readonly Dictionary<GTA.HandlingData, HandlingData> ModifiedHandlings = new Dictionary<GTA.HandlingData, HandlingData>();
        private bool _loaded = false;

        protected override void OnStart()
        {
            var path = Path.Combine(CurrentResource.DataFolder, "handling.json");
            if (File.Exists(path))
                Load(path);
            else
                Logger.Warning("handling.json not found in data folder: " + CurrentResource.DataFolder);

            KeyDown += (e) =>
            {
                if (e.KeyCode == Keys.U)
                {
                    ExportAll();
                    GTA.UI.Notification.Show("handling.json exported to working directory");
                }
            };
        }

        private void Load(string s)
        {
            if (s == null)
            {
                Logger.Info("null!");
                return;
            }
            Logger.Info("Reading handling data from " + s);

            foreach (var l in File.ReadAllLines(s))
            {
                var data = JsonConvert.DeserializeObject<HandlingData>(l);
                if (!HandlingDatamn.ContainsKey(data.Hash))
                {
                    HandlingDatamn.Add(data.Hash, data);
                    Logger.Trace("loaded data:" + data.Hash);
                }
            }

            _loaded = true;
        }

        protected override void OnTick()
        {
            base.OnTick();
            if (!_loaded) return;
            foreach (var v in GTA.World.GetAllVehicles())
                ApplyHandling(v);
        }

        private void ApplyHandling(GTA.Vehicle v)
        {
            lock (ModifiedHandlings)
            {
                if (v == null) { return; }
                if (HandlingDatamn.TryGetValue(v.Model.Hash, out var data))
                {
                    var h = v.HandlingData;
                    if (!ModifiedHandlings.ContainsKey(h))
                    {
                        ModifiedHandlings.Add(h, new HandlingData(h, v.Model.Hash));
                        Logger.Trace(JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.Indented));
                        Logger.Trace(JsonConvert.SerializeObject(new HandlingData(h, v.Model.Hash), Newtonsoft.Json.Formatting.Indented));
                        data.ApplyTo(h);
                    }
                }
            }
        }

        public static void ExportAll(string path = "handling.json")
        {
            using (var w = new StreamWriter(path))
            {
                foreach (var m in GTA.Vehicle.GetAllModels())
                {
                    w.WriteLine(JsonConvert.SerializeObject(new HandlingData(GTA.HandlingData.GetByVehicleModel(m), ((GTA.Model)m).Hash), Newtonsoft.Json.Formatting.None));
                }
            }
        }

        protected override void OnAborted(GTA.AbortedEventArgs args)
        {
            foreach (var p in ModifiedHandlings)
            {
                p.Value.ApplyTo(p.Key);
            }
            ModifiedHandlings.Clear();
        }
    }
}
