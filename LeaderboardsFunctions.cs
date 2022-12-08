﻿using Microsoft.Xna.Framework;
using Terraria;

namespace Leaderboards
{
    public static class LeaderboardsFunctions
    {
        public static void SendContribution(Player contributor, int contribution)
        {
            Main.NewText(
                contributor.name + " contributed " + contribution + " damage.",
                Color.Magenta
            );
        }
    }
}
