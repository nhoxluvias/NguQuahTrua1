
namespace MSSQL.Sql.Constraints
{
   public class SqlForeignKeyConstraint : SqlConstraint
    {
        public string PropertyName { get; set; }
        public string ToTable { get; set; }
        public string ToProperty { get; set; }
    }
}
