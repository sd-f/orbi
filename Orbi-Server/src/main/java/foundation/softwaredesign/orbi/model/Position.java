package foundation.softwaredesign.orbi.model;

import javax.validation.constraints.NotNull;
import javax.xml.bind.annotation.XmlTransient;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
public class Position {

    @NotNull
    private Double x;
    @NotNull
    private Double y;
    @NotNull
    private Double z;

    public Position() {
    }

    public Position(Double x, Double y, Double z) {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public Double getX() {
        return x;
    }

    public void setX(Double x) {
        this.x = x;
    }

    public Double getY() {
        return y;
    }

    public void setY(Double y) {
        this.y = y;
    }

    public Double getZ() {
        return z;
    }

    public void setZ(Double z) {
        this.z = z;
    }

}
