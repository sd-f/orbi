package foundation.softwaredesign.orbi.model.real.google;

import java.io.Serializable;
import java.math.BigDecimal;
import java.text.DecimalFormat;
import java.text.DecimalFormatSymbols;
import java.util.Locale;
import java.util.StringJoiner;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
public class Location implements Serializable {

    private BigDecimal lat;
    private BigDecimal lng;

    public Location() {
    }

    public Location(BigDecimal lat, BigDecimal lng) {
        this.lat = lat;
        this.lng = lng;
    }

    public BigDecimal getLat() {
        return lat;
    }

    public void setLat(BigDecimal lat) {
        this.lat = lat;
    }

    public BigDecimal getLng() {
        return lng;
    }

    public void setLng(BigDecimal lng) {
        this.lng = lng;
    }

    @Override
    public String toString() {

        DecimalFormat df = new DecimalFormat();
        DecimalFormatSymbols decimalFormatSymbols = DecimalFormatSymbols.getInstance(Locale.ENGLISH);
        df.setDecimalFormatSymbols(decimalFormatSymbols);
        df.setMaximumFractionDigits(6);
        df.setMinimumFractionDigits(6);
        df.setGroupingUsed(false);

        String latString = df.format(lat.setScale(6, BigDecimal.ROUND_DOWN));
        String lngString = df.format(lng.setScale(6, BigDecimal.ROUND_DOWN));

        StringJoiner stringJoiner = new StringJoiner(",");
        stringJoiner.add(latString);
        stringJoiner.add(lngString);

        return stringJoiner.toString();
    }
}
