using System.Collections.Generic;

namespace BusinessContext
{
    public interface IContext
    {
        List<Vehicle> GetVehicleData();
        Vehicle GetVehicleDataByID(int ID);

        bool UpdateVehicle(Vehicle vhc);
        bool InsertVehicle(Vehicle vhc);
        bool DeleteVehicle(int ID);
    }
}