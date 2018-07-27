using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace EndApi.Data
{
    public class FollowingRequestRepository
    {
        private EndContext _context;
        public FollowingRequestRepository(EndContext context)
        {
            _context = context;
        }

        public FollowingRequest FindById(string id){
            return _context.FollowingRequests
                .Include(x=>x.Followed)
                .Include(x=>x.Requester)
                .FirstOrDefault(x=>x.Id==id);
        }

        public FollowingRequest GetFollowingRequest(string requesterId, string userToFollowId){
            var requests = _context.FollowingRequests.Where(x=>x.RequesterId== requesterId && x.FollowedId == userToFollowId);
            if(requests.Count()>0){
                return requests.SingleOrDefault();
            }
            return null;   
        }

        public void Create(FollowingRequest followingRequest){
            _context.FollowingRequests.Add(followingRequest);
        }

        public void Update(FollowingRequest followingRequest){
            _context.FollowingRequests.Update(followingRequest);
        }
        
        public List<FollowingRequest> GetRequestsByFollowedId(string followedId, int? status=null){
            var queryable = _context.FollowingRequests.Include(x=>x.Requester).Where(x=>x.FollowedId == followedId);
            if(status!=null){
                     queryable.Where(x=>(int)x.Status == status.Value);
            }
            return queryable.ToList();
        }
    }
}