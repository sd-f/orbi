package foundation.softwaredesign.orbi.model.virtual;

import org.apache.commons.lang3.builder.ToStringBuilder;

import java.math.BigInteger;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
public class GameObject {

    private Long id;

    private Position position;

    private String name;

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public Position getPosition() {
        return position;
    }

    public void setPosition(Position position) {
        this.position = position;
    }

    public Long getId() {
        return id;
    }

    public void setId(Long id) {
        this.id = id;
    }

    @Override
    public String toString() {
        return ToStringBuilder.reflectionToString(this);
    }
}
