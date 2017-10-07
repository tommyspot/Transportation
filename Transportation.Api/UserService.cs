using Transportation.Api.Framework;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Transportation.Api
{
    public class UserService
    {
        private IPasswordHash passwordHash;
        public UserService() : this(new PasswordHash()) { }
        public UserService(IPasswordHash passwordHash)
        {
            this.passwordHash = passwordHash;
        }

        [Route(HttpVerb.Get, "/user")]
        public RestApiResult GetAllUsers()
        {
            var users = ClarityDB.Instance.Users;

            return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = BuildJsonArray(users) };
        }

        [Route(HttpVerb.Post, "/user")]
        public RestApiResult Create(JObject json)
        {
            if (json == null)
                return new RestApiResult { StatusCode = HttpStatusCode.BadRequest };

            User user = User.FromJson(json);
            user.CreatedDate = DateTime.Now;

            if (ClarityDB.Instance.Users.Any(x => x.UserName.ToLower() == user.UserName.ToLower()))
            {
                string errorJson = "{ 'error': 'User name exists' }";
                return new RestApiResult { StatusCode = HttpStatusCode.Conflict, Json = JObject.Parse(errorJson) };
            }

            user.Salt = passwordHash.CreateSalt();
            user.Password = passwordHash.CreatePasswordHash(user.Password, user.Salt);

            ClarityDB.Instance.Users.Add(user);
            ClarityDB.Instance.SaveChanges();

            return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = user.ToJson() };
        }

        [Route(HttpVerb.Get, "/user/{id}")]
        public RestApiResult GetUserByID(long id)
        {
            User user = ClarityDB.Instance.Users.FirstOrDefault(x => x.ID == id);

            if (user == null)
            {
                return new RestApiResult { StatusCode = HttpStatusCode.NotFound };
            }

            return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = user.ToJson() };
        }

        [Route(HttpVerb.Delete, "/user/{id}")]
        public RestApiResult Delete(long id)
        {
            User user = ClarityDB.Instance.Users.FirstOrDefault(x => x.ID == id);

            if (user == null)
            {
                return new RestApiResult { StatusCode = HttpStatusCode.NotFound };
            }

            ClarityDB.Instance.Users.Remove(user);
            ClarityDB.Instance.SaveChanges();

            return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = user.ToJson() };
        }

        [Route(HttpVerb.Put, "/user/{id}")]
        public RestApiResult Update(long id, JObject json)
        {
            User user = ClarityDB.Instance.Users.FirstOrDefault(x => x.ID == id);

            if (user == null)
            {
                return new RestApiResult { StatusCode = HttpStatusCode.NotFound };
            }

            user.ApplyJsonWithoutPassword(json);
            this.ApplyPasswordWhenUpdateUser(user, json);
            ClarityDB.Instance.SaveChanges();

            return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = json };
        }

        private JArray BuildJsonArray(IEnumerable<User> users)
        {
            JArray array = new JArray();

            foreach (User user in users)
            {
                array.Add(user.ToJson());
            }
            return array;
        }

        private void ApplyPasswordWhenUpdateUser(User user, JObject json)
        {
            string newPassword = json.Value<string>("password");
            string oldPassword = user.Password;
            if (newPassword != oldPassword)
            {
                user.Password = passwordHash.CreatePasswordHash(newPassword, user.Salt);
            }
        }
    }
}
