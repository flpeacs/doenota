package turnoffvm;

import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;
import java.util.HashMap;
import java.util.Map;

public class MySQLClass {

    public static Connection getConnectionAzure() throws ClassNotFoundException, SQLException {
        Connection conn = null;

        Class.forName("com.microsoft.sqlserver.jdbc.SQLServerDriver");
        conn = DriverManager.getConnection("");
        
        return conn;
    }

    public int getNumberOfPendenteRows() throws SQLException, ClassNotFoundException {
        ResultSet rs = null;
        Connection conn = getConnectionAzure();
        Statement st = null;

        st = conn.createStatement();

        String sql = "SELECT count(id) FROM doenota.images_data WHERE status = 'UNPROCESSED' OR status = 'PROCESSING' OR status = 'PROCESSED' ";

        rs = st.executeQuery(sql);
        rs.next();

        return rs.getInt(1);
    }
}
