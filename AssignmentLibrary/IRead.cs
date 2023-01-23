using System.Text.Json.Nodes;

namespace AssignmentLibrary
{
  public interface IRead<T>
    {
        Task<List<T>> ReadInformation();

        Task<bool> ClearInformation();
    }
}