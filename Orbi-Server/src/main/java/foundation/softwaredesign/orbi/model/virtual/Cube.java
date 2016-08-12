package foundation.softwaredesign.orbi.model.virtual;

import org.apache.commons.lang3.builder.ToStringBuilder;

import java.math.BigInteger;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
public class Cube {

    private BigInteger id;

    private Coordinates coordinates;

    public Coordinates getCoordinates() {
        return coordinates;
    }

    public void setCoordinates(Coordinates coordinates) {
        this.coordinates = coordinates;
    }

    public BigInteger getId() {
        return id;
    }

    public void setId(BigInteger id) {
        this.id = id;
    }

    @Override
    public String toString() {
        return ToStringBuilder.reflectionToString(this);
    }
}
