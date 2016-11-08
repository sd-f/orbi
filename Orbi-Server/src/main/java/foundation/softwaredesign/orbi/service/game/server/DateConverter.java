package foundation.softwaredesign.orbi.service.game.server;

import javax.enterprise.context.RequestScoped;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.logging.Logger;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@RequestScoped
public class DateConverter {

    private SimpleDateFormat dateFormat = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss");

    public String toString(Date date) {
        return dateFormat.format(date);
    }

    public Date toDate(String date) {
        try {
            return dateFormat.parse(date);
        } catch (ParseException e) {
            Logger.getLogger(DateConverter.class.getSimpleName()).finest(e.getMessage());
        }
        return null;
    }
}
