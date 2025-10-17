using System;
using System.Collections.Generic;

[Serializable]
public class BuildingEntry
{
    public string id;
    public string prefabPath;
    public int width;
    public int height;
}

[Serializable]
public class BuildingsConfig
{
    public int ppu = 32;
    public List<BuildingEntry> buildings = new List<BuildingEntry>();
}
