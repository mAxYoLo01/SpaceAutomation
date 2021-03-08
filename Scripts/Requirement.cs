using System;

[Serializable]
public class Requirement
{
    public RequirementType type;
    public CraftItem item;
}

public enum RequirementType
{
    ITEMS_SOLD,
    ITEMS_CRAFTED,
}