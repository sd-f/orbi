package foundation.softwaredesign.orbi.model;

import javax.validation.constraints.NotNull;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
public class Position {

    @NotNull
    private Double x = new Double(0);;
    @NotNull
    private Double y = new Double(0);;
    @NotNull
    private Double z = new Double(0);;

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
