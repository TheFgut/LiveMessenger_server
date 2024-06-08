using Cassandra;
using Cassandra.Mapping;
using LiveMessengerApi.Models;
using Microsoft.AspNetCore.Mvc;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LiveMessengerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatsController : ControllerBase
    {
        private ICluster _cluster;
        private Cassandra.ISession _session;
        private IMapper _mapper;

        public ChatsController() 
        {
            var cassandraPort = int.Parse(Environment.GetEnvironmentVariable("CASSANDRA_PORT") ?? "9042");

            _cluster = Cluster.Builder()
                .AddContactPoint("cassandra")
                .WithPort(cassandraPort)
                .Build();
            _session = _cluster.Connect("users_keyspace");
            _mapper = new Mapper(_session);
        }


        // GET: api/<ChatsController>
        [HttpGet]
        public OkObjectResult Get()
        {
            var listOfUsers = _mapper.Fetch<User>("SELECT * FROM users");
            List<User> users = listOfUsers.ToList();
            return Ok(users);
        }

        // GET api/<ChatsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {

            return "Jopa";
        }

        // POST api/<ChatsController>
        [HttpPost]
        public IActionResult Post([FromBody] User user)
        {
            Guid userId = Guid.NewGuid();

            // Создаем объект PreparedStatement для выполнения запроса на вставку данных в таблицу
            var insertStatement = _session.Prepare("INSERT INTO users (user_id, first_name, last_name, phone_number, password) VALUES (uuid(), ?, ?, ?, ?)");

            // Приводим объект пользователя к параметрам запроса
            var boundStatement = insertStatement.Bind(user.first_name, user.last_name, user.phone_number, user.password);

            // Выполняем запрос на вставку данных в базу данных
            _session.Execute(boundStatement);

            return Ok("You registered successfully");
        }

        // PUT api/<ChatsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ChatsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
