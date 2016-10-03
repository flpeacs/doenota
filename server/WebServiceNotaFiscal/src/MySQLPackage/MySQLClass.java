package MySQLPackage;

import java.sql.DriverManager;
import java.sql.ResultSet;
import java.sql.Statement;
import java.sql.Connection;

public class MySQLClass {

    public static Connection getConnection() {
        Connection conn = null;
        try {
            Class.forName("com.mysql.jdbc.Driver");
            conn = DriverManager.getConnection("","", "");
        } catch(Exception e){
        	e.printStackTrace();
        }

        return conn;
    }

    public ResultSet getNotProcessed() {
        Connection conn = getConnection();
        Statement st = null;

        try {
            st = conn.createStatement();
        } catch (Exception e) {
            System.out.println(e);
        }

        String sql = "SELECT data_id, file_path FROM images_data WHERE status = 'NOT_PROCESSED'";
        ResultSet rs = null;

        try {
            rs = st.executeQuery(sql);
        } catch (Exception e) {
        }

        return rs;
    }
    
    public void appendImage(String s, int data_id){
    	Connection conn = getConnection();
        Statement st = null;

        try {
            st = conn.createStatement();
        } catch (Exception e) {
        }

        String sql = "UPDATE images_data SET file_path += '" + s + "' WHERE data_id = " + data_id;

        try {
            st.executeUpdate(sql);
            conn.close();
        } catch (Exception e) {
            System.err.println(e);
        }
    }

    public void insertImagesFirstProcessing(int user_id, int institution_id, String filePath) {
        Connection conn = getConnection();
        Statement st = null;

        filePath = filePath.replaceAll("\\\\", "\\\\\\\\\\\\\\\\");

        try {
            st = conn.createStatement();
        } catch (Exception e) {
            System.out.println(e);
        }

        String sql = "INSERT INTO images_data(user_id, institution_id, status, file_path "
                + ") VALUES (" + user_id + ", " + institution_id + ", 'NOT_PROCESSED', '" + filePath + "')";

        try {
            st.executeUpdate(sql);
            conn.close();
        } catch (Exception e) {
            System.err.println(e);
        }
    }

    public void insertImagesData(int data_id, String cnpj, String emission_date, String coupon_code, String purchase_value) {
        Connection conn = getConnection();
        Statement st = null;

        try {
            st = conn.createStatement();
        } catch (Exception e) {
            System.err.println(e);
        }

        String sql = "UPDATE images_data SET cnpj = '" + cnpj + "', emission_date = '" + emission_date + "', coupon_code = '" + coupon_code + ""
                + "', purchase_value = " + purchase_value + ", status = 'ACCEPTED' WHERE data_id = " + data_id;

        try {
            st.executeUpdate(sql);
            conn.close();
        } catch (Exception e) {
            System.err.println(e);
        }
    }

    public void imageRejected(int data_id) {
        Connection conn = getConnection();
        Statement st = null;

        try {
            st = conn.createStatement();
        } catch (Exception e) {
            System.out.println(e);
        }

        String sql = "UPDATE images_data SET status = 'REJECTED' WHERE data_id = " + data_id;

        try {
            st.executeUpdate(sql);
            conn.close();
        } catch (Exception e) {
            System.err.println(e);
        }
    }

}
