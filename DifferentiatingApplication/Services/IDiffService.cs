using DifferentiatingApplication.DTO;

namespace DifferentiatingApplication.Services;

/// <summary>
/// Interface for the diff service
/// </summary>
public interface IDiffService
{
    void SaveData(int id, string side, byte[] data);
    object GetDiff(int id);
}
