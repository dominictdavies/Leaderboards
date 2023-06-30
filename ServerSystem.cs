using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace Leaderboards
{
    internal class ServerSystem : ModSystem
    {
        private const int PacketTimerMax = 30;
        private int _packetTimer = PacketTimerMax;
        private const int StallTimerMax = 5 * 60;
        private int _stallTimer = 0;
        private bool _oldAnyActiveBossNPC = false;

        public override void PreUpdateWorld()
        {
            if ((Main.CurrentFrameFlags.AnyActiveBossNPC || --_stallTimer > 0) && --_packetTimer == 0)
            {
                // Send large packet to all clients containing state of leaderboard
                List<Player> activePlayers = Utilities.GetActivePlayers();
                ModPacket packet = Mod.GetPacket();
                packet.Write(activePlayers.Count);
                foreach (Player player in activePlayers)
                {
                    Contribution contribution = player.GetModPlayer<LeaderboardsPlayer>().contribution;
                    packet.Write(player.whoAmI);
                    foreach (string stat in Contribution.StatNames)
                        packet.Write((long)contribution.GetStat(stat));
                }
                packet.Send();
                _packetTimer = PacketTimerMax;
            }
            else if (!Main.CurrentFrameFlags.AnyActiveBossNPC && _oldAnyActiveBossNPC) // Boss just died
            {
                _stallTimer = StallTimerMax;
            }

            _oldAnyActiveBossNPC = Main.CurrentFrameFlags.AnyActiveBossNPC;
        }
    }
}