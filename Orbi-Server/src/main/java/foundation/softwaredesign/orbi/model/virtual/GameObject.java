package foundation.softwaredesign.orbi.model.virtual;

import org.apache.commons.lang3.builder.ToStringBuilder;

import java.math.BigInteger;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
public class GameObject {

    private BigInteger id;

    private Position position;

    public Position getPosition() {
        return position;
    }

    public void setPosition(Position position) {
        this.position = position;
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
