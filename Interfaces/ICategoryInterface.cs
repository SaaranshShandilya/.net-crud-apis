using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Models;

namespace TodoApi.Interfaces
{
    public interface ICategoryInterface
    {
        ICollection<Category> GetCategories();
        Category Getcategory(int id);
        ICollection<Pokemon> GetPokemonByCategory(int id);
        bool IsCategory(int id);
        bool CreateCategory(Category category);
        bool UpdateCategory(Category category);
        bool Save();

    }
}