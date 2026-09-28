
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using Sportolok.Models;
using Sportolok.Models.DTO;

namespace Sportolok.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EredmenyController : ControllerBase
    {
        public string ConnectionString = "server=localhost;uid=root;password=;database= sportolo13b;";

        [HttpGet]
        public List<EredmenyM> GetEredmenyek()
        {
            List<EredmenyM> eredmenyek = new List<EredmenyM>();

            var connection = new MySqlConnection(ConnectionString);

            connection.Open();

            string sql = "SELECT * FROM eredmeny;";

            var cmd = new MySqlCommand(sql, connection);

            var data = cmd.ExecuteReader();

            while (data.Read())
            {
                var eredmeny = new EredmenyM
                {
                    Id = data.GetInt32("id"),
                    Competition = data.GetString("Competition"),
                    Description = data.GetString("Description"),
                    ResultTime = data.GetDateTime("ResultTime"),
                    UpdateTime = data.GetDateTime("UpdateTime"),
                    SportolOld = data.GetInt32("SportoloId"),
                };

                eredmenyek.Add(eredmeny);
            }

            connection.Close();

            return eredmenyek;

        }

        [HttpGet("byId")]
        public object GetEredmenyById(int id)
        {
            var connection = new MySqlConnection(ConnectionString);

            connection.Open();

            string sql = @"SELECT * FROM `eredmeny` WHERE `id`=@id";

            var cmd = new MySqlCommand(sql, connection);

            cmd.Parameters.AddWithValue("@id", id);

            var datareader = cmd.ExecuteReader();

            object? data = null;

            if (datareader.Read() == true)
            {

                var eredmeny = new EredmenyM
                {
                    Id = datareader.GetInt32("id"),
                    Competition = datareader.GetString("competition"),
                    Description = datareader.GetString("description"),
                    ResultTime = datareader.GetDateTime("resultTime"),
                    UpdateTime = datareader.GetDateTime("updateTime"),
                    SportolOld = datareader.GetInt32("sportoloId"),
                };

                data = new { message = "Sikeres lekérdezés.", result = eredmeny };
            }
            else
            {
                data = new { message = "Nincs ilyen sportolo.", result = "" };
            }

            connection.Close();
            return data;
        }


        [HttpPost]
        public object AddNewSportolo([FromBody] EredmenyPost addNewSportroloDto)
        {
            var connection = new MySqlConnection(ConnectionString);

            connection.Open();

            string sql = @"INSERT INTO `eredmeny`(`Competition`, `Description`, `ResultTime`, `UpdateTime`) VALUES (@Competition,@Description,@ResultTime,@UpdateTime)";

            var cmd = new MySqlCommand(sql, connection);

            cmd.Parameters.AddWithValue("@Competition", addNewSportroloDto.Competition);
            cmd.Parameters.AddWithValue("@Description", addNewSportroloDto.Description);
            cmd.Parameters.AddWithValue("@ResultTime", DateTime.Now);
            cmd.Parameters.AddWithValue("@UpdateTime", DateTime.Now);
          

            cmd.ExecuteNonQuery();

            connection.Close();

            return new { message = "Sikeres felvétel.", result = addNewSportroloDto};
        }

        [HttpPut]
        public object EredmenyPut([FromQuery] int id, EredmenyPut eredmenyPut)
        {
            var connection = new MySqlConnection(ConnectionString);

            connection.Open();

            var sql = @"UPDATE `eredmeny` SET `Competition`=@Competition,`Description`=@Description,`ResultTime`=@ResultTime,`UpdateTime`=@UpdateTime WHERE `id` = @id;";

            var cmd = new MySqlCommand(sql, connection);

            cmd.Parameters.AddWithValue("@Competition", eredmenyPut.Competition);
            cmd.Parameters.AddWithValue("@Description", eredmenyPut.Description);
            cmd.Parameters.AddWithValue("@ResultTime", DateTime.Now);
            cmd.Parameters.AddWithValue("@UpdateTime", DateTime.Now);
            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();

            connection.Close();

            return new { message = "Sikeres frissítés", result = eredmenyPut };
        }

        [HttpDelete]
        public object DeleteSportolo(int id)
        {
            var connection = new MySqlConnection(ConnectionString);

            connection.Open();

            string sql = @"DELETE FROM `eredmeny` WHERE id=@id";

            var cmd = new MySqlCommand(sql, connection);

            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();

            connection.Close();

            return new { message = "Sikeres törlés.", result = "" };
        }

        [HttpGet("bySportoloId")]
        public object GetSportoloById(int id)
        {
            var connection = new MySqlConnection(ConnectionString);

            connection.Open();

            string sql = @"SELECT `name`,`email` FROM `sportolo` WHERE `id`=@id";

            var cmd = new MySqlCommand(sql, connection);

            cmd.Parameters.AddWithValue("@id", id);

            var datareader = cmd.ExecuteReader();

            object? data = null;

            if (datareader.Read() == true)
            {

                var sportoloData = new SportoloIdGet
                {
                    Name = datareader.GetString("name"),
                    Email = datareader.GetString("email"),
                   
                };

                data = new { message = "Sikeres lekérdezés.", result = sportoloData };
            }
            else
            {
                data = new { message = "Nincs ilyen sportolo.", result = "" };
            }

            connection.Close();
            return data;
        }

        [HttpGet("bySportoloName")]
        public object GetSportoloByName(string name)
        {
            var connection = new MySqlConnection(ConnectionString);

            connection.Open();

            string sql = @"SELECT `name`,eredmeny.Competition, eredmeny.Description FROM `sportolo` INNER JOIN eredmeny ON sportolo.id = eredmeny.Id WHERE `name`=@name";

            var cmd = new MySqlCommand(sql, connection);

            cmd.Parameters.AddWithValue("@name", name);

            var datareader = cmd.ExecuteReader();

            object? data = null;

            if (datareader.Read() == true)
            {

                var sportoloData = new SportoloNameGet
                {
                    Name = datareader.GetString("name"),
                    Competition = datareader.GetString("competition"),
                    Description = datareader.GetString("Description"),

                };

                data = new { message = "Sikeres lekérdezés.", result = sportoloData };
            }
            else
            {
                data = new { message = "Nincs ilyen sportolo.", result = "" };
            }

            connection.Close();
            return data;
        }
    }
}