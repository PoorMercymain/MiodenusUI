﻿namespace MiodenusUI.MAFStructure
{
    public class Rotation
    {
        public float Angle { get; set; } = 0.0f;
        public string Unit { get; set; } = string.Empty;
        public float[] Vector { get; set; } = { 0.0f, 0.0f, 0.0f };
    }
}