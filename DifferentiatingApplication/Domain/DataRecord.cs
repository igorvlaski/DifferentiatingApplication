namespace DifferentiatingApplication.Domain;
/// <summary>
/// Class to represent the data record
/// </summary>
public class DataRecord
{
    public int Id { get; set; }
    public string Side { get; set; }
    public byte[] Data { get; set; }
}
