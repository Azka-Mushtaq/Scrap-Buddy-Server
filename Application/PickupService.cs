using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class PickupService
    {
        private readonly IRepository<Pickup> _pickupRepository;

        public PickupService(IRepository<Pickup> pickupRepository)
        {
            _pickupRepository = pickupRepository;
        }

        public async Task<int> Add(Pickup pickup)
        {

            string columnNames = "AddressId,CustomerId,PickupDate,Status,TotalWeight,CreatedAt,UpdatedAt";
            return await _pickupRepository.Add(columnNames, pickup, "Pickup");

        }
        public async Task Update(Pickup pickup)
        {
            //Insert into User table
            Console.WriteLine(pickup.Status);
            Console.WriteLine(pickup.RiderId);

            string columnNames = "Status,RiderId";
            await _pickupRepository.Update(columnNames, pickup);

        }
        public async Task<List<Pickup>> GetAll(int riderId = -1, int customerId = -1, string status = "")
        {
            // Retrieve the user data from the User table
            string columnNames = "Id,CustomerId,RiderId,AddressId,PickupDate,PickupTime,Status,TotalWeight,CreatedAt,UpdatedAt";

            List<Pickup> pickups;
            if (riderId == -1 && customerId == -1)
                pickups = await _pickupRepository.GetAll(columnNames);

            else
            {
                if (riderId > -1)
                    pickups = await _pickupRepository.GetAll(columnNames, compParameter: "riderId", compValue: riderId.ToString());
                else if (customerId > -1)
                    pickups = await _pickupRepository.GetAll(columnNames, compParameter: "customerId", compValue: customerId.ToString());
                else if(status!="")
                    pickups = await _pickupRepository.GetAll(columnNames, compParameter: "status", compValue: "pending");
                    
                else
                pickups = await _pickupRepository.GetAll(columnNames);

            }
            return pickups;
        }


        public async Task<Pickup> Find(int id)
        {
            string columnNames = "Id,CustomerId,PickupDate,PickupTime,Status,TotalWeight,CreatedAt,UpdatedAt";
            var pickup = await _pickupRepository.Find(columnNames, compValue: id.ToString());

            return pickup;
        }

        public async Task Delete(int id)
        {


            // Call the Delete method
            await _pickupRepository.DeleteById(id);
        }


    }
}
