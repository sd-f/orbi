package foundation.softwaredesign.orbi.model.real;

import org.apache.commons.lang3.builder.ToStringBuilder;

import java.math.BigDecimal;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
public class Position {
    // -180 W <-> 180 E
    private BigDecimal latitude;
    // 90 N <-> 90 S
    private BigDecimal longitute;
    // height
    private BigDecimal elevation;

    public Position() {
    }

    public Position(BigDecimal latitude, BigDecimal longitute, BigDecimal elevation) {
        this.latitude = latitude;
        this.longitute = longitute;
        this.elevation = elevation;
    }

    public BigDecimal getLatitude() {
        return latitude;
    }

    public void setLatitude(BigDecimal latitude) {
        this.latitude = latitude;
    }

    public BigDecimal getLongitute() {
        return longitute;
    }

    public void setLongitute(BigDecimal longitute) {
        this.longitute = longitute;
    }

    public BigDecimal getElevation() {
        return elevation;
    }

    public void setElevation(BigDecimal elevation) {
        this.elevation = elevation;
    }

    @Override
    public String toString() {
        return ToStringBuilder.reflectionToString(this);
    }
}
