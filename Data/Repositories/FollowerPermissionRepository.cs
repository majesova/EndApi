using System.Collections.Generic;
using System.Linq;

namespace EndApi.Data{
    public class FollowerPermissionRepository
    {
        
        private readonly EndContext _context;
        public FollowerPermissionRepository(EndContext endContext)
        {
            _context = endContext;
        }

        public List<FollowerPermission> GetStockPermissions(){
            return _context.FollowerPermissions.OrderBy(x=>x.Order).ToList();
        }

    }
}