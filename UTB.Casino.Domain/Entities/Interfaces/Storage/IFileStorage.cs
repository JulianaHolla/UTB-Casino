namespace UTB.Casino.Domain.Entities.Interfaces.Storage;

public interface IFileStorage
{
    //Stores an image and returns its public relative path
    Task<string> SaveImageAsync(Stream content, string extension);
    void Delete(string? relativePath);
}
