using GalaxyGuesserApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GalaxyGuesserApi.Repositories.Interfaces
{
  public interface IQuestionRepository
  {
    Task<int> GetQuestionCountForCategory(int categoryId);
  }
}