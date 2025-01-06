
using Project.Models;

namespace Project.interfaces
{
    public interface IJewelService
    {
        List<Jewel> GetAllList();

        Jewel GetJewelById(int id);

        void Create(Jewel newJewel);

        void Update(int id, Jewel jewel);

        void Delete(int id);


    }
}
