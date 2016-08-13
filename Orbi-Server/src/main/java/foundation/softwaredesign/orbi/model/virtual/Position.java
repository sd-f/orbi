package foundation.softwaredesign.orbi.model.virtual;

import java.math.BigDecimal;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
public class Position {
    private BigDecimal x;
    private BigDecimal y;
    private BigDecimal z;

    public Position() {
    }

    public Position(BigDecimal x, BigDecimal y, BigDecimal z) {
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
