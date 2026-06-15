namespace Persistence
{
  public interface ISerializable
  {
    string Serialize();
    void Deserialize(string data);
  }
}