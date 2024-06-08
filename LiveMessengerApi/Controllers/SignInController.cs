using Cassandra.Mapping;
using Cassandra;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LiveMessengerApi.Models;

namespace LiveMessengerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignInController : ControllerBase
    {
        private ICluster _cluster;
        private Cassandra.ISession _session;
        private IMapper _mapper;

        private readonly TokenService _tokenService;

        public SignInController()
        {
            var cassandraPort = int.Parse(Environment.GetEnvironmentVariable("CASSANDRA_PORT") ?? "9042");

            _cluster = Cluster.Builder()
                .AddContactPoint("cassandra")
                .WithPort(cassandraPort)
                .Build();
            _session = _cluster.Connect("users_keyspace");
            _mapper = new Mapper(_session);

            _tokenService = new TokenService();
        }

        [HttpPost]
        public IActionResult Post([FromBody] SignInRequest singInRequest)
        {
            if (!isUserExist(singInRequest.phone_number))
            {
                return BadRequest("User with this phone number is not exist");
            }
            SignInSuccessResponse response = new SignInSuccessResponse();

            User? user = getUser(singInRequest.phone_number);
            if (user == null) return BadRequest("User is not exist");

            if (string.IsNullOrEmpty(singInRequest.token))//sign in by [hone number + password
            {

                if (user.password != singInRequest.password)
                {
                    return BadRequest("Password is incorrect");
                }
                response.user = user;
                response.token = _tokenService.GenerateToken(singInRequest.phone_number);
            }
            else//sign in by token
            {
                if(!_tokenService.isTokenValid(singInRequest.token)) return BadRequest("Bad token");
                response.user = user;
                response.token = singInRequest.token;
            }

            return Ok(response);
        }

        private bool isUserExist(string phone_number)
        {
            return getUser(phone_number) != null;
        }

        private User? getUser(string phone_number)
        {
            var listOfUsers = _mapper.Fetch<User>(string.Format("SELECT * FROM users WHERE phone_number = \'{0}\'", phone_number));
            List<User> users = listOfUsers.ToList();
            return users.Count == 0 ? null : users[0];
        }
    }
}
