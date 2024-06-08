using Cassandra.Mapping;
using Cassandra;
using LiveMessengerApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LiveMessengerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignUpController : ControllerBase
    {
        private ICluster _cluster;
        private Cassandra.ISession _session;
        private IMapper _mapper;

        public SignUpController()
        {
            var cassandraPort = int.Parse(Environment.GetEnvironmentVariable("CASSANDRA_PORT") ?? "9042");

            _cluster = Cluster.Builder()
                .AddContactPoint("cassandra")
                .WithPort(cassandraPort)
                .Build();
            _session = _cluster.Connect("users_keyspace");
            _mapper = new Mapper(_session);
        }

        [HttpPost]
        public IActionResult Post([FromBody] User user)
        {
            if (isUserExist(user))
            {
                return BadRequest("User is already exist");
            }
            // Создаем объект PreparedStatement для выполнения запроса на вставку данных в таблицу
            var insertStatement = _session.Prepare("INSERT INTO users (user_id, first_name, last_name, phone_number, password) VALUES (uuid(), ?, ?, ?, ?)");

            // Приводим объект пользователя к параметрам запроса
            var boundStatement = insertStatement.Bind(user.first_name, user.last_name, user.phone_number, user.password);

            // Выполняем запрос на вставку данных в базу данных
            _session.Execute(boundStatement);

            return Ok("You registered successfully");
        }


        private bool isUserExist(User user)
        {
            var listOfUsers = _mapper.Fetch<User>(string.Format("SELECT * FROM users WHERE phone_number = \'{0}\'", user.phone_number));
            List<User> users = listOfUsers.ToList();

            return users.Count != 0;
        }
    }
}
