namespace StreetNameRegistry.Infrastructure
{
    using System.Data;
    using StreetName;
    using Microsoft.EntityFrameworkCore;

    public class SqlPersistentLocalIdGenerator : IPersistentLocalIdGenerator
    {
        private readonly SequenceContext _context;

        public SqlPersistentLocalIdGenerator(SequenceContext context) => _context = context;

        public PersistentLocalId GenerateNextPersistentLocalId()
        {
            var sqlStatement = $"SELECT NEXT VALUE FOR {Schema.Sequence}.{SequenceContext.StreetNamePersistentLocalIdSequenceName}";

            int nextNumber;
            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.Connection.Open();
                command.CommandText = sqlStatement;
                command.CommandType = CommandType.Text;

                nextNumber = (int)command.ExecuteScalar();
                command.Connection.Close();
            }

            return new PersistentLocalId(nextNumber);
        }
    }
}
