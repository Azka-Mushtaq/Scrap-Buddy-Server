using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class ScrapItemService
    {
        private readonly IRepository<ScrapItem> _scrapItemRepository;

        public ScrapItemService(IRepository<ScrapItem> scrapItemRepository)
        {
            _scrapItemRepository = scrapItemRepository;
            // _pickupRepository = pickupRepository;
            //_vehicleRepository = vehicleRepository;
        }

        public async Task Add(ScrapItem scrapItem)
        {
            await _scrapItemRepository.Add(scrapItem);
        }

        public async Task<List<ScrapItem>> GetAll()
        {
            return await _scrapItemRepository.GetAll();
        }


        public async Task<ScrapItem> Find(int id)
        {
            // Retrieve the user data from the User table
            return await _scrapItemRepository.Find(compValue: id.ToString());

        }

        public async Task Delete(int id)
        {

            // Call the Delete method
            await _scrapItemRepository.DeleteById(id);
        }
    }
}
