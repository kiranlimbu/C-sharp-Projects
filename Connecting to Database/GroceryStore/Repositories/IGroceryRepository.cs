using GroceryStore.Models;
using System.Collections.Generic;

namespace GroceryStore.Repositories
{
    public interface IGroceryRepository
    {
        IEnumerable<GroceryItem> GetItems();
        GroceryItem GetItemByID(int id);
        void AddItem(GroceryItem item);
        void DeleteItem(int id);
        void SaveChanges();
    }
}
