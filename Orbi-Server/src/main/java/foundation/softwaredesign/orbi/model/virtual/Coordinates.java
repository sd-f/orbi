package foundation.softwaredesign.orbi.model.virtual;

import java.math.BigDecimal;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
public class Coordinates {
    private BigDecimal x;
    private BigDecimal y;
    private BigDecimal z;

    public Coordinates() {
    }

    public Coordinates(BigDecimal x, BigDecimal y, BigDecimal z) {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public BigDecimal getX() {
        return x;
    }

    public void setX(BigDecimal x) {
        this.x = x;
    }

    public BigDecimal getY() {
        return y;
    }

    public void setY(BigDecimal y) {
        this.y = y;
    }

    public BigDecimal getZ() {
        return z;
    }

    public void setZ(BigDecimal z) {
        this.z = z;
    }
}
