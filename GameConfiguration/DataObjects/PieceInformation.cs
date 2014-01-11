﻿using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace GameConfiguration.DataObjects
{
    public struct PieceInformation
    {
        [JsonProperty]
        public String Name { get; set; }

        [JsonProperty]
        public Point[] Positions { get; set; }

        [JsonProperty]
        public int RotationsCount { get; set; }
    }
}