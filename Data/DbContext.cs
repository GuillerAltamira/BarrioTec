using System.Data.SqlClient;

namespace TiendaApp.Data {
    public class DbContext {
        private static DbContext instance;
        private readonly string connStr =
            "Server=PCA-16; Database=TiendaDb; Trusted_Connection=True;";

        private DbContext() { }

        public static DbContext Instance {
            get {
                if (instance == null) instance = new DbContext();
                return instance;
            }
        }

        public SqlConnection GetConnection() {
            return new SqlConnection(connStr);
        }
    }
}
