package turnoffvm;

import java.io.IOException;
import java.sql.SQLException;
import java.io.IOException;
import java.net.*;
import java.security.*;
import java.io.*;
import java.security.KeyManagementException;
import java.security.NoSuchAlgorithmException;
import java.security.KeyStoreException;
import java.security.UnrecoverableKeyException;
import javax.net.ssl.HttpsURLConnection;
import javax.net.ssl.KeyManagerFactory;
import javax.net.ssl.SSLContext;
import javax.net.ssl.SSLSocketFactory;


/**
 *
 * @author Felipe
 */
public class TurnOffVM {
    
    private static KeyStore getKeyStore(String keyStoreName, String password) throws IOException
	{
	    KeyStore ks = null;
	    FileInputStream fis = null;
	    try {
	        ks = KeyStore.getInstance("JKS");
	        char[] passwordArray = password.toCharArray();
	        fis = new java.io.FileInputStream(keyStoreName);
	        ks.load(fis, passwordArray);
	        fis.close();
	         
	    } catch (Exception e) {
	        // TODO Auto-generated catch block
	        e.printStackTrace();
	    }
	    finally {
	        if (fis != null) {
	            fis.close();
	        }
	    }
	    return ks;
	}
    
    private static SSLSocketFactory getSSLSocketFactory(String keyStoreName, String password) throws UnrecoverableKeyException, KeyStoreException, NoSuchAlgorithmException, KeyManagementException, IOException {
	    KeyStore ks = getKeyStore(keyStoreName, password);
	    KeyManagerFactory keyManagerFactory = KeyManagerFactory.getInstance("");
	    keyManagerFactory.init(ks, password.toCharArray());
	 
	      SSLContext context = SSLContext.getInstance("TLS");
	      context.init(keyManagerFactory.getKeyManagers(), null, new SecureRandom());
	 
	      return context.getSocketFactory();
	}
    
    private static int processPostRequest(URL url, byte[] data, String contentType, String keyStore, String keyStorePassword) throws UnrecoverableKeyException, KeyManagementException, KeyStoreException, NoSuchAlgorithmException, IOException {
	    SSLSocketFactory sslFactory = getSSLSocketFactory(keyStore, keyStorePassword);
	    HttpsURLConnection con = null;
	    con = (HttpsURLConnection) url.openConnection();
	    con.setSSLSocketFactory(sslFactory);
	    con.setDoOutput(true);
	    con.setRequestMethod("POST");
	    con.addRequestProperty("x-ms-version", "2013-06-01");
	    con.setRequestProperty("Content-Length", String.valueOf(data.length));
	    con.setRequestProperty("Content-Type", contentType);
	     
	    DataOutputStream  requestStream = new DataOutputStream (con.getOutputStream());
	    requestStream.write(data);
	    requestStream.flush();
	    requestStream.close();
	    System.out.println(con.getResponseMessage());
	    return con.getResponseCode();
	}
    
    private static void turnOFF(){
        String subscriptionId = "";
	//String keyStorePath = "";
        String keyStorePath = "";
	String keyStorePassword = "";
	String url = "";
	url = String.format("https://management.core.windows.net/%s/services/hostedservices/%s/deployments/%s/roleinstances/%s/Operations", subscriptionId, "docker-doenot", "docker-doenot", "docker-doenota");
	
	String requestBody = "<ShutdownRoleOperation xmlns=\"http://schemas.microsoft.com/windowsazure\" xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\"> <OperationType>ShutdownRoleOperation</OperationType><PostShutdownAction>StoppedDeallocated</PostShutdownAction></ShutdownRoleOperation>";
	try{
        int createResponseCode = processPostRequest(new URL(url), requestBody.getBytes(), "application/xml", keyStorePath, keyStorePassword);
        }catch(Exception e){
        }
    }

    /**
     * @param args the command line arguments
     */
    public static void main(String[] args) throws IOException {
                
        MySQLClass mysql = new MySQLClass();
        int num = 0, num2;
        
        while(true){                        
            try{     
                Thread.sleep(150000);
                
                num2 = mysql.getNumberOfPendenteRows();
                System.out.println(num + " " + num2);
                
                if(num == num2){
                    break;
                }
                
                num = num2;
                
                if(num > 0){
                    Thread.sleep(150000);
                } else {
                    break;
                }
            } catch(Exception e){
                break;
            }
        }
        
        turnOFF();
    }
    
}
