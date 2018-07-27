using System.Collections.Generic;
using EndApi.Data;
using EndApi.Models;

namespace EndApi.Factories
{
    public class GrantedFollowerPermissionFactory
    {
        public static List<GrantedFollowerPermission> Create(List<FollowerPermission> followerPermissions, bool defaultValue, string followingId){

            var granted = new List<GrantedFollowerPermission>();
            foreach(var perm in followerPermissions){
                granted.Add(new GrantedFollowerPermission{Key=perm.Key, Name =perm.Name, Order = perm.Order, Value = defaultValue, FollowingId = followingId });
            }

            return granted;
        }
        public static List<GrantedFollowerPermission> Create(List<PermissionFollowerToGrant> permissionsToGrant, string followingId){

            var granted = new List<GrantedFollowerPermission>();
            foreach(var perm in permissionsToGrant){
                granted.Add(new GrantedFollowerPermission{Key=perm.Key, Name =perm.Name, Order = perm.Order, Value = perm.Value, FollowingId = followingId });
            }

            return granted;
        }

    }
}