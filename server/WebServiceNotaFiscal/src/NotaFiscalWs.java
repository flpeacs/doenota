import javax.jws.WebService;
 
@WebService(targetNamespace = "http://lampiao.ic.unicamp.br:8085/WebServiceNotaFiscal/webservices/NotaFiscalImplementation?wsdl")
public interface NotaFiscalWs {
	public boolean upload(byte[] imageBytes, int user_id, int institution_id, String platform);
}