
using Project.Models;

namespace Project.interfaces
{
    public interface IJewelService
    {
        List<Jewel> GetAllList(string token);

        Jewel GetJewelById(int id, string token);

        int Create(Jewel newJewel, string token);

        int Update(int id, Jewel jewel, string token);

        void Delete(int id, string token);


    }
}
