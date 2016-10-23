package foundation.softwaredesign.orbi.model.game.transform;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
public class Transform {

    private Rotation rotation = new Rotation();
    private Position position = new Position();
    private GeoPosition geoPosition = new GeoPosition();

    public Transform() {
    }

    public Rotation getRotation() {
        return rotation;
    }

    public void setRotation(Rotation rotation) {
        this.rotation = rotation;
    }

    public Position getPosition() {
        return position;
    }

    public void setPosition(Position position) {
        this.position = position;
    }

    public GeoPosition getGeoPosition() {
        return geoPosition;
    }

    public void setGeoPosition(GeoPosition geoPosition) {
        this.geoPosition = geoPosition;
    }
}
