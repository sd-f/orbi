package foundation.softwaredesign.orbi.model;

import org.apache.commons.lang3.builder.ToStringBuilder;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
public class GameObject {

    private Long id;
    private GeoPosition geoPosition;
    private Rotation rotation;
    private String name;

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public GeoPosition getGeoPosition() {
        return geoPosition;
    }

    public void setGeoPosition(GeoPosition geoPosition) {
        this.geoPosition = geoPosition;
    }

    public Long getId() {
        return id;
    }

    public void setId(Long id) {
        this.id = id;
    }

    public Rotation getRotation() {
        return rotation;
    }

    public void setRotation(Rotation rotation) {
        this.rotation = rotation;
    }

    @Override
    public String toString() {
        return ToStringBuilder.reflectionToString(this);
    }
}