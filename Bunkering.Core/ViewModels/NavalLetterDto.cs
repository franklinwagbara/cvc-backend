public class NavalLetterDto
{
    public string MarketerName { get; }
    public string ProductName { get; }
    public decimal Volume { get; }
    public string VesselName { get; }
    public string? MotherVessel { get; }
    public int Jetty { get; }
    public DateTime? ETA { get; }
    public string LoadingPort { get; }
    public string Destination { get; }

    public NavalLetterDto(string marketerName, string productName, decimal volume, string vesselName, string? motherVessel, int jetty, DateTime? eTA, string loadingPort, string destination)
    {
        MarketerName = marketerName;
        ProductName = productName;
        Volume = volume;
        VesselName = vesselName;
        MotherVessel = motherVessel;
        Jetty = jetty;
        ETA = eTA;
        LoadingPort = loadingPort;
        Destination = destination;
    }
}
