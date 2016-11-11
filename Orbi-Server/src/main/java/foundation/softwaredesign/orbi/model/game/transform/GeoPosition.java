package foundation.softwaredesign.orbi.model.game.transform;

import org.apache.commons.lang3.builder.ToStringBuilder;

import javax.validation.constraints.NotNull;
import javax.xml.bind.annotation.XmlRootElement;

import static java.util.Objects.isNull;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@XmlRootElement
public class GeoPosition {

    @NotNull
    private Double latitude = new Double(0);
    @NotNull
    private Double longitude = new Double(0);
    @NotNull
    private Double altitude = new Double(0);

    public GeoPosition() {
    }

    public GeoPosition(Double latitude, Double longitude, Double altitude) {
        this.latitude = latitude;
        this.longitude = longitude;
        this.altitude = altitude;
    }

    public Double getLatitude() {
        return latitude;
    }

    public void setLatitude(Double latitude) {
        this.latitude = latitude;
    }

    public Double getLongitude() {
        return longitude;
    }

    public void setLongitude(Double longitude) {
        this.longitude = longitude;
    }

    public Double getAltitude() {
        return altitude;
    }

    public void setAltitude(Double altitude) {
        this.altitude = altitude;
    }

    @Override
    public String toString() {
        return ToStringBuilder.reflectionToString(this);
    }

    @Override
    public GeoPosition clone() {
        return new GeoPosition(this.latitude.doubleValue(),this.longitude.doubleValue(),this.altitude.doubleValue());
    }

    public Double distanceTo(GeoPosition geoPosition) {
        if (isNull(geoPosition)) {
            return null;
        }
        if (isNull(geoPosition.getAltitude()) || isNull(geoPosition.getLatitude()) || isNull(geoPosition.getLongitude())) {
            return null;
        }
        if (isNull(this.getAltitude()) || isNull(this.getLatitude()) || isNull(this.getLongitude())) {
            return null;
        }
        Double s1 = Math.pow(getAltitude() - geoPosition.getAltitude(),2);
        Double s2 = Math.pow(getLatitude() - geoPosition.getLatitude(),2);
        Double s3 = Math.pow(getLongitude() - geoPosition.getLongitude(),2);
        return Math.sqrt(s1+s2+s3);
    }
}
