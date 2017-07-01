package foundation.softwaredesign.orbi.model;

import javax.xml.bind.annotation.adapters.XmlAdapter;
import java.text.SimpleDateFormat;
import java.util.Date;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
public class DateAdapter extends XmlAdapter<String, Date> {

    private SimpleDateFormat dateFormat = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss");

    @Override
    public Date unmarshal(String v) throws Exception {
        return dateFormat.parse(v);
    }

    @Override
    public String marshal(Date v) throws Exception {
        return dateFormat.format(v);
    }
}