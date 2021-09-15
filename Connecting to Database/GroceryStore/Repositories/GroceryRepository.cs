using GroceryStore.Models;
using System.Collections.Generic;
using System.Linq;
using GroceryStore.Data;

namespace GroceryStore.Repositories
{
    public class GroceryRepository : IGroceryRepository
    {
        // private property
        private GroceryStoreContext _context;

        // constructor
        public GroceryRepository(GroceryStoreContext context)
        {
            _context = context;
        }

        public IEnumerable<GroceryItem> GetItems()
        {
            // pull data from database and return in list format
            return _context.GroceryTable.ToList();
        }

        public GroceryItem GetItemByID(int id)
        {
            // look for the given id in the database
            return _context.GroceryTable.SingleOrDefault(x => x.ItemID == id);
        }

        public void AddItem(GroceryItem item)
        {
            _context.Add(item);
            _context.SaveChanges(); // when item is added save changes in database
        }

        public void DeleteItem(int id)
        {
            // remove given item from the database
            var item = _context.GroceryTable.SingleOrDefault(x => x.ItemID == id);
            _context.GroceryTable.Remove(item);
            _context.SaveChanges();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
