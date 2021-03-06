﻿using System.Collections.Generic;

namespace EveIndyBoss.Infrastructure
{
    public static class Constants
    {
        public static class Systems
        {
            public const int Jita = 30000142;
            public const int O1Y = 30003686;
        }

        public static Dictionary<int, int> PackagedVolumes = new Dictionary<int, int>
        {
            [25] = 2500,
            [26] = 10000,
            [27] = 50000,
            [28] = 10000,
            [30] = 13000000,
            [31] = 500,
            [237] = 2500,
            [324] = 2500,
            [358] = 10000,
            [380] = 10000,
            [419] = 15000,
            [420] = 5000,
            [463] = 3750,
            [485] = 1300000,
            [513] = 1300000,
            [540] = 15000,
            [541] = 5000,
            [543] = 3750,
            [547] = 1300000,
            [659] = 13000000,
            [830] = 2500,
            [831] = 2500,
            [832] = 10000,
            [833] = 10000,
            [834] = 2500,
            [883] = 1300000,
            [893] = 2500,
            [894] = 10000,
            [898] = 50000,
            [900] = 50000,
            [902] = 1300000,
            [906] = 10000,
            [941] = 500000,
            [963] = 10000,
            [1022] = 500,
            [1201] = 15000,
            [1202] = 10000,
            [1283] = 2500,
            [1305] = 5000,
        };
    }
}