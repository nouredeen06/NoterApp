using System;

namespace NoterApp.Models;

public class Tag
{
    public int ID { get; set; }
    public string? Name { get; set; }
    public string? ColorName { get; set; }
}

public class FullTag()
{   
    public bool Selected { get; set; }
    public string? Name { get; set; }
    public string? ColorHex { get; set; }
    public bool isDirty { get; set; }
}