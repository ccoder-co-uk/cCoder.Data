namespace cCoder.Data.Services.Foundations;

public interface IMetadataTypeCacheService
{
    void Set(string scope, IEnumerable<string> typeSetPayloads);
    string[] Get(string scope);
    string[] GetAll();
    bool Contains(string scope);
    void Clear(string scope);
}


