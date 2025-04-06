using GD_API.Models;
using MySql.Data.MySqlClient;

namespace GD_API.Data
{
    public class ClaimRepository
    {
        // Zugriff auf Datenbank herstellen
        private readonly Database _database;
        private readonly ILogger<ClaimRepository> _logger;

        public ClaimRepository(Database database, ILogger<ClaimRepository> logger)
        {
            _database = database;
            _logger = logger;
        }


        public List<Claim> GetAllClaims()
        {
            try
            {
                var claims = new List<Claim>();
                using (var conn = _database.GetConnection())
                {
                    conn.Open();
                    var cmd = new MySqlCommand("SELECT * FROM claims", conn);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            claims.Add(MapReaderToClaim(reader));
                        }
                    }
                }
                return claims;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fehler beim Abrufen aller Schürfrechte.");
                throw;
            }
        }


        public Claim? GetClaimById(int id)
        {
            try
            {
                using (var conn = _database.GetConnection())
                {
                    conn.Open();

                    var cmd = new MySqlCommand("SELECT * FROM claims WHERE id = @id", conn);
                    cmd.Parameters.AddWithValue("@id", id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                            return MapReaderToClaim(reader);
                    }
                }
                return null; // null löst Fehlercode 404 (not found) aus
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Fehler beim Abrufen des Schürfrechts mit ID {id}.");
                throw;
            }
        }


        public Claim? GetByClaimNumber(string claimNumber)
        {
            try
            {
                using (var conn = _database.GetConnection())
                {
                    conn.Open();
                    var cmd = new MySqlCommand("SELECT * FROM claims WHERE claim_number = @claim_number", conn);
                    cmd.Parameters.AddWithValue("@claim_number", claimNumber);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                            return MapReaderToClaim(reader);
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Fehler beim Abrufen des Schürfrechts mit ClaimNumber {claimNumber}.");
                throw;
            }

        }


        public int CreateClaim(Claim claim)
        {
            try
            {
                using (var conn = _database.GetConnection())
                {
                    conn.Open();
                    var cmd = new MySqlCommand("INSERT INTO claims (claim_number, owner_name, location, gold_amount, date_claimed) " +
                                               "VALUES (@claim_number, @owner_name, @location, @gold_amount, @date_claimed)", conn);

                    cmd.Parameters.AddWithValue("@claim_number", claim.ClaimNumber);
                    cmd.Parameters.AddWithValue("@owner_name", claim.OwnerName);
                    cmd.Parameters.AddWithValue("@location", claim.Location);
                    cmd.Parameters.AddWithValue("@gold_amount", claim.GoldAmount);
                    cmd.Parameters.AddWithValue("@date_claimed", claim.DateClaimed);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        cmd = new MySqlCommand("SELECT LAST_INSERT_ID();", conn);
                        return Convert.ToInt32(cmd.ExecuteScalar());
                    }
                    
                }
                return -1;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fehler beim Erstellen eines neuen Schürfrechts.");
                throw;
            }

        }


        public bool UpdateClaim(int id, Claim claim)
        {
            try
            {
                using (var conn = _database.GetConnection())
                {
                    conn.Open();
                    var cmd = new MySqlCommand("UPDATE claims SET claim_number = @claim_number, " +
                                                                 "owner_name = @owner_name, " +
                                                                 "location = @location, " +
                                                                 "gold_amount = @gold_amount, " +
                                                                 "date_claimed = @date_claimed " +
                                                                 "WHERE id = @id", conn);

                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@claim_number", claim.ClaimNumber);
                    cmd.Parameters.AddWithValue("@owner_name", claim.OwnerName);
                    cmd.Parameters.AddWithValue("@location", claim.Location);
                    cmd.Parameters.AddWithValue("@gold_amount", claim.GoldAmount);
                    cmd.Parameters.AddWithValue("@date_claimed", claim.DateClaimed);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Fehler beim aktualisieren des Schürfrechts mit der ID {id}.");
                throw;
            }
        }


        public bool DeleteClaim(int id)
        {
            try
            {
                using (var conn = _database.GetConnection())
                {
                    conn.Open();
                    var cmd = new MySqlCommand("DELETE FROM claims WHERE id = @id", conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Fehler beim Löschen des Schürfrechts mit der ID {id}.");
                throw;
            }
        }


        private Claim MapReaderToClaim(MySqlDataReader reader)
        {
            return new Claim
            {
                Id = reader.GetInt32("id"),
                ClaimNumber = reader.GetString("claim_number"),
                OwnerName = reader.GetString("owner_name"),
                Location = reader.GetString("location"),
                GoldAmount = reader.GetDecimal("gold_amount"),
                DateClaimed = reader.GetDateTime("date_claimed")
            };
        }
    }
}
