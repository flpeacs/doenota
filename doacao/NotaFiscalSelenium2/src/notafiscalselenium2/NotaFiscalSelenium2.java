package notafiscalselenium2;

import java.io.IOException;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.text.DecimalFormat;
import java.util.ArrayList;
import java.util.List;
import java.util.Map;
import org.openqa.selenium.By;
import org.openqa.selenium.Keys;
import org.openqa.selenium.WebElement;
import org.openqa.selenium.interactions.Actions;
import org.openqa.selenium.phantomjs.PhantomJSDriver;
import org.openqa.selenium.phantomjs.PhantomJSDriverService;
import org.openqa.selenium.remote.DesiredCapabilities;

public class NotaFiscalSelenium2 {

    public static void main(String[] args) throws InterruptedException, IOException, SQLException {
        MySQLClass mysql = new MySQLClass();
        mysql.getConnectionAzure2();

        Map<String, String> memo = mysql.getInstitutions();
        DecimalFormat df = new DecimalFormat("#.00");

        DesiredCapabilities DesireCaps = new DesiredCapabilities();
        DesireCaps.setCapability(PhantomJSDriverService.PHANTOMJS_EXECUTABLE_PATH_PROPERTY, "/usr/local/bin/phantomjs");
        PhantomJSDriver driver = new PhantomJSDriver(DesireCaps);

        driver.get("https://www.nfp.fazenda.sp.gov.br/login.aspx");

        WebElement element = driver.findElement(By.id("UserName"));
        element.sendKeys("");
        WebElement element2 = driver.findElement(By.id("Password"));
        element2.sendKeys("");
        WebElement element3 = driver.findElement(By.id("Login"));
        element3.click();

        while (true) {
            ResultSet rs = mysql.getImagesDonation();

            if (!rs.next()) {
                break;
            }
            String CNPJEstab = rs.getString("cnpj");
            String data = rs.getString("emission_date");
            String COO = rs.getString("coupon_code");
            String valor = df.format(rs.getFloat("purchase_value")).replace(".", "").replace(",", "");
            String CNPJEnt = memo.get(rs.getString("institution_id"));
            data = data.replace("/", "");

            ArrayList<String> lst = new ArrayList<String>();
            lst.add(CNPJEstab);
            lst.add(data);
            lst.add(COO);
            lst.add(valor);
            lst.add(CNPJEnt);

            driver.get("https://www.nfp.fazenda.sp.gov.br/EntidadesFilantropicas/DoacaoNotasListagem.aspx");
            WebElement element4 = driver.findElement(By.id("btnNovaDoacao"));
            element4.click();
            Actions action = new Actions(driver);
            action.sendKeys(Keys.ESCAPE).build().perform();

            WebElement formElement = driver.findElement(By.id("divDocSemChave"));
            WebElement fieldset = formElement.findElements(By.xpath("*")).get(0);
            List<WebElement> allFormChildElements = fieldset.findElements(By.xpath("*"));

            int aux = -1;

            for (WebElement item : allFormChildElements) {
                if (item.getTagName().equals("div")) {
                    List<WebElement> div = item.findElements(By.xpath("*"));

                    for (WebElement item2 : div) {
                        if (item2.getTagName().equals("input") && "text".equals(item2.getAttribute("type"))) {
                            item2.sendKeys(lst.get(++aux));
                        }
                    }
                }
            }

            WebElement formElement2 = driver.findElement(By.id("divCNPJ"));
            WebElement div = formElement2.findElements(By.xpath("*")).get(0);
            List<WebElement> listDiv = div.findElements(By.xpath("*"));

            for (WebElement item : listDiv) {
                if (item.getTagName().equals("input") && "text".equals(item.getAttribute("type"))) {
                    item.sendKeys(lst.get(++aux));
                }
            }

            WebElement element10 = driver.findElement(By.id("btnBuscar"));
            element10.click();
            WebElement element11 = driver.findElement(By.id("btnSalvarNota"));
            element11.click();
            WebElement element12 = driver.findElement(By.id("divMensagemMaster"));
            WebElement element13 = driver.findElement(By.id("divErroMaster"));
            
            if (element12.isDisplayed()) {
                mysql.setStatus(rs.getInt("id"), "DONATED");
            } else if (element13.isDisplayed()) {
                mysql.setStatus(rs.getInt("id"), "INVALID");
            }
        }
        driver.quit();
    }
}
