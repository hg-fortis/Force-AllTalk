﻿using CounterStrikeSharp.API.Modules.Cvars;
using CounterStrikeSharp.API.Core;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Force_AllTalk
{
    public class ForceAllTalk : BasePlugin
    {
        public override string ModuleName => "Force AllTalk";
        public override string ModuleAuthor => "Fortis";
        public override string ModuleDescription => "Enables all convar variables";
        public override string ModuleVersion => "1.1";

        ConVar? sv_alltalk = null!;
        ConVar? sv_deadtalk = null!;
        ConVar? sv_full_alltalk = null!;
        ConVar? sv_talk_enemy_dead = null!;
        ConVar? sv_talk_enemy_living = null!;

        ConfigInfo _config = new();

        public override void Load(bool hotReload)
        {
            _config = CreateOrLoadConfig();

            if (!_config.GeneralConfig!.Enabled)
            {
                Console.WriteLine("[ForceAllTalk] Plugin not enabled!");
                return;
            }

            sv_alltalk = ConVar.Find("sv_alltalk");
            sv_deadtalk = ConVar.Find("sv_deadtalk");
            sv_full_alltalk = ConVar.Find("sv_full_alltalk");
            sv_talk_enemy_dead = ConVar.Find("sv_talk_enemy_dead");
            sv_talk_enemy_living = ConVar.Find("sv_talk_enemy_living");

            RegisterEventHandler<EventRoundStart>(OnRoundStart);
        }

        private HookResult OnRoundStart(EventRoundStart @event, GameEventInfo info)
        {
            if (!_config.GeneralConfig!.Enabled) return HookResult.Continue;

            if (sv_alltalk != null)
            {
                sv_alltalk.SetValue(true);
            }
            if (sv_deadtalk != null)
            {
                sv_deadtalk.SetValue(true);
            }
            if (sv_full_alltalk != null)
            {
                sv_full_alltalk.SetValue(true);
            }
            if (sv_talk_enemy_dead != null)
            {
                sv_talk_enemy_dead.SetValue(true);
            }
            if (sv_talk_enemy_living != null)
            {
                sv_talk_enemy_living.SetValue(true);
            }

            return HookResult.Continue;
        }

        private ConfigInfo CreateOrLoadConfig()
        {
            var configDirectory = Path.Join(ModuleDirectory, "force_alltalk.json");
            if (!File.Exists(configDirectory))
            {
                string newJson = GenerateNewConfig();
                File.WriteAllText(configDirectory, newJson);
                return JsonSerializer.Deserialize<ConfigInfo>(newJson)!;
            }

            string json = File.ReadAllText(configDirectory);
            return JsonSerializer.Deserialize<ConfigInfo>(json)!;
        }

        private string GenerateNewConfig()
        {
            var jObject = new JsonObject
            {
                ["General"] = new JsonObject
                {
                    ["Enabled"] = true
                }
            };

            string json = JsonSerializer.Serialize(jObject, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            return json;
        }

        public class ConfigInfo
        {
            [JsonPropertyName("General")]
            public GeneralConfig? GeneralConfig { get; set; }
        }

        public class GeneralConfig
        {
            public bool Enabled { get; set; }   
        }
    }
}
