using MSSQL_Lite.Access;
using MSSQL_Lite.Connection;

namespace Data.DAL
{
    internal class DBContext : SqlContext
    {
        private bool disposed;
        private SqlAccess<Role> roles;
        private SqlAccess<User> users;
        private SqlAccess<PaymentMethod> paymentMethods;
        private SqlAccess<PaymentInfo> paymentInfos;
        private SqlAccess<Category> categories;
        private SqlAccess<Tag> tags;
        private SqlAccess<Film> films;
        private SqlAccess<Language> languages;
        private SqlAccess<Country> countries;
        private SqlAccess<CategoryDistribution> categoryDistributions;
        private SqlAccess<Director> directors;
        private SqlAccess<DirectorOfFilm> directorOfFilms;
        private SqlAccess<Cast> casts;


        public DBContext(ConnectionType connectionType)
            : base(connectionType)
        {
            roles = null;
            users = null;
            paymentInfos = null;
            paymentMethods = null;
            categories = null;
            tags = null;
            films = null;
            languages = null;
            countries = null;
            categoryDistributions = null;
            directors = null;
            directorOfFilms = null;
            casts = null;
            disposed = false;
        }

        public SqlAccess<Role> Roles { get { return InitSqlAccess<Role>(ref roles); } }
        public SqlAccess<User> Users { get { return InitSqlAccess<User>(ref users); } }
        public SqlAccess<PaymentMethod> PaymentMethods { get { return InitSqlAccess<PaymentMethod>(ref paymentMethods); } }
        public SqlAccess<PaymentInfo> PaymentInfos { get { return InitSqlAccess<PaymentInfo>(ref paymentInfos); } }
        public SqlAccess<Category> Categories { get { return InitSqlAccess<Category>(ref categories); } }
        public SqlAccess<Tag> Tags { get { return InitSqlAccess<Tag>(ref tags); } }
        public SqlAccess<Film> Films { get { return InitSqlAccess<Film>(ref films); } }
        public SqlAccess<Language> Languages { get { return InitSqlAccess<Language>(ref languages); } }
        public SqlAccess<Country> Countries { get { return InitSqlAccess<Country>(ref countries); } }
        public SqlAccess<CategoryDistribution> CategoryDistributons { get { return InitSqlAccess<CategoryDistribution>(ref categoryDistributions); } }
        public SqlAccess<Director> Directors { get { return InitSqlAccess<Director>(ref directors); } }
        public SqlAccess<DirectorOfFilm> DirectorOfFilms { get { return InitSqlAccess<DirectorOfFilm>(ref directorOfFilms); } }
        public SqlAccess<Cast> Casts { get { return InitSqlAccess<Cast>(ref casts); } }

        protected override void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                try
                {
                    if (disposing)
                    {
                        DisposeSqlAccess<Role>(ref roles);
                        DisposeSqlAccess<User>(ref users);
                        DisposeSqlAccess<PaymentMethod>(ref paymentMethods);
                        DisposeSqlAccess<PaymentInfo>(ref paymentInfos);
                        DisposeSqlAccess<Category>(ref categories);
                        DisposeSqlAccess<CategoryDistribution>(ref categoryDistributions);
                        DisposeSqlAccess<Tag>(ref tags);
                        DisposeSqlAccess<Language>(ref languages);
                        DisposeSqlAccess<Country>(ref countries);
                        DisposeSqlAccess<Director>(ref directors);
                        DisposeSqlAccess<DirectorOfFilm>(ref directorOfFilms);
                        DisposeSqlAccess<Cast>(ref casts);
                    }
                    this.disposed = true;
                }
                finally
                {
                    base.Dispose(disposing);
                }
            }
        }
    }
}
