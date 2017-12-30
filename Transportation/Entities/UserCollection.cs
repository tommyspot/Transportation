using System.Collections.ObjectModel;

namespace Transportation
{
    public class UserCollection: KeyedCollection<string, User>
    {
        protected override string GetKeyForItem(User user)
        {
            return user.UserName;
        }
    }
}
