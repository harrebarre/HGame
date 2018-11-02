using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGame
{
    public static class LayerDepths
    {
        public const float Step = 0.001f;

        public const float Floor = 0f;
        public const float AboveFloor = 0.1f;
        public const float Markers = 0.4f;
        public const float SmallVehicles = 0.5f;
        public const float Vehicles = 0.6f;
        public const float LargeVehicles = 0.7f;
        public const float Modules = 0.8f;
        public const float Projectiles = 0.9f;

        public const float UserInterface = 1f;
    }
}
