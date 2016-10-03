import MySQLPackage.MySQLClass;
import java.io.BufferedOutputStream;
import java.io.FileOutputStream;
import java.util.Calendar;
import javax.jws.WebMethod;
import javax.jws.WebService;
import javax.xml.ws.BindingType;
import javax.xml.ws.soap.SOAPBinding;
import javax.ejb.Stateless;
import javax.xml.ws.Service;

import com.sun.org.apache.xml.internal.serialize.Printer;

@Stateless
@WebService(
        portName = "NotaFiscalPort",
        serviceName = "NotaFiscalService",
        targetNamespace = "",
        endpointInterface = "NotaFiscalWs")
@BindingType(value = SOAPBinding.SOAP11HTTP_MTOM_BINDING)
public class NotaFiscalImplementation implements NotaFiscalWs {

    private MySQLClass mySQL = new MySQLClass();

    @WebMethod
    public boolean upload(byte[] imageBytes, int user_id, int institution_id, String platform) {
    	    	
    	Calendar cal = Calendar.getInstance();
        String fileName = user_id + "" + institution_id + "" + cal.getTimeInMillis() + ".jpg";
        String filePath = "" + fileName;

        try {  
        	FileOutputStream fos = new FileOutputStream(filePath);
        	BufferedOutputStream outputStream = new BufferedOutputStream(fos);
        	outputStream.write(imageBytes);           
        	outputStream.close();   
            	mySQL.insertImagesFirstProcessing(user_id, institution_id, filePath);
        } catch (Exception ex) {
        	return false;
        }
        
        return true;
    }   	
}
