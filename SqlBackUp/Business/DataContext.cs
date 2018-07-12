using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessContext
{
    public class DataContext : IContext, IDisposable
    {
        public bool DeleteVehicle(int ID)
        {
            throw new System.NotImplementedException();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources
            }
            // free native resources if there are any.
        }

        public List<Vehicle> GetVehicleData()
        {
            using (VehicleDataContext context = new VehicleDataContext())
            {
                return context.Vehicle.ToList();
            }
        }

        public Vehicle GetVehicleDataByID(int ID)
        {
            using (VehicleDataContext context = new VehicleDataContext())
            {
                return context.Vehicle.FirstOrDefault(vh => vh.ID == ID);
            }
        }

        public bool InsertVehicle(Vehicle vhc)
        {
            using (VehicleDataContext context = new VehicleDataContext())
            {
                try
                {
                    context.Vehicle.Add(vhc);
                    context.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
        }

        public bool UpdateVehicle(Vehicle vhc)
        {
            using (VehicleDataContext context = new VehicleDataContext())
            {
                try
                {
                    Vehicle oldVhc = context.Vehicle.First(vh => vh.ID == vhc.ID);
                    oldVhc.MFDate = vhc.MFDate;
                    oldVhc.Name = vhc.Name;
                    oldVhc.Quantity = vhc.Quantity;
                    context.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
        }
    }
}