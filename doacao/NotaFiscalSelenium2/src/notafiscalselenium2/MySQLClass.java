package notafiscalselenium2;

import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;
import java.util.HashMap;
import java.util.Map;
import java.util.logging.Level;
import java.util.logging.Logger;

public class MySQLClass {

    Connection conn2 = null;

    public static Connection getConnectionAzure() {
        Connection conn = null;
        try {
            Class.forName("com.microsoft.sqlserver.jdbc.SQLServerDriver");
            conn = DriverManager.getConnection("");
        } catch (Exception e) {
            e.printStackTrace();
        }

        return conn;
    }

    public void getConnectionAzure2() throws SQLException {
        conn2 = getConnectionAzure();
        conn2.setAutoCommit(false);
    }

    public void setStatus(int id, String message) {
        Statement st = null;

        String sql = "UPDATE doenota.images_data SET status = '" + message + "' WHERE id = " + id;

        try {
            st = conn2.createStatement();
            st.executeUpdate(sql);
            conn2.commit();
        } catch (Exception e) {
            try {
                conn2.rollback();
            } catch (SQLException ex) {
                Logger.getLogger(MySQLClass.class.getName()).log(Level.SEVERE, null, ex);
            }
        }
    }

    public Map<String, String> getInstitutions() throws SQLException {
        Map<String, String> memo = new HashMap<String, String>();
        ResultSet rs = null;
        Connection conn = getConnectionAzure();
        Statement st = null;

        try {
            st = conn.createStatement();
        } catch (Exception e) {
        }

        String sql = "SELECT id, cnpj FROM doenota.institution";

        try {
            rs = st.executeQuery(sql);
        } catch (Exception e) {
        }

        while (rs.next()) {
            memo.put(rs.getString("id"), rs.getString("cnpj"));
        }

        return memo;
    }

    public ResultSet getImagesDonation() {
        ResultSet rs = null;
        Statement st = null;

        String sql = "SELECT TOP 1 id, cnpj, emission_date, coupon_code, purchase_value, institution_id FROM doenota.images_data WHERE status = 'PROCESSED'";

        try {
            st = conn2.createStatement();
            rs = st.executeQuery(sql);
        } catch (Exception e) {
            e.printStackTrace();

            try {
                conn2.rollback();
            } catch (SQLException ex) {
            }
        }

        return rs;
    }
}
