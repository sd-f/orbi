package foundation.softwaredesign.orbi.model.real.google;

import java.math.BigDecimal;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
public class ElevationResult {
    private Location location;
    private BigDecimal elevation;
    private BigDecimal resolution;

    public Location getLocation() {
        return location;
    }

    public void setLocation(Location location) {
        this.location = location;
    }

    public BigDecimal getElevation() {
        return elevation;
    }

    public void setElevation(BigDecimal elevation) {
        this.elevation = elevation;
    }

    public BigDecimal getResolution() {
        return resolution;
    }

    public void setResolution(BigDecimal resolution) {
        this.resolution = resolution;
    }
}
