package foundation.softwaredesign.orbi.service.game.server;

import java.util.Calendar;
import java.util.Date;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
public class DateComparator {

    public static Boolean isTimeOlderThan(int field, int amount, Date olderThan) {
        Calendar cal = Calendar.getInstance();
        cal.add(field,amount);
        return olderThan.before(cal.getTime());
    }
}
